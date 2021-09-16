/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Models 
// 文件名称 ：RasterResultModel.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/5/17 16:41:49 
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.Models
{
    /// <summary>
    /// 光栅结果类
    /// </summary>
    public class RasterResultModel
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public string PanelID { get; set; }
        /// <summary>
        /// 检测开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 检测结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 检测结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 不良数量
        /// </summary>
        public int DefectCount { get; set; }
        /// <summary>
        /// 不良原因
        /// </summary>
        public string Reason { get; set; }

        public void WriteToFile()
        {
            //写csv
            string folder = "D:\\Result\\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string filename = folder + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            if (!File.Exists(filename))
            {
                //文件不存在表头，写表头
                //写表头，产品ID,检测时间，OK/NG,不良数量
                string strHeader = "PanelID,StartTime,EndTime,Result,DefectCount,Reason\r\n";
                File.WriteAllText(filename, strHeader);
            }
            //写内容
            string strContent = PanelID + "," + StartTime + "," + EndTime + "," + Result + "," + DefectCount + "," + Reason + "\r\n";
            File.AppendAllText(filename, strContent);//写到文件里
        }
    }
}
