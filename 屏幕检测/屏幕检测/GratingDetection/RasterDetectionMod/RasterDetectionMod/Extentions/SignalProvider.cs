/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：StandardAoiDemo.Extentions
// 文件名称    ：SignalProvider.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/4/14 10:52:51 
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
using HYC.HTTools;
using HYC.StandardAoi.Interface;
using HYC.StandardAoi.Parameter;
using RasterDetectionMod.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RasterDetectionMod.Extentions
{
    /// <summary>
    /// 信号提供者
    /// </summary>
    class SignalProvider : ISignalProvider
    {
        /// <summary>
        /// 检测完成
        /// </summary>
        /// <param name="type">检测类型</param>
        /// <param name="channel">通道</param>
        /// <param name="result">结果</param>
        public void DetectionCompleted(DetectionType type, int channel, ResultType result)
        {

        }

        /// <summary>
        /// 图像获取已就绪
        /// </summary>
        /// <param name="type">检测类型</param>
        /// <param name="channel"></param>
        /// <param name="patternIndex"></param>
        public void GetImagesReady(DetectionType type, int channel, int patternIndex)
        {
            if (Global.CurCustomerType == CustomerType.RiDong)
            {
                if (patternIndex == 1)
                {
                    Global.MreGrab.WaitOne(5000);
                }
            }
        }

        /// <summary>
        /// 单个Pattern的图像获取完成
        /// </summary>
        /// <param name="type">检测类型</param>
        /// <param name="channel">检测通道</param>
        /// <param name="patternIndex"></param>
        public void GetImagesCompleted(DetectionType type, int channel, int patternIndex)
        {
            if (Global.CurCustomerType == CustomerType.RiDong)
            {
                if (patternIndex == 0)
                {
                    PlcManager.PlcCtrl.WriteBits(PlcAddr.Camera2DGrabComp, true);
                }
            }
        }

        /// <summary>
        /// 所有Pattern图像获取完成
        /// </summary>
        /// <param name="type">检测类型</param>
        /// <param name="channel"></param>
        public void GetImagesEnded(DetectionType type, int channel)
        {

        }

        /// <summary>
        /// 触发检测请求事件
        /// </summary>
        /// <param name="args"></param>
        public void OnDetectionRequest(RequestEventArgs args)
        {
            DetectionRequest?.Invoke(this, args);
        }

        /// <summary>
        /// 开始检测请求
        /// </summary>
        public event EventHandler<RequestEventArgs> DetectionRequest;
    }
}
