/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：Camera3D.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 17:10:08 
// 功能描述 ：3D相机
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
using HYC.HTLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RasterDetectionMod
{
    public class Camera3D
    {
        //深度计算参数
        public static string[] DepthParam = new string[10] { "", "", "", "", "", "", "", "", "", "" };
        public static _Variables Camera3DDepthParam = new _Variables();
        public static long Camera3DExposure = 1000;//us
        public static long Camera3DExposureMark = 1000;
        public static float ThreArea = 100;

        /// <summary>
        /// 搜寻3D相机
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static bool Search(StringBuilder desc)
        {
            try
            {
               
                return Cam_Search(desc);

            }
            catch (Exception ex)
            {
                LogManager.GetLogger("Error").Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 打开3D相机
        /// </summary>
        /// <returns></returns>
        public static bool Open()
        {
            return Cam_Open();
        }

        public static bool InitConfig()
        {
            try
            // 从xml文件加载相机的配置
            {
                XDocument xDoc = XDocument.Load(@"Config\Vomma3DCamera.xml");

                Camera3DExposure = long.Parse(xDoc.Root.Element("Exposure").Value);

                Camera3DDepthParam.level = int.Parse(xDoc.Root.Element("level").Value);
                Camera3DDepthParam.maxDepth = int.Parse(xDoc.Root.Element("maxDepth").Value);
                Camera3DDepthParam.minDepth = int.Parse(xDoc.Root.Element("minDepth").Value);
                Camera3DDepthParam.texture = int.Parse(xDoc.Root.Element("texture").Value);
                Camera3DDepthParam.smoothWindow = int.Parse(xDoc.Root.Element("smoothWindow").Value);
                Camera3DDepthParam.detection = int.Parse(xDoc.Root.Element("detection").Value);
                Camera3DDepthParam.BORW = int.Parse(xDoc.Root.Element("BORW").Value);
                Camera3DDepthParam.thre = float.Parse(xDoc.Root.Element("thre").Value);
                Camera3DDepthParam.step_value = float.Parse(xDoc.Root.Element("step_value").Value);

                if (xDoc.Root.Element("Camera3DExposureMark") != null)
                    Camera3DExposureMark = long.Parse(xDoc.Root.Element("Camera3DExposureMark").Value);

                if (xDoc.Root.Element("ThreArea") != null)
                    ThreArea = float.Parse(xDoc.Root.Element("ThreArea").Value);
                 
                CamSetDepthParam(
                   Camera3DDepthParam.level,
                   Camera3DDepthParam.maxDepth,
                   Camera3DDepthParam.minDepth,
                   Camera3DDepthParam.texture,
                   Camera3DDepthParam.smoothWindow,
                   Camera3DDepthParam.detection,
                   Camera3DDepthParam.BORW,
                   Camera3DDepthParam.thre,
                   Camera3DDepthParam.step_value
                   , true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 保存参数配置
        /// </summary>
        public static void SaveConfig()
        {
            try
            {
                XDocument xDoc = XDocument.Load(@"Config\Vomma3DCamera.xml");
                xDoc.Root.SetElementValue("Exposure", Camera3DExposure);

                xDoc.Root.SetElementValue("level", Camera3DDepthParam.level);
                xDoc.Root.SetElementValue("maxDepth", Camera3DDepthParam.maxDepth);
                xDoc.Root.SetElementValue("minDepth", Camera3DDepthParam.minDepth);
                xDoc.Root.SetElementValue("texture", Camera3DDepthParam.texture);
                xDoc.Root.SetElementValue("smoothWindow", Camera3DDepthParam.smoothWindow);
                xDoc.Root.SetElementValue("detection", Camera3DDepthParam.detection);
                xDoc.Root.SetElementValue("BORW", Camera3DDepthParam.BORW);
                xDoc.Root.SetElementValue("thre", Camera3DDepthParam.thre);
                xDoc.Root.SetElementValue("step_value", Camera3DDepthParam.step_value);

                if (xDoc.Root.Element("Camera3DExposureMark") != null)
                    xDoc.Root.SetElementValue("Camera3DExposureMark", Camera3DExposureMark);
                else
                    xDoc.Root.Add(new XElement("Camera3DExposureMark", Camera3DExposureMark));

                if (xDoc.Root.Element("ThreArea") != null)
                    xDoc.Root.SetElementValue("ThreArea", ThreArea);
                else
                    xDoc.Root.Add(new XElement("ThreArea", ThreArea));

                xDoc.Save(@"Config\Vomma3DCamera.xml");
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// 关闭3D相机
        /// </summary>
        /// <returns></returns>
        public static bool Close()
        {
            return Cam_Close();
        }

        /// <summary>
        /// 获取3D相机的曝光时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool GetExposureTime(ref Int64 time)
        {
            return Cam_GetExposureTime(ref time);
        }

        /// <summary>
        /// 设置3D相机的曝光时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool SetExposureTime(Int64 time)
        {
            return Cam_SetExposureTime(time);
        }

        /// <summary>
        /// 打开3D相机的图像数据流
        /// </summary>
        /// <returns></returns>
        public static bool StartAcquire()
        {
            return Cam_StartAcquire();
        }

        /// <summary>
        /// 关闭3D相机的图像数据流
        /// </summary>
        /// <returns></returns>
        public static bool StopAcquire()
        {
            return Cam_StopAcquire();
        }

        /// <summary>
        /// 获取3D相机的一帧的图像数据
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
        /// 获得3D相机的图像深度图
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool GetImage(byte[] data, byte[] depthPath, byte[] depthData)
        {
            return Cam_GetImage(data, depthPath, depthData);
        }

        /// <summary>
        /// 设置3D相机深度参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool CamSetDepthParam(int nLvl, int nMaxDepth, int nMinDepth, int nTexture, int nSmoothWnd, int nDetection, int nBorw, float fltThre, float fltStepValue, bool bIsColored)
        {
            return Cam_SetDepthParam(nLvl, nMaxDepth, nMinDepth, nTexture, nSmoothWnd, nDetection, nBorw, fltThre, fltStepValue, bIsColored);
        }

        /// <summary>
        /// 获得3D相机的图像深度图
        /// </summary>
        /// <param name="vars">参数结构体，可通过更改结构体内参数值来调整 得到效果较好的深度图像 </param>
        /// <param name="vImage">图像结构体，此处为计算得到的深度图</param>
        /// <param name="depthPath">生成的 depth.txt 所要保存路径</param>
        /// <param name="centerPath"> 生成的 center.txt 所要保存路径，默认值为空，传入空值即为选择不生成 center.txt。 </param>
        /// <returns></returns>
        public static bool GetImage(_Variables vars, ref _VOMMAImage vImage, byte[] depthPath, byte[] centerPath = null)
        {
            return 0 == VOMMAGetDepthImg(vars, ref vImage, depthPath, centerPath = null);
        }


        public static bool GetRealImage(byte[] data)
        {
            return Cam_GetRealImage(data);
        }


        [DllImport("Camera3D.dll")]
        private static extern bool Cam_Search(StringBuilder desc);
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_Open();
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_Close();
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_StartAcquire();
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_StopAcquire();
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_GetExposureTime(ref Int64 time);
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_SetExposureTime(Int64 time);
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_GetImageSize(ref int width, ref int height);
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_GetImage(byte[] data, byte[] depthPath, byte[] depthData);
        [DllImport("Camera3D.dll")]
        private static extern bool Cam_GetRealImage(byte[] data);

        [DllImport("Camera3D.dll")]
        private static extern int VOMMAGetDepthImg(_Variables vars, ref _VOMMAImage vImage, byte[] depthPath, byte[] centerPath = null);

        [DllImport("Camera3D.dll")]
        private static extern bool Cam_SetDepthParam(int nLvl, int nMaxDepth, int nMinDepth, int nTexture, int nSmoothWnd, int nDetection, int nBorw, float fltThre, float fltStepValue, bool bIsColored);

    }
    /// <summary>
    /// 参数结构体
    /// </summary>
    public struct _Variables
    {
        public int level;          //精度级别 (min=1, max=8, step=1) 
        public int maxDepth;       //最远景深 (min=−100, max=99, step=1) 
        public int minDepth;       //最近景深 (min=−99, max=100, step=1) 
        public int texture;        //纹理程度 (min=1, max=8, step=1) 
        public int smoothWindow;   //平滑窗口 (min=1, max=16, step=1) 
        public int detection;      //边缘检测 (min=0, max=3, step=1) 
        public int BORW;           //背景选择 (0=SHOW_LIGHT_PART, 1= SHOW_DARK_PART) 
        public float thre;         //阈值 (min=0.00, max=10.00, step =0.01) 
        public float step_value;   //点云插值步长 
    }
    /// <summary>
    /// 图像类型
    /// </summary>
    public enum _ImageType
    {
        VOMMA_8UC1 = 0,
        VOMMA_16UC1 = 2,
        VOMMA_32FC1 = 5,
        VOMMA_8UC3 = 16,
        VOMMA_16UC3 = 18,
        VOMMA_32FC3 = 21
    }
    public struct _VOMMAImage
    {
        public byte[] imageData;
        public _ImageType imageType;
        public int cols;
        public int rows;
    }
}
