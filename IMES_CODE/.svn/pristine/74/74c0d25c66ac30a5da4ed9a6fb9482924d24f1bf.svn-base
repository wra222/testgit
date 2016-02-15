// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块的删除DN功能中的删除Delivery、Pallet部分子功能
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-09   itc202017                     Create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块的删除DN功能中的删除Delivery、Pallet部分子功能
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Delete DN for PL user
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         按照DN删除Delivery、DeliveryInfo、Delivery_Pallet、Pallet相关数据
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.DeliveryNo
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    ///              IPalletRepository
    /// </para> 
    /// </remarks>
    public partial class DeleteDN : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeleteDN()
		{
			InitializeComponent();
		}



        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            string dn = (string)session.GetValue(Session.SessionKeys.DeliveryNo);
            string ship = (string)session.GetValue("ShipmentNo");
            string prj = (string)session.GetValue("Project");            

            IDeliveryRepository currentDNRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository currentPltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            if (dn != null && dn != "")
            {
                session.AddValue("ShipmentNo", currentDNRepository.Find(dn).ShipmentNo);
                IList<string> pnList = currentDNRepository.GetPalletNoListByDnAndWithSoloDn(dn);
                currentDNRepository.DeleteDeliveryAttrLogByDnDefered(session.UnitOfWork, dn);
                currentDNRepository.DeleteDeliveryAttrsByDnDefered(session.UnitOfWork, dn);
                currentDNRepository.DeleteDeliveryInfoByDnDefered(session.UnitOfWork, dn);
                currentDNRepository.DeleteDeliveryPalletByDnDefered(session.UnitOfWork, dn);
                currentDNRepository.DeleteDeliveryByDnDefered(session.UnitOfWork, dn);

                currentDNRepository.RemoveDeliveryExDefered(session.UnitOfWork, dn);

                //currentDNRepository.DeleteDnDefered(CurrentSession.UnitOfWork, dn);   //Includes above three calls.
                currentDNRepository.DeletePalletAttrLogDefered(session.UnitOfWork, pnList);
                currentPltRepository.DeletePalletAttrsDefered(session.UnitOfWork, pnList);
                currentPltRepository.DeletePalletsDefered(session.UnitOfWork, pnList);
                if (prj != null && prj == "Docking")
                {
                    //Call SP:Docking_DeleteDeliveryOfPC to delete DN related records.
                    //SP in-args:
                    //  @DeliveryNo char(20)
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                                                               "Docking_DeleteDeliveryOfPC",
                                                               new SqlParameter("DeliveryNo", dn));
                }

                #region 清空Pallet weight
                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IList<string> palletNoList = currentDNRepository.GetPalletNoListByDeliveryNo(dn);
                foreach (string pn in palletNoList)
                {
                    //mantis1666: Unpack DN by DN，清除棧板庫位時若unpack 的 DN為棧板唯一的DN才能清庫位
                    //在Pallet 結合DN最後一筆時，才能清空Pallet Location 
                    Pallet pallet = currentPltRepository.Find(pn);
                    IList<string> dnList = currentProductRepository.GetDeliveryNoListByPalletNo(pn);
                    if (dnList.Count < 2)
                    {
                        PakLocMasInfo setVal = new PakLocMasInfo();
                        PakLocMasInfo cond = new PakLocMasInfo();
                        setVal.editor = Editor;
                        setVal.pno = "";
                        cond.pno = pn;
                        cond.tp = "PakLoc";
                        currentPltRepository.UpdatePakLocMasInfoDefered(session.UnitOfWork, setVal, cond);
                        //Clear Floor in Pallet                    
                        pallet.Floor = "";
                        //Clear Floor in Pallet                    
                    }

                    //Clear  weight in Pallet 
                    pallet.Weight = 0;
                    pallet.Weight_L = 0;                   
                    if (!pnList.Contains(pn))
                    {
                        PalletLog palletLog = new PalletLog { PalletNo = pallet.PalletNo, Station = "RETURN", Line = "Weight:0", Cdt = DateTime.Now, Editor = this.Editor };
                        pallet.AddLog(palletLog);
                        currentPltRepository.Update(pallet, session.UnitOfWork);
                    }
                }
              
                #endregion
            }
            else if (ship != null && ship != "")
            {
                IList<string> pnList = currentDNRepository.GetPalletNoListByShipmentAndWithSoloShipment(ship);
                currentDNRepository.DeleteDeliveryAttrLogByShipmentNoDefered(session.UnitOfWork, ship);
                currentDNRepository.DeleteDeliveryAttrsByShipmentNoDefered(session.UnitOfWork, ship);
                currentDNRepository.DeleteDeliveryInfoByShipmentNoDefered(session.UnitOfWork, ship);
                currentDNRepository.DeleteDeliveryPalletByShipmentNoDefered(session.UnitOfWork, ship);
                currentDNRepository.DeleteDeliveryByShipmentNoDefered(session.UnitOfWork, ship);

                currentDNRepository.RemoveDeliveryExDefered(session.UnitOfWork, ship);

                currentDNRepository.DeletePalletAttrLogDefered(session.UnitOfWork, pnList);
                currentPltRepository.DeletePalletAttrsDefered(session.UnitOfWork, pnList);
                currentPltRepository.DeletePalletsDefered(session.UnitOfWork, pnList);
                if (prj != null && prj == "Docking")
                {
                    //Call SP:Docking_DeleteShipmentOfPC to delete Shipment related records.
                    //SP in-args:
                    //  @ShipmentNo char(20)
                    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    productRepository.ExecSpForNonQuery(SqlHelper.ConnectionString_PAK,
                                                               "Docking_DeleteShipmentOfPC",
                                                               new SqlParameter("ShipmentNo", ship));
                }

                #region 清空Pallet weight
                IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                IList<string> palletNoList = currentDNRepository.GetPalletNoListByShipmentNo(ship);
                foreach (string pn in palletNoList)
                {
                    //mantis1666: Unpack DN by DN，清除棧板庫位時若unpack 的 DN為棧板唯一的DN才能清庫位
                    //在Pallet 結合DN最後一筆時，才能清空Pallet Location 
                    Pallet pallet = currentPltRepository.Find(pn);
                    IList<string> dnList = currentProductRepository.GetDeliveryNoListByPalletNo(pn);
                    if (dnList.Count < 2)
                    {
                        PakLocMasInfo setVal = new PakLocMasInfo();
                        PakLocMasInfo cond = new PakLocMasInfo();
                        setVal.editor = Editor;
                        setVal.pno = "";
                        cond.pno = pn;
                        cond.tp = "PakLoc";
                        currentPltRepository.UpdatePakLocMasInfoDefered(session.UnitOfWork, setVal, cond);
                        //Clear Floor in Pallet                    
                        pallet.Floor = "";
                        //Clear Floor in Pallet                    
                    }

                    //Clear  weight in Pallet 
                    pallet.Weight = 0;
                    pallet.Weight_L = 0;
                    if (!pnList.Contains(pn))
                    {
                        PalletLog palletLog = new PalletLog { PalletNo = pallet.PalletNo, Station = "RETURN", Line = "Weight:0", Cdt = DateTime.Now, Editor = this.Editor };
                        pallet.AddLog(palletLog);
                        currentPltRepository.Update(pallet, session.UnitOfWork);
                    }
                }

                #endregion
            }
            
            return base.DoExecute(executionContext);
        }
	}
}
