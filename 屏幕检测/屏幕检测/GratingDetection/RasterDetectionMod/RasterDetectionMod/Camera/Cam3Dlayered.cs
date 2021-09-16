/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：Cam3Dlayered.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 17:10:08 
// 功能描述 ：分层检测3D
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
    public class Cam3Dlayered
    {
        /// <summary>
        /// 点云分层接口
        /// </summary>
        /// <param name="szDepthPath">depth.txt 文件路径，如：C:\\depth.txt</param>
        /// <param name="szCenterPath">center.txt 文件路径，如：C:\\center.txt</param>
        /// <param name="szCaliPath">cali.txt 文件路径，如：C:\\cali.txt</param>
        /// <param name="szOutDir">点云分层结果生成文件夹路径，如：C:\\OutDir</param>
        /// <param name="iParams">[0]-图像宽度 [1]-图像高度 [2]-样品层数(fParams 数组长度) [3]-检测结果(szResult)数组长度</param>
        /// <param name="fParams">样品每一层的层高</param>
        /// <param name="szResult">检测结果返回</param>
        /// <returns></returns>
        [DllImport("VOMMAPointCloudStratification.dll")]
        public static extern int GetDetectResult(string szDepthPath, string szCenterPath, string szCaliPath, string szOutDir, int[] iParams, float[] fParams, byte[] szResult);
    }
}
