/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：StandardAoiDemo.Controls
// 文件名称    ：UcMilCamera.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/4/16 10:51:38 
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
using HYC.StandardAoi.Algorithm;
using HYC.HTImage;
using HYC.StandardAoi.Parameter;
using HYC.HTCamera.Card.Matrox;
using RasterDetectionMod.Entities;

namespace RasterDetectionMod.Controls
{
    /// <summary>
    /// Mil相机控制
    /// </summary>
    public partial class UcMilCamera : UserControl
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public UcMilCamera()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitCtrl()
        {
            for (int i = 0; i < AoiCamera.CameraCount; i++)
            {
                this.cboCamera.Items.Add("相机" + (i + 1));
            }
            this.cboCamera.SelectedIndex = 0;
        }

        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrab_Click(object sender, EventArgs e)
        {
            int index = this.cboCamera.SelectedIndex;
            bool graytest = this.chkGrayTest.Checked;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    // 清除界面显示
                    this.Invoke(new Action(() =>
                    {
                        this.pictImg.Image = null;
                        this.btnGrab.Enabled = false;
                    }));
                    // 拍照
                    ImageData data = AoiCamera.Grab2d(index);
                    if (data != null)
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.pictImg.Image = data.ToBitmap();
                        }));
                        if (graytest)
                        {
                            GrayTest(data.ImageBytes, data.Height, data.Width);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        this.btnGrab.Enabled = true;
                    }));
                }
            });
        }

        /// <summary>
        /// 获取曝光时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetExposure_Click(object sender, EventArgs e)
        {
            int index = this.cboCamera.SelectedIndex;
            try
            {
                // 清除界面显示
                this.txtExposure.Text = string.Empty;
                // 获取
                float result = AoiCamera.GetExposureTime(index);
                this.txtExposure.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetExposure_Click(object sender, EventArgs e)
        {
            int index = this.cboCamera.SelectedIndex;
            try
            {
                int value = Convert.ToInt32(this.txtExposure.Text);
                bool ret = AoiCamera.SetExposureTime(index, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取相机增益
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetGain_Click(object sender, EventArgs e)
        {
            int index = this.cboCamera.SelectedIndex;
            try
            {
                // 清除界面显示
                this.txtGain.Text = string.Empty;
                // 获取
                float result = AoiCamera.GetGain(index);
                this.txtGain.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置相机增益
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetGain_Click(object sender, EventArgs e)
        {
            int index = this.cboCamera.SelectedIndex;
            try
            {
                float value = Convert.ToSingle(this.txtGain.Text);
                AoiCamera.SetGain(index, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 图片灰度及ROI检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImgTestAoi_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图片文件(*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";
            dlg.Multiselect = false;
            DialogResult dr = dlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.txtGrayLevel.Text = string.Empty;
                this.txtCellAoi.Text = string.Empty;

                string filename = dlg.FileName;
                int width = 0, height = 0;
                var bitmap = (Bitmap)Image.FromFile(filename);
                Bitmap bitmapA = BitmapConverter.Gray(bitmap);
                byte[] data = BitmapConverter.BitmapToArray(bitmapA, out height, out width);
                Task.Factory.StartNew(() =>
                {
                    GrayTest(data, height, width);
                });
            }
        }

        /// <summary>
        /// 图片灰度及ROI检测
        /// </summary>
        /// <param name="dataIn">图像数据</param>
        /// <param name="h">图像的height</param>
        /// <param name="w">图像的width</param>
        private void GrayTest(byte[] dataIn, int h, int w)
        {
            try
            {
                var dataOut = new byte[h * w * 3];
                ShowAoiValue("");
                double dd = 0;
                int[] aoi = new int[4];
                //算法计算
                if (chkBlack.Checked)
                {
                    WinCCTool.HT_GrayTest2(dataIn, dataOut, w, h, ref dd, aoi);
                }
                else
                {
                    WinCCTool.HT_GrayTest(dataIn, dataOut, w, h, ref dd, aoi);
                }

                string str = "";
                for (int i = 0; i < 4; i++)
                {
                    str += "," + aoi[i];
                }
                ShowAoiValue(str.Substring(1));

                Bitmap bitmap = BitmapConverter.ArrayToRgbBitmap(dataOut, h, w);
                ShowAoiImage(bitmap);
                ShowGrayValue(dd.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 显示ROI的值
        /// </summary>
        /// <param name="strValue"></param>
        private void ShowAoiValue(string strValue)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowAoiValue), strValue);
            }
            else
            {
                txtCellAoi.Text = strValue;
            }
        }

        /// <summary>
        /// 显示灰度值
        /// </summary>
        /// <param name="strValue"></param>
        private void ShowGrayValue(string strValue)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowGrayValue), strValue);
            }
            else
            {
                txtGrayLevel.Text = strValue;
            }
        }

        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="bitmap"></param>
        public void ShowAoiImage(Bitmap bitmap)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Bitmap>(ShowAoiImage), bitmap);
            }
            else
            {
                this.pictImg.Image = bitmap;
            }
        }
    }
}
