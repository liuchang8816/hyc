/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：IxDetectorAoi.Entities
// 文件名称    ：PlcManager.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/8/19 11:34:48 
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
using HYC.HTCommunication.SocketWrapper;
using HYC.HTLog;
using HYC.HTPLC.Profinet.Melsec;
using HYC.HTTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RasterDetectionMod.Entities
{
    /// <summary>
    /// PLC访问管理类
    /// </summary>
    class PlcManager
    {
        public static PlcWrapper PlcCtrl = new PlcWrapper();
        private static bool isRunning = true;
        public static int ConnectionsCount = 1;

        //PLC数据
        public static PlcReadBits ReadBits = new PlcReadBits();
        //public static PlcReadWords ReadWords = new PlcReadWords();
        //上一次的数据
        public static PlcReadBits LastReadBits = new PlcReadBits();

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            try
            {
                XDocument xDoc = XDocument.Load(@"Config\PLC.xml");
                List<HTSocketInfo> lstInfos = new List<HTSocketInfo>();
                foreach (var item in xDoc.Root.Element("Connections").Elements("Connection"))
                {
                    lstInfos.Add(new HTSocketInfo
                    {
                        IP = item.Attribute("IP").Value,
                        Port = Convert.ToInt32(item.Attribute("Port").Value)
                    });
                }
                ConnectionsCount = lstInfos.Count;
                PlcCtrl = new PlcWrapper();
                PlcCtrl.Init(lstInfos.ToArray());
                if (PlcCtrl.IsConnected)
                {
                    FormMain.Instance.ShowMsg("PLC连接成功！", Brushes.Green);
                    FormMain.Instance.AppendTxt3DLog("PLC连接成功！", Color.Green);
                    FormMain.Instance.ShowConnectInfo("PLC", true);
                    //FormMain.Instance.AppendTxt2DLog( "PLC连接成功！", Color.Green);
                    FormMain.Instance.UpdateDeviceState("PLC", true);
                }
                else
                {
                    FormMain.Instance.ShowMsg("PLC连接失败！", Brushes.Red);
                    FormMain.Instance.AppendTxt3DLog("PLC连接失败！", Color.Red);
                    FormMain.Instance.ShowConnectInfo("PLC", false);
                    //FormMain.Instance.AppendTxt2DLog("PLC连接失败！", Color.Green);
                    FormMain.Instance.UpdateDeviceState("PLC", false);
                }
            }
            catch (Exception ex)
            {
                FormMain.Instance.UpdateDeviceState("PLC", false);
                FormMain.Instance.ShowMsg("PLC连接失败！" + ex.Message, Brushes.Red);
                FormMain.Instance.AppendTxt3DLog("PLC连接失败！" + ex.Message, Color.Red);
                FormMain.Instance.ShowConnectInfo("PLC", false);
                //FormMain.Instance.AppendTxt2DLog("PLC连接失败！", Color.Green);
                LogManager.GetLogger("Error").Error(ex);
            }
            Thread thRead = new Thread(PlcRead);
            thRead.IsBackground = true;
            thRead.Start();
            Thread thWrite = new Thread(PlcWrite);
            thWrite.IsBackground = true;
            thWrite.Start();
        }

        /// <summary>
        /// 获取连接状态
        /// </summary>
        /// <returns></returns>
        public static bool GetConnected()
        {
            if (PlcCtrl == null)
            {
                return false;
            }
            return PlcCtrl.IsConnected;
        }

        /// <summary>
        /// 重连和心跳
        /// </summary>
        public static void PlcWrite()
        {
            while (isRunning)
            {
                try
                {
                    if (GetConnected())
                    {
                        FormMain.Instance.UpdateDeviceState("PLC", true);
                    }
                    else
                    {
                        FormMain.Instance.UpdateDeviceState("PLC", false);
                        PlcCtrl.ReInit();
                    }
                    if (Global.CurCheckCam == CheckCam.Cam2D)
                    {
                        PlcCtrl.WriteBits(PlcAddr.PC2DHeartbeatSign, true);
                        Thread.Sleep(500);
                        PlcCtrl.WriteBits(PlcAddr.PC2DHeartbeatSign, false);
                        Thread.Sleep(500);
                    }
                    else
                    {
                        PlcCtrl.WriteBits(PlcAddr.PC3DHeartbeatSign, true);
                        Thread.Sleep(500);
                        PlcCtrl.WriteBits(PlcAddr.PC3DHeartbeatSign, false);
                        Thread.Sleep(500);
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// 循环读取PLC信号和数据
        /// </summary>
        public static void PlcRead()
        {
            Thread.Sleep(5000);
            while (isRunning)
            {
                try
                {
                    if (Global.CurCheckCam == CheckCam.Cam2D &&
                        (Global.CurCheckType == CheckType.Raster || Global.CurCheckType == CheckType.ScreenLayer))
                    {
                        if (string.IsNullOrEmpty(FormMain.Instance.GetCurRecipeID()))
                        {
                            Thread.Sleep(500);
                            continue;
                        }
                    }
                    if (GetConnected() == false)
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    //判断设备初始化完成
                    //**********数据更新************************
                    //判断是否有2D拍照请求 3D拍照请求   从M16000开始读
                    bool[] dataReadBits = PlcCtrl.ReadBits(PlcAddr.BitReadStart, 100);
                    //根据读取的数据更新状态（分层和光栅不一致 光栅的时候分层的虽然更新了，但是不起作用）
                    ReadBits.Update(dataReadBits);
                    //ushort[] dataReadWords = AdvancedReadWords(PlcAddr.RegReadStart, 200);
                    //ReadWords.Update(dataReadWords);

                    //**********信号处理***********************
                    PlcSignals.ProcessSignals();
                    //**********保存上一次的信号***********************
                    LastReadBits.Update(dataReadBits);
                }
                catch (Exception ex)
                {
                    if (Global.CurCheckCam == CheckCam.Cam2D)
                    {
                        FormMain.Instance.ShowMsg("PLC读取数据失败！" + ex.Message, Brushes.Red);
                    }
                    else
                    {
                        FormMain.Instance.AppendTxt3DLog("PLC读取数据失败！" + ex.Message, Color.Red);
                    }
                    LogManager.GetLogger("Error").Error(ex);
                    Thread.Sleep(1000);
                }
                Thread.Sleep(20);
            }
        }

        private static ushort[] AdvancedReadWords(string addr, int count)
        {
            List<ushort> lstContent = new List<ushort>();
            ushort alreadyFinished = 0;
            while (alreadyFinished < count)
            {
                string readAddr = PlcAddr.GetAddr(addr, alreadyFinished);
                ushort readLength = (ushort)Math.Min(count - alreadyFinished, 900);
                ushort[] datas = PlcCtrl.ReadWords(readAddr, readLength);
                lstContent.AddRange(datas);
                alreadyFinished += readLength;
            }
            return lstContent.ToArray();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public static void UnInit()
        {
            try
            {
                isRunning = false;
                if (PlcCtrl != null)
                {
                    PlcCtrl.Dispose();
                    PlcCtrl = null;
                }
            }
            catch (Exception)
            { }
        }
    }

    /// <summary>
    /// Plc对象的二次封装，多个连接对象合成一个，循环使用
    /// </summary>
    class PlcWrapper : IDisposable
    {
        private MelsecMcNet[] plcs = null;
        private bool[] isConnected = null;
        private int[] errorCounts = null;
        private int connections = 1;
        private int curConnection = -1;
        private object lockObjConn = new object();

        /// <summary>
        /// 连接多个端口
        /// </summary>
        /// <param name="infos"></param>
        public void Init(HTSocketInfo[] infos)
        {
            if (infos.Length < 0) throw new Exception("Socket info length error");

            connections = infos.Length;
            plcs = new MelsecMcNet[connections];
            isConnected = new bool[connections];
            errorCounts = new int[connections];

            for (int i = 0; i < connections; i++)
            {
                plcs[i] = new MelsecMcNet() { IpAddress = infos[i].IP, Port = infos[i].Port };
                var result = plcs[i].ConnectServer();
                isConnected[i] = result.IsSuccess;
            }

            PlcAddr.Load();
        }

        /// <summary>
        /// 连接单个端口
        /// </summary>
        /// <param name="info"></param>
        public void Init(HTSocketInfo info)
        {
            Init(new HTSocketInfo[] { info });
        }

        /// <summary>
        /// 断开重连
        /// </summary>
        public void ReInit()
        {
            for (int i = 0; i < connections; i++)
            {
                if (isConnected[i] == false)
                {
                    plcs[i].ConnectClose();
                    var result = plcs[i].ConnectServer();
                    isConnected[i] = result.IsSuccess;
                }
            }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (plcs == null) return false;
                bool flag = true;
                for (int i = 0; i < connections; i++)
                {
                    if (isConnected[i] == false)
                    {
                        flag = false;
                        break;
                    }
                }

                return flag;
            }
        }

        /// <summary>
        /// 写字
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="datas"></param>
        public void WriteWords(string addr, params ushort[] datas)
        {
            int conn = GetCurConnIndex();
            var result = plcs[conn].Write(addr, datas);
            if (!result.IsSuccess)
            {
                errorCounts[conn]++;
                if (errorCounts[conn] > 5)
                {
                    isConnected[conn] = false;
                }
                throw new Exception(result.Message);
            }
            else
            {
                errorCounts[conn] = 0;
            }
        }
        /// <summary>
        /// 写浮点数字
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="datas"></param>
        public void WriteDoubleWords(string addr, params double[] datas)
        {
            int conn = GetCurConnIndex();
            var result = plcs[conn].Write(addr, datas);
            if (!result.IsSuccess)
            {
                errorCounts[conn]++;
                if (errorCounts[conn] > 5)
                {
                    isConnected[conn] = false;
                }
                throw new Exception(result.Message);
            }
            else
            {
                errorCounts[conn] = 0;
            }
        }

        /// <summary>
        /// 写位
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="datas"></param>
        public void WriteBits(string addr, params bool[] datas)
        {
            int conn = GetCurConnIndex();
            var result = plcs[conn].Write(addr, datas);
            if (!result.IsSuccess)
            {
                errorCounts[conn]++;
                if (errorCounts[conn] > 5)
                {
                    isConnected[conn] = false;
                }
                throw new Exception(result.Message);
            }
            else
            {
                errorCounts[conn] = 0;
            }
        }

        /// <summary>
        /// 读字
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ushort[] ReadWords(string addr, int count)
        {
            int conn = GetCurConnIndex();
            var result = plcs[conn].ReadUInt16(addr, (ushort)count);
            if (!result.IsSuccess)
            {
                errorCounts[conn]++;
                if (errorCounts[conn] > 5)
                {
                    isConnected[conn] = false;
                }
                throw new Exception(result.Message);
            }
            else
            {
                errorCounts[conn] = 0;
            }
            return result.Content;
        }

        /// <summary>
        /// 读浮点数
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public double[] ReadDoubletWords(string addr, int count)
        {
            int conn = GetCurConnIndex();
            var result = plcs[conn].ReadDouble(addr, (ushort)count);
            if (!result.IsSuccess)
            {
                errorCounts[conn]++;
                if (errorCounts[conn] > 5)
                {
                    isConnected[conn] = false;
                }
                throw new Exception(result.Message);
            }
            else
            {
                errorCounts[conn] = 0;
            }
            return result.Content;
        }

        /// <summary>
        /// 读位
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool[] ReadBits(string addr, int count)
        {
            int conn = GetCurConnIndex();
            var result = plcs[conn].ReadBool(addr, (ushort)count);
            if (!result.IsSuccess)
            {
                errorCounts[conn]++;
                if (errorCounts[conn] > 5)
                {
                    isConnected[conn] = false;
                }
                throw new Exception(result.Message);
            }
            else
            {
                errorCounts[conn] = 0;
            }
            return result.Content;
        }

        /// <summary>
        /// 获取当前使用的连接索引
        /// </summary>
        /// <returns></returns>
        private int GetCurConnIndex()
        {
            lock (lockObjConn)
            {
                curConnection++;
                if (curConnection >= connections)
                {
                    curConnection = 0;
                }
                return curConnection;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Dispose()
        {
            if (plcs != null)
            {
                for (int i = 0; i < connections; i++)
                {
                    try
                    {
                        if (plcs[i] != null)
                        {
                            plcs[i].Dispose();
                            plcs[i] = null;
                        }
                    }
                    catch (Exception)
                    { }
                }
                plcs = null;
            }
        }
    }
}
