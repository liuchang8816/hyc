/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod 
// 文件名称 ：FormRasterPara.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 16:29:37 
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
using HYC.HTLog;
using RasterDetectionMod.Recipes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod
{
    /// <summary>
    /// 光栅检测相关参数
    /// </summary>
    public partial class FormRasterPara : Form
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public FormRasterPara()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormRasterPara_Load(object sender, EventArgs e)
        {
            var para = RecipeManager.RasterJudgePara;
            this.txtDefectCount.Text = para.DefectCount.ToString();
            this.txtDefectSize.Text = para.DefectSize.ToString();
            this.txtDefectWidth.Text = para.DefectWidth.ToString();

            var layerPara = RecipeManager.LayerAlgorithmPara;
            this.txtLayerCount.Text = layerPara.LayerCount.ToString();
            TextBox[] texts = new TextBox[] { txtLayerHeight1, txtLayerHeight2, txtLayerHeight3, txtLayerHeight4, txtLayerHeight5 };
            for (int i = 0; i < texts.Length; i++)
            {
                if (i < layerPara.LayerCount && layerPara.ListLayerHeight != null)
                {
                    texts[i].Text = layerPara.ListLayerHeight[i].ToString();
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var para = RecipeManager.RasterJudgePara;
                para.DefectCount = Convert.ToInt32(this.txtDefectCount.Text);
                para.DefectSize = Convert.ToDouble(this.txtDefectSize.Text);
                para.DefectWidth = Convert.ToDouble(this.txtDefectWidth.Text);

                var layerPara = RecipeManager.LayerAlgorithmPara;
                layerPara.LayerCount = Convert.ToInt32(this.txtLayerCount.Text);
                layerPara.ListLayerHeight.Clear();
                TextBox[] texts = new TextBox[] { txtLayerHeight1, txtLayerHeight2, txtLayerHeight3, txtLayerHeight4, txtLayerHeight5 };
                for (int i = 0; i < layerPara.LayerCount; i++)
                {
                    float val = Convert.ToSingle(texts[i].Text);
                    layerPara.ListLayerHeight.Add(val);
                }

                RecipeManager.Save();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("Error").Error(ex);
                MessageBox.Show("保存失败!");
            }

        }
    }
}
