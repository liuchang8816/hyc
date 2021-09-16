/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：StandardAoi.Controls
// 文件名称    ：UcDetectionGroupEx.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/12/30 16:02:42 
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
using HYC.StandardAoi;
using HYC.StandardAoi.Parameter;
using HYC.StandardAoi.UI.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RasterDetectionMod.Controls
{
    /// <summary>
    /// 单个检测通道的图片、结果展示控件
    /// </summary>
    public partial class UcDetectionGroupEx : UserControl
    {
        /// <summary>
        /// 检测管理类
        /// </summary>
        private StandardAoiManager manager = null;

        /// <summary>
        /// 检测类型
        /// </summary>
        public DetectionType DetectionType { get; set; } = DetectionType.Aoi;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return this.pictImg.Title; }
            set { this.pictImg.Title = value; }
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        public string PanelID
        {
            get { return this.lblPanelID.Text; }
            set { this.lblPanelID.Text = value; }
        }

        /// <summary>
        /// 图像显示控件
        /// </summary>
        public PatternPictureBox ImageBox
        {
            get { return this.pictImg; }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public UcDetectionGroupEx()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置对象
        /// </summary>
        /// <param name="manager">检测管理对象</param>
        public void SetManager(StandardAoiManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// 显示产品ID
        /// </summary>
        /// <param name="id">产品ID</param>
        public void ShowPanelID(string id)
        {
            this.Invoke(new Action(() =>
            {
                this.lblPanelID.Text = id;
            }));
        }

        /// <summary>
        /// 显示检测进度
        /// </summary>
        /// <param name="percent">百分比 0.0-1.0</param>
        public void ShowProgress(double percent)
        {
            this.Invoke(new Action(() =>
            {
                this.progressBar1.Value = (int)(percent * this.progressBar1.Maximum);
            }));
        }

        /// <summary>
        /// 显示算法处理后的图片
        /// </summary>
        /// <param name="result">检测结果类</param>
        public void ShowProcessedImages(DetectionResult result)
        {
            this.Invoke(new Action(() =>
            {
                if (result != null)
                {
                    // 显示图像
                    var data = this.manager.CurRecipe.Data;
                    var pattNames = data.LcdInfo.Pat.Select(p => p.PattName)
                        .Take(data.LcdInfo.nCountPattern).ToArray();
                    this.pictImg.UpdateImages(result.ProcessedImages, pattNames);
                    // 显示第一张不良图
                    int defaultIndex = this.manager.DetectionType == DetectionType.Aoi ? 1 : 0;
                    int patternIndex = result.Detail.ListPattern.Count > 0 ?
                        result.Detail.ListPattern[0].PatternIndex : defaultIndex;
                    this.pictImg.ShowImage(patternIndex);
                }
                else
                {
                    this.pictImg.ClearImages();
                }
            }));
        }

        /// <summary>
        /// 显示检测结果
        /// </summary>
        /// <param name="result">检测结果类</param>
        public void ShowDetectionResult(DetectionResult result)
        {
            this.Invoke(new Action(() =>
            {
                if (result != null)
                {
                    // 显示结果
                    if (result.Detail.TestResult == ResultType.OK)
                    {
                        this.lblResult.Text = "OK";
                        this.lblResult.BackColor = Color.YellowGreen;
                        this.lblResult.ForeColor = Color.White;
                    }
                    else if (result.Detail.TestResult == ResultType.NG)
                    {
                        this.lblResult.Text = "NG";
                        if (!string.IsNullOrEmpty(result.Detail.NgText))
                            this.lblResult.Text += "(" + result.Detail.NgText + ")";
                        this.lblResult.BackColor = Color.Red;
                        this.lblResult.ForeColor = Color.White;
                    }
                    else if (result.Detail.TestResult == ResultType.Xsyc)
                    {
                        this.lblResult.Text = "显示异常";
                        this.lblResult.BackColor = Color.Red;
                        this.lblResult.ForeColor = Color.White;
                    }
                    else if (result.Detail.TestResult == ResultType.NULL)
                    {
                        this.lblResult.Text = "异常结束";
                        this.lblResult.BackColor = Color.Red;
                        this.lblResult.ForeColor = Color.White;
                    }
                    // 显示结果明细
                    this.dgvResult.DataSource = result.Summary.Results;
                }
                else
                {
                    this.lblResult.Text = "---";
                    this.lblResult.BackColor = SystemColors.Control;
                    this.lblResult.ForeColor = SystemColors.ControlText;
                    this.dgvResult.DataSource = null;
                }
            }));
        }

        /// <summary>
        /// 点击单元格，切换NG画面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                // 获取点击行的pattName
                string pattName = dgvResult.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (string.IsNullOrEmpty(pattName))
                {
                    return;
                }

                var data = this.manager.CurRecipe.Data;

                // 根据pattName，获取patternIndex
                int patternIndex = -1;
                for (int i = 0; i < data.LcdInfo.nCountPattern; i++)
                {
                    if (data.LcdInfo.Pat[i].PattName.Equals(pattName))
                    {
                        patternIndex = i;
                        break;
                    }

                    // 控件中显示的可能是备注名
                    if (!string.IsNullOrEmpty(data.LcdInfo.Pat[i].Remark))
                    {
                        if (data.LcdInfo.Pat[i].Remark.Equals(pattName))
                        {
                            patternIndex = i;
                            break;
                        }
                    }
                }
                if (patternIndex < 0)
                {
                    return;
                }

                // 显示图像
                this.pictImg.ShowImage(patternIndex);
            }
            catch (Exception)
            { }
        }
    }
}
