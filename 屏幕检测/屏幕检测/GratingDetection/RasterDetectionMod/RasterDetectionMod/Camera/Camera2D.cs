/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：Camera2D.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 17:10:08 
// 功能描述 ：2D相机
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
    public class Camera2D
    {
        #region
        //VOMMACam2D

        ///// <summary>
        ///// 得到当前2D相机数量
        ///// </summary>
        ///// <param name="Cam2DNum"></param>
        ///// <returns></returns>
        //public static bool Search(ref int Cam2DNum)
        //{
        //    if(VOMMAEnumDevices(ref Cam2DNum)==0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }           
        //}

        //[DllImport("VOMMACam2D.dll")]
        //private static extern int VOMMAEnumDevices(ref int camreaNum);
        #endregion

        /// <summary>
        /// 搜寻2D相机
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static bool Search(StringBuilder desc)
        {
            return Cam_Search(desc);
        }

        /// <summary>
        /// 打开2D相机
        /// </summary>
        /// <returns></returns>
        public static bool Open()
        {
            return Cam_Open();
        }

        /// <summary>
        /// 关闭2D相机
        /// </summary>
        /// <returns></returns>
        public static bool Close()
        {
            return Cam_Close();
        }

        /// <summary>
        /// 获取2D相机的曝光时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool GetExposureTime(ref Int64 time)
        {
            return Cam_GetExposureTime(ref time);
        }

        /// <summary>
        /// 设置2D相机的曝光时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool SetExposureTime(Int64 time)
        {
            return Cam_SetExposureTime(time);
        }

        /// <summary>
        /// 打开2D相机的图像数据流
        /// </summary>
        /// <returns></returns>
        public static bool StartAcquire()
        {
            return Cam_StartAcquire();
        }

        /// <summary>
        /// 关闭2D相机的图像数据流
        /// </summary>
        /// <returns></returns>
        public static bool StopAcquire()
        {
            return Cam_StopAcquire();
        }

        /// <summary>
        /// 获取2D相机的一帧的图像数据
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bitDepth"></param>
        /// <returns></returns>
        public static bool GetImageSize(ref int width, ref int height)
        {
            return Cam_GetImageSize(ref width, ref height);
        }

        /// <summary>
        /// 获得2D相机的图像深度图
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool GetImage(byte[] data)
        {
            return Cam_GetImage(data);
        }

        /// <summary>
        /// 释放图片
        /// </summary>
        /// <returns></returns>
        public static bool FreeImage()
        {
            return Cam_FreeImage();
        }

        [DllImport("Camera2D.dll")]
        private static extern bool Cam_Search(StringBuilder desc);
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_Open();
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_Close();
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_StartAcquire();
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_StopAcquire();
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_GetExposureTime(ref Int64 time);
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_SetExposureTime(Int64 time);
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_GetImageSize(ref int width, ref int height);
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_GetImage(byte[] data);
        [DllImport("Camera2D.dll")]
        private static extern bool Cam_FreeImage();
    }
}
