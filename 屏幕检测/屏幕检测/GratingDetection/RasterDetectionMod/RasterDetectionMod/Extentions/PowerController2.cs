/*---------------------------------------------------------------- 
// Copyright (C) Suzhou HYC Technology Co.,LTD 
// 版权所有。 
//
// ================================= 
// CLR版本  ：4.0.30319.42000 
// 命名空间 ：RasterDetectionMod.Extentions 
// 文件名称 ：PowerController2.cs 
// ================================= 
// 创 建 者 ：yangkang 
// 创建日期 ：2021/6/7 14:15:39 
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
using HYC.HTPower;
using HYC.StandardAoi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterDetectionMod.Extentions
{
    /// <summary>
    /// 光源控制器
    /// </summary>
    class PowerController2 : IPowerController
    {
        /// <summary>
        /// 背光控制器
        /// </summary>
        private IHTPower backpower = null;
        /// <summary>
        /// 侧光控制器
        /// </summary>
        private IHTPower sidepower = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="backpower"></param>
        /// <param name="sidepower"></param>
        public PowerController2(IHTPower backpower, IHTPower sidepower)
        {
            this.backpower = backpower;
            this.sidepower = sidepower;
        }

        /// <summary>
        /// 关闭背光
        /// </summary>
        /// <returns></returns>
        public bool PowerOffBack()
        {
            return backpower.PowerOff(HTPowerChannel.POWER_CHANNEL_ALL);
        }

        /// <summary>
        /// 关闭侧光
        /// </summary>
        /// <returns></returns>
        public bool PowerOffSide()
        {
            return sidepower.PowerOff(HTPowerChannel.POWER_CHANNEL_ALL);
        }

        /// <summary>
        /// 打开背光
        /// </summary>
        /// <returns></returns>
        public bool PowerOnBack()
        {
            return backpower.PowerOn(HTPowerChannel.POWER_CHANNEL_ALL);
        }

        /// <summary>
        /// 打开侧光
        /// </summary>
        /// <returns></returns>
        public bool PowerOnSide()
        {
            return sidepower.PowerOn(HTPowerChannel.POWER_CHANNEL_ALL);
        }
    }
}
