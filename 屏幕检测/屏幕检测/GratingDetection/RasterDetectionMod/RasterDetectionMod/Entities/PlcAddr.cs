/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：IxDetectorAoi.Entities
// 文件名称    ：PlcAddr.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/8/20 17:01:30 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RasterDetectionMod.Entities
{
    class PlcAddr
    {
        //**************PC <-> PLC地址起始************************
        public static string BitReadStart;
        public static string BitWriteStart;


        /// <summary>
        /// 2D相机拍照OK
        /// </summary>
        public static string Camera2DResultOK;
        /// <summary>
        /// 2D相机拍照NG
        /// </summary>
        public static string Camera2DResultNG;
        /// <summary>
        /// 2DPC心跳
        /// </summary>
        public static string PC2DHeartbeatSign;
        /// <summary>
        /// 3DPC心跳
        /// </summary>
        public static string PC3DHeartbeatSign;
        /// <summary>
        /// 3D相机拍照OK
        /// </summary>
        public static string Camera3DResultOK;
        /// <summary>
        /// 3D相机拍照NG
        /// </summary>
        public static string Camera3DResultNG;
        /// <summary>
        /// PC回复可以3D相机开始拍照
        /// </summary>
        public static string Camera3DPhotoStartOK;
        /// <summary>
        /// 当前缺陷是否已经调用算法完成了
        /// </summary>
        public static string IsCheckErrorOK;
        /// <summary>
        /// 2D相机拍照结果异常
        /// </summary>
        public static string Camera2DResultError;
        /// <summary>
        /// 3DMark点拍照OK
        /// </summary>
        public static string Camera3DMarkGrabOK;
        /// <summary>
        /// 3DMark点拍照NG
        /// </summary>
        public static string Camera3DMarkGrabNG;
        /// <summary>
        /// 一枚产品的3D测试开始反馈
        /// </summary>
        public static string PanelTestStart3DAck;
        /// <summary>
        /// 一枚产品的3D测试结束反馈
        /// </summary>
        public static string PanelTestEnd3DAck;
        /// <summary>
        /// 2D相机拍照完成
        /// </summary>
        public static string Camera2DGrabComp;

        /// <summary>
        /// 3D相机复判的X坐标地址起始
        /// </summary>
        public static string XPositionStart3D;
        /// <summary>
        /// 3D相机复判的Y坐标地址起始
        /// </summary>
        public static string YPositionStart3D;
        /// <summary>
        /// 缺陷的区域数量，每个区域的行列信息
        /// </summary>
        public static string DefectRegions;

        /// <summary>
        /// 当前测试区域
        /// </summary>
        public static string CurrentRegion;

        /// <summary>
        /// 加载地址
        /// </summary>
        public static void Load()
        {
            XDocument xDoc = XDocument.Load(@"Config\PLC.xml");
            XElement element = xDoc.Root.Element("Address");

            BitReadStart = element.Element("BitReadStart").Value;
            BitWriteStart = element.Element("BitWriteStart").Value;

            Camera2DResultOK = GetAddr(BitWriteStart, 0);
            Camera2DResultNG = GetAddr(BitWriteStart, 1);
            PC2DHeartbeatSign = GetAddr(BitWriteStart, 2);
            PC3DHeartbeatSign = GetAddr(BitWriteStart, 3);
            Camera3DResultOK = GetAddr(BitWriteStart, 4);
            Camera3DResultNG = GetAddr(BitWriteStart, 5);
            Camera3DPhotoStartOK = GetAddr(BitWriteStart, 8);
            IsCheckErrorOK = GetAddr(BitWriteStart, 9);
            Camera2DResultError = GetAddr(BitWriteStart, 10);
            Camera3DMarkGrabOK = GetAddr(BitWriteStart, 11);
            Camera3DMarkGrabNG = GetAddr(BitWriteStart, 12);
            PanelTestStart3DAck = GetAddr(BitWriteStart, 13);
            PanelTestEnd3DAck = GetAddr(BitWriteStart, 14);
            Camera2DGrabComp = GetAddr(BitWriteStart, 15);

            XPositionStart3D = element.Element("XPositionStart3D").Value;
            YPositionStart3D = element.Element("YPositionStart3D").Value;
            DefectRegions = element.Element("DefectRegions").Value;

            CurrentRegion = element.Element("CurrentRegion").Value;
        }

        /// <summary>
        /// 地址转换
        /// </summary>
        /// <param name="baseAddr"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetAddr(string baseAddr, int i)
        {
            string reg = baseAddr.Substring(0, 1);
            if (reg.ToUpper() == "M" || reg.ToUpper() == "D")
            {
                int num = Convert.ToInt32(baseAddr.Substring(1));
                return reg + (num + i);
            }
            else
            {
                int num = Convert.ToInt32(baseAddr.Substring(1), 16);
                return reg + (num + i).ToString("X");
            }
        }
    }
}
