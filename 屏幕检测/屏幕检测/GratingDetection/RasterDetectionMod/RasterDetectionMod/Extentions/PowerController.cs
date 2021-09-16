/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：IxDetectorAoi.Extentions
// 文件名称    ：PowerController.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/8/10 10:18:12 
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
using HYC.HTPower;
using HYC.StandardAoi.Interface;
using RasterDetectionMod.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RasterDetectionMod.Extentions
{
    /// <summary>
    /// 光源控制器
    /// </summary>
    class PowerController : IPowerController
    {
        /// <summary>
        /// 控制器实例
        /// </summary>
        private IHTPower power = null;
        private int backchannel = 0;
        private int circlechannel = 3;
        private static object lockObj = new object();

        public PowerController(IHTPower power, int backchannel)
        {
            this.power = power;
            this.backchannel = backchannel;
        }

        /// <summary>
        /// 打开背光
        /// </summary>
        /// <returns></returns>
        public bool PowerOnBack()
        {
            return this.power.PowerOn((HTPowerChannel)backchannel);
        }

        /// <summary>
        /// 关闭背光
        /// </summary>
        /// <returns></returns>
        public bool PowerOffBack()
        {
              return this.power.PowerOff((HTPowerChannel)backchannel);
        }

        /// <summary>
        /// 关闭侧光
        /// </summary>
        /// <returns></returns>
        public bool PowerOffSide()
        {
            return this.power.PowerOff((HTPowerChannel)circlechannel);
        }

        /// <summary>
        /// 打开侧光
        /// </summary>
        /// <returns></returns>
        public bool PowerOnSide()
        {
            return this.power.PowerOn((HTPowerChannel)circlechannel);
        }
    }
}
