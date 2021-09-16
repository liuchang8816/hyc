/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Recipes 
// 文件名称 ：LayerAlgorithmPara.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/6/10 14:27:20 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.Recipes
{
    /// <summary>
    /// 分层算法参数
    /// </summary>
    [Serializable]
    public class LayerAlgorithmPara
    {
        /// <summary>
        /// 层数
        /// </summary>
        public int LayerCount { get; set; }
        /// <summary>
        /// 每一层的高度
        /// </summary>
        public List<float> ListLayerHeight { get; set; } = new List<float>();
    }
}
