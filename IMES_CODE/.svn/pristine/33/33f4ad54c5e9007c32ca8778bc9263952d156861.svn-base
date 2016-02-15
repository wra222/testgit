/// <summary>
///  INVENTEC corporation (c)2008 all rights reserved. 
///  Description: WeekRule引擎类 ,3 bit week rule規則請見SN Composer.doc
///              
///  Update: 
///  Date       Name                  Reason 
/// ========== ===================== =====================================
/// 2008-10-15  Liu Dong(eB2-3)         Create   
/// 2009-01-18  Liu Dong(eB2-3)         Modify   ITC-932-0164
/// 2012-03-01  Liu Dong                Recreate: Translated to C# from VB.NET.
/// Known issues:
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates
{
    public class WeekRuleEngine
    {
        /// <summary>
        /// 根據WeekRule計算指定日期屬於哪一年的哪一周哪一天
        /// </summary>
        /// <param name="weekRule">由3位組成:[周劃分點(必需)(取值：0-6 (Sun.to Sat.))] + [允許"少於7天的周"(必需) (取值：0或1)] + [六种交界比例的歸前或歸后選擇(當上一位為0時必需賦有效值)(取值：0-6),或無效值X,X代表無效]</param>
        /// <param name="dat">需要計算的日期</param>
        /// <returns>計算的結果實體</returns>
        public static WeekRuleResult Calculate(string weekRule, DateTime dat)
        {
            //Check and split week rule

            if (String.IsNullOrEmpty(weekRule) || weekRule.Trim().Length != 3 || dat == null)
                return null;

            weekRule = weekRule.Trim();

            if (weekRule.Equals(String.Empty))
                return null;

            //周劃分點(必需)(取值:0-6)(Sun. to Sat.)
            int iRuleBit_WeekDiv = -1;
            //允許"少於7天的周"(必需) (取值：0或1)
            bool bRuleBit_SmallWeekPermit = true;
            //歸前或歸后比例(當上一位為0時必需賦有效值)(取值:0-6),或無效值X,X代表無效
            int iRuleBit_Ratio = -1;
            
            int.TryParse(weekRule.Substring(0, 1),out iRuleBit_WeekDiv);

            if (false == weekRule.Substring(1, 1).Equals("0") && false == weekRule.Substring(1, 1).Equals("1"))
                return null;

          if (weekRule.Substring(1, 1).Equals("0"))
          {      
            bRuleBit_SmallWeekPermit = false;
            int.TryParse(weekRule.Substring(2, 1), out iRuleBit_Ratio);
          }
          else
          {
                bRuleBit_SmallWeekPermit = true;
          }

          if (iRuleBit_WeekDiv < 0 || iRuleBit_WeekDiv > 6)
              return null;

          if (!bRuleBit_SmallWeekPermit && (iRuleBit_Ratio < 0 || iRuleBit_Ratio > 6))
              return null;

            //Prepare data

            //如果是本年最後六天裏的一天
            if (! bRuleBit_SmallWeekPermit && (dat.Month == 12 && dat.Day > 25))
            {
                //遞歸調用
                WeekRuleResult wrr = Calculate(weekRule, new DateTime(dat.Year + 1, 1, 1));
                if (wrr.Day != 1 && wrr.Year == dat.Year + 1 && 31 - dat.Day + 1 < wrr.Day)
                {
                    WeekRuleResult result_inner = new WeekRuleResult();
                    result_inner.Year = dat.Year + 1;
                    result_inner.Week = 1;
                    result_inner.Day = wrr.Day - 31 + dat.Day - 1;
                    return result_inner;
                }
            }

            DateTime dt_yearStartDate = new DateTime(dat.Year, 1, 1); //當年一月一日
            //{0, 0, 0, 0, 0, 0, 0}
            int[] actualRatio = new int[] { -1, -1 }; //現實的比例
            int dayOfWeek = Convert.ToInt32(dt_yearStartDate.DayOfWeek); //(Sun. to Sat.)
            int idxDiff = (dayOfWeek - iRuleBit_WeekDiv);
            int idxDiffWithMode7 = 0;
            if (idxDiff < 0)
            {
                idxDiffWithMode7 = 7 + idxDiff;
            }
            else
            {
                idxDiffWithMode7 = idxDiff;
            }

            actualRatio[0] = idxDiffWithMode7;
            actualRatio[1] = 7 - idxDiffWithMode7;

            int days = dat.Subtract(dt_yearStartDate).Days + 1; //與當年一月一日相差的縂天數 + 1

            //Calculate
            int iRemain = 0;
            int iWeekCnt = 0;

            if (days > actualRatio[1])
            {
                iRemain = (days - actualRatio[1]) % 7;
                iWeekCnt = (days - actualRatio[1]) / 7 + 1;
            }
            else
            {
                iRemain = days;
                iWeekCnt = 0;
            }

            // 2009-01-18  Liu Dong(eB2-3)         Modify   ITC-932-0164
            if (iRemain == 0)
            {
                iRemain = 7;
                iWeekCnt = iWeekCnt - 1;
            }
            // 2009-01-18  Liu Dong(eB2-3)         Modify   ITC-932-0164

            //Find result with the rule

            WeekRuleResult result = new WeekRuleResult();

            if (bRuleBit_SmallWeekPermit)
            {
                result.Year = dat.Year;
                result.Week = iWeekCnt + 1;
                result.Day = iRemain;
            }
            else
            {
                if (actualRatio[0] == 0)
                {
                    result.Year = dat.Year;
                    result.Week = iWeekCnt + 1;
                    result.Day = iRemain;
                }
                else
                {
                    //B:B'
                    //1  '1:6有2种選擇，本周歸前或歸后。
                    //2  '2:5有2种選擇，本周歸前或歸后。
                    //3  '3:4有2种選擇，本周歸前或歸后。
                    //4  '4:3有2种選擇，本周歸前或歸后。
                    //5  '5:2有2种選擇，本周歸前或歸后。
                    //6  '6:1有2种選擇，本周歸前或歸后。

                    //A  (0-6) 針對B的6种可能的分配處理方式
                    //0  '6個歸前、
                    //1  '1個歸后+5個歸前、
                    //2  '2個歸后+4個歸前、
                    //3  '3個歸后+3個歸前、
                    //4  '4個歸后+2個歸前、
                    //5  '5個歸后+1個歸前、
                    //6  '6個歸后

                    if (iRuleBit_Ratio == 6 
                        || (iRuleBit_Ratio == 5 && actualRatio[1] > 1) 
                        || (iRuleBit_Ratio == 4 && actualRatio[1] > 2) 
                        || (iRuleBit_Ratio == 3 && actualRatio[1] > 3) 
                        || (iRuleBit_Ratio == 2 && actualRatio[1] > 4) 
                        || (iRuleBit_Ratio == 1 && actualRatio[1] > 5) 
                        )//A6    A5,B'>1  A4,B'>2    A3,B'>3    A2,B'>4     A1,B'>5
                    {
                        //歸后
                        if (iWeekCnt == 0)
                        {
                            result.Year = dat.Year;

                            if (iRemain == 7)// 2009-01-18  Liu Dong(eB2-3)         Modify   ITC-932-0164
                            {
                                result.Week = 2;
                                result.Day = actualRatio[0];
                            }
                            else
                            {
                                result.Week = 1;
                                result.Day = actualRatio[0] + iRemain;
                            }
                        }
                        else
                        {
                            result.Year = dat.Year;
                            result.Week = iWeekCnt + 1;
                            result.Day = iRemain;
                        }
                    }
                    else
                    {
                        //歸前
                        if (iWeekCnt == 0)
                        {
                            //上一年此算法的最後一周 '遞歸調用
                            //result.Week = Calculate(weekRule, New DateTime(dat.Year - 1, 12, 31)).Week '有無限遞歸調用危險，故注掉
                            WeekRuleResult wrrst = Calculate(weekRule, new DateTime(dat.Year - 1, 12, 25));
                            if (wrrst.Day == 1) //不歸前
                            { 
                                result.Year = dat.Year;
                                result.Week = iWeekCnt + 1;
                                result.Day = iRemain;
                            }
                            else
                            {
                                result.Year = dat.Year - 1;

                                if (iRemain == 7) //2009-01-18  Liu Dong(eB2-3)         Modify   ITC-932-0164
                                { 
                                    result.Week = wrrst.Week + 2;
                                    result.Day = actualRatio[0];
                                }
                                else
                                {
                                    result.Week = wrrst.Week + 1;
                                    result.Day = actualRatio[0] + iRemain;
                                }
                            }
                        }
                        else
                        {
                            result.Year = dat.Year;
                            result.Week = iWeekCnt;
                            result.Day = iRemain;
                        }
                    }
                }
            }
            return result;
        }
    }
}
