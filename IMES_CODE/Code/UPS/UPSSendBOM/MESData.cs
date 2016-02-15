﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTL.MetaData;
using UPS.UTL.SQL;
using UTL.Config;
using System.Data.SqlClient;
using System.Data.Common;
using UPS.UTL.LINQhelper;
using log4net;
using UTL.SQL;

namespace UPS
{
    public class MESData
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			
        public static List<HPPO> AddUPSHPPO(AppConfig config, 
                                                                        UPSDatabase db,    
                                                                        IList<SAPPO> sapPoList, 
                                                                        List<ModelBom> modelBomList, 
                                                                        string editor)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<HPPO> upspoList = new List<HPPO>();
                DateTime now = DateTime.Now;
                List<String> poNumList = sapPoList.Select(x => x.HPPO).Distinct().ToList();
                //UPSDatabase db = new UPSDatabase(config.DBConnectStr);

                var mesPoList = (from x in db.UPSHPPOEntity
                                 where poNumList.Contains(x.HPPO)
                                 select x).ToList();

                var mesIecPoList = (from x in db.UPSIECPOEntity
                                    where poNumList.Contains(x.HPPO)
                                    select x).ToList();

                var mesPoPartList = (from x in db.UPSPOAVPartEntity
                                     where poNumList.Contains(x.HPPO)
                                     select x).ToList();

                foreach (var poId in poNumList)
                {
                    var poList = sapPoList.Where(x => x.HPPO == poId).ToList();
                    var first = poList[0];
                    var po = mesPoList.Where(x => x.HPPO == poId).FirstOrDefault();                   
 
                    if (po == null) //新增
                    {
                        #region 新增
                        HPPO hppo = new HPPO();
                        hppo.PO = new UPSHPPO
                        {
                            HPPO = poId,
                            Plant = first.Plant,
                            POType = first.POType,
                            HPSKU = first.HPSku,
                            EndCustomerPO = first.CustPO,
                            Status = SendBOMState.CreatedPoBOM.ToString(),
                            Qty = poList.Sum(x => x.Qty),
                            Editor = editor,
                            ErrorText = string.Empty,
                            ReceiveDate = new DateTime(now.Year, now.Month, now.Day),
                            Cdt = now,
                            Udt = now
                        };



                        hppo.IECPOList = new List<UPSIECPO>();
                        hppo.PartNoList = new List<UPSPOAVPart>();
                        foreach (var sappo in poList)
                        {
                            UPSIECPO iecpo = new UPSIECPO
                            {
                                HPPO = poId,
                                IECPO = sappo.IECPO,
                                IECPOItem = sappo.IECPOItem,
                                Model = sappo.IECSku,
                                Qty = sappo.Qty,
                                //Status = SendBOMState.Waiting.ToString(),
                                Cdt = now,
                                Udt = now,
                                Editor = editor,
                                Status ="New"
                            };
                            hppo.IECPOList.Add(iecpo);


                         
                            //有2笔HPPO,且机型不同，AV 也不同，会导致漏赛UPSPOAVPart。
                           // foreach (var partNo in first.UPSMatchAVPart)
                            foreach (var partNo in sappo.UPSMatchAVPart)
                            {
                                var modelBom = modelBomList.Where(x => x.AVPartNo == partNo).FirstOrDefault();
                                UPSPOAVPart part = new UPSPOAVPart
                                {
                                    HPPO = poId,
                                    AVPartNo = partNo,
                                    IECPartNo = modelBom == null ? string.Empty : modelBom.IECPartNo,
                                    IECPartType = modelBom == null ? string.Empty : modelBom.IECPartType,
                                    Editor = editor,
                                    Remark = string.Empty,
                                    Udt = now,
                                    Cdt = now
                                };

                                hppo.PartNoList.Add(part);
                            }

                        }

                       
                        hppo.Insert = true;
                        upspoList.Add(hppo);
                        #endregion
                    }                    
                    else //查詢 需考慮Cancel PO,再產生新的IECPO case
                    {
                        HPPO hppo = new HPPO();
                        bool isWithdraw = false;
                        #region 檢查及計算是否有拉單Case
                        var hpIECPOList = mesIecPoList.Where(x => x.HPPO == poId).ToList();
                        var hpAvPOList = mesPoPartList.Where(x => x.HPPO == poId).ToList();
                        var withdrawPOList =poList.Where(x => !hpIECPOList.Any(y => x.IECPO == y.IECPO && x.IECPOItem == y.IECPOItem)).ToList();
                        if (withdrawPOList.Count > 0)
                        {
                            hppo.WithdrawIECPOList = new List<UPSIECPO>();
                            foreach (var sappo in withdrawPOList)
                            {
                                UPSIECPO iecpo = new UPSIECPO
                                {
                                    HPPO = poId,
                                    IECPO = sappo.IECPO,
                                    IECPOItem = sappo.IECPOItem,
                                    Model = sappo.IECSku,
                                    Qty = sappo.Qty,
                                    //Status = SendBOMState.Waiting.ToString(),
                                    Cdt = now,
                                    Udt = now,
                                    Status ="Withdraw",
                                    Editor = "Withdraw"
                                };
                                hppo.WithdrawIECPOList.Add(iecpo);
                            }
                            
                            hppo.WithdrawPartNoList = new List<UPSPOAVPart>();
                            foreach (var partNo in withdrawPOList[0].UPSMatchAVPart)
                            {
                                var modelBom = modelBomList.Where(x => x.AVPartNo == partNo).FirstOrDefault();
                                UPSPOAVPart part = new UPSPOAVPart
                                {
                                    HPPO = poId,
                                    AVPartNo = partNo,
                                    IECPartNo = modelBom == null ? string.Empty : modelBom.IECPartNo,
                                    IECPartType = modelBom == null ? string.Empty : modelBom.IECPartType,
                                    Editor = "Withdraw",
                                    Remark ="Withdraw",
                                    Udt = now,
                                    Cdt = now
                                };

                                hppo.WithdrawPartNoList.Add(part);
                            }
                            isWithdraw = true;
                            po.Status = SendBOMState.CreatedWithdrawPoBOM.ToString();
                            po.WithdrawQty = withdrawPOList.Sum(x => x.Qty);
                            po.Qty = po.Qty + po.WithdrawQty;
                        }                        
                        #endregion
                        
                        hppo.PO = po;
                        hppo.IECPOList = mesIecPoList.Where(x => x.HPPO == poId).ToList();
                        hppo.PartNoList = mesPoPartList.Where(x => x.HPPO == poId).ToList();
                        hppo.isWithdraw = isWithdraw;
                        hppo.Insert = false;
                        upspoList.Add(hppo);
                    }
                }

                var insertDataList = upspoList.Where(x => x.Insert || x.isWithdraw ).ToList();
                if (insertDataList.Count > 0)
                {
                    #region insert MES db data case
                    if (db.Connection.State == System.Data.ConnectionState.Closed)
                    {
                        db.Connection.Open();
                    }


                    db.Transaction = db.Connection.BeginTransaction();
                    var hppoList = insertDataList.Where(x=>x.Insert).Select(x => x.PO).ToList();
                    DataTableHelper.BulkCopyToDatabase(hppoList, "UPSHPPO", (SqlConnection)db.Connection, (SqlTransaction)db.Transaction);
                    //db.UPSHPPOEntity.InsertAllOnSubmit(hppoList);

                    var iecpoList = new List<UPSIECPO>();

                    insertDataList.ForEach(x => {
                        if (x.Insert)
                        {
                            iecpoList = iecpoList.Union(x.IECPOList).ToList();
                        }
                        else
                        {
                            iecpoList = iecpoList.Union(x.WithdrawIECPOList).ToList();
                        }
                    });

                    DataTableHelper.BulkCopyToDatabase(iecpoList, "UPSIECPO", (SqlConnection)db.Connection, (SqlTransaction)db.Transaction);
                    //db.UPSIECPOEntity.InsertAllOnSubmit(iecpoList);

                    var avPartList = new List<UPSPOAVPart>();
                    insertDataList.ForEach(x =>
                    {
                        if (x.Insert)
                        {
                            avPartList = avPartList.Concat(x.PartNoList).ToList();
                        }
                        else
                        {
                            avPartList = avPartList.Concat(x.WithdrawPartNoList).ToList();
                        }
                    });
                    //db.UPSPOAVPartEntity.InsertAllOnSubmit(avPartList);
                    DataTableHelper.BulkCopyToDatabase(avPartList, "UPSPOAVPart", (SqlConnection)db.Connection, (SqlTransaction)db.Transaction);
                    //db.SubmitChanges();

                    if (insertDataList.Any(x => x.isWithdraw))
                    {
                        db.SubmitChanges();
                    }

                    db.Transaction.Commit();
                    db.Connection.Close();

                    //重新get data
                    var mesPoList1 = (from x in db.UPSHPPOEntity
                                      where poNumList.Contains(x.HPPO)
                                      select x).ToList();

                    var mesIecPoList1 = (from x in db.UPSIECPOEntity
                                         where poNumList.Contains(x.HPPO)
                                         select x).ToList();

                    var mesPoPartList1 = (from x in db.UPSPOAVPartEntity
                                          where poNumList.Contains(x.HPPO)
                                          select x).ToList();
                    upspoList = new List<HPPO>();
                    foreach (var poId in poNumList)
                    {
                        HPPO hppo = new HPPO();
                        hppo.PO = mesPoList1.Where(x => x.HPPO == poId).First();
                        hppo.IECPOList = mesIecPoList1.Where(x => x.HPPO == poId).ToList();
                        hppo.PartNoList = mesPoPartList1.Where(x => x.HPPO == poId).ToList();
                        var withdrawDataList= insertDataList.Where(x => x.PO.HPPO == poId && x.isWithdraw).FirstOrDefault();
                        if (withdrawDataList!=null)
                        {
                            hppo.WithdrawIECPOList = withdrawDataList.WithdrawIECPOList;
                            hppo.isWithdraw = true;
                            hppo.WithdrawPartNoList = withdrawDataList.WithdrawPartNoList;
                            hppo.PO.WithdrawQty = withdrawDataList.PO.WithdrawQty;
                        }
                        hppo.Insert = false;
                        upspoList.Add(hppo);
                    }
                    #endregion
                }
                return upspoList;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                if (db.Transaction != null && db.Transaction.Connection != null)
                {
                    db.Transaction.Rollback();
                }
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            
        }

        public static List<HPPO> GetUPSHPPO(AppConfig config, 
                                                                    UPSDatabase db,  
                                                                    DateTime receiveDate)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<HPPO> upspoList = new List<HPPO>();

                //UPSDatabase db = new UPSDatabase(config.DBConnectStr);
                if (db.Connection.State == System.Data.ConnectionState.Closed)
                {
                    db.Connection.Open();
                }

                var mesPoList = (from x in db.UPSHPPOEntity
                                 where x.ReceiveDate >= receiveDate
                                 select x).ToList();
                var poNumList = mesPoList.Select(x => x.HPPO).ToList();
                var mesIecPoList = (from x in db.UPSIECPOEntity
                                    where poNumList.Contains(x.HPPO)
                                    select x).ToList();

                var mesPoPartList = (from x in db.UPSPOAVPartEntity
                                     where poNumList.Contains(x.HPPO)
                                     select x).ToList();

                foreach (var po in mesPoList)
                {
                    HPPO hppo = new HPPO();
                    hppo.PO = po;
                    hppo.IECPOList = mesIecPoList.Where(x => x.HPPO == po.HPPO).ToList();
                    hppo.PartNoList = mesPoPartList.Where(x => x.HPPO == po.HPPO).ToList();
                    hppo.isWithdraw = mesIecPoList.Any(x => x.HPPO == po.HPPO && x.Editor == "Withdraw");
                    if (hppo.isWithdraw)
                    {
                        hppo.WithdrawIECPOList = mesIecPoList.Where(x => x.HPPO == po.HPPO && x.Editor == "Withdraw").ToList();
                        hppo.WithdrawPartNoList = mesPoPartList.Where(x => x.HPPO == po.HPPO && x.Editor == "Withdraw").ToList();
                        hppo.PO.WithdrawQty = hppo.WithdrawIECPOList.Sum(x => x.Qty);                       
                    }
                    hppo.Insert = false;
                    upspoList.Add(hppo);
                }

                return upspoList;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
          
        }

        public static void UpdateUPSHPPO(AppConfig config, 
                                                                UPSDatabase db, 
                                                                HPPO dbPo, 
                                                                 UPSPOBOM poBom, 
                                                                 string editor)
        {
            DateTime now = DateTime.Now;
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //UPSDatabase db = new UPSDatabase(config.DBConnectStr);

                if (db.Connection.State == System.Data.ConnectionState.Closed)
                {
                    db.Connection.Open();
                }
                
                db.Transaction = db.Connection.BeginTransaction();

                if (dbPo.PO.Status != poBom.State.ToString())
                {
                    dbPo.PO.Status = poBom.State.ToString();
                    dbPo.PO.ErrorText = poBom.ErrorText;
                    dbPo.PO.Udt = now;
                    //db.UPSHPPOEntity.Attach(dbPo.PO, true);
                }

                if (poBom.State == SendBOMState.VerifyOK)
                {
                    //產生UPSCombinePO
                    List<UPSCombinePO> UPSCombinePOList = new List<UPSCombinePO>();
                    List<UPSIECPO> IECPOList = null;
                    if (dbPo.isWithdraw)
                    {
                        IECPOList = dbPo.WithdrawIECPOList;
                        foreach (UPSIECPO iecpo in dbPo.IECPOList)
                        {
                            if (iecpo.Editor == "Withdraw")
                            {
                                iecpo.Editor = "UPS-" + iecpo.Editor;
                                iecpo.Udt = now;
                            }
                        }

                        foreach (UPSPOAVPart avpart in dbPo.PartNoList)
                        {
                            if (avpart.Editor == "Withdraw")
                            {
                                avpart.Editor = "UPS-" + avpart.Editor;
                                avpart.Udt = now;
                            }
                        }
                    }
                    else
                    {
                        IECPOList = dbPo.IECPOList;
                    }

                    IECPOList.ForEach(n =>
                    {
                        //n.Status = SendBOMState.VerifyOK.ToString();
                        n.Udt = now;
                        var upsModel = db.UPSModelEntity.Where(x => x.Model == n.Model).FirstOrDefault();
                        if (upsModel == null)
                        {
                            upsModel = new UPSModel
                            {
                                Model = n.Model,
                                FirstReceiveDate = dbPo.PO.ReceiveDate,
                                LastReceiveDate = dbPo.PO.ReceiveDate,
                                Remark = string.Empty,
                                Status = EnumUPSModelStatus.Enable.ToString(),
                                Editor = dbPo.PO.Editor,
                                Cdt = now,
                                Udt = now
                            };
                            db.UPSModelEntity.InsertOnSubmit(upsModel);
                        }
                        else
                        {
                            upsModel.LastReceiveDate = dbPo.PO.ReceiveDate;
                            upsModel.Udt = now;
                        }

                        int qty = n.Qty;
                        for (int i = 0; i < qty; i++)
                        {
                            UPSCombinePO combinePO = new UPSCombinePO
                            {
                                HPPO = dbPo.PO.HPPO,
                                IECPO = n.IECPO,
                                IECPOItem = n.IECPOItem,
                                Model = n.Model,
                                ReceiveDate = dbPo.PO.ReceiveDate,
                                ProductID = string.Empty,
                                CUSTSN = string.Empty,
                                Station = string.Empty,
                                IsShipPO = "N",
                                Status = EnumCombinePoState.Free.ToString(),
                                Remark = string.Empty,
                                Editor = editor,
                                Cdt = now,
                                Udt = now
                            };
                            UPSCombinePOList.Add(combinePO);
                        }
                    });

                    DataTableHelper.BulkCopyToDatabase(UPSCombinePOList,
                                                                                "UPSCombinePO",
                                                                                (SqlConnection)db.Connection,
                                                                                (SqlTransaction)db.Transaction);
                }
               
                db.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
                
                db.Transaction.Commit();
                // db.Connection.Close();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                if (db.Transaction != null && db.Transaction.Connection != null)
                {
                    db.Transaction.Rollback();
                }
                
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
        public static void UpdateUPSModel(AppConfig config,
                                                                UPSDatabase db,
                                                                HPPO dbPo,
                                                                 UPSPOBOM poBom,
                                                                 string editor)
        {

            DateTime now = DateTime.Now;
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                if (db.Connection.State == System.Data.ConnectionState.Closed)
                {
                    db.Connection.Open();
                }
               db.Transaction = db.Connection.BeginTransaction();
               dbPo.IECPOList.ForEach(n =>
                    {
                        n.Udt = now;
                        var upsModel = db.UPSModelEntity.Where(x => x.Model == n.Model).FirstOrDefault();
                        if (upsModel == null)
                        {
                            upsModel = new UPSModel
                            {
                                Model = n.Model,
                                FirstReceiveDate = dbPo.PO.ReceiveDate,
                                LastReceiveDate = dbPo.PO.ReceiveDate,
                                Remark = "NEW",
                                Status = EnumUPSModelStatus.Enable.ToString(),
                                Editor = dbPo.PO.Editor,
                                Cdt = now,
                                Udt = now
                            };
                            db.UPSModelEntity.InsertOnSubmit(upsModel);
                            db.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
                        }
                        //else
                        //{
                        //    upsModel.LastReceiveDate = dbPo.PO.ReceiveDate;
                        //    upsModel.Udt = now;
                        //}
                    });
                

                db.Transaction.Commit();
                //db.Connection.Close();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                if (db.Transaction != null && db.Transaction.Connection != null)
                {
                    db.Transaction.Rollback();
                }
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static void TestDB(AppConfig config,
                                     UPSDatabase db)
        {
            //UPSDatabase db = new UPSDatabase(config.DBConnectStr);
            DateTime now = DateTime.Now;

            var upsModelList = db.UPSModelEntity.Where(x => x.Remark == "NEW").ToList();
            db.Connection.Close();

            if (db.Connection.State == System.Data.ConnectionState.Closed)
            {
                db.Connection.Open();
            }

            db.Transaction = db.Connection.BeginTransaction();
            foreach (var item in upsModelList)
            {
                item.Udt = now;
            }



            var upsModel = db.UPSModelEntity.Where(x => x.Model == "testModel").FirstOrDefault();
            if (upsModel == null)
            {
                upsModel = new UPSModel
                {
                    Model = "testModel",
                    FirstReceiveDate = now,
                    LastReceiveDate = now,
                    Remark = "NEW",
                    Status = EnumUPSModelStatus.Enable.ToString(),
                    Editor = string.Empty,
                    Cdt = now,
                    Udt = now
                };
                db.UPSModelEntity.InsertOnSubmit(upsModel);
                db.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
            }
            db.Transaction.Commit();
            if (db.Transaction == null)
            {
                Console.WriteLine("null transaction!!");
            }
            db.Transaction = db.Connection.BeginTransaction();
            var upsModel1 = db.UPSModelEntity.Where(x => x.Model == "testModel").FirstOrDefault();
            if (upsModel1 == null)
            {
                upsModel1 = new UPSModel
                {
                    Model = "testModel",
                    FirstReceiveDate = now,
                    LastReceiveDate = now,
                    Remark = "NEW",
                    Status = EnumUPSModelStatus.Enable.ToString(),
                    Editor = string.Empty,
                    Cdt = now,
                    Udt = now
                };
                db.UPSModelEntity.InsertOnSubmit(upsModel1);
                db.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
            }

            db.Transaction.Commit();
           
            //db.Connection.Close();
        }
    }

    public class HPPO
    {
        public UPSHPPO PO;
        public List<UPSIECPO> IECPOList;
        public List<UPSPOAVPart> PartNoList;
        public List<UPSIECPO> WithdrawIECPOList;
        public List<UPSPOAVPart> WithdrawPartNoList;        
        public bool isWithdraw = false;
        public bool Insert=false;
    }
}