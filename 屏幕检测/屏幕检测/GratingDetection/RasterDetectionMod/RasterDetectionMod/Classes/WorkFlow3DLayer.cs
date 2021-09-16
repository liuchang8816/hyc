/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：WorkFlow3DLayer.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/23 19:10:27 
// 功能描述 ： 
// 使用说明 ： 
//
//
// 创建标识： 
//
// 修改标识： 
// 修改描述： 
//
// 修改标识： 
// 修改描述： 
//----------------------------------------------------------------*/
using HYC.HTFile.HTXML;
using HYC.HTImage;
using HYC.HTLog;
using RasterDetectionMod.Entities;
using RasterDetectionMod.Models;
using RasterDetectionMod.Recipes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.Classes
{
    /// <summary>
    /// 3D分层检测流程 -日东
    /// </summary>
    public class WorkFlow3DLayer : WorkFlow
    {
        /// <summary>
        /// 检测结果
        /// </summary>
        public RasterResultModel ResultModel = new RasterResultModel();

        /// <summary>
        /// Mark点的序号
        /// </summary>
        public int RasterMarkNum = 0;
        /// <summary>
        /// 图像宽度
        /// </summary>
        public int ImgWidth = 0;
        /// <summary>
        /// 图像高度
        /// </summary>
        public int ImgHeight = 0;
        /// <summary>
        /// 图像深度
        /// </summary>
        public int ImgBitDepth = 0;
        /// <summary>
        /// 图像数据
        /// </summary>
        public byte[] ImgData = null;
        /// <summary>
        /// 深度图图像数据
        /// </summary>
        public byte[] ImgDepthData = null;
        /// <summary>
        /// 当前自动检测每个缺陷创建的路径目录
        /// </summary>
        public string dateInfo = string.Empty;
        /// <summary>
        /// 当前自动检测每个产品的主目录，每个缺陷都在此路径下创建文件夹
        /// </summary>
        public string ProductdateInfo = string.Empty;

        /// <summary>
        /// 用于测试
        /// </summary>
        public string ProductdateInfo2 = string.Empty;

        /// <summary>
        /// 所有检测的不良信息
        /// </summary>
        private List<DefectList3D> lstAllDefects = new List<DefectList3D>();


        /// <summary>
        /// Mark图像的存储路径
        /// </summary>
        public string StrMarkDir = string.Empty;

        /// <summary>
        /// depthtxt保存路径
        /// </summary>
        public string PathDepthTxt = ".\\Param\\depth.txt";
        public string PathCenterTxt = ".\\Param\\center.txt";


        /// <summary>
        /// 初始化，创建产品文件夹
        /// </summary>
        public override void Init()
        {
            ResultModel.PanelID = "PNL" + DateTime.Now.ToString("yyyyMMddHHmmss");
            ResultModel.StartTime = DateTime.Now;

            //创建文件夹：3DImageInfo，文件夹位置在D根目录下，存储数据
            ProductdateInfo = "D:\\3DImageInfo\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + DateTime.Now.ToString("HHmmss");
            Directory.CreateDirectory(ProductdateInfo);
            StrMarkDir = ProductdateInfo + "\\Mark";
            Directory.CreateDirectory(StrMarkDir);
            Directory.CreateDirectory(StrMarkDir + "\\Img3D");
            PlcManager.PlcCtrl.WriteBits(PlcAddr.PanelTestStart3DAck, true);  //创建文件夹完成后，回复PLC可以开始后面的拍照请求了
        }

        /// <summary>
        /// 创建缺陷文件夹
        /// </summary>
        public override void CreateFolder()
        {
            dateInfo = ProductdateInfo + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss"); //不是第一个缺陷只创建子文件夹
            Directory.CreateDirectory(dateInfo);
            Directory.CreateDirectory(dateInfo + "\\Img3D");
            PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera3DPhotoStartOK, true);  //创建文件夹完成后，回复PLC可以开始后面的拍照请求了
        }

        /// <summary>
        /// 3D自动拍照
        /// </summary>
        public override void GrabMark()
        {
            RasterMarkNum++;
            if (Camera3D.GetImageSize(ref ImgWidth, ref ImgHeight))
            {
                FormMain.Instance.AppendTxt3DLog("获取图片成功!尺寸宽度：" + ImgWidth + " 高度：" + ImgHeight, Color.Green);
                ImgData = new byte[ImgWidth * ImgHeight];
                ImgDepthData = new byte[ImgWidth * ImgHeight * 3];
                Camera3D.SetExposureTime(Camera3D.Camera3DExposureMark);
                Camera3D.GetImage(ImgData, Encoding.Default.GetBytes(PathDepthTxt), ImgDepthData);
                Camera3D.SetExposureTime(Camera3D.Camera3DExposure);
            }
            else
            {
                FormMain.Instance.AppendTxt3DLog("获取3D图片失败!", Color.Red);
                PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera3DMarkGrabNG, true);
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
            bitmap.Save(StrMarkDir + "\\Img3D\\Picture" + RasterMarkNum + ".jpg");
            bitmapdepth.Save(StrMarkDir + "\\Img3D\\depthPicture" + RasterMarkNum + ".jpg");
            FormMain.Instance.ShowPicture(bitmap, bitmapdepth);

            File.Copy(Environment.CurrentDirectory + PathDepthTxt, StrMarkDir + "\\depth_mark" + RasterMarkNum + ".txt", true);
            File.Copy(Environment.CurrentDirectory + PathCenterTxt, StrMarkDir + "\\center_mark" + RasterMarkNum + ".txt", true);

            PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera3DMarkGrabOK, true);
        }

        /// <summary>
        /// 3D自动拍照
        /// </summary>
        public override void Grab()
        {
            PhotoNum++;
            if (Camera3D.GetImageSize(ref ImgWidth, ref ImgHeight))
            {
                FormMain.Instance.AppendTxt3DLog("获取图片成功!尺寸宽度：" + ImgWidth + " 高度：" + ImgHeight, Color.Green);
                ImgData = new byte[ImgWidth * ImgHeight];
                ImgDepthData = new byte[ImgWidth * ImgHeight * 3];
                Camera3D.GetImage(ImgData, Encoding.Default.GetBytes(PathDepthTxt), ImgDepthData);
            }
            else
            {
                FormMain.Instance.AppendTxt3DLog("获取3D图片失败!", Color.Red);
                PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera3DResultNG, true);   //通知PLC3D拍照NG
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
                    //bitmapdepth = BitmapConverter.ArrayTo32RgbBitmap(ImgDepthData, ImgHeight, ImgWidth, true);
                    break;
                default:
                    bitmap = BitmapConverter.ArrayToBitmap(ImgData, ImgHeight, ImgWidth);
                    bitmapdepth = BitmapConverter.ArrayTo32RgbBitmap(ImgDepthData, ImgHeight, ImgWidth, true);
                    break;
            }
            bitmap.Save(dateInfo + "\\Img3D\\Picture" + PhotoNum + ".bmp", ImageFormat.Bmp);
            bitmapdepth.Save(dateInfo + "\\Img3D\\depthPicture" + PhotoNum + ".bmp", ImageFormat.Bmp);
            FormMain.Instance.ShowPicture(bitmap, bitmapdepth);

            File.Copy(Environment.CurrentDirectory + PathDepthTxt, dateInfo + "\\depth" + PhotoNum + ".txt", true);
            File.Copy(Environment.CurrentDirectory + PathCenterTxt, dateInfo + "\\center" + PhotoNum + ".txt", true);

            PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera3DResultOK, true);   //通知PLC3D拍照OK
        }

        /// <summary>
        /// 调用3D光栅检测算法（6层）
        /// </summary>
        public override void CheckPhoto()
        {
            var layerPara = RecipeManager.LayerAlgorithmPara;

            string strDepthPath = dateInfo + "\\depth" + PhotoNum + ".txt";
            string strCenterPath = dateInfo + "\\center" + PhotoNum + ".txt";
            string strCaliPath = Environment.CurrentDirectory + "\\FileRaster3D\\Cali\\cali.txt";
            string strOutDir = dateInfo + "\\Img3D";
            byte[] SzResult = new byte[10240];
            int[] iParams = new int[50];
            iParams[0] = ImgWidth;
            iParams[1] = ImgHeight;
            iParams[2] = layerPara.LayerCount;
            iParams[3] = SzResult.Length;
            iParams[4] = (int)Camera3D.ThreArea;
            if (Global.CurCustomerType == CustomerType.SE)
            {
                iParams[5] = 0;//产品ID: 0表示：车载屏幕, 1表示：薄膜
            }
            else if (Global.CurCustomerType == CustomerType.RiDong)
            {
                iParams[5] = 1;//产品ID: 0表示：车载屏幕, 1表示：薄膜
            }

            float[] fParams = layerPara.ListLayerHeight.ToArray();
            FormMain.Instance.AppendTxt3DLog($"3D分层检测开始!", Color.Black);
            int ret = Cam3Dlayered.GetDetectResult(strDepthPath, strCenterPath, strCaliPath, strOutDir, iParams, fParams, SzResult);
            FormMain.Instance.AppendTxt3DLog($"3D分层检测返回值{ret}!", Color.Black);
            if (ret == 0)
            {
                string errorInfo = Encoding.Default.GetString(SzResult).Trim(' ', '\0');
                LogManager.GetLogger("Result").Info(errorInfo);

                DefectList3D defectList = new DefectList3D();
                defectList.ConvertErrorInfo2(errorInfo, OffsetX, OffsetY);
                lstAllDefects.Add(defectList);

                string strDefectList3D = HTXMLSerializer.EntityToXMLString(defectList);
                LogManager.GetLogger("Result").Info(strDefectList3D);
            }
            else
            {
                FormMain.Instance.AppendTxt3DLog("3D分层检测失败!", Color.Red);
            }
            PlcManager.PlcCtrl.WriteBits(PlcAddr.IsCheckErrorOK, true);
        }

        /// <summary>
        /// 结果整合、去重
        /// </summary>
        public override void GetResult()
        {
            DefectList3D total = new DefectList3D();
            foreach (var item in lstAllDefects)
            {
                foreach (var layer in item.ListLayers)
                {
                    var totalLayer = total.ListLayers.Where(p => p.Layer == layer.Layer).FirstOrDefault();
                    if (totalLayer != null)
                    {
                        //  defect:缺陷信息集合
                        foreach (var defect in layer.ListDefects)
                        {
                            //判断是否包含重复区域的不良
                            if (!totalLayer.IsContain(defect))
                            {
                                totalLayer.ListDefects.Add(defect);
                                totalLayer.Count = totalLayer.ListDefects.Count;
                            }
                            else
                            {
                                Console.WriteLine(defect);
                            }
                        }
                    }
                    else
                    {
                        total.ListLayers.Add(layer);
                    }
                }
            }
            string str = HTXMLSerializer.EntityToXMLString(total);
            LogManager.GetLogger("Result").Info("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");
            LogManager.GetLogger("Result").Info(str);
            LogManager.GetLogger("Result").Info("---------------------------------------------------------\r\n");

            // 记录检测结束时间
            ResultModel.EndTime = DateTime.Now;

            //获取不良数量
            int defectCount = 0;
            foreach (var layer in total.ListLayers)
            {
                defectCount += layer.Count;
            }
            ResultModel.DefectCount = defectCount;

            // 结果判定
            ResultJudge(total);
            // 写csv文件
            ResultModel.WriteToFile();
            // 生成示意图
            GenerateIndexImage(total);
        }

        /// <summary>
        /// 结果判定
        /// </summary>
        /// <param name="total"></param>
        private void ResultJudge(DefectList3D total)
        {
            var para = RecipeManager.RasterJudgePara;
            bool isOK = true;
            string reason = string.Empty;
            if (ResultModel.DefectCount > para.DefectCount)
            {
                reason = $"Defect Count {ResultModel.DefectCount} > {para.DefectCount}";
                isOK = false;
            }
            else
            {
                foreach (var layer in total.ListLayers)
                {
                    foreach (var defect in layer.ListDefects)
                    {
                        if (defect.Area > para.DefectSize)
                        {
                            reason = $"Defect Size {defect.Area} > {para.DefectSize}";
                            isOK = false;
                            break;
                        }
                        double w = defect.Width / 1000.0;
                        if (w > para.DefectWidth)
                        {
                            reason = $"Defect Width {w} > {para.DefectWidth}";
                            isOK = false;
                            break;
                        }
                        double h = defect.Height / 1000.0;
                        if (h > para.DefectWidth)
                        {
                            reason = $"Defect Width {h} > {para.DefectWidth}";
                            isOK = false;
                            break;
                        }
                    }
                    if (!isOK)
                    {
                        break;
                    }
                }
            }
            ResultModel.Result = isOK ? "OK" : "NG";
            ResultModel.Reason = reason;
            if (isOK)
            {
                FormMain.Instance.ShowCheckInfo("OK", Color.YellowGreen, "");
            }
            else
            {
                FormMain.Instance.ShowCheckInfo("NG", Color.Red, reason);
            }
        }

        private static bool IsShielded_0 = Convert.ToBoolean(Work3DLayer_Set.IsShielded_01);
        private static bool IsShielded_1 = Convert.ToBoolean(Work3DLayer_Set.IsShielded_11);
        private static bool IsShielded_2 = Convert.ToBoolean(Work3DLayer_Set.IsShielded_21);
        private static bool IsShielded_3 = Convert.ToBoolean(Work3DLayer_Set.IsShielded_31);
        private static bool IsShielded_4 = Convert.ToBoolean(Work3DLayer_Set.IsShielded_41);

        /// <summary>
        /// 生成示意图
        /// </summary>
        /// <param name="total"></param>
        private void GenerateIndexImage(DefectList3D total)
        {
            try
            {
                Bitmap_Set.Load();//加载配置文件
                Work3DLayer_Set.Load();
                int width = Convert.ToInt32(Bitmap_Set.Width);
                int height = Convert.ToInt32(Bitmap_Set.Height);
                Bitmap bitmap = new Bitmap(width, height);// 日东屏幕200*100mm
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    // 绘制边框
                    g.DrawRectangle(Pens.Black, new Rectangle(0, 0, bitmap.Width - 1, bitmap.Height - 1));

                    // 绘制图例
                    if (Global.CurCustomerType == CustomerType.SE)
                    {
                        g.FillRectangle(Brushes.Red, new Rectangle(10, 10, 20, 20));
                        g.DrawString("层一：上表面", new Font("微软雅黑", 10.5f), Brushes.Gray, new Point(35, 10));
                        g.FillRectangle(Brushes.Yellow, new Rectangle(10, 35, 20, 20));
                        g.DrawString("层二：上偏夹层", new Font("微软雅黑", 10.5f), Brushes.Gray, new Point(35, 35));
                        g.FillRectangle(Brushes.Blue, new Rectangle(10, 60, 20, 20));
                        g.DrawString("层三：液晶层", new Font("微软雅黑", 10.5f), Brushes.Gray, new Point(35, 60));
                        g.FillRectangle(Brushes.Gray, new Rectangle(10, 85, 20, 20));
                        g.DrawString("层四：下偏夹层", new Font("微软雅黑", 10.5f), Brushes.Gray, new Point(35, 85));
                        g.FillRectangle(Brushes.Black, new Rectangle(10, 110, 20, 20));
                        g.DrawString("层五：下表面", new Font("微软雅黑", 10.5f), Brushes.Gray, new Point(35, 110));
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Black, new Rectangle(10, 10, 20, 20));
                        g.DrawString("层一：SPV层", new Font("微软雅黑", 10.5f), Brushes.Gray, new Point(35, 10));
                        g.FillRectangle(Brushes.Gray, new Rectangle(10, 35, 20, 20));
                        g.DrawString("层二：偏光层", new Font("微软雅黑", 10.5f), Brushes.Gray, new Point(35, 35));
                        g.FillRectangle(Brushes.Blue, new Rectangle(10, 60, 20, 20));
                        g.DrawString("层三：SEPA层", new Font("微软雅黑", 10.5f), Brushes.Gray, new Point(35, 60));
                    }

                    foreach (var layer in total.ListLayers)//遍历缺陷层集合
                    {
                        foreach (var defect in layer.ListDefects)//遍历对应层的缺陷信息缺陷
                        {
                            //X,Y毫米
                            int x = (int)(defect.WorldX * 10);
                            int y = (int)(defect.WorldY * 10);
                            //W,H:微米  
                            //int w = (int)(defect.Width / 1000 * 100);
                            //int h = (int)(defect.Height / 1000 * 100);
                            //目前固定宽，高，模拟绘画，后期需要根据实际的坐标参数，绘画出缺陷。
                            int w = 20;
                            int h = 20;

                            if (Global.CurCustomerType == CustomerType.SE)
                            {
                                if (layer.Layer == 0 && Work3DLayer_Set.IsShielded_01 == true)
                                    g.FillRectangle(Brushes.Black, new Rectangle(x, y, w, h));
                                else if (layer.Layer == 1 && Work3DLayer_Set.IsShielded_11 == true)
                                    g.FillRectangle(Brushes.Gray, new Rectangle(x, y, w, h));
                                else if (layer.Layer == 2 && Work3DLayer_Set.IsShielded_21 == true)
                                    g.FillRectangle(Brushes.Blue, new Rectangle(x, y, w, h));
                                else if (layer.Layer == 3 && Work3DLayer_Set.IsShielded_31 == true)
                                    g.FillRectangle(Brushes.Yellow, new Rectangle(x, y, w, h));
                                else if (layer.Layer == 4 && Work3DLayer_Set.IsShielded_41 == true)
                                    g.FillRectangle(Brushes.Red, new Rectangle(x, y, w, h));
                            }
                            else if (Global.CurCustomerType == CustomerType.RiDong)
                            {
                                if (layer.Layer == 0 && Work3DLayer_Set.IsShielded_01 == true)                               
                                {
                                    //分层缺陷数量记录在Log文件夹下的"SPV层"文件夹
                                    LogManager.GetLogger("SPV层").Error(defect.ToString());
                                    g.FillRectangle(Brushes.Black, new Rectangle(x, y, w, h));
                                }
                                else if (layer.Layer == 1 && Work3DLayer_Set.IsShielded_11 == true)
                                {
                                    g.FillRectangle(Brushes.Gray, new Rectangle(x, y, w, h));
                                    LogManager.GetLogger("偏光层").Error(defect.ToString());
                                }
                                else if (layer.Layer == 2 && Work3DLayer_Set.IsShielded_21 == true)
                                {
                                    g.FillRectangle(Brushes.Blue, new Rectangle(x, y, w, h));
                                    LogManager.GetLogger("SEPA层").Error(defect.ToString());
                                }
                            }

                            // 绘制深度
                            if (Global.CurCustomerType == CustomerType.SE)
                            {
                                g.DrawString(defect.Depth.ToString(), new Font("微软雅黑", 7.5f),
                                    Brushes.Gray, new Point(x + 25, y));
                            }
                        }
                    }
                    string filename = Path.Combine(ProductdateInfo, "Result.jpg");
                    bitmap.Save(filename, ImageFormat.Jpeg);

                    // 显示结果图
                    FormMain.Instance.ShowResultImage((Bitmap)bitmap.Clone());

                    bitmap.Dispose();
                }
            }
            catch (Exception ex)
            {
                //异常信息记录在Log文件夹下的Error文件夹
                LogManager.GetLogger("Error").Error(ex);
            }
        }
    }
}
