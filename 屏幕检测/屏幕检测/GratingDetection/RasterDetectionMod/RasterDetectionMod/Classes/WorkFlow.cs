/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：IWorkFlow.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/6/7 15:06:10 
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
    public abstract class WorkFlow
    {
        /// <summary>
        /// 自动模式下当前3D拍照的次数  2D拍照开始后重置 PLC发信号调用算法后重置
        /// </summary>
        public int PhotoNum = 0;

        /// <summary>
        /// 偏移X
        /// </summary>
        public float OffsetX = 0;
        /// <summary>
        /// 偏移Y
        /// </summary>
        public float OffsetY = 0;
        /// <summary>
        /// 区域所在行
        /// </summary>
        public int Row = 0;
        /// <summary>
        /// 区域所在列
        /// </summary>
        public int Column = 0;

        /// <summary>
        /// 初始化，创建产品文件夹
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// 创建缺陷文件夹
        /// </summary>
        public abstract void CreateFolder();
        /// <summary>
        /// 3D自动拍照
        /// </summary>
        public abstract void Grab();
        /// <summary>
        /// 3D自动拍照
        /// </summary>
        public abstract void GrabMark();
        /// <summary>
        /// 调用3D光栅检测算法（6层）
        /// </summary>
        public abstract void CheckPhoto();
        /// <summary>
        /// 结果整合、去重
        /// </summary>
        public abstract void GetResult();
    }
}
