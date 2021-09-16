/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：StandardAoiDemo.Entities
// 文件名称    ：AoiCamera.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/4/14 14:33:51 
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
using HYC.HTCamera.Card.Matrox;
using HYC.HTCommunication.SerialPortWrapper;
using HYC.HTLog;
using HYC.StandardAoi.Extentions;
using HYC.StandardAoi.Parameter;
using RasterDetectionMod.Extentions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RasterDetectionMod.Entities
{
    /// <summary>
    /// AOI相机管理
    /// </summary>
    class AoiCamera
    {
        /// <summary>
        /// Mil相机管理对象
        /// </summary>
        private static MilApplication milApp = new MilApplication();
        /// <summary>
        /// 总的相机数
        /// </summary>
        public static int CameraCount = 1;

        /// <summary>
        /// 初始化  连接相机
        /// </summary>
        public static void Init()
        {
            try
            {
                // 从xml文件加载相机的配置
                XDocument xDoc = XDocument.Load(@"Config\AoiCamera.xml");
                MilCardInfo info = new MilCardInfo();
                //info.IsLine = false;
                info.SpInfos = new HTSerialPortInfo[CameraCount];
                for (int i = 0; i < CameraCount; i++)
                {
                    // 设置相机的串口信息
                    info.SpInfos[i] = new HTSerialPortInfo();
                    info.SpInfos[i].PortName = "COM" + xDoc.Root.Element("Port_" + i).Value;
                    info.SpInfos[i].BaudRate = Convert.ToInt32(xDoc.Root.Element("BaudRate").Value);
                    info.SpInfos[i].DataBits = Convert.ToInt32(xDoc.Root.Element("DataBits").Value);
                    info.SpInfos[i].StopBits = HTSerialPortInfo.GetStopBits(xDoc.Root.Element("StopBits").Value);
                    info.SpInfos[i].Parity = HTSerialPortInfo.GetParity(xDoc.Root.Element("Parity").Value);
                }
                info.StrDcfs = new string[CameraCount];
                for (int i = 0; i < CameraCount; i++)
                {
                    info.StrDcfs[i] = xDoc.Root.Element("Dcfs_" + i).Value;
                }
                List<MilCardInfo> lstInfos = new List<MilCardInfo>();
                lstInfos.Add(info);
                // 初始化
                milApp.InitApplication(lstInfos);
                
                for (int i = 0; i < CameraCount; i++)
                {
                    MilCamera camera = milApp.GetCamera(i / 2, i % 2);
                    MilCameraPort port = milApp.GetCameraPort(i / 2, i % 2);
                    // 添加图像数据生产者
                    Global.ImageProducers.Add(new MilAoiCameraImageProducer(camera, port));
                    if (camera != null && port != null)
                    {
                        FormMain.Instance.UpdateDeviceState("AOI相机" + (i + 1), true);
                        FormMain.Instance.ShowMsg("AOI相机" + (i + 1) + "连接成功！", Brushes.Green);
                    }
                    else
                    {
                        FormMain.Instance.UpdateDeviceState("AOI相机" + (i + 1), false);
                        FormMain.Instance.ShowMsg("AOI相机" + (i + 1) + "连接失败！", Brushes.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                FormMain.Instance.UpdateDeviceState("AOI相机", false);
                FormMain.Instance.ShowMsg("AOI相机连接失败！" + ex.Message, Brushes.Red);
                LogManager.GetLogger("Error").Error(ex.ToString());
            }
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="index">相机序号</param>
        /// <param name="milliseconds">曝光时间</param>
        /// <returns>true:成功 false:失败</returns>
        public static bool SetExposureTime(int index, int milliseconds)
        {
            MilCameraPort port = milApp.GetCameraPort(index / 2, index % 2);
            if (port != null)
            {
                return port.SetExposure(milliseconds);
            }
            return false;
        }

        /// <summary>
        /// 获取曝光时间
        /// </summary>
        /// <param name="index">相机序号</param>
        /// <returns>曝光时间</returns>
        public static float GetExposureTime(int index)
        {           
            MilCameraPort port = milApp.GetCameraPort(index / 2, index % 2);
            if (port != null)
            {
                float milliseconds = 0;
                port.GetExposure(ref milliseconds);
                return milliseconds;
            }
            return 0;
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="index">相机序号</param>
        /// <param name="gain">增益</param>
        /// <returns>true:成功 false:失败</returns>
        public static bool SetGain(int index, float gain)
        {
            MilCameraPort port = milApp.GetCameraPort(index / 2, index % 2);
            if (port != null)
            {
                return port.SetGain(gain);
            }
            return false;
        }

        /// <summary>
        /// 获取增益
        /// </summary>
        /// <param name="index">相机序号</param>
        /// <returns>增益</returns>
        public static float GetGain(int index)
        {
            MilCameraPort port = milApp.GetCameraPort(index / 2, index % 2);
            if (port != null)
            {
                float gain = 0;
                port.GetGain(ref gain);
                return gain;
            }
            return 0;
        }

        /// <summary>
        /// 设置线扫描周期
        /// </summary>
        /// <param name="index">相机序号</param>
        /// <param name="value">线扫周期</param>
        /// <returns>true:成功 false:失败</returns>
        public static bool SetLinePeriod(int index, int value)
        {
            MilCameraPort port = milApp.GetCameraPort(index / 2, index % 2);
            if (port != null)
            {
                return port.SetLinePeriod(value);
            }
            return false;
        }

        /// <summary>
        /// 获取线扫周期
        /// </summary>
        /// <param name="index">相机序号</param>
        /// <returns>线扫周期</returns>
        public static int GetLinePeriod(int index)
        {
            MilCameraPort port = milApp.GetCameraPort(index / 2, index % 2);
            if (port != null)
            {
                int value = 0;
                port.GetLinePeriod(ref value);
                return value;
            }
            return 0;
        }

        /// <summary>
        /// 面阵相机拍照
        /// </summary>
        /// <param name="index"></param>
        /// <returns>图像数据</returns>
        public static ImageData Grab2d(int index)
        {
            MilCamera camera = milApp.GetCamera(index / 2, index % 2);
            if (camera != null)
            {
                int width = camera.GetWidth();
                int height = camera.GetHeight();
                byte[] data = new byte[width * height];
                camera.Grab2d(data);
                return new ImageData()
                {
                    ImageBytes = data,
                    Width = width,
                    Height = height
                };
            }
            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public static void UnInit()
        {
            try
            {
                milApp.FreeApplication();
            }
            catch (Exception)
            { }
        }
    }
}
