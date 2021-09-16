using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.calsses
{
    public class CapBmp
    {
        #region 从大图中截取一部分图片
        /// <summary>
        /// 从大图中截取一部分图片
        /// </summary>
        /// <param name="fromImagePath">来源图片地址</param>        
        /// <param name="x">从偏移X坐标位置开始截取</param>
        /// <param name="y">从偏移Y坐标位置开始截取</param>
        /// <param name="toImagePath">保存图片地址</param>
        /// <param name="w">保存图片的宽度</param>
        /// <param name="h">保存图片的高度</param>
        /// <returns></returns>
        static public Bitmap CaptureImage(Image fromImage, int x, int y,  int w, int h)
        {
            //创建新图位图
            Bitmap bitmap = new Bitmap(w, h);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, 0, 0, new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
            //从作图区生成新图
            //Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //保存图片
            graphic.Dispose();
            bitmap.Dispose();
            return bitmap;
        }
        #endregion
    }
}
