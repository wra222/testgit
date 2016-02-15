// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-02-27   210003                       ITC-1360-0482
// 2012-02-27   210003                       ITC-1360-0473
// 2012-03-05   210003                       ITC-1360-1041
// 2012-03-05   210003                       ITC-1360-0455
// 2012-03-09   210003                       ITC-1360-1280
// Known issues:


using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace IMES.CheckItemModule.CQ.RCTOMB.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.RCTOMB.Filter.dll")]
    class SaveModule : ISaveModule
    {
//        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            if (part_unit == null)
            {
                throw new ArgumentNullException();
            }
            if (part_owner == null)
            {
                throw new ArgumentNullException();
            }

            var product = (Product)part_owner;
            product.PCBID = ((PartUnit)part_unit).Sn;
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            var mb_repository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB mb = mb_repository.Find(product.PCBID);
            if (mb != null)
            {
                product.PCBModel = mb.PCBModelID;
                product.MAC = mb.MAC;
                product.MBECR = mb.ECR;
                if (!string.IsNullOrEmpty(mb.CVSN))
                {
                    product.CVSN = mb.CVSN;
                }
//                product.CVSN = mb.CVSN;
                product.UUID = mb.UUID;
                //ProductInfo表
                //InfoType=‘IECECR’ 
                //InfoValue= PCB. IECECR
                product.SetExtendedProperty("IECECR", mb.IECVER, session.Editor);
                product.SetExtendedProperty("DateCode", mb.DateCode, session.Editor);
                //ValueType=‘EEPROM’ 
                //InfoValue= PCBInfo.Value(ValueType=‘EEPROM’)
                object eeprom = mb.GetExtendedProperty("EEPROM");
                if (eeprom != null)
                {
                    product.SetExtendedProperty("EEPROM", eeprom, session.Editor);
                }
                //InfoType=’MBCT’
                //InfoValue= PCBInfo.Value(ValueType=‘MBCT’)
                object mbct = mb.GetExtendedProperty("MBCT");
                if (mbct != null)
                {
                    product.SetExtendedProperty("MBCT", mbct, session.Editor);
                }

                #region recode PCBStatusEx for table structure

                IList<TbProductStatus> preStatusList = mb_repository.GetMBStatus(new List<string>() { mb.Sn });
                mb_repository.UpdatePCBPreStationDefered(session.UnitOfWork,
                                                                                    preStatusList);
                #endregion

                #region record previous PCB Status
                //IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                //System.Data.DataTable preStatus = CreateProductStatusTb();
                //preStatus.Rows.Add(mb.Sn,
                //                                   mb.MBStatus.Station,
                //                                    mb.MBStatus.Status == MBStatusEnum.Pass ? 1 : 0,
                //                                    "",
                //                                    mb.MBStatus.Line,
                //                                    mb.MBStatus.TestFailCount,
                //                                    mb.MBStatus.Editor,
                //                                    //mb.MBStatus.Udt.ToString("yyyy-MM-dd HH:mm:ss.fff")
                //                                    mb.MBStatus.Udt
                //                                    );

                //System.Data.DataTable curStatus = CreateProductStatusTb();
                //curStatus.Rows.Add(mb.Sn,
                //                                   "32",
                //                                    1,
                //                                   "",
                //                                   session.Line,
                //                                    mb.MBStatus.TestFailCount,
                //                                   session.Editor,
                //                                   //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                //                                   DateTime.Now
                //                                   );

                //SqlParameter para1 = new SqlParameter("PreStatus", System.Data.SqlDbType.Structured);
                //para1.Direction = System.Data.ParameterDirection.Input;
                //para1.Value = preStatus;

                //SqlParameter para2 = new SqlParameter("Status", System.Data.SqlDbType.Structured);
                //para2.Direction = System.Data.ParameterDirection.Input;
                //para2.Value = curStatus;


                //productRepository.ExecSpForNonQueryDefered(session.UnitOfWork,
                //                                                                                 IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_PCA,
                //                                                                                 "IMES_UpdatePCBStatus",
                //                                                                                para1,
                //                                                                                para2);
                #endregion


                mb.MBStatus.Station = "32";
                mb.MBStatus.Status = MBStatusEnum.Pass;
                MBLog mb_log = new MBLog(0, mb.Sn, mb.Model, "32", (int) MBStatusEnum.Pass, session.Line, session.Editor,
                                         new DateTime());
                mb.AddLog(mb_log);
                mb_repository.Update(mb, session.UnitOfWork);
                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                product_repository.Update(product, session.UnitOfWork);

                ProductPart set_value = new ProductPart();
                ProductPart condition = new ProductPart();
                if (!string.IsNullOrEmpty(mb.CVSN))
                {
                    set_value.PartSn = mb.CVSN;
                    condition.CheckItemType = "CPU";
                    condition.ProductID = product.ProId;
                    product_repository.UpdateProductPartDefered(session.UnitOfWork, set_value, condition);
                }
                if (product.IsCDSI)
                {
                    string CNRSModel = (string)product.ModelObj.GetAttribute("CNRS");
                    SpecialDetInfo item;
                    if (!string.IsNullOrEmpty(CNRSModel) && CNRSModel == "Y")
                    {
                        item = new SpecialDetInfo { snoId = product.ProId, tp = "CNRS", sno1 = product.MAC, cdt = new DateTime(), id = 0, udt = new DateTime() };
                    }
                    else
                    {
                        item = new SpecialDetInfo { snoId = product.ProId, tp = "CDSI", sno1 = product.MAC, cdt = new DateTime(), id = 0, udt = new DateTime() };
                    }
                        product_repository.InsertSpecialDet(item);
//                    logger.Debug(" IMES.CheckItemModule.MB.Filter.SaveModule.Save  Time : " + new DateTime());
                }
            }
        }

        //private System.Data.DataTable CreateProductStatusTb()
        //{
        //    System.Data.DataTable status = new System.Data.DataTable("TbProductStatus");
        //    status.Columns.Add("ProductID", typeof(string));
        //    status.Columns.Add("Station", typeof(string));
        //    status.Columns.Add("Status", typeof(int));
        //    status.Columns.Add("ReworkCode", typeof(string));
        //    status.Columns.Add("Line", typeof(string));
        //    status.Columns.Add("TestFailCount", typeof(int));
        //    status.Columns.Add("Editor", typeof(string));
        //    status.Columns.Add("Udt", typeof(DateTime));
        //    return status;
        //}
    }
}
