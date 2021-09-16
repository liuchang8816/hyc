/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Extentions 
// 文件名称 ：ImageProducer.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 13:32:48 
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
using HYC.HTCamera.Card.Matrox;
using HYC.StandardAoi.Interface;
using HYC.StandardAoi.Parameter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.Extentions
{
    /// <summary>
    /// 图像数据生产者
    /// </summary>
    class ImageProducer : IImageProducer
    {
        /// <summary>
        /// Mil相机
        /// </summary>
        private MilCamera camera = null;
        /// <summary>
        /// 相机串口
        /// </summary>
        private MilCameraPort port = null;

        /// <summary>
        /// 图像裁剪X
        /// </summary>
        private int startX = 0;
        /// <summary>
        /// 图像裁剪Y
        /// </summary>
        private int startY = 0;
        /// <summary>
        /// 图像裁剪Width
        /// </summary>
        private int imgWidth = 0;
        /// <summary>
        /// 图像裁剪Height
        /// </summary>
        private int imgHeight = 0;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="camera">Mil相机</param>
        /// <param name="port">相机串口</param>
        public ImageProducer(MilCamera camera, MilCameraPort port)
        {
            this.camera = camera;
            this.port = port;
        }

        /// <summary>
        /// 构造方法，指定了相机图像裁剪区域
        /// </summary>
        /// <param name="camera">Mil相机</param>
        /// <param name="port">相机串口</param>
        /// <param name="startX">图像裁剪X</param>
        /// <param name="startY">图像裁剪Y</param>
        /// <param name="imgWidth">图像裁剪Width</param>
        /// <param name="imgHeight">图像裁剪Height</param>
        public ImageProducer(MilCamera camera, MilCameraPort port,
            int startX, int startY, int imgWidth, int imgHeight)
        {
            this.camera = camera;
            this.port = port;
            this.startX = startX;
            this.startY = startY;
            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;
        }

        /// <summary>
        /// 设置相机图像裁剪区域
        /// </summary>
        /// <param name="startX">图像裁剪X</param>
        /// <param name="startY">图像裁剪Y</param>
        /// <param name="imgWidth">图像裁剪Width</param>
        /// <param name="imgHeight">图像裁剪Height</param>
        public void SetBmpSize(int startX, int startY, int imgWidth, int imgHeight)
        {
            this.startX = startX;
            this.startY = startY;
            this.imgWidth = imgWidth;
            this.imgHeight = imgHeight;
        }

        /// <summary>
        /// 获取图像数据
        /// </summary>
        /// <param name="pattName">画面名称</param>
        /// <returns>图形数据</returns>
        public ImageData CreateImageData(string pattName)
        {
            if (camera != null)
            {
                if (imgWidth <= 0 || imgHeight <= 0)
                {
                    // 图像不裁剪的情况
                    int width = camera.GetWidth();
                    int height = camera.GetHeight();
                    byte[] data = new byte[width * height];
                    camera.Grab2d(data);
                    if (data != null)
                    {
                        var imgData = new ImageData()
                        {
                            ImageBytes = data,
                            Width = width,
                            Height = height
                        };
                        // 旋转180度
                        Bitmap bitmap = imgData.ToBitmap();
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        var imgDataNew = new ImageData();
                        imgDataNew.FromBitmap(bitmap, false);
                        bitmap.Dispose();
                        return imgDataNew;
                    }
                }
                else
                {
                    // 图像裁剪的情况
                    byte[] data = new byte[imgWidth * imgHeight];
                    camera.Grab2d(data, startX, startY, imgWidth, imgHeight);
                    if (data != null)
                    {
                        return new ImageData()
                        {
                            ImageBytes = data,
                            Width = imgWidth,
                            Height = imgHeight
                        };
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="millisecond">曝光时间，毫秒</param>
        /// <returns>true:成功；false:失败</returns>
        public bool SetExposureTime(float millisecond)
        {
            if (port != null)
            {
                return port.SetExposure((int)millisecond);
            }
            return false;
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="value">增益值</param>
        /// <returns>true:成功；false:失败</returns>
        public bool SetGain(float value)
        {
            if (port != null)
            {
                return port.SetGain(value);
            }
            return false;
        }

        /// <summary>
        /// 设置Gamma值
        /// </summary>
        /// <param name="isEnabled">是否启用Gamma</param>
        /// <param name="value">Gamma值</param>
        /// <returns>true:成功；false:失败</returns>
        public bool SetGamma(bool isEnabled, float value)
        {
            return true;
        }
    }
}
