/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：Cam3DGold.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 17:10:08 
// 功能描述 ：金线检测3D
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod
{
    public class Cam3DGold
    {
        public static bool Search(StringBuilder desc)
        {
            return VOMMAEnumDevices(desc);
        }

        public static bool Open()
        {
            return VOMMADeviceInit();
        }

        public static bool Close()
        {
            return VOMMADeviceExit();
        }

        public static bool GetExposureTime(ref Int64 time)
        {
            return VOMMAGetExposureTime(ref time);
        }

        public static bool SetExposureTime(Int64 time)
        {
            return VOMMASetExposureTime(time);
        }

        public static bool StartAcquire()
        {
            return VOMMAOpenStreamChannel();
        }

        public static bool StopAcquire()
        {
            return VOMMACloseStreamChannel();
        }

        public static bool GetImageSize(ref int width, ref int height)
        {
            return VOMMAGetImageFromRing(ref width, ref height);
        }

        public static bool GetImage(byte[] data)
        {
            return Cam_GetImage(data);
        }

        public static bool FreeImage()
        {
            return VOMMAQueueImageToRing();
        }

        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMAEnumDevices(StringBuilder desc);
        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMADeviceInit();
        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMADeviceExit();
        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMAOpenStreamChannel();
        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMACloseStreamChannel();
        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMAGetExposureTime(ref Int64 time);
        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMASetExposureTime(Int64 time);
        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMAGetImageFromRing(ref int width, ref int height);
        [DllImport("VOMMACam3D.dll")]
        private static extern bool Cam_GetImage(byte[] data);
        [DllImport("VOMMACam3D.dll")]
        private static extern bool VOMMAQueueImageToRing();
    }
}
