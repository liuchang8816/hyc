/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：RasterDetectionMod.Controls
// 文件名称    ：ZoomablePictureBox.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/10/20 13:13:51 
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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RasterDetectionMod.Controls
{
    /// <summary>
    /// 可以缩放图像的PictureBox
    /// </summary>
    public partial class ZoomablePictureBox : PictureBox
    {
        private bool _isZoomable;
        /// <summary>
        /// The zoom scale of the image to be displayed
        /// </summary>
        private double _zoomScale;

        private double _zoomModeScale;

        private Point _mouseDownPosition;
        private MouseButtons _mouseDownButton;
        private Point _bufferPoint;
        private InterpolationMode _interpolationMode = InterpolationMode.NearestNeighbor;
        private static readonly Cursor _defaultCursor = Cursors.Cross;

        private Rectangle selectedRegion;

        /// <summary>
        /// 可用的缩放等级
        /// </summary>
        private double[] zoomLevels = new double[] { 0.015625, 0.03125, 0.0625, 0.125, 0.25, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0 };

        /// <summary>
        /// 构造方法
        /// </summary>
        public ZoomablePictureBox()
        {
            InitializeComponent();
            _zoomScale = 1.0;

            SetScrollBarVisibilityAndMaxMin();
            //Enable double buffering
            ResizeRedraw = false;
            DoubleBuffered = true;
            IsZoomable = true;
            Cursor = _defaultCursor;
        }

        /// <summary>
        /// 是否可以框选区域
        /// </summary>
        public bool IsRegionSelectEnabled { get; set; } = true;

        /// <summary>
        /// 是否可以缩放
        /// </summary>
        protected bool IsZoomable
        {
            get
            {
                return _isZoomable;
            }
            set
            {
                if (_isZoomable != value)
                {
                    _isZoomable = value;
                    if (_isZoomable)
                    {
                        MouseEnter += OnMouseEnter;
                        MouseWheel += OnMouseWheel;
                        MouseMove += OnMouseMove;
                        Resize += OnResize;
                        MouseDown += OnMouseDown;
                        MouseUp += OnMouseUp;
                    }
                    else
                    {
                        MouseEnter -= OnMouseEnter;
                        MouseWheel -= OnMouseWheel;
                        MouseMove -= OnMouseMove;
                        Resize -= OnResize;
                        MouseDown -= OnMouseDown;
                        MouseUp -= OnMouseUp;
                    }
                }
            }
        }

        /// <summary>
        /// 获取控件的水平滚动条
        /// </summary>
        [Browsable(false)]
        public HScrollBar HorizontalScrollBar
        {
            get
            {
                return horizontalScrollBar;
            }
        }

        /// <summary>
        /// 获取控件的垂直滚动条
        /// </summary>
        [Browsable(false)]
        public VScrollBar VerticalScrollBar
        {
            get
            {
                return verticalScrollBar;
            }
        }

        /// <summary>
        /// 获取缩放比例
        /// </summary>
        [Browsable(false)]
        public double ZoomScale
        {
            get
            {
                return _zoomScale;
            }
        }

        /// <summary>
        /// 获取或设置缩放时的插值模式
        /// </summary>
        [Bindable(false)]
        [Category("Design")]
        [DefaultValue(InterpolationMode.NearestNeighbor)]
        public InterpolationMode InterpolationMode
        {
            get
            {
                return _interpolationMode;
            }
            set
            {
                _interpolationMode = value;
            }
        }

        /// <summary>
        /// 鼠标进入控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseEnter(object sender, EventArgs e)
        {
            if (SizeMode == PictureBoxSizeMode.Zoom && Image != null)
            {
                _zoomScale = GetZoomModeScale();
                _zoomModeScale = _zoomScale;
            }
            //set this as the active control 
            Form f = TopLevelControl as Form;
            if (f != null) f.ActiveControl = this;
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownPosition = e.Location;
            _mouseDownButton = e.Button;

            _bufferPoint = Point.Empty;
            if (e.Button == MouseButtons.Middle)
                Cursor = Cursors.Hand;
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _mouseDownButton == MouseButtons.Left)
            {
                if (!_bufferPoint.IsEmpty)
                {
                    DrawReversibleRectangle(GetRectanglePreserveAspect(_bufferPoint, _mouseDownPosition));
                    _bufferPoint = Point.Empty;
                }

                if (Image != null && IsRegionSelectEnabled)
                {
                    if (SizeMode == PictureBoxSizeMode.Zoom)
                    {
                        Point p1 = ControlPtToImagePt(_mouseDownPosition, Size, Image.Size);
                        Point p2 = ControlPtToImagePt(e.Location, Size, Image.Size);

                        Rectangle rect = GetRectangle(p1, p2);
                        rect.Intersect(new Rectangle(0, 0, Image.Width, Image.Height));
                        if (rect.Width != 0 && rect.Height != 0)
                        {
                            selectedRegion = rect;
                            this.Invalidate();
                            RegionSelectedEventArgs args = new RegionSelectedEventArgs();
                            args.Region = rect;
                            RegionSelected?.Invoke(this, args);
                        }
                    }
                    else if (SizeMode == PictureBoxSizeMode.Normal)
                    {
                        Point p1 = new Point((int)(_mouseDownPosition.X / _zoomScale) + horizontalScrollBar.Value,
                            (int)(_mouseDownPosition.Y / _zoomScale) + verticalScrollBar.Value);
                        Point p2 = new Point((int)(e.Location.X / _zoomScale) + horizontalScrollBar.Value,
                            (int)(e.Location.Y / _zoomScale) + verticalScrollBar.Value);

                        Rectangle rect = GetRectangle(p1, p2);
                        rect.Intersect(new Rectangle(0, 0, Image.Width, Image.Height));
                        if (rect.Width != 0 && rect.Height != 0)
                        {
                            selectedRegion = rect;
                            this.Invalidate();
                            RegionSelectedEventArgs args = new RegionSelectedEventArgs();
                            args.Region = rect;
                            RegionSelected?.Invoke(this, args);
                        }
                    }
                }
            }
            Cursor = _defaultCursor;
            _mouseDownButton = MouseButtons.None;
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDownButton == MouseButtons.Middle && (horizontalScrollBar.Visible || verticalScrollBar.Visible))
            {
                int horizontalShift = (int)((e.X - _mouseDownPosition.X) / _zoomScale);
                int verticalShift = (int)((e.Y - _mouseDownPosition.Y) / _zoomScale);

                if (horizontalShift == 0 && verticalShift == 0) return;

                //if (horizontalScrollBar.Visible)
                horizontalScrollBar.Value =
                      Math.Max(Math.Min(horizontalScrollBar.Value - horizontalShift, horizontalScrollBar.Maximum), horizontalScrollBar.Minimum);
                //if (verticalScrollBar.Visible)
                verticalScrollBar.Value =
                      Math.Max(Math.Min(verticalScrollBar.Value - verticalShift, verticalScrollBar.Maximum), verticalScrollBar.Minimum);

                if (horizontalShift != 0) _mouseDownPosition.X = e.Location.X;
                if (verticalShift != 0) _mouseDownPosition.Y = e.Location.Y;

                Invalidate();
            }
            else if (_mouseDownButton == MouseButtons.Left)
            {
                //reverse the previous highlighted rectangle, if there is any
                if (!_bufferPoint.IsEmpty)
                {
                    DrawReversibleRectangle(GetRectanglePreserveAspect(_bufferPoint, _mouseDownPosition));
                }

                //draw the newly selected area
                DrawReversibleRectangle(GetRectanglePreserveAspect(e.Location, _mouseDownPosition));

                _bufferPoint = e.Location;
            }

            if (Image != null)
            {
                if (SizeMode == PictureBoxSizeMode.Zoom)
                {
                    MousePosChangeEventArgs args = new MousePosChangeEventArgs();
                    args.Location = ControlPtToImagePt(e.Location, Size, Image.Size);
                    if (args.Location.X >= 0 && args.Location.Y >= 0 && args.Location.X < Image.Width && args.Location.Y < Image.Height)
                        args.Color = (Image as Bitmap).GetPixel(args.Location.X, args.Location.Y);
                    MousePosChanged?.Invoke(this, args);
                }
                else if (SizeMode == PictureBoxSizeMode.Normal)
                {
                    MousePosChangeEventArgs args = new MousePosChangeEventArgs();
                    args.Location = new Point((int)(e.Location.X / _zoomScale) + horizontalScrollBar.Value,
                      (int)(e.Location.Y / _zoomScale) + verticalScrollBar.Value);
                    if (args.Location.X >= 0 && args.Location.Y >= 0 && args.Location.X < Image.Width && args.Location.Y < Image.Height)
                        args.Color = (Image as Bitmap).GetPixel(args.Location.X, args.Location.Y);
                    MousePosChanged?.Invoke(this, args);
                }
            }
        }

        /// <summary>
        /// 鼠标滚轮滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (Image != null)
            {
                if (e.Delta > 0)
                {
                    if (ZoomScale < 1)
                    {
                        SetZoomScale(ZoomScale * 2, e.Location);
                    }
                    else
                    {
                        SetZoomScale(ZoomScale + 1, e.Location);
                    }
                }
                else if (e.Delta < 0)
                {
                    if (ZoomScale <= 1)
                    {
                        SetZoomScale(ZoomScale / 2, e.Location);
                    }
                    else
                    {
                        SetZoomScale(ZoomScale - 1, e.Location);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 滚动条滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnScroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// 控件大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResize(object sender, EventArgs e)
        {
            Size viewSize = GetViewSize();
            if (base.Image != null && viewSize.Width > 0 && viewSize.Height > 0)
            {
                SetScrollBarVisibilityAndMaxMin();
                Invalidate();
            }
        }

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="pe">The paint event</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (IsDisposed) return;
            if (Image != null          //image is set
               &&          //either pan or zoom
               (_zoomScale != 1.0 ||
               (horizontalScrollBar.Visible && horizontalScrollBar.Value != 0) ||
               (verticalScrollBar.Visible && verticalScrollBar.Value != 0)))
            {
                if (pe.Graphics.InterpolationMode != _interpolationMode)
                    pe.Graphics.InterpolationMode = _interpolationMode;

                if (SizeMode == PictureBoxSizeMode.Zoom)
                    pe.Graphics.InterpolationMode = InterpolationMode.Bilinear;

                using (Matrix transform = pe.Graphics.Transform)
                {
                    if (_zoomScale != 1.0 && SizeMode != PictureBoxSizeMode.Zoom)
                        transform.Scale((float)_zoomScale, (float)_zoomScale, MatrixOrder.Append);

                    int horizontalTranslation = horizontalScrollBar.Visible ? -horizontalScrollBar.Value : 0;
                    int verticleTranslation = verticalScrollBar.Visible ? -verticalScrollBar.Value : 0;
                    if (horizontalTranslation != 0 || verticleTranslation != 0)
                        transform.Translate(horizontalTranslation, verticleTranslation);

                    pe.Graphics.Transform = transform;
                    base.OnPaint(pe);
                }
            }
            else
            {
                base.OnPaint(pe);
            }

            if (!selectedRegion.IsEmpty && IsRegionSelectEnabled)
            {
                if (SizeMode == PictureBoxSizeMode.Zoom)
                {
                    Point p1 = new Point(selectedRegion.X, selectedRegion.Y);
                    Point p2 = new Point(selectedRegion.X + selectedRegion.Width, selectedRegion.Y + selectedRegion.Height);
                    Point ctrlP1 = ImagePtToControlPt(p1, this.Size, this.Image.Size);
                    Point ctrlP2 = ImagePtToControlPt(p2, this.Size, this.Image.Size);
                    Rectangle rect = GetRectangle(ctrlP1, ctrlP2);
                    pe.Graphics.ResetTransform();
                    pe.Graphics.DrawRectangle(Pens.Orange, rect);
                    string str = $"X={selectedRegion.X} Y={selectedRegion.Y}\r\nW={selectedRegion.Width} H={selectedRegion.Height}";
                    Point strpoint = new Point(ctrlP1.X, ctrlP1.Y);
                    pe.Graphics.DrawString(str, new Font("Consolas", 9f), Brushes.Orange, strpoint);
                }
                else
                {
                    pe.Graphics.DrawRectangle(Pens.Orange, selectedRegion);
                    string str = $"X={selectedRegion.X} Y={selectedRegion.Y}\r\nW={selectedRegion.Width} H={selectedRegion.Height}";
                    Point strpoint = new Point(selectedRegion.X, selectedRegion.Y);
                    pe.Graphics.DrawString(str, new Font("Consolas", 12f), Brushes.Orange, strpoint);
                }
            }

            if (SizeMode == PictureBoxSizeMode.Zoom && Image != null)
            {
                double zoomScale = GetZoomModeScale();
                if (_zoomScale != zoomScale)
                {
                    _zoomScale = zoomScale;
                    _zoomModeScale = _zoomScale;
                    ZoomScaleChangedEventArgs args = new ZoomScaleChangedEventArgs();
                    args.SizeMode = SizeMode;
                    args.ZoomScale = _zoomScale;
                    ZoomScaleChanged?.Invoke(this, args);
                }
            }
        }

        /// <summary>
        /// 设置滚动条的可见性和最大最小值
        /// </summary>
        private void SetScrollBarVisibilityAndMaxMin()
        {
            #region determine if the scroll bar should be visible or not
            horizontalScrollBar.Visible = false;
            verticalScrollBar.Visible = false;

            if (Image == null) return;
            if (SizeMode == PictureBoxSizeMode.Zoom)
            {
                horizontalScrollBar.Maximum = 0;
                verticalScrollBar.Maximum = 0;
                return;
            }

            // If the image is wider than the PictureBox, show the HScrollBar.
            horizontalScrollBar.Visible =
               (int)(Image.Size.Width * _zoomScale) > ClientSize.Width;

            // If the image is taller than the PictureBox, show the VScrollBar.
            verticalScrollBar.Visible =
               (int)(Image.Size.Height * _zoomScale) > ClientSize.Height;

            #endregion

            // Set the Maximum, LargeChange and SmallChange properties.
            if (horizontalScrollBar.Visible)
            {  // If the offset does not make the Maximum less than zero, set its value.            
                horizontalScrollBar.Maximum =
                   Image.Size.Width -
                   (int)(Math.Max(0, ClientSize.Width - (verticalScrollBar.Visible ? verticalScrollBar.Width : 0)) / _zoomScale);
            }
            else
            {
                horizontalScrollBar.Maximum = 0;
            }
            horizontalScrollBar.LargeChange = (int)Math.Max(horizontalScrollBar.Maximum / 10, 1);
            horizontalScrollBar.SmallChange = (int)Math.Max(horizontalScrollBar.Maximum / 20, 1);

            if (verticalScrollBar.Visible)
            {  // If the offset does not make the Maximum less than zero, set its value.            
                verticalScrollBar.Maximum =
                   Image.Size.Height -
                   (int)(Math.Max(0, ClientSize.Height - (horizontalScrollBar.Visible ? horizontalScrollBar.Height : 0)) / _zoomScale);
            }
            else
            {
                verticalScrollBar.Maximum = 0;
            }
            verticalScrollBar.LargeChange = (int)Math.Max(verticalScrollBar.Maximum / 10, 1);
            verticalScrollBar.SmallChange = (int)Math.Max(verticalScrollBar.Maximum / 20, 1);
        }

        /// <summary>
        /// 获取不包含滚动条的视图尺寸
        /// </summary>
        /// <returns>视图尺寸</returns>
        protected internal Size GetViewSize()
        {
            return new Size(
               ClientSize.Width - (verticalScrollBar.Visible ? verticalScrollBar.Width : 0),
               ClientSize.Height - (horizontalScrollBar.Visible ? horizontalScrollBar.Height : 0));
        }

        /// <summary>
        /// 获取缩放后的图像尺寸
        /// </summary>
        /// <returns>图像尺寸</returns>
        protected internal Size GetImageSize()
        {
            if (base.Image == null) return new Size();
            Size imageSize = base.Image.Size;
            return new Size(
               (int)Math.Round(imageSize.Width * _zoomScale),
               (int)Math.Round(imageSize.Height * _zoomScale));
        }

        /// <summary>
        /// 获取两个Size中小的一个
        /// </summary>
        /// <param name="s1">The first size</param>
        /// <param name="s2">The second size</param>
        /// <returns>The minimum of the two sizes</returns>
        protected internal static Size Min(Size s1, Size s2)
        {
            return new Size(
               s1.Width < s2.Width ? s1.Width : s2.Width,
               s1.Height < s2.Height ? s1.Height : s2.Height);
        }

        /// <summary>
        /// 在控件上绘制可逆的矩形框
        /// </summary>
        /// <param name="rect">The rectangle using the control's coordinate system</param>
        public void DrawReversibleRectangle(Rectangle rect)
        {
            rect.Location = PointToScreen(rect.Location);
            ControlPaint.DrawReversibleFrame(
               rect,
               Color.White,
               FrameStyle.Dashed);
        }

        private Rectangle GetRectanglePreserveAspect(Point p1, Point p2)
        {
            Rectangle rect = GetRectangle(p1, p2);
            return rect;
        }

        /// <summary>
        /// 两个点确定一个矩形
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public Rectangle GetRectangle(Point p1, Point p2)
        {
            int top = Math.Min(p1.Y, p2.Y);
            int bottom = Math.Max(p1.Y, p2.Y);
            int left = Math.Min(p1.X, p2.X);
            int right = Math.Max(p1.X, p2.X);
            Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
            return rect;
        }

        /// <summary>
        /// 把控件上的点坐标转化为图像上的点坐标
        /// 图像通过缩放方式，居中在控件上顶齐，把控件上的某一点坐标，转化为居中顶齐的图像坐标
        /// </summary>
        /// <param name="controlPoint">控件上点坐标</param>
        /// <param name="controlSize">控件尺寸</param>
        /// <param name="imageSize">图像尺寸</param>
        /// <returns>控件上点坐标对应的图像点坐标</returns>
        private Point ControlPtToImagePt(Point controlPoint, Size controlSize, Size imageSize)
        {
            int controlHeight = controlSize.Height;
            int controlWidth = controlSize.Width;
            int imageHeight = imageSize.Height;
            int imageWidth = imageSize.Width;
            int controlPtRow = controlPoint.Y;
            int controlPtCol = controlPoint.X;
            float controlRatio = controlHeight / (float)controlWidth;
            float imageRatio = imageHeight / (float)imageWidth;
            if (controlRatio > imageRatio) //上下有黑条
            {
                float controlCenterRow = (controlHeight - 1) / 2.0f;
                float controlCenterCol = (controlWidth - 1) / 2.0f;
                float imageCenterRow = (imageHeight - 1) / 2.0f;
                float imageCenterCol = (imageWidth - 1) / 2.0f;

                float controlPtRowTran = controlPtRow - controlCenterRow;
                float controlPtColTran = controlPtCol - controlCenterCol;
                float imageControlRatio = imageWidth / (float)controlWidth;
                return new Point((int)(controlPtColTran * imageControlRatio + imageCenterCol),
                                  (int)(controlPtRowTran * imageControlRatio + imageCenterRow));
            }
            else //左右有黑条
            {
                float controlCenterRow = (controlHeight - 1) / 2.0f;
                float controlCenterCol = (controlWidth - 1) / 2.0f;
                float imageCenterRow = (imageHeight - 1) / 2.0f;
                float imageCenterCol = (imageWidth - 1) / 2.0f;

                float controlPtRowTran = controlPtRow - controlCenterRow;
                float controlPtColTran = controlPtCol - controlCenterCol;
                float imageControlRatio = imageHeight / (float)controlHeight;
                return new Point((int)(controlPtColTran * imageControlRatio + imageCenterCol),
                                  (int)(controlPtRowTran * imageControlRatio + imageCenterRow));
            }
        }

        /// <summary>
        /// 图像上的坐标转换为控件上的坐标
        /// </summary>
        /// <param name="imagePoint"></param>
        /// <param name="controlSize"></param>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        public Point ImagePtToControlPt(PointF imagePoint, Size controlSize, Size imageSize)
        {
            float controlRatio = controlSize.Height / (float)controlSize.Width;
            float imageRatio = imageSize.Height / (float)imageSize.Width;
            if (controlRatio > imageRatio) //上下有黑条
            {
                float height = imageRatio * controlSize.Width;

                int x = (int)(imagePoint.X / imageSize.Width * controlSize.Width);
                int y = (int)(imagePoint.Y / imageSize.Height * height + (controlSize.Height - height) / 2);
                return new Point(x, y);
            }
            else //左右有黑条
            {
                float width = controlSize.Height / imageRatio;

                int x = (int)(imagePoint.X / imageSize.Width * width + (controlSize.Width - width) / 2);
                int y = (int)(imagePoint.Y / imageSize.Height * controlSize.Height);
                return new Point(x, y);
            }
        }

        /// <summary>
        /// 设置缩放比例
        /// </summary>
        /// <param name="zoomScale">The new zoom scale</param>
        /// <param name="fixPoint">The point to be fixed, in display coordinate</param>
        public void SetZoomScale(double zoomScale, Point fixPoint)
        {
            if (SizeMode == PictureBoxSizeMode.Zoom && zoomScale < _zoomScale)
            {
                return;
            }
            if (zoomScale > zoomLevels[zoomLevels.Length - 1] || zoomScale < zoomLevels[0])
            {
                return;
            }

            if (zoomScale <= _zoomModeScale)
            {
                SizeMode = PictureBoxSizeMode.Zoom;
                _zoomScale = GetZoomModeScale();
                SetScrollBarVisibilityAndMaxMin();
                horizontalScrollBar.Value = 0;
                verticalScrollBar.Value = 0;

                Invalidate();
                return;
            }
            SizeMode = PictureBoxSizeMode.Normal;

            for (int i = 0; i < zoomLevels.Length - 1; i++)
            {
                if (zoomScale > zoomLevels[i] && zoomScale < zoomLevels[i + 1])
                {
                    zoomScale = zoomLevels[i];
                }
            }

            if (Image != null && _zoomScale != zoomScale)
            {
                //constrain the coordinate to be within valide range
                fixPoint.X = Math.Min(fixPoint.X, (int)(Image.Size.Width * _zoomScale));
                fixPoint.Y = Math.Min(fixPoint.Y, (int)(Image.Size.Height * _zoomScale));

                int shiftX = (int)(fixPoint.X * (zoomScale - _zoomScale) / zoomScale / _zoomScale);
                int shiftY = (int)(fixPoint.Y * (zoomScale - _zoomScale) / zoomScale / _zoomScale);

                _zoomScale = zoomScale;

                int h = (int)(horizontalScrollBar.Value + shiftX);
                int v = (int)(verticalScrollBar.Value + shiftY);
                SetScrollBarVisibilityAndMaxMin();
                horizontalScrollBar.Value = Math.Min(Math.Max(horizontalScrollBar.Minimum, h), horizontalScrollBar.Maximum);
                verticalScrollBar.Value = Math.Min(Math.Max(verticalScrollBar.Minimum, v), verticalScrollBar.Maximum);

                Invalidate();

                ZoomScaleChangedEventArgs args = new ZoomScaleChangedEventArgs();
                args.SizeMode = SizeMode;
                args.ZoomScale = _zoomScale;
                ZoomScaleChanged?.Invoke(this, args);
            }
        }

        /// <summary>
        /// 获取Zoom模式下的缩放比例
        /// </summary>
        /// <returns></returns>
        private double GetZoomModeScale()
        {
            double scale1 = GetViewSize().Width * 1.0 / Image.Width;
            double scale2 = GetViewSize().Height * 1.0 / Image.Height;
            return Math.Min(scale1, scale2);
        }

        /// <summary>
        /// 清除框选区域
        /// </summary>
        public void ClearSelectedRegion()
        {
            selectedRegion = new Rectangle();
            Invalidate();
        }

        /// <summary>
        /// 鼠标位置改变事件
        /// </summary>
        public event EventHandler<MousePosChangeEventArgs> MousePosChanged;
        /// <summary>
        /// 图像区域选择事件
        /// </summary>
        public event EventHandler<RegionSelectedEventArgs> RegionSelected;
        /// <summary>
        /// 图像缩放改变事件
        /// </summary>
        public event EventHandler<ZoomScaleChangedEventArgs> ZoomScaleChanged;
    }

    /// <summary>
    /// 鼠标位置改变事件参数类
    /// </summary>
    public class MousePosChangeEventArgs : EventArgs
    {
        /// <summary>
        /// 鼠标位置
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// 鼠标所在位置的像素颜色
        /// </summary>
        public Color Color { get; set; }
    }

    /// <summary>
    /// 图像区域选择事件参数类
    /// </summary>
    public class RegionSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// 图像区域
        /// </summary>
        public Rectangle Region { get; set; }
    }

    /// <summary>
    /// 图像缩放改变事件参数类
    /// </summary>
    public class ZoomScaleChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 图像显示模式
        /// </summary>
        public PictureBoxSizeMode SizeMode { get; set; }
        /// <summary>
        /// 缩放比例
        /// </summary>
        public double ZoomScale { get; set; }
    }
}
