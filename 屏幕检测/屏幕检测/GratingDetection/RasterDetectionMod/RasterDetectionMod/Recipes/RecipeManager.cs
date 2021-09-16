/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Recipes 
// 文件名称 ：RecipeManager.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/5/17 15:50:29 
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
using HYC.HTFile.HTXML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.Recipes
{
    /// <summary>
    /// 机种参数关联
    /// </summary>
    public class RecipeManager
    {
        /// <summary>
        /// 分层算法参数
        /// </summary>
        public static LayerAlgorithmPara LayerAlgorithmPara { get; set; } = new LayerAlgorithmPara();
        /// <summary>
        /// 光栅检测参数
        /// </summary>
        public static RasterJudgePara RasterJudgePara { get; set; } = new RasterJudgePara();

        /// <summary>
        /// 加载参数
        /// </summary>
        public static void Load()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "Recipes\\";
            string filename = folder + "RasterJudgePara.xml";
            if (File.Exists(filename))
            {
                RasterJudgePara = HTXMLSerializer.EntityFromXMLFile<RasterJudgePara>(filename);
            }

            filename = folder + "LayerAlgorithmPara.xml";
            if (File.Exists(filename))
            {
                LayerAlgorithmPara = HTXMLSerializer.EntityFromXMLFile<LayerAlgorithmPara>(filename);
            }
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        public static void Save()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "Recipes\\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string filename = folder + "RasterJudgePara.xml";
            HTXMLSerializer.EntityToXMLFile(RasterJudgePara, filename);

            filename = folder + "LayerAlgorithmPara.xml";
            HTXMLSerializer.EntityToXMLFile(LayerAlgorithmPara, filename);
        }
    }
}
