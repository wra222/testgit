// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-21   200038                       Create
// Known issues:
//

using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.CheckItemModule.DockingMB.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingMB.Filter.dll")]
    public class SaveModule: ISaveModule
    {
        /// <summary>
        /// save for Docking MB
        /// </summary>
        /// <param name="partUnit">part Unit</param>
        /// <param name="partOwner">part owner</param>
        /// <param name="station">station</param>
        /// <param name="key">session key</param>
        public void Save(object partUnit, object partOwner, string station, string key)
        {

            if (partUnit == null)
            {
                throw new ArgumentNullException();
            }
            if (partOwner == null)
            {
                throw new ArgumentNullException();
            }

            var product = (Product)partOwner;
            product.PCBID = ((PartUnit)partUnit).Sn;
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB mb = mbRepository.Find(product.PCBID);
            if (mb != null)
            {
                //Product
                // PCBID: MBSN
                // PCBModel: PCB.PCBModelID
                // MAC: PCB.MAC
                // UUID: PCB.UUID
                // MBECR: PCB.ECR
                // CVSN: PCB.CVSN
                product.PCBModel = mb.PCBModelID;
                product.MAC = mb.MAC;
                product.MBECR = mb.ECR;
                if (!string.IsNullOrEmpty(mb.CVSN))
                {
                    product.CVSN = mb.CVSN;
                }
                product.UUID = mb.UUID;

/*
                //ProductInfo表
                //InfoType=‘IECECR’ 
                //InfoValue= PCB. IECECR
                product.SetExtendedProperty("IECECR", mb.IECVER, session.Editor);
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
*/


                ///PCBStatus
                /// PCBStatus.Station=’32’
                /// PCBStatus.Status=1
                /// 
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
                //    //mb.MBStatus.Udt.ToString("yyyy-MM-dd HH:mm:ss.fff")
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
                //    //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
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


                #region recode PCBStatusEx for table structure

                //IMES.DataModel.TbProductStatus preStatus = new IMES.DataModel.TbProductStatus()
                //{
                //    ProductID = mb.Sn,
                //    Line = mb.MBStatus.Line,
                //    ReworkCode = string.Empty,
                //    TestFailCount = mb.MBStatus.TestFailCount,
                //    Station = mb.MBStatus.Station,
                //    Status = (int)mb.MBStatus.Status,
                //    Editor = mb.MBStatus.Editor,
                //    Udt = mb.MBStatus.Udt
                //};
                IList<TbProductStatus> preStatusList = mbRepository.GetMBStatus(new List<string>() { mb.Sn });
                mbRepository.UpdatePCBPreStationDefered(session.UnitOfWork,
                                                                                    preStatusList);
                #endregion

                mb.MBStatus.Station = "32";
                mb.MBStatus.Status = MBStatusEnum.Pass;
                var mbLog = new MBLog(0, mb.Sn, mb.Model, "32", (int)MBStatusEnum.Pass, mb.MBStatus.Line, session.Editor,
                                         new DateTime());
                mb.AddLog(mbLog);
                mbRepository.Update(mb, session.UnitOfWork);
                //IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                //productRepository.Update(product, session.UnitOfWork);

/*
                var setValue = new ProductPart();
                var condition = new ProductPart();
                if (!string.IsNullOrEmpty(mb.CVSN))
                {
                    setValue.PartSn = mb.CVSN;
                    condition.CheckItemType = "CPU";
                    condition.ProductID = product.ProId;
                    productRepository.UpdateProductPartDefered(session.UnitOfWork, setValue, condition);
                }
                if (product.IsCDSI)
                {
                    var item = new SpecialDetInfo { snoId = product.ProId, tp = "CDSI", sno1 = product.MAC, cdt = new DateTime(), id = 0, udt = new DateTime() };
                    productRepository.InsertSpecialDet(item);
                    //                    logger.Debug(" IMES.CheckItemModule.MB.Filter.SaveModule.Save  Time : " + new DateTime());
                }
*/
            }
        }

        private System.Data.DataTable CreateProductStatusTb()
        {
            System.Data.DataTable status = new System.Data.DataTable("TbProductStatus");
            status.Columns.Add("ProductID", typeof(string));
            status.Columns.Add("Station", typeof(string));
            status.Columns.Add("Status", typeof(int));
            status.Columns.Add("ReworkCode", typeof(string));
            status.Columns.Add("Line", typeof(string));
            status.Columns.Add("TestFailCount", typeof(int));
            status.Columns.Add("Editor", typeof(string));
            status.Columns.Add("Udt", typeof(DateTime));
            return status;
        }
    }
}
