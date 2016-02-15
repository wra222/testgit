/* INVENTEC corporation (c)2011 all rights reserved. 
 * Description: MoveIn FloorAreaLoc 
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * Known issues:
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Misc;
using IMES.Common;

namespace IMES.Activity
{
   /// <summary>
    ///  CreateDefectComponent 
   /// </summary>
    public partial class CreateDefectComponent: BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateDefectComponent()
        {
            InitializeComponent();
        }       
        
		
		/// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AllowStatusProperty = DependencyProperty.Register("AllowStatus", typeof(string), typeof(CreateDefectComponent));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("AllowStatus")]
        [CategoryAttribute("AllowStatus Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AllowStatus
        {
            get
            {
                return ((string)(base.GetValue(CreateDefectComponent.AllowStatusProperty)));
            }
            set
            {
                base.SetValue(CreateDefectComponent.AllowStatusProperty, value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(CreateDefectComponent));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(CreateDefectComponent.StatusProperty)));
            }
            set
            {
                base.SetValue(CreateDefectComponent.StatusProperty, value);
            }
        }


		/// <summary>
        /// 
        /// </summary>
        public static DependencyProperty LogActionNameProperty = DependencyProperty.Register("LogActionName", typeof(string), typeof(CreateDefectComponent));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("LogActionName")]
        [CategoryAttribute("LogActionName Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string LogActionName
        {
            get
            {
                return ((string)(base.GetValue(CreateDefectComponent.LogActionNameProperty)));
            }
            set
            {
                base.SetValue(CreateDefectComponent.LogActionNameProperty, value);
            }
        }
		
		
        
        /// <summary>
        /// CheckAndLockSelectPallet
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            string[] AllowStatuses = AllowStatus.Split('~');

            IProduct prod = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            RepairInfo repairInfo = session.GetValue("RepairInfo") as RepairInfo;
            string OldPartSno = repairInfo.oldPartSno;
            int repairID = int.Parse(repairInfo.repairID);
            int defectId = int.Parse((string)session.GetValue("DefectId"));

            string oldPart = prod.Repairs.Where(x => x.ID == repairID).FirstOrDefault().Defects.Where(y => y.ID == defectId).FirstOrDefault().OldPart;
            oldPart = (null == oldPart) ? "" : oldPart;

            string partType = "", bomNodeType = "";
            IList<IProductPart> lstProductPart = prod.ProductParts.Where(x => x.PartID == oldPart).ToList();
            if (null != lstProductPart && lstProductPart.Count > 0)
            {
                partType = (null == lstProductPart[0].PartType) ? "" : lstProductPart[0].PartType;
                bomNodeType = (null == lstProductPart[0].BomNodeType) ? "" : lstProductPart[0].BomNodeType;
            }

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string IECPn = partRep.GetPartInfoValue(oldPart, "RDESC");
            IECPn = (null == IECPn) ? "" : IECPn;

            DefectComponentInfo condQuery = new DefectComponentInfo();
            condQuery.PartSn = OldPartSno;

            IList<string> lstPartSN = new List<string>();
            lstPartSN.Add(OldPartSno);

            IList<DefectComponentInfo> lstDefectComponent = miscRep.GetDataByList<IMES.Infrastructure.Repository._Metas.DefectComponent, DefectComponentInfo>(condQuery, "PartSn", lstPartSN);

            IProductPart pp = null;


            // 若@OldPartSno與@OldPart 不同(表示有唯一的PartSn)：
            // 才進行以下退料追蹤程序
            if (OldPartSno == oldPart)
                return base.DoExecute(executionContext);


            if (null != lstDefectComponent)
                lstDefectComponent = lstDefectComponent.Where(x => x.RepairID == repairID).ToList();

            DefectComponentInfo dc = null;
            if (null == lstDefectComponent || lstDefectComponent.Count == 0)
            {
                dc = new DefectComponentInfo();
                CreateDefectComponentInfo(dc, repairInfo, prod, pp, oldPart, partType, bomNodeType, IECPn, repairInfo.partType);
                dc.Cdt = DateTime.Now;

                miscRep.InsertDefectComponentAndLogDefered(session.UnitOfWork, dc, this.LogActionName, "");
            }
            else
            {
                dc = lstDefectComponent[0];
                
                if (AllowStatuses.Contains(dc.Status))
                {
                    CreateDefectComponentInfo(dc, repairInfo, prod, pp, oldPart, partType, bomNodeType, IECPn, repairInfo.partType);

                    miscRep.InsertDataWithIDDefered<IMES.Infrastructure.Repository._Metas.DefectComponentLog, DefectComponentLogInfo>(session.UnitOfWork,
                        new DefectComponentLogInfo
                        {
                            ActionName = this.LogActionName,
                            Remark = "",
                            ComponentID = dc.ID,
                            BatchID = "",
                            Customer = this.Customer,
                            Model = prod.Model,
                            Family = prod.Family == null ? "" : prod.Family,
                            DefectCode = repairInfo.defectCodeID,
                            DefectDescr = repairInfo.defectCodeDesc,
                            ReturnLine = "",
                            PartSn = repairInfo.oldPartSno,
                            RepairID = repairID,
                            Comment = "",
                            Status = this.Status,
                            Editor = this.Editor,
                            Cdt = DateTime.Now
                        }
                        );

                    DefectComponentInfo condUpd = new DefectComponentInfo();
                    condUpd.ID = dc.ID;
                    miscRep.UpdateDataByIDDefered<IMES.Infrastructure.Repository._Metas.DefectComponent, DefectComponentInfo>(session.UnitOfWork, condUpd, dc);

                }
                else
                {
                    // 此機器：@ProductID換下的舊料：@OldPartSno 已進入退料覆判流程，不可打印!
                    throw new FisException("CQCHK5018", new List<string> { prod.ProId, OldPartSno });
                }
                
            }

            return base.DoExecute(executionContext);
        }

        void CreateDefectComponentInfo(DefectComponentInfo v, RepairInfo ri, IProduct prod, IProductPart pp, string OldPart, string PartType, string BomNodeType, string IECPn, string CheckItemType)
        {
            v.RepairID = int.Parse(ri.repairID);
            v.BatchID = "";
            //remark
            v.Customer = this.Customer;
            v.Model = prod.Model;
            v.Family = prod.Family == null ? "" : prod.Family;
            v.DefectCode = ri.defectCodeID;
            v.DefectDescr = ri.defectCodeDesc == null ? "" : ri.defectCodeDesc;
            v.ReturnLine = "";
            v.PartSn = ri.oldPartSno;
            v.PartNo = OldPart;
            v.PartType = PartType;
            v.BomNodeType = BomNodeType;
            v.IECPn = IECPn;
            v.CustomerPn = "";
            v.Vendor = "";
            v.CheckItemType = CheckItemType;
            v.Comment = "";
            v.Status = this.Status;
            v.Editor = this.Editor;
            v.Cdt = DateTime.Now;
            v.Udt = DateTime.Now;
        }
       
        
    }
}
