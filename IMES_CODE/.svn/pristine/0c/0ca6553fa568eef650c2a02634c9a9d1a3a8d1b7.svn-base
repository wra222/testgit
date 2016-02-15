/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckDataForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create
依据刷入不同的code判断方式不同：
A. 刷入的是DN
select @instr = INSTR_FLAG from [PAK_PAKComn] nolock where left(InternalID,10)=@DN
if upper(rtrim(@instr)) = 'X'
begin
	if not exists(select * from dbo.PAKEDI_INSTR where PO_NUM = @PO_NUM)
	begin
		select '0',@DN+' :此船务需要Instruction' 
		return
	end  
end
B.刷入的是Shipment
select distinct  PO_NUM into #SInstr from [PAK_PAKComn] nolock where InternalID in (select InternalID from #3) and upper(INSTR_FLAG) = 'X'
if exists (select * from #SInstr where PO_NUM not in (select PO_NUM from dbo.PAKEDI_INSTR))
begin
	select '0',@DN+' :缺少Instruction资料'
	return
end
C.输入的是WayBill Number
select distinct PO_NUM  into #WInstr from [PAK_PAKComn] nolock where InternalID in (select InternalID from #1) and upper(INSTR_FLAG) = 'X'
if exists (select * from #WInstr where PO_NUM not in (select PO_NUM from dbo.PAKEDI_INSTR))
begin
	select '0',@DN+' :缺少Instruction资料'
	return
end

* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PAK.Paking;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity.Special
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckInstrForPackingList : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckInstrForPackingList()
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
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            bool bInstr = false;

            switch (InputType)
            {
                case InputTypeEnum.DN:
                    string dn = (string)CurrentSession.GetValue("Data");
                    IList<PakDashPakComnInfo> dashList = new List<PakDashPakComnInfo>();
                    //select @instr = INSTR_FLAG from [PAK_PAKComn] nolock where left(InternalID,10)=@DN
                    //接口异常
                    
                    dashList = repPizza.GetPakDashPakComnListByInternalID(dn.Substring(0, 10));
                    foreach (PakDashPakComnInfo temp in dashList)
                    {
                        if (temp.instr_flag.TrimEnd().ToUpper() == "X")
                        {
                            //exists(select * from dbo.PAKEDI_INSTR where PO_NUM = @PO_NUM)
                            bInstr = repPizza.CheckExistPakEdiInstr(temp.po_num);
                            if (bInstr == false)
                            {
                                FisException ex;
                                List<string> erpara = new List<string>();
                                //ITC-1360-1095 提示信息错误
                                erpara.Add(dn);
                                ex = new FisException("CHK818", erpara);
                                //ex.stopWF = false;
                                throw ex;
                            }
                        }
                    }                    
                    break;
                case InputTypeEnum.DNForPL:
                    //3G.
                    string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                    IList<string> instrList = new List<string>();
                    instrList = repPizza.GetInstrFlagListFromPakDotPakComn(deliveryNo);
                    IList<VShipmentPakComnInfo> listShip = (IList<VShipmentPakComnInfo>)CurrentSession.GetValue("ComnList");

                    foreach (string ins in instrList)
                    {
                        if (ins.TrimEnd() == "X")
                        {
                            foreach (VShipmentPakComnInfo temp in listShip)
                            {
                                if (!repPizza.CheckExistPakEdiInstr(temp.po_num))
                                {
                                    FisException ex;
                                    List<string> erpara = new List<string>();                                    
                                    ex = new FisException("CHK818", erpara);
                                    throw ex;
                                }
                            }
                        }
                    }
                    break;
                case InputTypeEnum.Shipment:
                    IList<VShipmentPakComnInfo> comnList2 = (IList<VShipmentPakComnInfo>)CurrentSession.GetValue("ComnList");
                    string shipment = (string)CurrentSession.GetValue("Data");

                    foreach (VShipmentPakComnInfo temp in comnList2)
                    {
                        IList<string> internalId2 = new List<string>();
                        IList<string> instrList2 = new List<string>();

                        internalId2.Add(temp.internalID);

                        instrList2 = repPizza.GetPoNumListFromPakDashPakComn(internalId2, "X");
                        bInstr = repPizza.CheckExistPoNumNotInPakEdiInstr(instrList2);
                        //true:
                        if (bInstr == true)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            ex = new FisException("CHK818", erpara);
                            //ex.stopWF = false;
                            throw ex;
                        }
                    }
                    break;
                case InputTypeEnum.Waybill:
                    IList<VShipmentPakComnInfo> comnList3 = (IList<VShipmentPakComnInfo>)CurrentSession.GetValue("ComnList");
                    string waybill = (string)CurrentSession.GetValue("Data");


                    foreach (VShipmentPakComnInfo temp in comnList3)
                    {
                        IList<string> internalId3 = new List<string>();
                        IList<string> instrList3 = new List<string>();
                        internalId3.Add(temp.internalID);

                        instrList3 = repPizza.GetPoNumListFromPakDashPakComn(internalId3, "X");
                        bInstr = repPizza.CheckExistPoNumNotInPakEdiInstr(instrList3);
                        //接口疑问???????
                        if (bInstr == true)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            ex = new FisException("CHK818", erpara);
                            //ex.stopWF = false;
                            throw ex;
                        }
                    }
                    break;
                default:
                    FisException ex1;
                    List<string> erpara1 = new List<string>();
                    erpara1.Add((string)CurrentSession.GetValue("Data"));
                    ex1 = new FisException("CHK816", erpara1);
                    //ex1.stopWF = false;
                    throw ex1;
                //break;
            }
            return base.DoExecute(executionContext);
        }
        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(CheckInstrForPackingList));

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArugment Of CheckInstrForPackingList")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(CheckInstrForPackingList.InputTypeProperty)));
            }
            set
            {
                base.SetValue(CheckInstrForPackingList.InputTypeProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:DN,Shipment,Waybill
        /// </summary>
        public enum InputTypeEnum
        {
            /// <summary>
            /// 
            /// </summary>
            DN = 0,
            /// <summary>
            /// 
            /// </summary>
            Shipment = 1,
            /// <summary>
            /// 
            /// </summary>
            Waybill = 2,
            /// <summary>
            /// 
            /// </summary>
            DNForPL = 3, 
        }
    }
}
