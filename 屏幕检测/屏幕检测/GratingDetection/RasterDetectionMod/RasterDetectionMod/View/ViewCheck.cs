/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：SurfaceDetect.View
// 文件名称    ：ViewCheck.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/4/9 14:31:28 
// 功能描述    ：
// 使用说明    ：
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
using HYC.HTImage;
using HYC.StandardAoi.Algorithm;
using HYC.StandardAoi.Parameter;
using HYC.StandardAoi.UI.View;
using RasterDetectionMod.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod.View
{
    /// <summary>
    /// 设备点检视图
    /// </summary>
    public partial class ViewCheck : ViewBaseEx
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ViewCheck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewCheck_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                ucMilCamera1.InitCtrl();
                ucPower1.InitCtrl();
            }
        }

        private void btnMread_Click(object sender, EventArgs e)
        {
            bool[] dataReadBits = PlcManager.PlcCtrl.ReadBits("M" + this.txtMAddress.Text, 1);
            if (dataReadBits[0] == true)
            {
                this.comboBox1.SelectedIndex = 0;
            }
            else
            {
                this.comboBox1.SelectedIndex = 1;
            }
        }

        private void btnMWrite_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == 0)
            {
                PlcManager.PlcCtrl.WriteBits("M" + this.txtMAddress.Text, true);
            }
            else
            {
                PlcManager.PlcCtrl.WriteBits("M" + this.txtMAddress.Text, false);
            }
        }

        private void btnDread_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                double[] datas = PlcManager.PlcCtrl.ReadDoubletWords("D" + this.txtDAddress.Text, 1);
                this.txtDValue.Text = datas[0].ToString();
            }
            else
            {
                ushort[] datas = PlcManager.PlcCtrl.ReadWords("D" + this.txtDAddress.Text, 1);
                this.txtDValue.Text = datas[0].ToString();
            }
        }

        private void btnDWrite_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                PlcManager.PlcCtrl.WriteDoubleWords("D" + this.txtDAddress.Text, Convert.ToDouble(this.txtDValue.Text));
            }
            else
            {
                PlcManager.PlcCtrl.WriteWords("D" + this.txtDAddress.Text, Convert.ToUInt16(this.txtDValue.Text));
            }
        }
    }
}
