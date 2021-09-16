using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            float width = 4f;
            float height = 4f;

            string errorInfo = "<0:11<4712.810,125.309,39.571,-1.489,0.605,-0.450;2705.502,112.119,32.976,-1.204,0.602,-0.450;16189.375,201.519,128.789,-1.288,0.481,-0.450;4058.252,113.374,42.173,-0.974,0.468,-0.450;720972.625,784.830,2176.420,-2.332,-0.361,-0.450;10080.177,114.757,97.609,-2.322,0.083,-0.450;5105.544,139.035,42.989,0.035,-0.050,-0.450;421534.625,796.799,1114.933,-0.099,-0.435,-0.450;2487.316,44.242,67.838,-0.466,-0.306,-0.450;154431.797,692.497,448.474,-0.094,-1.112,-0.450;8247.416,98.928,92.333,-2.573,-1.083,-0.450>;1:4<218.186,27.113,4.171,-1.394,-0.852,-0.214;20509.449,98.928,276.999,-1.447,-1.051,-0.280;9992.901,158.285,79.143,-1.463,-1.222,-0.218;261.823,32.976,0.000,-1.383,-1.231,-0.198>;2:0<>;3:0<>;4:13<3578.244,0.450,60.626,0.450,65.289,0.450;-1.177,0.450,1.131,0.450,8421.966,0.450;92.333,0.450,98.928,0.450,-2.131,0.450;0.962,0.450,2094.582,0.450,46.166,0.450;39.571,0.450,-0.225,0.450,0.857,0.450;2269.131,0.450,46.166,0.450,46.166,0.450;-1.495,0.450,0.042,0.450,5934.649,0.450;85.738,0.450,72.547,0.450,-0.614,0.450;-0.173,0.450,4581.899,0.450,69.953,0.450;74.616,0.450,-1.075,0.450,-0.441,0.450;4843.721,0.450,59.357,0.450,98.928,0.450;-1.542,0.450,-0.838,0.450,2400.042,0.450;39.571,0.450,59.357,0.450,-1.549,0.450;-1.090,0.450>>";
            DefectList3D defectList = new DefectList3D();
            defectList.ConvertErrorInfo2(errorInfo, width * 0.5f, height * 0.5f);

            string errorInfo2 = "<0:2<23.2,2.0,3.0,-1.0,-1.0;23.2,2.0,3.0,5.0,7.0>;1:3<23.2,2.0,3.0,5.0,7.0;23.2,2.0,3.0,5.0,7.0;23.2,2.0,3.0,5.0,7.0>>";
            DefectList3D defectList2 = new DefectList3D();
            defectList2.ConvertErrorInfo(errorInfo2, width * 1.5f, height * 1.5f);

            DefectList3D[] defects = new DefectList3D[] { defectList, defectList2 };

            DefectList3D total = new DefectList3D();
            foreach (var item in defects)
            {
                foreach (var layer in item.ListLayers)
                {
                    var totalLayer = total.ListLayers.Where(p => p.Layer == layer.Layer).FirstOrDefault();
                    if (totalLayer != null)
                    {
                        foreach (var defect in layer.ListDefects)
                        {
                            if (!totalLayer.IsContain(defect))
                            {
                                totalLayer.ListDefects.Add(defect);
                                totalLayer.Count = totalLayer.ListDefects.Count;
                            }
                            else
                            {
                                Console.WriteLine(defect);
                            }
                        }
                    }
                    else
                    {
                        total.ListLayers.Add(layer);
                    }
                }
            }
        }
    }

    public class DefectList3D
    {
        public List<DefectLayer3D> ListLayers { get; set; } = new List<DefectLayer3D>();

        /// <summary>
        /// 从文本转换
        /// </summary>
        /// <param name="errorInfo"></param>
        public void ConvertErrorInfo(string errorInfo, float offsetX, float offsetY)
        {
            ListLayers.Clear();
            string[] strLayerInfos = errorInfo.Split(new string[] { ">;" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strLayerInfos.Length; i++)
            {
                string[] strTmps = strLayerInfos[i].Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);

                DefectLayer3D defectLayer = new DefectLayer3D();
                string[] strLayers = strTmps[0].Split(new char[] { ':', ';' }, StringSplitOptions.RemoveEmptyEntries);
                defectLayer.Layer = Convert.ToInt32(strLayers[0]);
                defectLayer.Count = Convert.ToInt32(strLayers[1]);
                if (strTmps.Length >= 2)
                {
                    string[] strdefects = strTmps[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < strdefects.Length; j++)
                    {
                        string[] paras = strdefects[j].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        Defect3D defect = new Defect3D();
                        defect.Area = Convert.ToSingle(paras[0]);
                        defect.Width = Convert.ToSingle(paras[1]);
                        defect.Height = Convert.ToSingle(paras[2]);
                        defect.X = Convert.ToSingle(paras[3]);
                        defect.Y = Convert.ToSingle(paras[4]);
                        defect.WorldX = defect.X + offsetX;
                        defect.WorldY = defect.Y + offsetY;
                        defectLayer.ListDefects.Add(defect);
                    }
                }
                ListLayers.Add(defectLayer);
            }
        }

        /// <summary>
        /// 从文本转换
        /// </summary>
        /// <param name="errorInfo"></param>
        public void ConvertErrorInfo2(string errorInfo, float offsetX, float offsetY)
        {
            ListLayers.Clear();
            errorInfo = errorInfo.Trim('<', '>');
            string[] strLayerInfos = errorInfo.Split(new string[] { ">;" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strLayerInfos.Length; i++)
            {
                string[] strTmps = strLayerInfos[i].Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);

                DefectLayer3D defectLayer = new DefectLayer3D();
                string[] strLayers = strTmps[0].Split(new char[] { ':', ';' }, StringSplitOptions.RemoveEmptyEntries);
                defectLayer.Layer = Convert.ToInt32(strLayers[0]);
                defectLayer.Count = Convert.ToInt32(strLayers[1]);
                if (strTmps.Length >= 2)
                {
                    string[] strdefects = strTmps[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < strdefects.Length; j++)
                    {
                        string[] paras = strdefects[j].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (paras.Length < 5) continue;

                        Defect3D defect = new Defect3D();
                        defect.Area = Convert.ToSingle(paras[0]);
                        defect.Width = Convert.ToSingle(paras[1]);
                        defect.Height = Convert.ToSingle(paras[2]);
                        defect.X = Convert.ToSingle(paras[3]);
                        defect.Y = Convert.ToSingle(paras[4]);
                        if (paras.Length > 5)
                            defect.Depth = Convert.ToSingle(paras[5]);
                        //if (Global.CurCustomerType == CustomerType.RiDong)//起始点位置为右上
                        //{
                        //    defect.WorldX = -(defect.X + offsetY);
                        //    defect.WorldY = -(defect.Y + offsetX);
                        //}
                        //else
                        {
                            defect.WorldX = defect.X + offsetX;
                            defect.WorldY = -defect.Y - offsetY;
                        }
                        defectLayer.ListDefects.Add(defect);
                    }
                }
                ListLayers.Add(defectLayer);
            }
        }
    }

    public class DefectLayer3D
    {
        public int Layer { get; set; }
        public int Count { get; set; }
        public List<Defect3D> ListDefects { get; set; } = new List<Defect3D>();

        /// <summary>
        /// 判断是否包含重复区域的不良
        /// </summary>
        /// <param name="defect"></param>
        /// <returns></returns>
        public bool IsContain(Defect3D defect)
        {
            for (int i = 0; i < ListDefects.Count; i++)
            {
                if (ListDefects[i].IsIntersect(defect))
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 缺陷信息
    /// </summary>
    [Serializable]
    public class Defect3D
    {
        /// <summary>
        /// 面积
        /// </summary>
        [XmlAttribute]
        public float Area { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        [XmlAttribute]
        public float Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        [XmlAttribute]
        public float Height { get; set; }
        /// <summary>
        /// 局部X坐标
        /// </summary>
        [XmlAttribute]
        public float X { get; set; }
        /// <summary>
        /// 局部Y坐标
        /// </summary>
        [XmlAttribute]
        public float Y { get; set; }
        /// <summary>
        /// 深度值
        /// </summary>
        public float Depth { get; set; }
        /// <summary>
        /// 全局X坐标
        /// </summary>
        [XmlAttribute]
        public float WorldX { get; set; }
        /// <summary>
        /// 全局Y坐标
        /// </summary>
        [XmlAttribute]
        public float WorldY { get; set; }

        /// <summary>
        /// 判断区域是否相交
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool IsIntersect(Defect3D b)
        {
            return IsIntersect(this, b);
        }

        /// <summary>
        /// 判断区域是否相交
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsIntersect(Defect3D a, Defect3D b)
        {
            float x1 = Math.Max(a.WorldX, b.WorldX);
            float y1 = Math.Max(a.WorldY, b.WorldY);
            float x2 = Math.Min(a.WorldX + a.Width, b.WorldX + b.Width);
            float y2 = Math.Min(a.WorldY + a.Height, b.WorldY + b.Height);
            if (x1 < x2 && y1 < y2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Area:{Area},Width:{Width},Height:{Height},X:{X},Y:{Y},Depth:{Depth},WorldX:{WorldX},WorldY:{WorldY}";
        }
    }
}
