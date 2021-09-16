/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：RasterDetectionMod.Controls
// 文件名称    ：UcPower.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/8/21 17:11:22 
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RasterDetectionMod.Entities;
using HYC.HTPower;

namespace RasterDetectionMod.Controls
{
    /// <summary>
    /// 光源控制
    /// </summary>
    public partial class UcPower : UserControl
    {
        private CheckBox[] checkboxes = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        public UcPower()
        {
            InitializeComponent();
            checkboxes = new CheckBox[] { chkChannel1, chkChannel2, chkChannel3, chkChannel4 };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitCtrl()
        {
            for (int i = 0; i < PowerManager.PowerCount; i++)
            {
                this.cboPower.Items.Add("光源" + (i + 1));
            }
            this.cboPower.SelectedIndex = 0;
        }

        /// <summary>
        /// 开启光源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowerOn_Click(object sender, EventArgs e)
        {
            int index = this.cboPower.SelectedIndex;
            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (checkboxes[i].Checked)
                {
                    PowerManager.GetPower(index).PowerOn(HTPowerChannel.POWER_CHANNEL_1 + i);
                }
            }
        }

        /// <summary>
        /// 关闭光源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            int index = this.cboPower.SelectedIndex;
            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (checkboxes[i].Checked)
                {
                    PowerManager.GetPower(index).PowerOff(HTPowerChannel.POWER_CHANNEL_1 + i);
                }
            }
        }
    }
}
