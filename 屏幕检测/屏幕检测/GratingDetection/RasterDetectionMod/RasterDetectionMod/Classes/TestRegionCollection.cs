/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Recipes 
// 文件名称 ：TestRegionCollection.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/22 14:00:17 
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

namespace RasterDetectionMod.Classes
{
    [Serializable]
    public class TestRegionCollection
    {
        /// <summary>
        /// 3D相机的视野大小Width
        /// </summary>
        public float Size3dW { get; set; }
        /// <summary>
        /// 3D相机的视野大小Height
        /// </summary>
        public float Size3dH { get; set; }
        /// <summary>
        /// 总行数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 总列数
        /// </summary>
        public int Columns { get; set; }
        /// <summary>
        /// 所有区域的信息
        /// </summary>
        public List<TestRegion> ListRegions { get; set; } = new List<TestRegion>();
    }

    [Serializable]
    public class TestRegion
    {
        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        public int Column { get; set; }
        /// <summary>
        /// X偏移
        /// </summary>
        public float XPosition { get; set; }
        /// <summary>
        /// Y偏移
        /// </summary>
        public float YPosition { get; set; }
    }
}
