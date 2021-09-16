using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HYC.WindowsForms;
using HYC.StandardAoi.UI;
using HYC.StandardAoi.UI.Parameter;
using HYC.StandardAoi.Events;
using RasterDetectionMod.Entities;
using System.Net.Sockets;
using System.Net;
using System.IO;
using HYC.HTTools;
using RasterDetectionMod.Classes;
using HYC.HTLog;
using HYC.HTImage;
using RasterDetectionMod.Recipes;
using System.Configuration;
using RasterDetectionMod.Forms;

namespace RasterDetectionMod
{
    public partial class FormMain : FormBase
    {
        #region 相机框架

        /// <summary>
        /// 窗体静态实例
        /// </summary>
        public static FormMain Instance = null;

        /// <summary>
        /// 显示文本提示信息
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="brush"></param>
        public void ShowMsg(string strValue, Brush brush)
        {
            Global.FrmStandardAoi.ShowMsg(strValue, brush);
        }

        /// <summary>
        /// 显示组件连接状态
        /// </summary>
        /// <param name="devName"></param>
        /// <param name="status"></param>
        public void UpdateDeviceState(string devName, bool? status)
        {
            Global.FrmStandardAoi.UpdateDeviceState(devName, status);
        }

        /// <summary>
        /// 获取当前正在使用的机种ID
        /// </summary>
        public string GetCurRecipeID()
        {
            return Global.FrmStandardAoi.SelectedRecipeID;
        }

        public void AddForm(FormStandardAoi frmStandardAoi)
        {
            //添加窗体
            frmStandardAoi.FormBorderStyle = FormBorderStyle.None;
            frmStandardAoi.TopLevel = false;
            frmStandardAoi.Dock = DockStyle.Fill;
            this.tabPage2D.Controls.Add(frmStandardAoi);
            //设置属性
            frmStandardAoi.DetectionTypeUI = DetectionTypeUI.Aoi;
            //传入实例
            frmStandardAoi.SetSignalProvider(Global.SignalProvider);
            frmStandardAoi.SetImageProducer(Global.ImageProducers);
            frmStandardAoi.SetPatternProducers(Global.PatternProducers);
            frmStandardAoi.SetPowerController(Global.PowerControllers);
            //监听事件
            frmStandardAoi.GetManager().DetectionCompleted += FrmStandardAoi_DetectionCompleted;
            frmStandardAoi.Show();
        }

        /// <summary>
        /// 检测完成事件 这个只能在2D主机上触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStandardAoi_DetectionCompleted(object sender, DetectionCompletedArgs e)
        {
            if (!e.IsImageTestMode && !e.IsManualMode)
            {
                //2D测试结果
                if (e.Result.Detail.TestResult == HYC.StandardAoi.Parameter.ResultType.OK)
                {
                    PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera2DResultOK, true);   //通知PLC检测OK
                    //OK的时候通知PLC结束，并将数据存储到数据库中
                }
                else if (e.Result.Detail.TestResult == HYC.StandardAoi.Parameter.ResultType.NG)  //需要判断不同检测类型时传输的数值是如何变化（分层检测时，X和Y的数值应该对调传到PLC）
                {
                    SendPosDetailToPLC(e);

                    PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera2DResultNG, true);   //通知PLC检测NG
                }
            }
        }
        public static SoftRunParamaters softRun = new SoftRunParamaters();

        /// <summary>
        /// 发送2D检测结果给PLC
        /// </summary>
        /// <param name="e"></param>
        private static void SendPosDetailToPLC(DetectionCompletedArgs e)
        {
            TestRegionCollection collection = new TestRegionCollection();

            //缺陷所在的划分的网格大小
            collection.Size3dW = Convert.ToSingle(softRun.Size3dW) * 0.8f;
            collection.Size3dH = Convert.ToSingle(softRun.Size3dH) * 0.8f;

            // 将反馈的坐标转换为区域
            foreach (var pattern in e.Result.Detail.ListPattern)
            {
                foreach (var reason in pattern.ListReason)
                {
                    foreach (var area in reason.DefectAreas)
                    {
                        LogManager.GetLogger("log_area").Info("X:" + area.X + ",Y:" + area.Y);
                        // 像素转换为毫米
                        float x = area.X / 1000f;
                        float y = area.Y / 1000f;

                        TestRegion region = new TestRegion();
                        region.Row = (int)(y / collection.Size3dH);
                        region.Column = (int)(x / collection.Size3dW);
                        region.XPosition = (region.Column + 0.5f) * collection.Size3dW;
                        region.YPosition = (region.Row + 0.5f) * collection.Size3dH;
                        if (collection.ListRegions.Where(p => p.Row == region.Row && p.Column == region.Column).Count() <= 0)
                        {
                            collection.ListRegions.Add(region);
                        }
                    }
                }
            }
            // 区域划分数量    将区域信息写给PLC
            int Max_Image_Area = Convert.ToInt32(softRun.Image_Area);//TODO,i < 50:变为可需改

            List<ushort> lstX = new List<ushort>();
            List<ushort> lstY = new List<ushort>();
            List<ushort> lstPos = new List<ushort>();
            int count = 0;
            //如果缺陷数量超出你配置的数量，跳出循环
            for (int i = 0; i < collection.ListRegions.Count; i++)
            {
                if (count > Max_Image_Area) break;

                if (Global.CurCustomerType == CustomerType.Geer)
                {
                    // 图片左下角作为原点
                    lstX.AddRange(HTValueConverter.IntToUshortArray((int)(-collection.ListRegions[i].XPosition * 1000)));
                    lstY.AddRange(HTValueConverter.IntToUshortArray((int)(-collection.ListRegions[i].YPosition * 1000)));
                }
                else if (Global.CurCustomerType == CustomerType.SE)
                {
                    // 图片左上角作为原点
                    lstX.AddRange(HTValueConverter.IntToUshortArray((int)(collection.ListRegions[i].XPosition * 1000)));
                    lstY.AddRange(HTValueConverter.IntToUshortArray((int)(-collection.ListRegions[i].YPosition * 1000)));
                }
                else if (Global.CurCustomerType == CustomerType.RiDong)
                {
                    // 图片左下角作为原点
                    lstX.AddRange(HTValueConverter.IntToUshortArray((int)(-collection.ListRegions[i].YPosition * 1000)));
                    lstY.AddRange(HTValueConverter.IntToUshortArray((int)(-collection.ListRegions[i].XPosition * 1000)));
                }
                else
                {
                    lstX.AddRange(HTValueConverter.IntToUshortArray((int)(collection.ListRegions[i].XPosition * 1000)));
                    lstY.AddRange(HTValueConverter.IntToUshortArray((int)(-collection.ListRegions[i].YPosition * 1000)));
                }
                lstPos.Add((ushort)collection.ListRegions[i].Row);
                lstPos.Add((ushort)collection.ListRegions[i].Column);
                count++;
            }

            //int Max_Image_Area = Convert.ToInt32(softRun.Image_Area);//TODO  ,i < 50:变为可需改

            for (int i = count; i < Max_Image_Area; i++)
            {
                lstX.AddRange(new ushort[] { 0, 0 });
                lstY.AddRange(new ushort[] { 0, 0 });
                lstPos.Add(0);
                lstPos.Add(0);
            }
            lstPos.Insert(0, (ushort)count);

            //根据2D缺陷位置给发PLC坐标，进行3D
            PlcManager.PlcCtrl.WriteWords(PlcAddr.XPositionStart3D, lstX.ToArray());
            PlcManager.PlcCtrl.WriteWords(PlcAddr.YPositionStart3D, lstY.ToArray());
            PlcManager.PlcCtrl.WriteWords(PlcAddr.DefectRegions, lstPos.ToArray());

            string strCollection = HYC.HTFile.HTXML.HTXMLSerializer.EntityToXMLString(collection);
            LogManager.GetLogger("Result").Info("Count:" + count);
            LogManager.GetLogger("Result").Info(strCollection);
        }

        /// <summary>
        /// 初始化硬件链接
        /// </summary>
        private void InitHardware()
        {
            if (Global.CurCheckType == CheckType.Raster)
            {
                if (Global.CurCheckCam == CheckCam.Cam2D)
                {
                    AoiCamera.Init();//Aoi镜头 
                    PowerManager.Init(); //光源控制器
                    tabcontrolcam.TabPages[1].Parent = null;
                    tabcontrolcam.TabPages[1].Parent = null;
                }
                else
                {
                    Init3DCam(); //奕目的3D相机初始化
                    PowerManager.Init(); //光源控制器
                    tabcontrolcam.TabPages[1].Parent = null;
                    tabcontrolcam.TabPages[0].Parent = null;
                }
            }
            else if (Global.CurCheckType == CheckType.ScreenLayer)
            {
                if (Global.CurCheckCam == CheckCam.Cam2D)
                {
                    AoiCamera.Init();    //Aoi镜头 
                    PowerManager.Init(); //光源控制器
                    tabcontrolcam.TabPages[1].Parent = null;
                    tabcontrolcam.TabPages[1].Parent = null;
                }
                else
                {
                    Init3DCam();    //奕目的3D相机初始化
                    PowerManager.Init(); //光源控制器
                    tabcontrolcam.TabPages[0].Parent = null;
                    tabcontrolcam.TabPages[0].Parent = null;
                }
            }
            else if (Global.CurCheckType == CheckType.GoldWire)
            {
                if (Global.CurCheckCam == CheckCam.Cam2D)
                {
                    Init2DCam();        //奕目的2D初始化
                    PowerManager.Init(); //光源控制器
                    tabcontrolcam.TabPages[0].Parent = null;
                    tabcontrolcam.TabPages[1].Parent = null;
                }
                else
                {
                    Init3DCam();       //奕目的3D相机初始化
                    PowerManager.Init(); //光源控制器
                    tabcontrolcam.TabPages[0].Parent = null;
                    tabcontrolcam.TabPages[0].Parent = null;
                }
            }
            Task.Factory.StartNew(() =>
            {
                PlcManager.Init();//PLC                                         
            });
        }

        #endregion

        /// <summary>
        /// 当前软件中做的机种名称
        /// </summary>
        string currentRasterName = string.Empty;
        /// <summary>
        /// 当前软件中拍照复判次数
        /// </summary>
        public int PhotoNum = 0;
        /// <summary>
        /// 当前软件中2D图片存储路径
        /// </summary>
        public string PhotoSavePath2D = string.Empty;
        /// <summary>
        /// 当前软件中3D图片存储路径
        /// </summary>
        public string PhotoSavePath3D = string.Empty;

        /// <summary>
        /// 结果图显示窗口
        /// </summary>
        private FormIndexImage frmIndexImage = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            Instance = this;
            Initialization();
            if ((Global.CurCheckType == CheckType.Raster || Global.CurCheckType == CheckType.ScreenLayer) &&
                Global.CurCheckCam == CheckCam.Cam2D)
            {
                AddForm(Global.FrmStandardAoi);
            }
        }

        /// <summary>
        /// 窗体启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            InitHardware();

        }

        /// <summary>
        /// 初始化的一些操作
        /// </summary>
        private void Initialization()
        {
            //读取配置文件中的当前机种
            this.currentRasterName = ConfigurationManager.AppSettings["CurrentRaster"];
            this.lblCurrentRasterType.Text = this.currentRasterName;
            //读取配置文件中的2D 3D图片存储路径
            this.PhotoSavePath2D = ConfigurationManager.AppSettings["ImgSavePath2D"];
            this.txtSave2DImgPath.Text = this.PhotoSavePath2D;
            this.PhotoSavePath3D = ConfigurationManager.AppSettings["ImgSavePath3D"];
            this.txtSave3DImgPath.Text = this.PhotoSavePath3D;

            string TestName = ConfigurationManager.AppSettings["CurrentTestType"];
            switch (TestName)
            {
                case "光栅":
                    Global.CurCheckType = CheckType.Raster;
                    break;
                case "分层":
                    Global.CurCheckType = CheckType.ScreenLayer;
                    break;
                case "金线":
                    Global.CurCheckType = CheckType.GoldWire;
                    break;
                default:
                    MessageBox.Show("当前配置文件中的检测类型有误，未读取正确的检测类型");
                    break;
            }
            //this.lblCheckType.Text = this.currentTestName;
            //this.cbbCheckType.Text = this.lblCheckType.Text;

            string CamNum = ConfigurationManager.AppSettings["CurrentCam"];
            switch (CamNum)
            {
                case "2D":
                    Global.CurCheckCam = CheckCam.Cam2D;
                    break;
                case "3D":
                    Global.CurCheckCam = CheckCam.Cam3D;
                    RecipeManager.Load();
                    break;
                default:
                    MessageBox.Show("当前配置文件中的相机有误，未读取正确的相机");
                    break;
            }

            string customer = ConfigurationManager.AppSettings["CurrentCustomer"];
            if (!string.IsNullOrEmpty(customer))
            {
                switch (customer)
                {
                    case "Geer":
                        Global.CurCustomerType = CustomerType.Geer;
                        break;
                    case "RiDong":
                        Global.CurCustomerType = CustomerType.RiDong;
                        break;
                    case "SE":
                        Global.CurCustomerType = CustomerType.SE;
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnCurrentRasterType_Click(object sender, EventArgs e)
        {
            ConfigurationHelper.Changeparameter(this.cbbSelectRasterType.Text, "CurrentRaster");  //切换机种的时候显示到日志中，并且通知到PLC切换的目标机种         
            this.currentRasterName = this.cbbSelectRasterType.Text;
            this.lblCurrentRasterType.Text = this.currentRasterName;
            //this.txtLog.AppendText(DateTime.Now.ToString() + "：当前机种切换成功-" + this.currentRasterName+System.Environment.NewLine);
            MessageBox.Show("机种切换成功，当前机种为：" + this.currentRasterName);

        }

        private void txtOnlyNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != 8 && e.KeyChar != 46 && e.KeyChar < 48) || e.KeyChar > 57)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 释放硬件连接
        /// </summary>
        private void UnInitHardware()
        {
            AoiCamera.UnInit();//Aio镜头
            PlcManager.UnInit();//PLC
            //PowerManager.UnInit();//光源控制器
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出应用程序吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                //释放硬件链接
                try
                {
                    // 释放硬件连接
                    UnInitHardware();
                }
                catch (Exception)
                { }
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 对textbox控件添加不同颜色的文本
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="str"></param>
        /// <param name="color"></param>
        public void AppendTxt(RichTextBox txt, string str, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<RichTextBox, string, Color>(AppendTxt), txt, str, color);
            }
            else
            {
                txt.SelectionColor = color;
                txt.AppendText(DateTime.Now.ToString("HH:mm:ss") + " " + str + System.Environment.NewLine);
            }
        }

        /// <summary>
        /// 对3Dtextbox控件添加不同颜色的文本
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="str"></param>
        /// <param name="color"></param>
        public void AppendTxt3DLog(string str, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, Color>(AppendTxt3DLog), str, color);
            }
            else
            {
                lstMsg3D.AddListBoxMsg(str, new SolidBrush(color));
                LogManager.GetLogger("3D").Info(str);
            }
        }

        /// <summary>
        /// 显示连接状态
        /// </summary>
        /// <param name="PLCOrCam"></param>
        /// <param name="status"></param>
        /// <param name="str"></param>
        public void ShowConnectInfo(string PLCOrCam, bool status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, bool>(ShowConnectInfo), PLCOrCam, status);
            }
            else
            {
                pnlStatus.UpdateDeviceState(PLCOrCam, PLCOrCam, status);
            }
        }


        #region 2D相机基础操作
        private void btnSearch2D_Click(object sender, EventArgs e)
        {
            StringBuilder Desc = new StringBuilder(64);
            if (!Camera2D.Search(Desc))
            {
                AppendTxt(this.txt2DLog, "没有搜索到2D相机设备，请检查硬件连接!", Color.Red);
            }
            else
            {
                AppendTxt(this.txt2DLog, "搜索到2D相机设备：" + Desc.ToString(), Color.Green);
                this.txtCamDesc2D.Text = Desc.ToString();
            }
        }
        private void btnConnect2D_Click(object sender, EventArgs e)
        {
            if (!Camera2D.Open())
            {
                AppendTxt(this.txt2DLog, "打开2D相机失败，请检查硬件连接!", Color.Red);
                return;
            }
            else
            {
                AppendTxt(this.txt2DLog, "打开2D相机成功！", Color.Green);
            }
        }
        private void btnDisconnect2D_Click(object sender, EventArgs e)
        {
            if (!Camera2D.Close())
            {
                AppendTxt(this.txt2DLog, "关闭2D相机失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt(this.txt2DLog, "关闭2D相机成功！", Color.Green);
            }
        }

        private void btnCollectContinuity2D_Click(object sender, EventArgs e)
        {
            if (!Camera2D.StartAcquire())
            {
                AppendTxt(this.txt2DLog, "2D相机连续采集失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt(this.txt2DLog, "2D相机正在连续采集!", Color.Green);
            }
        }
        private int ImgWidth = 0;
        private int ImgHeight = 0;
        private byte[] ImgData = null;
        private byte[] ImgDepthData = null;
        private void btnCollectOnce2D_Click(object sender, EventArgs e)
        {
            if (Camera2D.GetImageSize(ref ImgWidth, ref ImgHeight))
            {
                ImgData = new byte[ImgWidth * ImgHeight];
                AppendTxt(this.txt2DLog, "获取图片成功!尺寸宽度：" + ImgWidth + " 高度：" + ImgHeight, Color.Green);
                Camera2D.GetImage(ImgData);
            }
            else
            {
                AppendTxt(this.txt2DLog, "获取2D图片失败!", Color.Red);
                return;
            }
            Bitmap bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
            this.pictureBox2D.Image = bitmap;
            Camera2D.FreeImage();
        }

        private void btnStopCollect2D_Click(object sender, EventArgs e)
        {
            if (!Camera2D.StopAcquire())
            {
                AppendTxt(this.txt2DLog, "停止2D相机采图失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt(this.txt2DLog, "停止2D相机采图成功!", Color.Green);
            }
        }

        private void btnSetTime2D_Click(object sender, EventArgs e)
        {
            if (!Camera2D.SetExposureTime(long.Parse(this.txtExposureTime2D.Text)))
            {
                AppendTxt(this.txt2DLog, "设置2D相机曝光时间失败!", Color.Red);
            }
            else
            {
                AppendTxt(this.txt2DLog, "设置2D相机曝光时间成功：" + this.txtExposureTime2D.Text, Color.Green);
            }
        }

        private void btnGetTime2D_Click(object sender, EventArgs e)
        {
            long time = 0;
            if (!Camera2D.GetExposureTime(ref time))
            {
                AppendTxt(this.txt2DLog, "获取2D相机曝光时间失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt(this.txt2DLog, "获取2D相机曝光时间成功：" + time + "ms", Color.Green);
            }
            this.txtExposureTime2D.Text = time.ToString();
        }
        private void btnSet2DPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog OpenFile = new FolderBrowserDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                string path2D = OpenFile.SelectedPath;
                ConfigurationHelper.Changeparameter(path2D, "ImgSavePath2D");
                this.txtSave2DImgPath.Text = path2D;
                this.PhotoSavePath2D = path2D;

            }
        }
        #endregion

        /// <summary>
        /// 3D相机初始化搜索以及连接
        /// </summary>
        private void Init3DCam()
        {
            StringBuilder Desc = new StringBuilder(64);
            if (!Camera3D.Search(Desc))
            {
                ShowConnectInfo("3D相机", false);
                AppendTxt3DLog("没有搜索到3D相机设备，请检查硬件连接!", Color.Red);
                return;
            }
            else
            {
                this.txtCamDesc3D.Text = Desc.ToString();
                AppendTxt3DLog("3D相机已搜索到:" + Desc.ToString(), Color.Green);
            }
            if (!Camera3D.Open())
            {
                ShowConnectInfo("3D相机", false);
                AppendTxt3DLog("打开3D相机失败，请检查硬件连接!", Color.Red);
                return;
            }
            else
            {
                ShowConnectInfo("3D相机", true);
                AppendTxt3DLog("打开3D相机成功", Color.Green);
            }

            if (!Camera3D.InitConfig())
            {
                AppendTxt3DLog("设置3D相机深度计算参数失败", Color.Red);
            }
            else
            {
                AppendTxt3DLog("设置3D相机深度计算参数成功", Color.Green);
            }

            if (!Camera3D.SetExposureTime(Camera3D.Camera3DExposure))                                //初始化就要设置曝光时间，要设置在配置参数里
            {
                AppendTxt3DLog("设置3D相机曝光时间失败!", Color.Red);
            }
            else
            {
                AppendTxt3DLog("设置3D相机曝光时间成功：", Color.Green);
            }

            if (!Camera3D.StartAcquire())
            {
                ShowConnectInfo("3D相机", false);
                AppendTxt3DLog("3D相机开始采集失败!", Color.Red);
                return;
            }
            else
            {
                ShowConnectInfo("3D相机", true);
                AppendTxt3DLog("3D相机开始采集成功!", Color.Green);
            }
        }
        /// <summary>
        /// 2D相机初始化搜索以及连接
        /// </summary>
        private void Init2DCam()
        {
            StringBuilder Desc = new StringBuilder(64);
            if (!Camera2D.Search(Desc))
            {
                AppendTxt(this.txt2DLog, "没有搜索到2D相机设备，请检查硬件连接!", Color.Red);
            }
            else
            {
                this.txtCamDesc2D.Text = Desc.ToString();
                AppendTxt(this.txt2DLog, "2D相机已搜索到:" + Desc.ToString(), Color.Green);
            }

            if (!Camera2D.Open())
            {
                AppendTxt(this.txt2DLog, "打开2D相机失败，请检查硬件连接!", Color.Red);
                return;
            }
            else
            {
                AppendTxt(this.txt2DLog, "打开2D相机成功", Color.Green);
            }
            if (!Camera2D.SetExposureTime(8000))                                //初始化就要设置曝光时间，要设置在配置参数里
            {
                AppendTxt(this.txt2DLog, "设置2D相机曝光时间失败!", Color.Red);
            }
            else
            {
                AppendTxt(this.txt2DLog, "设置2D相机曝光时间成功：" + 8000 + "us", Color.Green);
            }
            if (!Camera2D.StartAcquire())
            {
                AppendTxt(this.txt2DLog, "2D相机开始采集失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt(this.txt2DLog, "2D相机开始采集成功!", Color.Green);
            }
        }

        #region 3D相机基础操作
        private void btnSearch3D_Click(object sender, EventArgs e)
        {
            StringBuilder Desc = new StringBuilder(64);
            if (!Camera3D.Search(Desc))
            {
                Desc = null;
                AppendTxt3DLog("没有搜索到3D相机设备，请检查硬件连接!", Color.Red);
            }
            else
            {
                this.txtCamDesc3D.Text = Desc.ToString();
                AppendTxt3DLog("搜索到3D相机设备：" + Desc.ToString(), Color.Green);
            }
        }

        private void btnConnect3D_Click(object sender, EventArgs e)
        {
            if (!Camera3D.Open())
            {
                AppendTxt3DLog("打开3D相机失败，请检查硬件连接!", Color.Red);
                this.btnDisconnect3D.Enabled = true;
                return;
            }
            else
            {
                AppendTxt3DLog("打开3D相机成功!", Color.Green);
            }
        }

        private void btnDisconnect3D_Click(object sender, EventArgs e)
        {
            if (!Camera3D.Close())
            {
                AppendTxt3DLog("关闭3D相机失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt3DLog("关闭3D相机成功!", Color.Green);
            }
        }

        private void btnCollectContinuity3D_Click(object sender, EventArgs e)
        {
            if (!Camera3D.StartAcquire())
            {
                AppendTxt3DLog("3D相机开始采集失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt3DLog("3D相机开始采集成功!", Color.Green);
            }
        }

        private void btnStopCollect3D_Click(object sender, EventArgs e)
        {
            if (!Camera3D.StopAcquire())
            {
                AppendTxt3DLog("停止3D相机采集失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt3DLog("停止3D相机采集成功!", Color.Green);
            }
        }

        private void btnSetTime3D_Click(object sender, EventArgs e)
        {
            if (!long.TryParse(this.txtExposureTime3D.Text, out Camera3D.Camera3DExposure))
            {
                MessageBox.Show("请输入正确数值！");
                return;
            }

            if (!Camera3D.SetExposureTime(Camera3D.Camera3DExposure))
            {
                AppendTxt3DLog("设置3D相机曝光时间失败!", Color.Red);
            }
            else
            {
                Camera3D.SaveConfig();
                AppendTxt3DLog("设置3D相机曝光时间成功：" + long.Parse(this.txtExposureTime3D.Text) + "us", Color.Green);
            }
        }

        private void btnGetTime3D_Click(object sender, EventArgs e)
        {
            long time = 0;
            if (!Camera3D.GetExposureTime(ref time))
            {
                AppendTxt3DLog("获取3D相机曝光时间失败!", Color.Red);
                return;
            }
            else
            {
                AppendTxt3DLog("获取3D相机曝光时间成功：" + time, Color.Green);
                this.txtExposureTime3D.Text = time.ToString();
            }
        }


        private int ImgBitDepth = 0;


        public void mem_copy(byte[] dst, int dst_offst, byte[] src, int src_offst)
        {
            for (int i = 0; i < src_offst; i++)
            {
                dst[i] = src[i];
            }

        }
        string dateInfo = string.Empty;

        public void ShowPicture(Bitmap img1, Bitmap img2)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Bitmap, Bitmap>(ShowPicture), img1, img2);
            }
            else
            {
                this.imageBox3D.Image = img1;
                this.imageBoxDepth.Image = img2;
            }
        }


        private void btnCollectOnce3D_Click(object sender, EventArgs e)
        {
            if (this.cbPhotoNum.SelectedIndex < 0)
            {
                MessageBox.Show("当前未确定第几次拍照");
                return;
            }
            if (this.cbPhotoNum.SelectedIndex == 0)
            {
                dateInfo = "D:\\3DImageInfo\\" + DateTime.Now.ToString("HHmmss");
                System.IO.Directory.CreateDirectory(dateInfo);
                System.IO.Directory.CreateDirectory(dateInfo + "\\Img3D");
            }

            if (Camera3D.GetImageSize(ref ImgWidth, ref ImgHeight))
            {
                AppendTxt3DLog("获取图片成功!尺寸宽度：" + ImgWidth + " 高度：" + ImgHeight, Color.Green);
                //ImgData = new byte[ImgWidth * ImgHeight];
                //ImgDepthData = new byte[ImgWidth * ImgHeight * 3];
                //Camera3D.GetImage(ImgData, ImgDepthData);

                ImgData = new byte[ImgWidth * ImgHeight];
                Camera3D.GetRealImage(ImgData);

                // 光栅检测深度计算参数
                _Variables vars = new _Variables()
                {
                    level = 1,
                    maxDepth = -30,
                    minDepth = 30,
                    texture = 3,
                    smoothWindow = 1,
                    detection = 1,
                    BORW = 0,
                    thre = 0.3F,
                };

                _VOMMAImage vImage = new _VOMMAImage();
                vImage.imageData = new byte[ImgWidth * ImgHeight * 3];
                //mem_copy(ImgData, ImgData.Length, vImage.imageData, vImage.imageData.Length, ImgData.Length + 1);
                vImage.imageData = ImgData;
                vImage.rows = ImgHeight;
                vImage.cols = ImgWidth;
                switch (ImgBitDepth)
                {
                    case 8: vImage.imageType = _ImageType.VOMMA_8UC3; break;
                    case 16: vImage.imageType = _ImageType.VOMMA_16UC3; break;
                    case 32: vImage.imageType = _ImageType.VOMMA_32FC3; break;
                    default: vImage.imageType = _ImageType.VOMMA_8UC3; break;
                }
                byte[] depthPath = Encoding.Default.GetBytes("D:\\1\\depth.txt");   //生成的 depth.txt 所要保存路径   后面可以通过配置文件更改
                byte[] centerPath = Encoding.Default.GetBytes("D:\\1\\center.txt");  //生成的 center.txt 所要保存路径  后面可以通过配置文件更改
                Camera3D.GetImage(vars, ref vImage, depthPath, centerPath);
                //mem_copy(ImgDepthData, ImgDepthData.Length, vImage.imageData, vImage.imageData.Length, ImgDepthData.Length + 1);
            }
            else
            {
                AppendTxt3DLog("获取3D图片失败!", Color.Red);
                return;
            }

            Bitmap bitmap = null;
            Bitmap bitmapdepth = null;
            switch (ImgBitDepth)
            {
                case 8:
                    bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
                    bitmapdepth = BitmapConverter.ArrayTo32RgbBitmap(ImgDepthData, ImgHeight, ImgWidth, true);
                    break;
                case 16:
                    bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
                    bitmapdepth = BitmapConverter.ArrayTo32RgbBitmap(ImgDepthData, ImgHeight, ImgWidth, true);
                    break;
                case 32:
                    bitmap = BitmapConverter.ArrayTo32RgbBitmap(ImgData, ImgHeight, ImgWidth, true);
                    bitmapdepth = BitmapConverter.ArrayTo32RgbBitmap(ImgDepthData, ImgHeight, ImgWidth, true);
                    break;
                default:
                    bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
                    bitmapdepth = BitmapConverter.ArrayTo32RgbBitmap(ImgDepthData, ImgHeight, ImgWidth, true);
                    break;
            }
            this.imageBox3D.Image = bitmap;
            this.imageBoxDepth.Image = bitmapdepth;
            bitmap.Save(dateInfo + "\\Img3D\\Picture" + this.cbPhotoNum.Text + ".jpg");
            bitmapdepth.Save(dateInfo + "\\Img3D\\depthPicture" + this.cbPhotoNum.Text + ".jpg");
            File.Copy(Environment.CurrentDirectory + "\\Param\\depth.txt", dateInfo + "\\depth" + this.cbPhotoNum.Text + ".txt", true);
            File.Copy(Environment.CurrentDirectory + "\\Param\\center.txt", dateInfo + "\\center" + this.cbPhotoNum.Text + ".txt", true);
        }

        private void btnGrab_Click(object sender, EventArgs e)
        {
            if (Camera3D.GetImageSize(ref ImgWidth, ref ImgHeight))
            {
                AppendTxt3DLog("获取图片成功!尺寸宽度：" + ImgWidth + " 高度：" + ImgHeight, Color.Green);

                ImgData = new byte[ImgWidth * ImgHeight];
                Camera3D.GetRealImage(ImgData);

                Bitmap bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
                this.imageBox3D.Image = bitmap;
            }
            else
            {
                AppendTxt3DLog("获取3D图片失败!", Color.Red);
                return;
            }
        }

        private void btnSet3DPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog OpenFile = new FolderBrowserDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                string path3D = OpenFile.SelectedPath;
                ConfigurationHelper.Changeparameter(path3D, "ImgSavePath3D");
                this.txtSave3DImgPath.Text = path3D;
                this.PhotoSavePath3D = path3D;
            }
        }
        #endregion


        private void btnMread_Click(object sender, EventArgs e)
        {
            bool[] dataReadBits = PlcManager.PlcCtrl.ReadBits("M" + this.txtMAddress.Text, 1);
            if (dataReadBits[0] == true)
            {
                this.comboBox1.SelectedIndex = 0;
            }
            else
            {
                this.comboBox1.SelectedIndex = 1;
            }
        }

        private void btnMWrite_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == 0)
            {
                PlcManager.PlcCtrl.WriteBits("M" + this.txtMAddress.Text, true);
            }
            else
            {
                PlcManager.PlcCtrl.WriteBits("M" + this.txtMAddress.Text, false);
            }

        }

        private void btnDread_Click(object sender, EventArgs e)
        {
            ushort[] datas = PlcManager.PlcCtrl.ReadWords("D" + this.txtDAddress.Text, 1);
            this.txtDValue.Text = datas[0].ToString();
        }

        private void btnDWrite_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {

            }
            else
            {
                PlcManager.PlcCtrl.WriteWords("D" + this.txtDAddress.Text, Convert.ToUInt16(this.txtDValue.Text));
            }

        }

        /// <summary>
        /// 给产品添加提示信息
        /// </summary>
        /// <param name="ErrorInfo"></param>
        /// <param name="backcolor"></param>
        /// <param name="showInfo"></param>
        public void ShowCheckInfo(string ErrorInfo, Color backcolor, string showInfo)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, Color, string>(ShowCheckInfo), ErrorInfo, backcolor, showInfo);
            }
            else
            {
                this.lbl3Dinfo.BackColor = backcolor;
                this.lbl3Dinfo.Text = ErrorInfo;
                this.lblErrorInfo.Text = showInfo;
            }
        }

        private void BtnCheckByHand3D_Click(object sender, EventArgs e)
        {
            if (Global.CurCheckType == CheckType.Raster)
            {
                string ProductdateInfo = "D:\\3DImageInfo\\" + DateTime.Now.ToString("yyyyMMddHHmmss");

                dateInfo = ProductdateInfo + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
                System.IO.Directory.CreateDirectory(ProductdateInfo);


                string strTXTDir = dateInfo;
                string strMarkPath = Environment.CurrentDirectory + "\\FileRaster3D\\Mark\\mark.txt";
                string strCaliPath = Environment.CurrentDirectory + "\\FileRaster3D\\Cali\\cali.txt";
                string strOutDir = dateInfo + "\\Img3D";
                System.IO.Directory.CreateDirectory(strOutDir);

                int[] iParams = new int[3];
                iParams[0] = ImgWidth;
                iParams[1] = ImgHeight;
                iParams[2] = 1;
                float[] fParams = new float[3];
                fParams[0] = 100;
                byte[] SzResult = new byte[1024];
                FormMain.Instance.AppendTxt3DLog($"3D光栅检测开始!", Color.Black);
                int ret = Cam3DRaster.GetDetectResult(dateInfo, "", strMarkPath, strCaliPath, strOutDir, iParams, fParams, SzResult);
                FormMain.Instance.AppendTxt3DLog($"3D光栅检测结束 返回值{ret}!", Color.Black);
                if (ret == 0)
                {
                    string str = string.Empty;
                    for (int i = 0; i < 6; i++)
                    {
                        str += SzResult[i].ToString();
                    }
                    if (str.Contains("1"))
                    {
                        string errorInfo = string.Empty;
                        this.lbl3Dinfo.BackColor = Color.Red;
                        this.lbl3Dinfo.Text = "NG";
                        for (int i = 1; i <= str.Length; i++)
                        {
                            if (str[i - 1] == '1')
                            {
                                errorInfo += i + ",";
                            }
                        }
                        this.lblErrorInfo.Text = "当前位置缺陷层为：第 " + errorInfo.Substring(0, errorInfo.Length - 1) + " 层";
                    }
                    else
                    {
                        this.lbl3Dinfo.BackColor = Color.Green;
                        this.lbl3Dinfo.Text = "OK";
                        this.lblErrorInfo.Text = "当前位置检测无缺陷";
                    }
                }
                else
                {
                    AppendTxt3DLog("3D光栅检测失败!", Color.Red);
                }
            }
            else if (Global.CurCheckType == CheckType.ScreenLayer)
            {

            }
            else if (Global.CurCheckType == CheckType.GoldWire)
            {

            }
        }

        /// <summary>
        /// 相机标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCamCalibration_Click(object sender, EventArgs e)
        {
            if (this.cbPhotoNum.SelectedIndex < 0)
            {
                MessageBox.Show("当前未确定第几次拍照");
                return;
            }
            if (this.cbPhotoNum.SelectedIndex == 0)
            {
                dateInfo = "D:\\3DImageInfo\\" + DateTime.Now.ToString("HHmmss");
                System.IO.Directory.CreateDirectory(dateInfo);
                System.IO.Directory.CreateDirectory(dateInfo + "\\Img3D");
            }

            if (Camera3D.GetImageSize(ref ImgWidth, ref ImgHeight))
            {
                AppendTxt3DLog("获取图片成功!尺寸宽度：" + ImgWidth + " 高度：" + ImgHeight, Color.Green);

                ImgData = new byte[ImgWidth * ImgHeight];
                Camera3D.GetRealImage(ImgData);

                string strCaliPath = Environment.CurrentDirectory + "\\FileRaster3D\\Cali\\cali.txt";
                float fPos_X = 0;
                float fPos_Y = 0;
                Cam3DRaster.VOMMADetectCornerMark(strCaliPath, ImgData, ImgHeight, ImgWidth, ref fPos_X, ref fPos_Y);

                this.txtCaliResult.Text = $"X={fPos_X},Y={fPos_Y}";
            }
            else
            {
                AppendTxt3DLog("获取3D图片失败!", Color.Red);
                return;
            }

            Bitmap bitmap = null;
            switch (ImgBitDepth)
            {
                case 8:
                    bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
                    break;
                case 16:
                    bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
                    break;
                case 32:
                    bitmap = BitmapConverter.ArrayTo32RgbBitmap(ImgData, ImgHeight, ImgWidth, true);
                    break;
                default:
                    bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
                    break;
            }
            this.imageBox3D.Image = bitmap;
            bitmap.Save(dateInfo + "\\Img3D\\Picture" + this.cbPhotoNum.Text + ".jpg");
            File.Copy(Environment.CurrentDirectory + "\\Param\\depth.txt", dateInfo + "\\depth" + this.cbPhotoNum.Text + ".txt", true);
        }

        private void BtnDepthPara_Click(object sender, EventArgs e)
        {
            var fm = new FormVommaCameraSetting();
            fm.Show();
        }

        /// <summary>
        /// 检测参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRasterPara_Click(object sender, EventArgs e)
        {
            FormRasterPara frm = new FormRasterPara();
            frm.ShowDialog(this);
        }

        /// <summary>
        /// 显示结果图
        /// </summary>
        /// <param name="img"></param>
        public void ShowResultImage(Image img)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Image>(ShowResultImage), img);
            }
            else
            {
                if (frmIndexImage == null)
                {
                    frmIndexImage = new FormIndexImage();
                }
                frmIndexImage.Show();
                frmIndexImage.ShowImage(img);
            }
        }

        /// <summary>
        /// 隐藏结果图
        /// </summary>
        public void HideResultImage()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(HideResultImage));
            }
            else
            {
                if (frmIndexImage != null)
                {
                    frmIndexImage.Hide();
                }
            }
        }

        /// <summary>
        /// BitMap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BitMap_Set_Click(object sender, EventArgs e)
        {
            FormBitMap frm = new FormBitMap();
            frm.ShowDialog(this);
        }

        /// <summary>
        /// 层数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Work3DLayer_Set_Click(object sender, EventArgs e)
        {
            if (Global.CurCustomerType== CustomerType.SE)
            {
                FormSESet frm = new FormSESet();
                frm.ShowDialog(this);
            }
            else if (Global.CurCustomerType==CustomerType.RiDong)
            {
                FormRiDong_Set frm = new FormRiDong_Set();
                frm.ShowDialog(this);
            }
            
        }
    }
}
