﻿// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-02   Du Xuan (itc98066)          create
// ITC-1360-1426 根据UC定义修改6060B0763601打印label
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.FA.LCM;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using System.Linq;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// 2 China label, GOST label, ICASA Label, ICASA Label2, KC Label, Taiwan Label 的列印条件
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
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
    public partial class CheckLabelLanguagePrint : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckLabelLanguagePrint()
		{
			InitializeComponent();
		}
        private IList<string> GetEnergyPartNoList()
        {
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList =
                       PartRepository.GetConstValueTypeList("ChinaLabelPartNo").Select(x => x.value).Where(y => y != "").Distinct().ToList();
        //    CurrentSession.AddValue("EnergyLabel", "");
            return valueList;
        }



        /// <summary>
        /// Check RMN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            CurrentSession.AddValue("LanguageMessage", "");

            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            
            Delivery curDelivery = DeliveryRepository.Find(curProduct.DeliveryNo);
            Model curModel = modelRep.Find(curProduct.Model);
            

            //1.	请参考下表，如果ModelBOM 中Model 的直接下阶存在对应的Part 时，就表明需要列印该Label
            //，如果都不存在，则不需要列印这些Label （如果存在，应只存在其中一个）
            //Label	        Part No
            //China label	6060B0464901        
            //China label	6060BCHINA01        
            //GOST label	6060B000GS01        
            //GOST label	6060B000GS02        
            //ICASA Label	6060BICASA01        
            //ICASA Label for South Africa	6060B0763601        
            //KC Label	6060B000KC01        
            //Taiwan Label	6060B0487001        
            //Taiwan Label	6060BTAIWAN1        

            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(curProduct.Model);
            IList<IBOMNode> bomList = curBom.FirstLevelNodes;
            string language = string.Empty;

            int checkChina = 0;
            string site = (string)CurrentSession.GetValue("Site");
            IList<string> lstPartNo = GetEnergyPartNoList();
            foreach (IBOMNode node in bomList)
            {
                if (site == "ICC")
                {
                    if (lstPartNo.Count>0 && lstPartNo.Contains(node.Part.PN))
                    {
                        checkChina = 2;
                        language = "China label";
                        if (node.Part.PN == "6060B0464901")
                        {
                            checkChina = 3;
                        }
                        break;
                    }
                
                }
                if (site!="ICC" && node.Part.PN == "6060B0464901" || node.Part.PN == "6060BCHINA01")
                {
                    checkChina =2;
                    if (node.Part.PN == "6060B0464901")
                    {
                        checkChina =3;
                    }
                    language = "China label";
                    break;
                }
                else if(node.Part.PN == "6060B000GS01" || node.Part.PN == "6060B000GS02")
                {
                    language = "GOST Lable";
                    break;
                }
                else if (node.Part.PN == "6060BICASA01")
                {
                    language = "ICASA Label L";
                    break;
                }
                else if (node.Part.PN == "6060B0763601")
                {
                    language = "ICASA Label L";
                    break;
                }
                else if (node.Part.PN == "6060B000KC01")
                {
                    language = "KC Label";
                    break;
                }
                else if (node.Part.PN == "6060B0487001" || node.Part.PN == "6060BTAIWAN1")
                {
                    language = "Taiwan Label";
                    break;
                }
            }
            if (string.IsNullOrEmpty(language))
            {
                //无需打印
                CurrentSession.AddValue("LanguageLabel", "");
                return base.DoExecute(executionContext);
            }
            else
            {
                CurrentSession.AddValue("LanguageLabel", language);
            }

            //当需要列印ICASA Label, ICASA Label2时，
            if (language == "ICASA Label L")
            {
                //对于在ModelBOM 中Model 的直接下阶中BomNodeType = 'PL' AND IMES_GetData..Part.Descr='ICASA Label2' 的Part，
                //并且与Product 结合的Part 中存在BomNodeType = 'KP' 并且Vendor CT （IMES_FA..Product_Part.PartSn）以'G' 开头的Part 时，
                //取与Product 结合的Part 中BomNodeType = 'KP' 并且Vendor CT （IMES_FA..Product_Part.PartSn）以'G' 开头的Part 的记录，
                //取这些Vendor CT 的前5位（作Distinct），使用这些不同的Vendor CT 前5位等于FA..ICASA.CT 查询FA..ICASA，查询到的ICASA 字段值，
                //即是环境变量ICASA 的取值
                //如果有使用Vendor CT 前5位等于FA..ICASA.CT 查询FA..ICASA 表没有记录的情况，
                //则报告错误：“存在未Maintain 的CT，请IE Maintain!”
                ILCMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository>();
                foreach (IBOMNode node in bomList)
                {
                    if (node.Part.BOMNodeType != "PL" || node.Part.Descr != "ICASA Label2")
                    {
                        continue;
                    }
                    IList<ProductPart> partList = partRep.GetProductPart("G", "KP", curProduct.ProId);
                    
                    foreach (ProductPart part in partList)
                    {
                        string vendorCT = part.PartSn.Substring(0, 5);
                        ICASADef iCASA = itemRepository.GetICASAInfoByVC(vendorCT);
                        if (iCASA == null)
                        {
                            FisException fe = new FisException("PAK145", new string[] { });   //存在未Maintain 的CT，请IE Maintain!
                            throw fe;
                        }
                        // For Mantis 0001569
                        else if (string.IsNullOrEmpty(iCASA.icasa) || string.IsNullOrEmpty(iCASA.anatel1) || string.IsNullOrEmpty(iCASA.anatel2) )
                        {
                            FisException fe = new FisException("PAK145", new string[] { });   //存在未Maintain 的CT，请IE Maintain!
                            throw fe;
                        }
                        // For Mantis 0001569
                        
                    }
                  

                }

            }



           //对于China Label 需要在列印完Label 后，满足下文所述情况，需提示用户：@family + “请贴” + @lpn + “ China Label!”
            if (language == "China label")
            {
                string size = string.Empty;
                //a)取Product 绑定的Delivery (IMES_FA..Product.DeliveryNo)赋值给变量@dn
                string dn = curProduct.DeliveryNo;
                //b)取Product Family (IMES_GetData..Model.Family)赋值给变量@family
                string family = curModel.FamilyName;
                //c)取Delivery 的属性RegId (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'RegId') / Flag (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Flag') / Country (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Country')
                //赋值给变量@regid / @flag / @country
                string regid = (string)curDelivery.GetExtendedProperty("RegId");
                if (regid != null && regid.Length == 3)
                { regid = regid.Substring(1, 2); }
                else
                { regid = ""; }
                string flag = (string)curDelivery.GetExtendedProperty("Flag");
                string country = (string)curDelivery.GetExtendedProperty("Country");

                //d)取@family第一个空格前的字符串赋值给变量@family3
                string[] splitpattern = family.Split(' ');
                string family3 = splitpattern[0].Trim();
                string family2 = string.Empty;

                //e)如果使用UPPER(Family) like @family3 +'%' 查询IMES_PAK..ChinaLabel表存在记录，
                //并且在ModelBOM 中Model的直接下阶中存在Part No = '6060B0464901' 的记录时，按照如下方法获取@lpn；
                //如果不满足上述条件，则不需要提示，退出
                IList<ChinaLabelInfo> chinaList = palletRep.GetChinaLabelByLikeFamily(family3);
                if ((checkChina !=3) ||(chinaList.Count==0))
                {
                    //无需提示
                    CurrentSession.AddValue("LanguageMessage", "");
                    return base.DoExecute(executionContext);
                }

                //i.首先在IMES_FA..Product_Part 表中取PartSn 以字符'C' 开头BomNodeType 为'KP' 的记录的PartSn 的前5为赋值给变量@lcm
                //ii.取在ModelBOM 中@lcm 直接上阶BomNodeType 为'KP' 的Part 的Descr 属性，
                //ModelBOM.Component = @lcm
                //并将Descr 属性的第5，6位字符串赋值给变量@size
                
                IList<ProductPart> partList = partRep.GetProductPart("C", "KP",curProduct.ProId);
                if (partList.Count == 0)
                {
                    return base.DoExecute(executionContext);
                }
                string lcm = partList[0].PartSn.Substring(0, 5);

                IList<IBOMNode> tmpList = bomRep.GetParentBomNode(lcm);
                foreach (IBOMNode node in tmpList)
                {
                    if (node.Part.Type == "KP")
                    {
                        size = node.Part.Descr.Substring(4, 2);
                        break;
                    }
                }

                string lpn = string.Empty;
                IList<ChinaLabelInfo> cList = new List<ChinaLabelInfo>();
                ChinaLabelInfo citem= new ChinaLabelInfo();
                //iii.	如果@family 中含有'DD' 或者'TT'，则按照如下方法获取@lpn
                if (family.Contains("DD") || family.Contains("TT"))
                {
                    //SELECT @family2=LEFT(@family,CHARINDEX(' ',@family)-1)+' '+@size
                    //SELECT @lpn=PN FROM ChinaLabel WHERE Family=@family2
                    family2 = splitpattern[0]+ size;
                    citem.family = family2;
                    cList = palletRep.GetChinaLabel(citem);
                    lpn = cList[0].pn;
                }
                else //iv.	如果@family 中不含有'DD' 和'TT'，则按照如下方法获取@lpn
                {
                    //IF CHARINDEX(' ',@family)>0
                    //BEGIN
	                //SELECT @lpn=PN FROM ChinaLabel WHERE Family=LEFT(@family,CHARINDEX(' ',@family)-1)
                    //END
                    //ELSE
                    //BEGIN
	                //SELECT @lpn=PN FROM ChinaLabel WHERE UPPER(Family)=RTRIM(UPPER(@family))
                    //END
                    if (splitpattern.Length > 1)
                    {
                        citem.family = splitpattern[0];
                        cList = palletRep.GetChinaLabel(citem);
                        lpn = cList[0].pn;
                    }
                    else
                    {
                        citem.family = family.ToUpper();
                        cList = palletRep.GetChinaLabel(citem);
                        lpn = cList[0].pn;
                    }
                }

                string languageMsg = string.Empty;
                //v.	如果ModelBOM 中Model 的直接下阶中存在Part No 为'6060B0464901' 或者'6060BCHINA01' 的Part，
                //并且(@regid='SCN' or (@flag='C' AND @country='CHINA' ))时，需要提示用户，并记录ProductLog；否则不需要提示
                //记录ProductLog
                //Line – 'China Label'
                //Station – 'CL'
                //Status – '1'
                //Editor – 'PAK'
                 
                //if ((regid == "CN" || flag == "C") && (country == "CHINA"))
                if ((ActivityCommonImpl.Instance.CheckDomesticDN(regid) || flag == "C") && (country == "CHINA"))
                {
                    //@family + “请贴” + @lpn + “ China Label!”
                    languageMsg = family + "请贴" + lpn + " China Label!";
                    CurrentSession.AddValue("LanguageMessage", languageMsg);

                    var productLog = new ProductLog
                    {
                        Model = curProduct.Model,
                        Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                        Editor = "PAK",
                        Line = "China Label",
                        Station = "CL",
                        Cdt = DateTime.Now
                    };

                    curProduct.AddLog(productLog);
                    productRepository.Update(curProduct, CurrentSession.UnitOfWork);
                }
                else
                {
                    CurrentSession.AddValue("LanguageMessage", "");
                }

            }

	        return base.DoExecute(executionContext);
        }
	}
}
