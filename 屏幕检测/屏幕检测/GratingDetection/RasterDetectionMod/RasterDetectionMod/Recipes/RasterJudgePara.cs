/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Recipes 
// 文件名称 ：RasterJudgePara.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/5/17 15:43:50 
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

namespace RasterDetectionMod.Recipes
{
    [Serializable]
    public class RasterJudgePara
    {
        /// <summary>
        /// 缺陷数量
        /// </summary>
        public int DefectCount { get; set; }
        /// <summary>
        /// 缺陷面积
        /// </summary>
        public double DefectSize { get; set; }
        /// <summary>
        /// 缺陷长宽
        /// </summary>
        public double DefectWidth { get; set; }
    }
}
