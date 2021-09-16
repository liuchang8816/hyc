/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：StandardAoiDemo
// 文件名称    ：Global.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/4/14 10:51:59 
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
using HYC.StandardAoi.Interface;
using HYC.StandardAoi.UI;
using RasterDetectionMod.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RasterDetectionMod
{
    class Global
    {
        // 用于StandardAoi模块的实例
        public static SignalProvider SignalProvider = new SignalProvider();
        public static List<IImageProducer> ImageProducers = new List<IImageProducer>();
        public static List<IPatternProducer> PatternProducers = new List<IPatternProducer>();
        public static List<IPowerController> PowerControllers = new List<IPowerController>();
        /// <summary>
        /// AOI检测窗体
        /// </summary>
        public static FormStandardAoi FrmStandardAoi = new FormStandardAoi();
        /// <summary>
        /// 分层检测2D第二次拍照
        /// </summary>
        public static ManualResetEvent MreGrab = new ManualResetEvent(false);

        /// <summary>
        /// 当前检测类型
        /// </summary>
        public static CheckType CurCheckType = CheckType.Raster;
        /// <summary>
        /// 当前的检测相机
        /// </summary>
        public static CheckCam CurCheckCam = CheckCam.Cam2D;
        /// <summary>
        /// 当前客户，不同客户设备不一样
        /// </summary>
        public static CustomerType CurCustomerType = CustomerType.Geer;
    }

    /// <summary>
    /// 当前检测类型
    /// </summary>
    public enum CheckType
    {
        GoldWire,
        Raster,
        ScreenLayer
    }

    /// <summary>
    /// 当前检测主机是2D还是3D
    /// </summary>
    public enum CheckCam
    {
        Cam2D,
        Cam3D
    }

    /// <summary>
    /// 客户类型，对应不同的设备
    /// </summary>
    public enum CustomerType
    {
        Geer,
        RiDong,
        SE
    }
}
