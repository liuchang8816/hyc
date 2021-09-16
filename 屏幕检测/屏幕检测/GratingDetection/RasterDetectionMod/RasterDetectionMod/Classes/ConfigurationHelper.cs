/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Classes 
// 文件名称 ：ConfigurationHelper.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/6/2 21:22:38 
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
using System.Xml;

namespace RasterDetectionMod.Classes
{
    class ConfigurationHelper
    {
        /// <summary>
        /// 读取配置文件里的值
        /// </summary>
        /// <returns></returns>
        public static string GetParameter(string str)
        {
            string result = string.Empty;
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径         
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\RasterDetectionMod.exe.config";
            doc.Load(strFileName);
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                var attributes = nodes[i].Attributes;
                XmlAttribute att = attributes["key"];
                if (att == null)
                {
                    continue;
                }
                if (att.Value == str)             //根据元素的第一个属性来判断当前的元素是不是目标元素
                {
                    att = attributes["value"];             //对目标元素中的第二个属性进行读取
                    if (att.Value == "" || att.Value == null)
                    {
                        result = "";
                    }
                    else
                    {
                        result = att.Value;
                    }

                }
                else
                {
                    continue;
                }
            }
            return result;
        }


        /// <summary>
        /// 根据传递过来的参数修改对应的参数的值
        /// </summary>
        /// <param name="value">要修改的参数的值</param>
        /// <param name="str">参数名</param>
        public static void Changeparameter(string value, string str)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径         
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\RasterDetectionMod.exe.config";
            doc.Load(strFileName);
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                var attributes = nodes[i].Attributes;
                XmlAttribute att = attributes["key"];
                if (att.Value == str)             //根据元素的第一个属性来判断当前的元素是不是目标元素
                {
                    att = attributes["value"];  //对目标元素中的第二个属性判断
                    att.Value = value;
                    break;
                }
            }
            doc.Save(strFileName);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
