/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Forms 
// 文件名称 ：FormIndexImage.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/6/15 20:10:55 
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod.Forms
{
    public partial class FormIndexImage : Form
    {
        public FormIndexImage()
        {
            InitializeComponent();
        }

        public void ShowImage(Image img)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Image>(ShowImage), img);
            }
            else
            {
                this.pictImage.Image = img;
            }
        }
    }
}
