using HYC.StandardAoi;
using HYC.StandardAoi.Events;
using HYC.StandardAoi.Parameter;
using HYC.StandardAoi.UI.View;
using RasterDetectionMod.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod.View
{
    public partial class ViewMain : ViewBaseEx
    {
        //// 显示控件列表
        private List<UcDetectionGroupEx> lst = new List<UcDetectionGroupEx>();
        private StandardAoiManager[] managers = new StandardAoiManager[2];
        public ViewMain()
        {
            InitializeComponent();
            lst.Clear();
            lst.Add(this.ucDetectionGroupEx1);

            // 监听事件
            for (int i = 0; i < 2; i++)
            {
                managers[i] = Global.FrmStandardAoi.GetManager(i);
                if (managers[i] != null)
                {
                    managers[i].DetectionStarted += Manager_DetectionStarted;
                    managers[i].DetectionCompleted += Manager_DetectionCompleted;
                    managers[i].ImageReceived += Manager_ImageReceived;
                    managers[i].ProgressChanged += Manager_ProgressChanged;
                }
            }
        }

        /// <summary>
        /// 检测开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_DetectionStarted(object sender, DetectionStartedArgs e)
        {
            if (e.IsImageTestMode || e.IsManualMode) return;

            var ctrl = GetUcDetectionGroup(e.DetectionType, e.Channel);
            ctrl.ShowPanelID(e.PanelID);
        }
        /// <summary>
        /// 获取指定的控件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public UcDetectionGroupEx GetUcDetectionGroup(DetectionType type, int channel)
        {
            var lstTmp = lst.Where(p => p.DetectionType == type).ToList();
            return lstTmp[channel];
        }
        /// <summary>
        /// 检测完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_DetectionCompleted(object sender, DetectionCompletedArgs e)
        {
            if (e.IsImageTestMode || e.IsManualMode) return;

            // 显示结果
            var ctrl = GetUcDetectionGroup(e.DetectionType, e.Channel);
            int index = e.Channel / 2;

            // 显示算法处理后的图片
            if (index == 0 && e.DetectionType == DetectionType.Aoi)
            {
                ctrl.ShowProcessedImages(e.Result);
                // 显示检测结果
                ctrl.ShowDetectionResult(e.Result);
            }
            else
            {
                ctrl.ShowProcessedImages(e.Result);
                // 显示检测结果
                ctrl.ShowDetectionResult(e.Result);
            }
        }
        /// <summary>
        /// 接收到图像数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_ImageReceived(object sender, ImageReceivedArgs e)
        {
            if (e.IsImageTestMode || e.IsManualMode) return;

            var setting = Global.FrmStandardAoi.GetSetting();
            if (setting.IsShowOriginalImages)
            {
                if (e.Channel / 2 == 0 && e.DetectionType == DetectionType.Aoi)//通道为0时,并且拍照为Aoi相机时,旋转180
                {
                    var bmp = e.ImageData.ToBitmap();
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    e.ImageData = new ImageData();
                    e.ImageData.FromBitmap(bmp, false);
                }
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        this.Invoke(new Action(() =>
                        {
                            var ctrl = GetUcDetectionGroup(e.DetectionType, e.Channel);
                            ctrl.ImageBox.ShowImage(e.PatternName, e.ImageData.ToBitmap());
                        }));
                    }
                    catch { }
                });
            }
        }
        /// <summary>
        /// 检测进度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_ProgressChanged(object sender, ProgressChangedArgs e)
        {
            if (e.IsImageTestMode || e.IsManualMode) return;

            var ctrl = GetUcDetectionGroup(e.DetectionType, e.Channel);
            ctrl.ShowProgress(e.Percent);
        }
        /// <summary>
        /// 更新视图显示
        /// </summary>
        public override void UpdateView()
        {
            // 更新控件
            for (int i = 0; i < lst.Count; i++)
            {
                var setting = Global.FrmStandardAoi.GetSetting();
                if (i < setting.ListGroup.Count)
                {
                    var type = setting.ListGroup[i].DetectionType;
                    int managerIndex = managers[0].DetectionType == type ? 0 : 1;

                    if (managerIndex >= 0)
                    {
                        lst[i].Enabled = i < setting.ListGroup.Count;
                        lst[i].ImageBox.IsNeedRotate = setting.IsNeedRotate[managerIndex];
                        lst[i].DetectionType = type;
                        lst[i].Title = setting.ListGroup[i].GroupName;
                        lst[i].SetManager(managers[managerIndex]);
                    }
                }
            }
        }
    }
}
