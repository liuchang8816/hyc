/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Controls
// 文件名称 ：HTImageBox.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/3/25 9:57:11 
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

namespace RasterDetectionMod.Controls
{
    /// <summary>
    /// 图像显示控件
    /// </summary>
    public partial class ImageBox : UserControl
    {
        /// <summary>
        /// 图像
        /// </summary>
        public Image Image
        {
            get { return pict.Image; }
            set { pict.Image = value; }
        }

        /// <summary>
        /// 是否可以框选区域
        /// </summary>
        public bool IsRegionSelectEnabled
        {
            get { return pict.IsRegionSelectEnabled; }
            set { pict.IsRegionSelectEnabled = value; }
        }

        private bool isDrawCrossLine = false;
        /// <summary>
        /// 是否绘制十字线
        /// </summary>
        public bool IsDrawCrossLine
        {
            get
            {
                return isDrawCrossLine;
            }
            set
            {
                isDrawCrossLine = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 鼠标位置
        /// </summary>
        public Point MousePos { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public ImageBox()
        {
            InitializeComponent();
            pict.SizeMode = PictureBoxSizeMode.Zoom;
            pict.RegionSelected += Pict_RegionSelected;
            pict.MousePosChanged += Pict_MousePosChanged;
            pict.ZoomScaleChanged += Pict_ZoomScaleChange;
        }

        /// <summary>
        /// 图像区域选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pict_RegionSelected(object sender, RegionSelectedEventArgs e)
        {
            RegionSelected?.Invoke(this, e);
        }

        /// <summary>
        /// 鼠标移动，显示鼠标所在的图像坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pict_MousePosChanged(object sender, MousePosChangeEventArgs e)
        {
            MousePos = e.Location;
            this.lblMousePos.Text = e.Location.X + "," + e.Location.Y;
            this.lblColor.Text = $"R:{e.Color.R} G:{e.Color.G} B:{e.Color.B}";
        }

        /// <summary>
        /// 图像缩放改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pict_ZoomScaleChange(object sender, ZoomScaleChangedEventArgs e)
        {
            this.lblSizeMode.Text = e.SizeMode.ToString();
            this.lblZoomScale.Text = e.ZoomScale.ToString("P");
        }

        /// <summary>
        /// 清除框选区域
        /// </summary>
        public void ClearSelectedRegion()
        {
            pict.ClearSelectedRegion();
        }

        /// <summary>
        /// 绘制图片显示控件 显示十字线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pict_Paint(object sender, PaintEventArgs e)
        {
            if (IsDrawCrossLine)
            {
                e.Graphics.DrawLine(Pens.Orange, new Point(0, pict.Height / 2), new Point(pict.Width, pict.Height / 2));
                e.Graphics.DrawLine(Pens.Orange, new Point(pict.Width / 2, 0), new Point(pict.Width / 2, pict.Height));

                if (Global.CurCustomerType == CustomerType.RiDong)
                {
                    int w = (int)(1.5 / 3.7 * pict.Width);
                    int h = (int)(w / 1.5);
                    e.Graphics.DrawRectangle(Pens.Orange, new Rectangle(pict.Width / 2 - w / 2, pict.Height / 2 - h / 2, w, h));
                }
            }
        }

        /// <summary>
        /// 图像区域选择事件
        /// </summary>
        public event EventHandler<RegionSelectedEventArgs> RegionSelected;
    }
}
