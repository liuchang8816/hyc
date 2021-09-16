
using HYC.HTCamera.GigE;
using HYC.StandardAoi.Extentions;
using HYC.StandardAoi.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.calsses
{
    public  class GigECameraImageProducerEx:GigECameraImageProducer
    {
        int[] rect = new int[] { };
        public GigECameraImageProducerEx(IHTCamera camera,int x,int y, int w ,int h) : base(camera)
        {
            rect = new int[] {x,y,w,h };
        }
        //
        // 摘要:
        //     获取图像数据
        //
        // 返回结果:
        //     图形数据
        public new ImageData CreateImageData(string pattName) {
            ImageData img= base.CreateImageData(pattName);
           var newbmp=  CapBmp.CaptureImage(img.ToBitmap(), rect[0], rect[1], rect[2], rect[3]);
            ImageData imgnew = new ImageData();
            imgnew.FromBitmap(newbmp);
            newbmp.Dispose();
            GC.Collect();
            return imgnew;
        }
    }
}
