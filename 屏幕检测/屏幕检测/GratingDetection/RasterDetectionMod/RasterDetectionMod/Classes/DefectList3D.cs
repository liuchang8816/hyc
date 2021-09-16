/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：DefectList3D.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/4/25 17:10:08 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RasterDetectionMod.Classes
{
    [Serializable]
    public class DefectList3D
    {
        /// <summary>
        /// 缺陷层集合
        /// </summary>
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
                        defect.WorldX = defect.X - offsetX;
                        defect.WorldY = -defect.Y - offsetY;
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
                        if (Global.CurCustomerType == CustomerType.RiDong)//起始点位置为右上
                        {
                            defect.WorldX = -(defect.X + offsetY);
                            defect.WorldY = -(defect.Y + offsetX);
                        }
                        else
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

    /// <summary>
    /// 缺陷层
    /// </summary>
    [Serializable]
    public class DefectLayer3D
    {
        /// <summary>
        /// 缺陷所在层
        /// </summary>
        [XmlAttribute]
        public int Layer { get; set; }
        /// <summary>
        /// 缺陷个数
        /// </summary>
        [XmlAttribute]
        public int Count { get; set; }
        /// <summary>
        /// 缺陷信息集合
        /// </summary>
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
