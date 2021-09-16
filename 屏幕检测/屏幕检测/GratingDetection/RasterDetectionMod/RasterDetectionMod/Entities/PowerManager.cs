/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：IxDetectorAoi.Entities
// 文件名称    ：PowerManager.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/8/10 10:40:44 
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
using RasterDetectionMod.Extentions;
using HYC.HTCommunication.SerialPortWrapper;
using HYC.HTCommunication.SocketWrapper;
using HYC.HTPower;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Threading;
using HYC.HTLog;

namespace RasterDetectionMod.Entities
{
    /// <summary>
    /// 光源管理类
    /// </summary>
    class PowerManager
    {
        /// <summary>
        /// 光源控制器的数量
        /// </summary>
        public static int PowerCount = 1;
        /// <summary>
        /// 所有的光源控制器
        /// </summary>
        private static IHTPower[] powers = new IHTPower[PowerCount];
        /// <summary>
        /// 光源的连接信息
        /// </summary>
        private static HTCommunicationInfo[] infos = new HTCommunicationInfo[PowerCount];

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            try
            {
                // 从配置文件加载
                XDocument xDoc = XDocument.Load(@"Config\Power.xml");
                var elements = xDoc.Root.Elements("Power").ToList();
                PowerCount = elements.Count;
                powers = new IHTPower[PowerCount];
                infos = new HTCommunicationInfo[PowerCount];

                // 初始化
                for (int i = 0; i < powers.Length; i++)
                {
                    try
                    {
                        // 创建实例
                        powers[i] = HTPowerFactory.CreatePower(elements[i].Attribute("Type").Value);
                        powers[i].Name = "POWER" + i;
                        powers[i].PowerIndex = i;
                        // 初始化相关参数
                        infos[i] = new HTCommunicationInfo();
                        if (elements[i].Element("SerialPort") != null)
                        {
                            XElement element = elements[i].Element("SerialPort");
                            infos[i].serialPortInfo = new HTSerialPortInfo();
                            infos[i].serialPortInfo.PortName = "COM" + element.Attribute("Port").Value;
                            infos[i].serialPortInfo.BaudRate = Convert.ToInt32(element.Attribute("BaudRate").Value);
                            infos[i].serialPortInfo.DataBits = Convert.ToInt32(element.Attribute("DataBits").Value);
                            infos[i].serialPortInfo.StopBits = HTSerialPortInfo.GetStopBits(element.Attribute("StopBits").Value);
                            infos[i].serialPortInfo.Parity = HTSerialPortInfo.GetParity(element.Attribute("Parity").Value);
                        }
                        if (elements[i].Element("Socket") != null)
                        {
                            XElement element = elements[i].Element("Socket");
                            infos[i].socketInfo = new HTSocketInfo();
                            infos[i].socketInfo.IP = element.Attribute("IP").Value;
                            infos[i].socketInfo.Port = Convert.ToInt32(element.Attribute("Port").Value);
                        }

                        // 执行初始化
                        bool result = powers[i].Init(infos[i]);
                        if (result)
                        {
                            FormMain.Instance.UpdateDeviceState("光源" + (i + 1), true);
                            FormMain.Instance.ShowMsg("光源" + (i + 1) + "连接成功！", Brushes.Green);
                        }
                        else
                        {
                            FormMain.Instance.UpdateDeviceState("光源" + (i + 1), false);
                            FormMain.Instance.ShowMsg("光源" + (i + 1) + "连接失败！", Brushes.Red);
                        }
                    }
                    catch (Exception ex)
                    {
                        FormMain.Instance.UpdateDeviceState("光源" + (i + 1), false);
                        FormMain.Instance.ShowMsg("光源" + (i + 1) + "连接失败！" + ex.Message, Brushes.Red);
                        LogManager.GetLogger("Error").Error(ex);
                    }
                }

                // 添加IPowerController
                if (Global.CurCustomerType == CustomerType.Geer || Global.CurCustomerType == CustomerType.RiDong)
                {
                    Global.PowerControllers.Add(new PowerController(powers[0], 0));
                }
                else if (Global.CurCustomerType == CustomerType.SE)
                {
                    Global.PowerControllers.Add(new PowerController2(powers[0], powers[1]));
                }
                //TODO
                ////// 断线重连
                //Task.Factory.StartNew(Connect);
            }
            catch (Exception ex)
            {
                for (int i = 0; i < powers.Length; i++)
                {
                    FormMain.Instance.UpdateDeviceState("光源" + (i + 1), false);
                }
                FormMain.Instance.ShowMsg("光源初始化异常！" + ex.Message, Brushes.Red);
                LogManager.GetLogger("Error").Error(ex);
            }
        }

        /// <summary>
        /// 断线重连
        /// </summary>
        private static void Connect()
        {
            while (true)
            {
                try
                {
                    for (int i = 0; i < powers.Length; i++)
                    {
                        if (!powers[i].GetConnect())
                        {
                            FormMain.Instance.UpdateDeviceState("光源" + (i + 1), false);
                            powers[i].Uninit();
                            powers[i].Init(infos[i]);
                            if (powers[i].GetConnect())
                            {
                                FormMain.Instance.UpdateDeviceState("光源" + (i + 1), true);
                                FormMain.Instance.ShowMsg("光源" + (i + 1) + "连接成功！", Brushes.Black);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    FormMain.Instance.ShowMsg("光源重连异常：" + ex.Message, Brushes.Red);
                    LogManager.GetLogger("Error").Error(ex);
                }
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 获取光源
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>光源的实例</returns>
        public static IHTPower GetPower(int index)
        {
            return powers[index];
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public static void UnInit()
        {
            for (int i = 0; i < powers.Length; i++)
            {
                try
                {
                    powers[i].Uninit();
                }
                catch (Exception)
                { }
            }
        }
    }
}
