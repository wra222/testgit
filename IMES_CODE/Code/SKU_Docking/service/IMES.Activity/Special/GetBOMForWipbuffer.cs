// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据wipbuffer相关Bom，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-02   Du Xuan (itc98066)          create
// ITC-1360-1454 根据UC增加RUSSIA LABEL的相关处理
// Known issues:
using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的productID,RMN 检查RMN，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Pallet为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据输入的productID,RMN 检查RMN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class GetBOMForWipbuffer : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public GetBOMForWipbuffer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check RMN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            string ID = (string)CurrentSession.GetValue(Session.SessionKeys.RMN);
            string spno = string.Empty;
            string model = string.Empty;
            string family = string.Empty;
            string family2 = string.Empty;
            string family3 = string.Empty;
            string size = string.Empty;
            string pno = string.Empty;

            //string pattern = "SS09";

            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

            //取Product Model（IMES_FA..Product.Model）
            //取Family(IMES_GetData..Model.Family)
            //并存放到变量@Pno，@family 中
            pno = curProduct.Model.Trim();
            family = modelRep.Find(pno).FamilyName;


            //Get BOM
            //a)取ModelBOM 中Model 的直接下阶数据
            //#Bom 定义如下： MPno 父，SPno 子
            //CREATE TABLE #Bom (MPno char(20),SPno char(20) )
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(pno);
            IList<IBOMNode> bomNodeList = curBom.FirstLevelNodes;

            //b)取@syscode，@bc_code
            //SELECT @syscode=''
            //SELECT @syscode=os_code FROM Bom_Code (nolock) WHERE part_number=@Pno
            //SELECT @bc_code=SUBSTRING(@Pno,7,1)
            Boolean syscodeflag = false;
            IList<string> codeList = bomRep.GetOsCodeFromBomCode(pno);
            foreach (String codeNode in codeList)
            {
                if ((codeNode == "00010") || (codeNode == "00011"))
                {
                    syscodeflag = true;
                }
            }

            //c)确定是否需要取消Microsoft Label
            //IF not (RIGHT(RTRIM(@Pno),1)='A' 
            //or RIGHT(RTRIM(@Pno),1)='B' 
            //or @syscode='00010' 
            //or @syscode='00011' 
            //or CHARINDEX(@bc_code,'0123456789') >0)
            //BEGIN
            //DELETE FROM #Bom WHERE SPno='6060B0232501' AND MPno=@Pno
            //DELETE FROM #Bom WHERE SPno='6060B0153901' AND MPno=@Pno
            //END
            string bcCode = pno.Substring(7, 1);
            string tmpPno = pno.Substring(pno.Length - 1);
            string tmpStr = "0123456789";
            int index = tmpStr.IndexOf(bcCode);
            if (!(tmpPno == "A") || (tmpPno == "B") || syscodeflag || (index >= 0))
            {
                //foreach (IBOMNode bomNode in bomNodeList)
                for (int i = bomNodeList.Count-1; i >= 0; i--)
                {
                    IBOMNode bomNode = bomNodeList[i];
                    if ((bomNode.Part.PN == "6060B0232501") || (bomNode.Part.PN == "6060B0153901"))
                    {
                        bomNodeList.RemoveAt(i);
                    }
                }
            }

            //d)取得与Product 绑定的Delivery 及该Delivery 的RegId
            //(IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'RegId') 
            // Flag (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Flag') 
            // Country (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Country') 
            // PartNo (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'PartNo') 
            // SO (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'SO')等属性，
            //分别保存到变量@dn / @regid / @flag / @country / @PN / @so 中
            String DN = string.Empty;
            String regID = string.Empty;
            String flag = string.Empty;
            String country = string.Empty;
            String PN = string.Empty;
            String so = string.Empty;


            Delivery delivery = deliveryRep.Find(curProduct.DeliveryNo);
            if (delivery != null)
            {
                IList<DeliveryInfo> dInfoList = delivery.DeliveryInfoes;
                regID = (string)delivery.GetExtendedProperty("RegId");
                flag = (string)delivery.GetExtendedProperty("Flag");
                country = ((string)delivery.GetExtendedProperty("Country"))??string.Empty;
                PN = (string)delivery.GetExtendedProperty("PartNo");
                so = (string)delivery.GetExtendedProperty("SO");
                DN = delivery.DeliveryNo;
            }
            //if (string.IsNullOrEmpty(country))
            //{
            //    FisException fe = new FisException("PAK047", new string[] {"country"});  //DeliveryInfo %1 的资料维护不全！
            //    throw fe;
            //}

            //e)确定是否需要取消Russian Label
            //i. 如果ModelBOM 中Model 的直接下阶存在'60' 开头，
            //UPPER(Descr) in ('RUSSIA LABEL1','RUSSIA LABEL2') 的Part时，
            //取其中UPPER(Descr) = 'RUSSIA LABEL1' 的Part 的Part No 赋值给变量@Lable1；
            //取其中UPPER(Descr) = 'RUSSIA LABEL2' 的Part 的Part No 赋值给变量@Lable2

            //ii. 当(RTRIM(RIGHT(@PN,3))='ACB') AND (CHARINDEX('RUSSIAN',UPPER(@country))>0) 时
            //如果LEFT(@so,2)='61'，则从#Bom 中删除掉@Lable1 所指Part；
            //否则从#Bom 中删除掉@Lable2 所指Part

            //iii.	当不满足(RTRIM(RIGHT(@PN,3))='ACB') AND (CHARINDEX('RUSSIAN',UPPER(@country))>0)的时候，
            //从#Bom 中删除掉@Lable1 所指Par 和@Lable2 所指Part

            if (!string.IsNullOrEmpty(pno))
            {
                if (pno.Length >= 3)
                {
                    tmpPno = pno.Substring(pno.Length - 3);
                }
            }
            if (!string.IsNullOrEmpty(so))
            {
                if (so.Length >= 2)
                {
                    tmpStr = so.Substring(0, 2);
                }
            }
            index = country.IndexOf("RUSSIAN");

            //foreach (IBOMNode bomNode in bomNodeList)
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
            if (valueList.Count == 0)
            {
                throw new FisException("PAK095", new string[] { "Site" });
            }
            if (valueList[0].Trim() == "ICC")
            {
                CurrentSession.AddValue(Session.SessionKeys.SessionBom, bomNodeList);
                return base.DoExecute(executionContext);
            }

            for (int i = bomNodeList.Count-1; i>=0; i--)
            {
                IBOMNode bomNode = bomNodeList[i];
                String str = bomNode.Part.PN.Substring(0, 2);
                if (str == "60")
                {
                    if ((tmpPno == "ACB") && (index > 0))
                    {
                        if (tmpStr == "61")
                        {
                            if (bomNode.Part.Descr.ToUpper() == "RUSSIA LABEL1")
                            {
                                bomNodeList.RemoveAt(i);
                            }
                        }
                        else
                        {
                            if (bomNode.Part.Descr.ToUpper() == "RUSSIA LABEL2")
                            {
                                bomNodeList.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        if ((bomNode.Part.Descr.ToUpper() == "RUSSIA LABEL1")
                            || (bomNode.Part.Descr.ToUpper() == "RUSSIA LABEL2"))
                        {
                            bomNodeList.RemoveAt(i);
                        }
                    }
                }
            }



            CurrentSession.AddValue(Session.SessionKeys.SessionBom, bomNodeList);
            return base.DoExecute(executionContext);
        }
    }
}
