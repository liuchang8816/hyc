using HYC.HTFile.HTXML;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace RasterDetectionMod.Entities
{
    /// <summary>
    /// Bitmap绘制尺寸类
    /// </summary>
    public class Bitmap_Set
    {
       
        private static string width;

        private static string height;

        public static string Width { get => width; set => width = value; }
        public static string Height { get => height; set => height = value; }


    
        public static void Load()
        {
            
            XDocument xDoc = XDocument.Load(@"Config\Bitmap_Set.xml");
            XElement element = xDoc.Root.Element("Bitmap_Size");

            Width = element.Element("Bitmap_Width").Value;
            Height = element.Element("Bitmap_height").Value;
        }

  
        public static void SaveConfig()
        {
            try
            {
                XDocument xDoc = XDocument.Load(@"Config\Bitmap_Set.xml");
                XElement element = xDoc.Root.Element("Bitmap_Size");
                element.SetElementValue("Bitmap_Width", Width);

                element.SetElementValue("Bitmap_height", Height);

                xDoc.Save(@"Config\Bitmap_Set.xml");

            }
            catch (Exception ex)
            {
                
            }

        }

    }
}
