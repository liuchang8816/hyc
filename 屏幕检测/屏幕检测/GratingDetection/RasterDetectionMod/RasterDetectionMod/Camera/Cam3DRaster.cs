/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：Cam3DRaster.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 17:10:08 
// 功能描述 ：光栅检测3D
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
    public class Cam3DRaster
    {
        [DllImport("VOMMAGratingApi.dll")]
        public static extern int VOMMAGratingStratification(byte[] szFileDir, byte[] szCaliPath, byte[] szModelPath, byte[] szPlyDir, int iRows, int iCols, byte[] szResult);

        /// <summary>
        /// 光栅算法接口
        /// </summary>
        /// <param name="szTXTDir">depth.txt 和 center.txt 所在文件夹，如：C:\\filesDir</param>
        /// <param name="szMarkPath">mark.txt 所在文件夹，如：C:\\markDir</param>
        /// <param name="szModelPath">标记点模板文件路径，如：C:\\mark.txt</param>
        /// <param name="szCaliPath">校准文件 cali.txt 文件路径，如：C:\\cali.txt</param>
        /// <param name="szOutDir">输出点云的文件夹，如：C:\\outfileDir</param>
        /// <param name="iParams">[0]-图像宽度 [1]-图像高度 [2]-光栅片数(1,2,3)</param>
        /// <param name="fParams">[0]-缺陷面积阈值，以像素为单位</param>
        /// <param name="szResult">返回缺陷信息</param>
        /// <returns></returns>
        [DllImport("VOMMAGratingApi.dll")]
        public static extern int GetDetectResult(string szTXTDir, string szMarkPath, string szModelPath, string szCaliPath, string szOutDir, int[] iParams, float[] fParams, byte[] szResult);
        //int GetDetectResult(const char* szTXTDir, const char* szMarkPath, const char* szModelPath, const char* szCaliPath, const char* szOutDir, int iParams[], float fParams[], char* szResult);

        [DllImport("VOMMAGratingApi.dll")]
        public static extern int VOMMADetectCornerMark(string szCaliPath, byte[] data, int iRows, int iCols, ref float fPos_x, ref float fPos_y);
    }
}
