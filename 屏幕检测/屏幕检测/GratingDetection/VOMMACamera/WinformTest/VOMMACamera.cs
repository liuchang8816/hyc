/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：WinformTest 
// 文件名称 ：VOMMACamera.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/6/17 9:26:57 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinformTest
{
    public delegate void CameraEventCallBack(IntPtr data, int width, int height);

    public static class VOMMACamera
    {
        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraSearch(StringBuilder desc);

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraOpen();

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraClose();

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraSetTriggerMode(int mode);

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraSoftTrigger();

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraStartAcquisition();

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraStopAcquisition();

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraGetExposureTime(ref long time);

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraSetExposureTime(long time);

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraGetImageSize(ref int width, ref int height);

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraGetImage(byte[] data);

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraSetDepthCalPara(Variables variables);

        [DllImport("VOMMACamera.dll")]
        public static extern bool CameraGetDepthImg(byte[] data, int width, int height, byte[] depthData, string depthDir);

        [DllImport("VOMMACamera.dll")]
        public static extern bool RegisterImageReceived(CameraEventCallBack callBack);
    }

    public struct Variables
    {
        public int maxDepth;                   //最远景深   (min=-100, max=99,    step=1)
        public int minDepth;                   //最近景深   (min=-99,  max=100,   step=1)
        public int texture;                    //纹理程度   (min=1,    max=8,      step=1)
        public int smoothWindow;               //平滑窗口   (min=1,    max=16,    step=1)
        public int smoothFilterType;
        public int detection;                  //边缘检测   (min=0,    max=3,     step=1)
        public int BORW;                       //背景选择   (0=SHOW_LIGHT_PART,    1=SHOW_DARK_PART)
        public float thre;                     //阈值          (min=0.00,   max=10.00,     step=0.01)
    }

    public enum ImageInputType
    {
        VOMMA_8UC1 = 0,
        VOMMA_16UC1 = 2,
        VOMMA_32FC1 = 5,
        VOMMA_8UC3 = 16,
        VOMMA_16UC3 = 18,
        VOMMA_32FC3 = 21
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VOMMAImage
    {
        public byte[] imageData { get; set; }
        public ImageInputType imageType { get; set; }
        public int cols { get; set; }
        public int rows { get; set; }
    }
}
