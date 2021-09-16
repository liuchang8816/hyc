/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：RasterDetectionMod.Entities
// 文件名称    ：PlcSingals.cs
// =================================
// 创 建 者    ：zl
// 创建日期    ：2021/3/8 17:55:14 
// 功能描述    ：
// 使用说明    ：
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
using RasterDetectionMod.View;
using HYC.HTTools;
using HYC.StandardAoi.Interface;
using HYC.StandardAoi.Parameter;
using HYC.StandardAoi.UI.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using HYC.HTLog;
using RasterDetectionMod.Classes;
using HYC.HTPower;

namespace RasterDetectionMod.Entities
{
    class PlcSignals
    {
        private static WorkFlow wf = null;

        /// <summary>
        /// 处理PLC的请求信号
        /// </summary>
        public static void ProcessSignals()
        {
            ProcessSignals2D();
            ProcessSignals3D();
            
        }

        /// <summary>
        /// 处理2D信号
        /// </summary>
        private static void ProcessSignals2D()
        {
            if (Global.CurCheckCam != CheckCam.Cam2D) return;

            //对不同的测试要求执行不同的算法   还要根据不同的主机执行不同的请求 2D主机不执行3D的请求
            if (PlcManager.ReadBits.TestRequest2D && !PlcManager.LastReadBits.TestRequest2D)
            {
                FormMain.Instance.ShowMsg("收到PLC测试请求", Brushes.Blue);
                if (Global.CurCheckType == CheckType.ScreenLayer ||
                    Global.CurCheckType == CheckType.Raster)
                {
                    Task.Factory.StartNew(() =>
                    {
                        TestRequest(DetectionType.Aoi);
                    });
                }
                if (Global.CurCheckType == CheckType.GoldWire)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera2DResultOK, true);   //通知PLC2D拍照OK
                    });
                }
            }

            // 分层检测2D第二张拍照
            if (PlcManager.ReadBits.Camera2DGrabReq && !PlcManager.LastReadBits.Camera2DGrabReq)
            {
                FormMain.Instance.ShowMsg("收到PLC位置2拍照请求", Brushes.Blue);
                // 通知拍照已就绪
                Global.MreGrab.Set();
            }
        }

        /// <summary>
        /// 处理3D信号
        /// </summary>
        private static void ProcessSignals3D()
        {
           
            if (Global.CurCheckCam != CheckCam.Cam3D) return;

            if (wf == null)
            {
                if (Global.CurCheckType == CheckType.Raster)
                    wf = new WorkFlow3DRaster();
                else if (Global.CurCheckType == CheckType.ScreenLayer)
                    wf = new WorkFlow3DLayer();
            }

            //收到2D检测信号时，清除上一次的结果
            if (PlcManager.ReadBits.TestRequest2D && !PlcManager.LastReadBits.TestRequest2D)
            {
                FormMain.Instance.ShowCheckInfo("---", Color.LightGray, "");
                FormMain.Instance.HideResultImage(); // 隐藏3D检测结果图
            }

            //新的产品检测开始
            if (PlcManager.ReadBits.PanelTestStart3D && !PlcManager.LastReadBits.PanelTestStart3D)
            {
                if (Global.CurCustomerType == CustomerType.RiDong)
                {
                    //打开背光--背光，接CH1
                    PowerManager.GetPower(0).PowerOn(HTPowerChannel.POWER_CHANNEL_1);
                }
                else if (Global.CurCustomerType==CustomerType.SE)
                {
                    //打开背光--,SE背光接两个
                    PowerManager.GetPower(0).PowerOn(HTPowerChannel.POWER_CHANNEL_1);//CH1
                    PowerManager.GetPower(0).PowerOn(HTPowerChannel.POWER_CHANNEL_2);//CH2
                   
                }
                FormMain.Instance.AppendTxt3DLog("收到PLC新的产品开始信号...", Color.Green);

                if (Global.CurCheckType == CheckType.Raster)
                    wf = new WorkFlow3DRaster();
                else if (Global.CurCheckType == CheckType.ScreenLayer)
                    wf = new WorkFlow3DLayer();

                wf.Init();
            }

            //当前产品检测完成
            if (PlcManager.ReadBits.PanelTestEnd3D && !PlcManager.LastReadBits.PanelTestEnd3D)
            {
                FormMain.Instance.AppendTxt3DLog("收到PLC当前产品检测完成信号...", Color.Green);
                wf.GetResult();
                PlcManager.PlcCtrl.WriteBits(PlcAddr.PanelTestEnd3DAck, true);
                if (Global.CurCustomerType == CustomerType.RiDong)
                {
                    //关闭背光--,日东接一个
                    PowerManager.GetPower(0).PowerOff(HTPowerChannel.POWER_CHANNEL_1);
                }
                else if (Global.CurCustomerType == CustomerType.SE)
                {
                    //关闭背光--,SE背光接两个通道
                    PowerManager.GetPower(0).PowerOff(HTPowerChannel.POWER_CHANNEL_1);//CH1
                    PowerManager.GetPower(0).PowerOff(HTPowerChannel.POWER_CHANNEL_2);//CH2
      
                    
                }
            }

            //此触发后表示PLC已经做好3D相机拍照准备 需要创建好产品信息存放的路径
            if (PlcManager.ReadBits.Camera3DPhotoStart && !PlcManager.LastReadBits.Camera3DPhotoStart)
            {
                FormMain.Instance.AppendTxt3DLog("收到PLC新的缺陷开始信号...", Color.Green);

                //当前测试区域
                ushort[] datas = PlcManager.PlcCtrl.ReadWords(PlcAddr.CurrentRegion, 6);

                wf.OffsetX = HTValueConverter.UshortArrayToInt(datas, 0) / 1000f;
                wf.OffsetY = HTValueConverter.UshortArrayToInt(datas, 2) / 1000f;
                wf.Row = datas[4];
                wf.Column = datas[5];
                FormMain.Instance.AppendTxt3DLog($"缺陷区域：Row:{wf.Row},Col:{wf.Column}", Color.Orange);
                FormMain.Instance.AppendTxt3DLog($"偏移距离：X:{wf.OffsetX},Y:{wf.OffsetY}", Color.Orange);

                wf.PhotoNum = 0;
                wf.CreateFolder();
            }

            //调用算法，这时候就要区分是那种检测类型了。
            if (PlcManager.ReadBits.Camera3DPhotoEnd && !PlcManager.LastReadBits.Camera3DPhotoEnd)
            {
                if (Global.CurCheckType == CheckType.Raster)    //调用光栅算法
                {
                    FormMain.Instance.AppendTxt3DLog("正在调用光栅算法进行检测，请稍候...", Color.Green);
                    wf.CheckPhoto();//调用算法
                    FormMain.Instance.AppendTxt3DLog("当前缺陷点检测完成", Color.Green);
                }
                else if (Global.CurCheckType == CheckType.ScreenLayer)  //调用屏幕分层算法
                {
                    FormMain.Instance.AppendTxt3DLog("正在调用屏幕分层检测算法进行检测，请稍候...", Color.Green);
                    wf.CheckPhoto();//调用算法
                    FormMain.Instance.AppendTxt3DLog("当前缺陷点检测完成", Color.Green);
                }
                else if (Global.CurCheckType == CheckType.GoldWire)       //调用金线3D算法
                {

                }
            }

            //如果是3D拍照请求，因为调用的是同一种相机，所以拍照是一样的。
            if (PlcManager.ReadBits.TestRequest3D && !PlcManager.LastReadBits.TestRequest3D)
            {
                Task.Factory.StartNew(() =>
                {
                    wf.Grab();//调用拍照方法
                });
            }

            //拍Mark
            if (PlcManager.ReadBits.GrabRequest3DMark && !PlcManager.LastReadBits.GrabRequest3DMark)
            {
                Task.Factory.StartNew(() =>
                {
                    FormMain.Instance.AppendTxt3DLog("Mark点拍照...", Color.Green);
                    wf.GrabMark();
                });
            }
          
        }

        /// <summary>
        /// 测试流程
        /// </summary>
        /// <param name="stageIndex">治具号</param>
        /// <param name="type">检测类型</param>
        private static void TestRequest(DetectionType type)
        {
            RequestEventArgs args = new RequestEventArgs();
            args.DetectionType = type;
            args.Channel = 0;
            args.Timeout = 50;//检测时间：50秒
            Global.SignalProvider.OnDetectionRequest(args);
            LogManager.GetLogger("Process").Info("触发测试事件");
            FormMain.Instance.ShowMsg("触发测试事件", Brushes.Blue);
        }
    }
}
