/*----------------------------------------------------------------
// Copyright (C) Suzhou HYC Electronic Technology Co.,LTD
// 版权所有。
//
// =================================
// CLR版本     ：4.0.30319.42000
// 命名空间    ：IxDetectorAoi.Entities
// 文件名称    ：DBHelper.cs
// =================================
// 创 建 者    ：yangkang
// 创建日期    ：2020/8/19 10:57:07 
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
using HYC.DbQuery.DAL;
using HYC.DbQuery.Model;
using HYC.StandardAoi.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSAoi.Entities
{
    /// <summary>
    /// 数据库访问帮助类
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// 保存检测结果到数据库
        /// </summary>
        /// <param name="type"></param>
        /// <param name="result"></param>
        public static void SaveResultToDB(DetectionType type, DetectionResult result)
        {
            try
            {
                if (!result.IsNormallyCompleted) return;

                // 获取产品进入设备的时间
                var cellStartTime = new CellStartTimeDal().GetByID(result.Detail.PanelID);
                DateTime eqpInTime = cellStartTime != null ? cellStartTime.StartTime : DateTime.Now;
                // 创建数据库模型
                CellInfo cellInfo = new CellInfo();
                cellInfo.PanelID = result.Detail.PanelID;
                cellInfo.EQPInTime = eqpInTime;
                cellInfo.TestMode = 0;
                cellInfo.StepID = (int)type;
                cellInfo.CarrierID = string.Empty;
                cellInfo.LotID = string.Empty;
                cellInfo.RecipeID = result.Detail.RecipeID;
                cellInfo.TestStartTime = result.Detail.StartTime;
                cellInfo.TestEndTime = result.Detail.EndTime;
                cellInfo.ProcessingTime = (result.Detail.EndTime - result.Detail.StartTime)
                    .ToString().Substring(0, 8);
                cellInfo.Result = result.Detail.TestResult.ToString();
                cellInfo.ErrorCode = result.Detail.MainDefectCode;
                cellInfo.ErrorText = result.Detail.NgText;
                cellInfo.Remark = string.Empty;
                cellInfo.ImgROI = result.Detail.ImgROI.ToString();
                cellInfo.BmpSavePath = result.Detail.ImgPath;
                cellInfo.FrontEqpResult = string.Empty;
                cellInfo.SendFlg = 1;
                // 插入记录
                new CellInfoDal().Insert(cellInfo);
                // 保存详细结果模型到数据库
                foreach (var pattern in result.Detail.ListPattern)
                {
                    foreach (var reason in pattern.ListReason)
                    {
                        DefectInfo defectInfo = new DefectInfo();
                        defectInfo.PanelID = result.Detail.PanelID;
                        defectInfo.EQPInTime = eqpInTime;
                        defectInfo.TestMode = 0;
                        defectInfo.StepID = 1;
                        defectInfo.PatternName = pattern.PatternName;
                        defectInfo.ErrorCode = reason.DefectCode;
                        defectInfo.ErrorCount = reason.DefectCount;
                        defectInfo.Points = string.Join(",", reason.DefectAreas);
                        new DefectInfoDal().Insert(defectInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("DB").Error(ex);
            }
        }

        public static void UpdateCountInfo(int stageIndex, ResultType type)
        {
            try
            {
                //更新总的产量信息
                DetectCountInfoDal dal = new DetectCountInfoDal();
                var lst = dal.GetAll().Where(p => p.StageIndex == stageIndex).ToList();
                DetectCountInfo count = lst.Count > 0 ? lst[0] :
                    new DetectCountInfo() { PcName = "AOI", StageIndex = stageIndex };
                if (type == ResultType.OK)
                    count.OKCount++;
                else if (type == ResultType.NG || type == ResultType.Xsyc)
                    count.NGCount++;
                else
                    count.OtherCount++;
                if (dal.Update(count) < 1)
                {
                    dal.Insert(count);
                }
                //更新详细的产量信息
                DetectCountDetailInfoDal dal2 = new DetectCountDetailInfoDal();
                var lst2 = dal2.GetByDate(DateTime.Now.Date).Where(p => p.StageIndex == stageIndex).ToList();
                DetectCountDetailInfo countDetail = lst2.Count > 0 ? lst2[0] :
                    new DetectCountDetailInfo() { EqpID = "AOI", PcName = "AOI", StageIndex = stageIndex, CheckDate = DateTime.Now.Date };
                int hour = DateTime.Now.Hour;
                int okCount = countDetail.GetOKCount(hour);
                int ngCount = countDetail.GetNGCount(hour);
                int otherCount = countDetail.GetOtherCount(hour);
                if (type == ResultType.OK)
                    countDetail.SetOKCount(hour, okCount + 1);
                else if (type == ResultType.NG || type == ResultType.Xsyc)
                    countDetail.SetNGCount(hour, ngCount + 1);
                else
                    countDetail.SetOtherCount(hour, otherCount + 1);
                if (dal2.Update(countDetail) < 1)
                {
                    dal2.Insert(countDetail);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("DB").Error(ex);
            }
        }
    }
}
