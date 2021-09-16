/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：IxDetectorAoi.Extentions
// 文件名称    ：PatternProducer.cs
// =================================
// 创 建 者    ：zhaoxin
// 创建日期    ：2020/11/03 10:16:36 
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
using RasterDetectionMod.Entities;
using HYC.HTPatternGenerator;
using HYC.StandardAoi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HYC.HTLog;

namespace RasterDetectionMod.Extentions
{
    /// <summary>
    /// 画面生产者 检查机需要实现
    /// </summary>
    class PatternProducer : IPatternProducer
    {
        private IHTPatternGenerator tester = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="tester"></param>
        public PatternProducer(IHTPatternGenerator tester)
        {
            this.tester = tester;
        }

        /// <summary>
        /// 切换画面 RGB方式
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool CreatePattern(int r, int g, int b)
        {
            try
            {
                int result = tester.PatternDisplay(0, (uint)r, (uint)g, (uint)b);
                return result >= 0;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("Error").Error(ex);
                return false;
            }
        }
        public bool CreatePattern(string a)
        {
            return true;
        }
        bool isoff = false;
        /// <summary>
        /// 切换画面 序号方式
        /// </summary>
        /// <param name="patternIndex"></param>
        /// <returns></returns>
        public bool CreatePattern(int patternIndex)
        {
            try
            {
                tester.PatternDisplay(0, patternIndex);
                //if (patternIndex == 3)
                //{
                //    Thread.Sleep(200);
                //}
                return true;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("Error").Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 关电
        /// </summary>
        /// <returns></returns>
        public bool PowerOff()
        {
            tester.PowerOff(0);
            return true;
        }

        /// <summary>
        /// 开电
        /// </summary>
        /// <returns></returns>
        public bool PowerOn()
        {
            tester.PowerOn(0);
            return true;
        }
    }
}
