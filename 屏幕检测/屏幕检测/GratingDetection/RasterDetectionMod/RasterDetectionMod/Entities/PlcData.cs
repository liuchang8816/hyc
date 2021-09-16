/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：IxDetectorAoi.Entities
// 文件名称    ：PlcData.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/8/20 17:01:47 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.Entities
{
    /// <summary>
    /// 动作信号 PLC->PC
    /// </summary>
    [Serializable]
    public class PlcReadBits
    {
        /// <summary>
        /// 2DAOI测试请求
        /// </summary>
        public bool TestRequest2D { get; set; }
        /// <summary>
        /// 3D测试请求
        /// </summary>
        public bool TestRequest3D { get; set; }
        /// <summary>
        /// 开电请求
        /// </summary>
        public bool LightON { get; set; }
        /// <summary>
        /// 断电请求
        /// </summary>
        public bool LightOff { get; set; }
        /// <summary>
        /// 3D相机拍照准备开始
        /// </summary>
        public bool Camera3DPhotoStart { get; set; }
        /// <summary>
        /// 当前缺陷是否全部拍照完成(可以调用算法了)
        /// </summary>
        public bool Camera3DPhotoEnd { get; set; }

        /// <summary>
        /// 3DMark点拍照请求
        /// </summary>
        public bool GrabRequest3DMark { get; set; }
        /// <summary>
        /// 一枚产品的3D测试开始
        /// </summary>
        public bool PanelTestStart3D { get; set; }
        /// <summary>
        /// 一枚产品的3D测试结束
        /// </summary>
        public bool PanelTestEnd3D { get; set; }
        /// <summary>
        /// 2D相机拍照请求，用于第二张拍照
        /// </summary>
        public bool Camera2DGrabReq { get; set; }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data"></param>
        public void Update(bool[] data)
        {
            TestRequest2D = data[0];     //2D相机请求拍照
            TestRequest3D = data[1];        //3D相机请求拍照
            LightON = data[6];              //检查机上断电
            LightOff = data[7];
            Camera3DPhotoStart = data[8];   // 3D相机拍照开始
            Camera3DPhotoEnd = data[9];// 当前缺陷是否全部拍照完成(可以调用算法了)

            GrabRequest3DMark = data[11];
            PanelTestStart3D = data[13];
            PanelTestEnd3D = data[14];
            Camera2DGrabReq = data[15];
        }

        public bool Equals(PlcReadBits item)
        {
            PropertyInfo[] props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                object obj1 = prop.GetValue(this, null);
                object obj2 = prop.GetValue(item, null);
                if (obj1.GetType() == typeof(bool))
                {
                    bool val1 = (bool)obj1;
                    bool val2 = (bool)obj2;
                    if (val1 != val2)
                        return false;
                }
                else if (obj1.GetType() == typeof(bool[]))
                {
                    bool[] val1 = (bool[])obj1;
                    bool[] val2 = (bool[])obj2;
                    for (int i = 0; i < val1.Length; i++)
                    {
                        if (val1[i] != val2[i])
                            return false;
                    }
                }
                else if (obj1.GetType() == typeof(bool[,]))
                {
                    bool[,] val1 = (bool[,])obj1;
                    bool[,] val2 = (bool[,])obj2;
                    for (int i = 0; i < val1.GetLength(0); i++)
                    {
                        for (int j = 0; j < val1.GetLength(1); j++)
                        {
                            if (val1[i, j] != val2[i, j])
                                return false;
                        }
                    }
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Plc数据 PC->PLC
    /// </summary>
    public class PlcReadWords
    {
        /// <summary>
        /// 产品状态/结果
        /// </summary>
        public PanelStatus PanelStatus { get; set; }
        /// <summary>
        /// 产品NG数量
        /// </summary>
        public int PanelErrorNum { get; set; }
        /// <summary>
        /// 产品NG复判次数
        /// </summary>
        public int PanelErrorphotoNum { get; set; }
        /// <summary>
        /// 产品NG的信息
        /// </summary>
        public Dictionary<double, double> PanelErrorInfo { get; set; }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data"></param>
        public void Update(ushort[] data)
        {
            PanelErrorNum = (Int32)data[0];
            PanelErrorphotoNum = (Int32)data[1];
            PanelStatus = (PanelStatus)data[51];
            for (int i = 0; i < PanelErrorNum; i++)
            {
                //PanelErrorInfo.Add()   //看有多少个数据被传过来，这里就添加多少个数据
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public PlcReadWords()
        {
            PanelErrorInfo = new Dictionary<double, double>();
        }

    }

    public enum PanelStatus
    {
        OK = 0,
        NG = 1,
    }
}
