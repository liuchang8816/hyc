/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod
// 文件名称 ：VommaCameraSetting.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 17:10:08 
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

namespace RasterDetectionMod
{
    public partial class FormVommaCameraSetting : Form
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public FormVommaCameraSetting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体启动，加载参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraSetting_Load(object sender, EventArgs e)
        {
            this.txtLevel.Text = Camera3D.Camera3DDepthParam.level.ToString();
            this.txtMaxDepth.Text = Camera3D.Camera3DDepthParam.maxDepth.ToString();
            this.txtMinDepth.Text = Camera3D.Camera3DDepthParam.minDepth.ToString();
            this.txtTexture.Text = Camera3D.Camera3DDepthParam.texture.ToString();
            this.txtSmoothWnd.Text = Camera3D.Camera3DDepthParam.smoothWindow.ToString();
            this.txtDetection.Text = Camera3D.Camera3DDepthParam.detection.ToString();
            this.txtBORW.Text = Camera3D.Camera3DDepthParam.BORW.ToString();
            this.txtThreshold.Text = Camera3D.Camera3DDepthParam.thre.ToString();
            this.txtStepVal.Text = Camera3D.Camera3DDepthParam.step_value.ToString();

            this.txtThreArea.Text = Camera3D.ThreArea.ToString();
            this.txtCamera3DExposure.Text = Camera3D.Camera3DExposure.ToString();
            this.txtCamera3DExposureMark.Text = Camera3D.Camera3DExposureMark.ToString();
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            //if (Is3D)
            try
            {
                Camera3D.Camera3DDepthParam.level = int.Parse(this.txtLevel.Text);
                Camera3D.Camera3DDepthParam.maxDepth = int.Parse(this.txtMaxDepth.Text);
                Camera3D.Camera3DDepthParam.minDepth = int.Parse(this.txtMinDepth.Text);
                Camera3D.Camera3DDepthParam.texture = int.Parse(this.txtTexture.Text);
                Camera3D.Camera3DDepthParam.smoothWindow = int.Parse(this.txtSmoothWnd.Text);
                Camera3D.Camera3DDepthParam.detection = int.Parse(this.txtDetection.Text);
                Camera3D.Camera3DDepthParam.BORW = int.Parse(this.txtBORW.Text);
                Camera3D.Camera3DDepthParam.thre = float.Parse(this.txtThreshold.Text);
                Camera3D.Camera3DDepthParam.step_value = float.Parse(this.txtStepVal.Text);

                Camera3D.ThreArea = float.Parse(this.txtThreArea.Text);
                Camera3D.Camera3DExposure = long.Parse(this.txtCamera3DExposure.Text);
                Camera3D.Camera3DExposureMark = long.Parse(this.txtCamera3DExposureMark.Text);

                if (Camera3D.CamSetDepthParam(
                   Camera3D.Camera3DDepthParam.level,
                   Camera3D.Camera3DDepthParam.maxDepth,
                   Camera3D.Camera3DDepthParam.minDepth,
                   Camera3D.Camera3DDepthParam.texture,
                   Camera3D.Camera3DDepthParam.smoothWindow,
                   Camera3D.Camera3DDepthParam.detection,
                   Camera3D.Camera3DDepthParam.BORW,
                   Camera3D.Camera3DDepthParam.thre,
                   Camera3D.Camera3DDepthParam.step_value
                   , true))
                {
                    Camera3D.SaveConfig();
                    MessageBox.Show("设置参数成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
