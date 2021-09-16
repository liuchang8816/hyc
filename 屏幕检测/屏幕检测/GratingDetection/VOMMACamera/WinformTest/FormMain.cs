using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformTest
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(1024);
            VOMMACamera.CameraSearch(sb);
            this.txtDesc.Text = sb.ToString().Trim();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            bool ret = VOMMACamera.CameraOpen();
            if (ret)
            {
                VOMMACamera.CameraSetTriggerMode(1);
                VOMMACamera.RegisterImageReceived(new CameraEventCallBack(OnImageReceived));
                MessageBox.Show("打开成功！");
            }
            else
            {
                MessageBox.Show("打开失败！");
            }
        }

        byte[] ImgBytes = null;
        int ImgWidth = 0;
        int ImgHeight = 0;

        private void OnImageReceived(IntPtr data, int width, int height)
        {
            //MessageBox.Show($"Image Received！Width:{width} Height:{height}");
            byte[] bytes = new byte[width * height];
            Marshal.Copy(data, bytes, 0, bytes.Length);
            Bitmap bitmap = ArrayToBitmap(bytes, height, width);
            this.pictImg.Image = bitmap;

            ImgBytes = bytes;
            ImgWidth = width;
            ImgHeight = height;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            bool ret = VOMMACamera.CameraClose();
            if (ret)
            {
                MessageBox.Show("关闭成功！");
            }
            else
            {
                MessageBox.Show("关闭失败！");
            }
        }

        private void btnStartAcquisition_Click(object sender, EventArgs e)
        {
            bool ret = VOMMACamera.CameraStartAcquisition();
            if (ret)
            {
                MessageBox.Show("开始采集成功！");
            }
            else
            {
                MessageBox.Show("开始采集失败！");
            }
        }

        private void btnStopAcquisition_Click(object sender, EventArgs e)
        {
            bool ret = VOMMACamera.CameraStopAcquisition();
            if (ret)
            {
                MessageBox.Show("停止采集成功！");
            }
            else
            {
                MessageBox.Show("停止采集失败！");
            }
        }

        private void btnGetExposureTime_Click(object sender, EventArgs e)
        {
            long exposureTime = 0;
            VOMMACamera.CameraGetExposureTime(ref exposureTime);
            this.txtExposureTime.Text = exposureTime.ToString();
        }

        private void btnSetExposureTime_Click(object sender, EventArgs e)
        {
            long exposureTime = Convert.ToInt64(this.txtExposureTime.Text);
            VOMMACamera.CameraSetExposureTime(exposureTime);
        }

        private void btnGrab_Click(object sender, EventArgs e)
        {
            
        }

        public static Bitmap ArrayToBitmap(byte[] data, int height, int width)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            ColorPalette colorPalette = bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bitmap.Palette = colorPalette;
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        public static Bitmap ArrayToRgbBitmap(byte[] data, int height, int width)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                                                    bitmap.PixelFormat);
            Marshal.Copy(data, 0, bitmapData.Scan0, height * bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmap;

        }

        private void btnTriggerMode_Click(object sender, EventArgs e)
        {
            if (this.cboTriggerMode.SelectedIndex < 0) return;

            int mode = this.cboTriggerMode.SelectedIndex;
            bool ret = VOMMACamera.CameraSetTriggerMode(mode);
            if (ret)
            {
                MessageBox.Show("触发模式设置成功！");
            }
            else
            {
                MessageBox.Show("触发模式设置失败！");
            }
        }

        private void btnGetDepthImg_Click(object sender, EventArgs e)
        {
            byte[] dataDepth = new byte[ImgWidth * ImgHeight * 3];
            bool ret = VOMMACamera.CameraGetDepthImg(ImgBytes, ImgWidth, ImgHeight, dataDepth, ".\\Param\\depth.txt");
            if (ret)
            {
                Bitmap bitmapDepth = ArrayToRgbBitmap(dataDepth, ImgHeight, ImgWidth);
                this.pictImg.Image = bitmapDepth;
            }
            else
            {
                MessageBox.Show("采集失败！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VOMMACamera.CameraSoftTrigger();
        }
    }
}
