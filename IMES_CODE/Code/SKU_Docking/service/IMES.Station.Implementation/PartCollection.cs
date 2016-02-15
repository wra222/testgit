﻿// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 负责所有检料站的Bom获取和Part收集
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-06   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure;
using log4net;
namespace IMES.Station.Implementation
{
    public class PartCollection
    {

        #region IPartCollection Members

        /// <summary>
        /// 获取当前站应该收集的料
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="currentSessionType"></param>
        /// <returns></returns>
        public static IList<IMES.DataModel.BomItemInfo> GeBOM(string sessionKey, Session.SessionType currentSessionType)
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            logger.Debug("GeBOM start, SessionKey:" + sessionKey);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    IFlatBOM CurrenBom = (IFlatBOM)currentSession.GetValue(Session.SessionKeys.SessionBom);
                    //IList<IMES.FisObject.Common.CheckItem.ICheckItem> checkItems = (IList<IMES.FisObject.Common.CheckItem.ICheckItem>)Session.GetValue(Session.SessionKeys.ExplicityCheckItemList);
                    IList<IMES.DataModel.BomItemInfo> retLst = new List<BomItemInfo>();
                    if (CurrenBom != null)
                    {
                        retLst = CurrenBom.ToBOMItemInfoList();
                    }
                    //if (retLst == null)
                    //{
                    //    new List<BomItemInfo>();
                    //}

                    //if (checkItems != null)
                    //{
                    //    foreach (ICheckItem item in checkItems)
                    //    {
                    //        BomItemInfo checkItemInfo = new BomItemInfo();
                    //        checkItemInfo.qty = 1;
                    //        checkItemInfo.scannedQty = 0;

                    //        IList<PartNoInfo> allPart = new List<PartNoInfo>();
                    //        PartNoInfo aPart = new PartNoInfo();
                    //        aPart.description = string.Empty;
                    //        aPart.id = item.ItemDisplayName;
                    //        aPart.friendlyName = aPart.id;
                    //        aPart.partTypeId = string.Empty;
                    //        aPart.iecPartNo = aPart.id;
                    //        allPart.Add(aPart);
                    //        checkItemInfo.parts = allPart;

                    //        retLst.Add(checkItemInfo);
                    //    }

                    //}
                    return retLst;
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 根据输入的字符串做PartMatch,成功返回匹配上的料,失败抛对应的业务异常
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="currentSessionType"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        public static IMES.DataModel.MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, Session.SessionType currentSessionType, string checkValue)
        {
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            logger.Debug("TryPartMatchCheck start, SessionKey:" + sessionKey);

            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey,currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.PartID, checkValue);
                    currentSession.AddValue(Session.SessionKeys.ValueToCheck, checkValue);
                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                // IList<MatchedPartOrCheckItem> MatchedList = new List<MatchedPartOrCheckItem>();
                //get matchedinfo
                PartUnit matchedPart = (PartUnit)currentSession.GetValue(Session.SessionKeys.MatchedParts);

                if (matchedPart != null)
                {
                    MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                    tempMatchedPart.PNOrItemName = matchedPart.Pn;
                    tempMatchedPart.CollectionData = checkValue;
                    tempMatchedPart.ValueType = matchedPart.ItemType;
                    tempMatchedPart.FlatBomItemGuid = matchedPart.FlatBomItemGuid;
                    return tempMatchedPart;
                }
                else
                {
                    throw new FisException("MAT010", new string[] { checkValue });
                }
                //else
                //{
                //    ICheckItem citem = (ICheckItem)currentSession.GetValue(Session.SessionKeys.MatchedCheckItem);
                //    if (citem == null)
                //    {
                //        throw new FisException("MAT010", new string[] { checkValue });
                //    }
                //    else
                //    {
                //        MatchedPartOrCheckItem tempMatchedPart = new MatchedPartOrCheckItem();
                //        tempMatchedPart.PNOrItemName = citem.ItemDisplayName;
                //        tempMatchedPart.CollectionData = citem.ValueToCollect;
                //        tempMatchedPart.ValueType = "";
                //        MatchedList.Add(tempMatchedPart);
                //        return MatchedList;
                //    }
                //}
                //return null;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("TryPartMatchCheck end, SessionKey:" + sessionKey);
            }
        }

        #endregion
    }
}
