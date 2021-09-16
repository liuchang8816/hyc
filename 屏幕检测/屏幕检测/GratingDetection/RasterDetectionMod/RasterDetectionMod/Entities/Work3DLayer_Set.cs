using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RasterDetectionMod.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Work3DLayer_Set
    {
        private static bool IsShielded_0;
        private static bool IsShielded_1;
        private static bool IsShielded_2;
        private static bool IsShielded_3;
        private static bool IsShielded_4;

        public static bool IsShielded_01 { get => IsShielded_0; set => IsShielded_0 = value; }
        public static bool IsShielded_11 { get => IsShielded_1; set => IsShielded_1 = value; }
        public static bool IsShielded_21 { get => IsShielded_2; set => IsShielded_2 = value; }
        public static bool IsShielded_31 { get => IsShielded_3; set => IsShielded_3 = value; }
        public static bool IsShielded_41 { get => IsShielded_4; set => IsShielded_4 = value; }







        /// <summary>
        /// 加载地址
        /// </summary>
        public static void Load()
        {

            XDocument xDoc = XDocument.Load(@"Config\3DLayer_Set.xml");
            XElement element = xDoc.Root.Element("Address");

            //TODO ---层数显示

            IsShielded_0 = Convert.ToBoolean(element.Element("First_Layer").Value);
            IsShielded_1 = Convert.ToBoolean(element.Element("Two_Layer").Value);
            IsShielded_2 = Convert.ToBoolean(element.Element("Three_Layer").Value);
            IsShielded_3 = Convert.ToBoolean(element.Element("Four_Layer").Value);
            IsShielded_4 = Convert.ToBoolean(element.Element("Five_Layer").Value);


        }

        /// <summary>
        /// 保存参数配置
        /// </summary>
        public static void SaveConfig()
        {
            try
            {
                XDocument xDoc = XDocument.Load(@"Config\3DLayer_Set.xml");
                XElement element = xDoc.Root.Element("Address");

                element.SetElementValue("First_Layer", IsShielded_0);
                element.SetElementValue("Two_Layer", IsShielded_1);
                element.SetElementValue("Three_Layer", IsShielded_2);
                element.SetElementValue("Four_Layer", IsShielded_3);
                element.SetElementValue("Five_Layer", IsShielded_4);

                xDoc.Save(@"Config\3DLayer_Set.xml");
              
            }
            catch (Exception ex)
            {

            }

        }

    }
}
