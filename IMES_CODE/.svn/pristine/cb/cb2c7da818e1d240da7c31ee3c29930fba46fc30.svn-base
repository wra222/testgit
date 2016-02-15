// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据输入的productID,RMN 检查RMN，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Unit Weight.docx
// UC:CI-MES12-SPEC-PAK-UC Unit Weight.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-02   Chen Xu (itc208014)          create
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;

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
    public partial class CheckRMN : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckRMN()
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
            //2012-09-24:若ModelBOM 中Model 直接下阶存在Descr in ('China label','TAIWAN Label')，Check RMN
            //获取@RMNModel =  Upper(rtrim(ModelInfo.Value))(Condition: ModelInfo.Model=Product.Model# and Name=’RMN’)
            //若@RMNModel<>Upper(rtrim(@ID))，则报错“此China Label or Taiwan Label 与机型不匹配！”；
            //否则，检查通过，将RMN 显示在UI上的RMN Label 中
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string ID = (string)CurrentSession.GetValue(Session.SessionKeys.RMN);
            string spno = string.Empty;
            IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IModelRepository imodelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IHierarchicalBOM sessionBOM = ibomRepository.GetHierarchicalBOMByModel(currentProduct.Model);
            IList<IBOMNode> bomNodeLst = sessionBOM.FirstLevelNodes;
            bool bNeedCheckRMN = false;
            bool bCheckPass = false;
            foreach (IBOMNode ibomnode in bomNodeLst)
            {
                IPart currentPart = ibomnode.Part;
                if (currentPart.Descr == "China label" || currentPart.Descr == "TAIWAN Label")
                {
                    spno = currentPart.PN;
                    bNeedCheckRMN = true;
                    break;
                }
            }

            if (bNeedCheckRMN)
            {
                IList<IMES.FisObject.Common.Model.ModelInfo> rmnList = imodelRepository.GetModelInfoByModelAndName(currentProduct.Model, "RMN");
                if (rmnList.Count > 0)
                {
                    string rmn = rmnList[0].Value;
                    if (rmn.Trim() == ID.Trim())
                    {
                        bCheckPass = true;
                    }
                }
            }

            if (!bCheckPass)
            {
                //Possible exceptions:
                //1.Not necessary to check RMN, that means no BOM parts with Descr as "China label" or "TAIWAN Label" can be found;
                //  In fact, this means this activity should not be executed.
                //2.No ModelInfo record with Model='this-model' and Name='RMN' can be found;
                //3.The Value first ModelInfo record with Model='this-model' and Name='RMN' does not match input RMN value.
                FisException fe = new FisException("PAK033", new string[] { });   //此China Label or Taiwan Label 与机型不匹配！
                fe.stopWF = false;
                throw fe;
            }
            CurrentSession.AddValue(Session.SessionKeys.PartNo, spno);
            return base.DoExecute(executionContext);



            /*
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string ID = (string)CurrentSession.GetValue(Session.SessionKeys.RMN);
            string spno = string.Empty;
            string model = string.Empty;
            string family = string.Empty;
            string family2 = string.Empty;
            string size = string.Empty;

            IProductRepository iproductRepository =RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository ipalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IModelRepository imodelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

            //查询ModelBOM 中Model 直接下阶存在Descr in ('China label','TAIWAN Label')的Part 

            IMES.FisObject.Common.FisBOM.IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IHierarchicalBOM sessionBOM = null;
            sessionBOM = ibomRepository.GetHierarchicalBOMByModel(currentProduct.Model);
            IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            bomNodeLst = sessionBOM.FirstLevelNodes;
            foreach (IBOMNode ibomnode in bomNodeLst)
            {
                IPart currentPart = ibomnode.Part;
                if (currentPart.Descr == "China label" ||currentPart.Descr =="TAIWAN Label")
                {
                    spno = currentPart.PN;
                    break;
                }
            }


            //使用条件Type,PartNo查询IMES_PAK..PAK_CHN_TW_Light 表，将得到的记录的Model 字段值
            IList<string> modelLst = ipalletRepository.GetModelByTypeAndPartNo(ID, spno);
            if (modelLst!=null && modelLst.Count> 0)
            {
                model=modelLst[0];
            }
            else
            {
                FisException fe = new FisException("PAK033", new string[] { });   //此China Label or Taiwan Label 与机型不匹配！
                fe.stopWF = false;
                throw fe;   
            }
            //  family = imodelRepository.Find(model).FamilyName; // Maintis: 0000784: unit weight 页面刷入taiwan label后报错: UC与产线确认MES_PAK..PAK_CHN_TW_Light 表实际没有用
            string curModel = currentProduct.Model;
            Model modelItem = imodelRepository.Find(curModel);
            if (modelItem != null && !string.IsNullOrEmpty(modelItem.FamilyName))
            {
                family = modelItem.FamilyName;
            }
            else
            {
                FisException fe = new FisException("CHK156", new string[] { curModel, "Family" });   //%1:%2参数不能为空。
                fe.stopWF = false;
                throw fe;
            }
            string pattern = "SS09";
            if (family.IndexOf(pattern) > -1)
            {
                string lcm = ipartRepository.GetPartSnListFromProductPart("C", "KP", currentProduct.ProId)[0].Substring(0, 5);
                
                //12. size=	取在ModelBOM 中@lcm 直接上阶BomNodeType 为'KP' 的Part 的Descr 属性的第5，6位字符串赋值给变量@size
                 //  IList<IBOMNode> GetParentBomNodeByPnListAndBomNodeType(IList<string> pnList, string bomNodeType)
                //SELECT LEFT(@family, CHARINDEX(' ', @family) - 1) AS famliy2 FROM Family WHERE (Family LIKE '% %')
                IList<IBOMNode> parentBomNodeList = new List<IBOMNode>();
                IList<string> pnList = new List<string>();
                pnList.Add(lcm);
                parentBomNodeList = ibomRepository.GetParentBomNodeByPnListAndBomNodeType(pnList, "KP");
                if (parentBomNodeList == null || parentBomNodeList.Count <= 0)
                {
                    //List<string> errpara = new List<string>();
                    //errpara.Add(lcm);
                    //throw new FisException("PAK073", errpara);
                    FisException fe = new FisException("PAK073", new string[] { lcm });   //此机器Family不符合RMN条件！
                    fe.stopWF = false;
                    throw fe;
 
                }
                size= parentBomNodeList[0].Part.Descr.Substring(4,2);  

                string[] splitpattern = family.Split(' ');
                if (splitpattern.Length < 3)
                {
                    FisException fe = new FisException("PAK032", new string[] { });   //此机器Family不符合RMN条件！
                    fe.stopWF = false;
                    throw fe;
                }
                family2 = splitpattern[0].Trim() + " " + splitpattern[1].Trim() + " " + size;
             }
            else 
            {
                string[] condition={"DIESEL","TIMEX","VILLEMONTD","VILLEMONTU","DD","TT","VV","PP","ZZ","VULCAIND","VULCAINU","VOLNAD","VOLNAU","NEUTRON","VILLEMONTD","VILLEMONTU","VALIMAU","VALIMAD","ROXETTE","RAMONES"};
                Boolean flag = false;
                int len = condition.Length;
                for (int i = 0; i < len; i++)
                {
                    if (family.Contains(condition[i]))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag || (family.Contains("PPD") && !family.Contains("SS09")) || (family.Contains("PPU") && !family.Contains("SS09")) || (family.Contains("ZI09") && !family.Contains("SS09")) || (family.Contains("ZZ09") && !family.Contains("SS09")))
                {
                    string lcm = ipartRepository.GetPartSnListFromProductPart("C", "KP", currentProduct.ProId)[0].Substring(0, 5);
                    
                    //12. size=	取在ModelBOM 中@lcm 直接上阶BomNodeType 为'KP' 的Part 的Descr 属性的第5，6位字符串赋值给变量@size
                    //  IList<IBOMNode> GetParentBomNodeByPnListAndBomNodeType(IList<string> pnList, string bomNodeType)
                    IList<IBOMNode> parentBomNodeList = new List<IBOMNode>();
                    IList<string> pnList = new List<string>();
                    pnList.Add(lcm);
                    parentBomNodeList = ibomRepository.GetParentBomNodeByPnListAndBomNodeType(pnList, "KP");
                    if (parentBomNodeList == null || parentBomNodeList.Count <= 0)
                    {
                        //List<string> errpara = new List<string>();
                        //errpara.Add(lcm);
                        //throw new FisException("PAK073", errpara);

                        FisException fe = new FisException("PAK073", new string[] { lcm });   //此机器Family不符合RMN条件！
                        fe.stopWF = false;
                        throw fe;

                    }
                    size = parentBomNodeList[0].Part.Descr.Substring(4, 2);  

                    string[] splitpattern = family.Split(' ');
                    if (splitpattern.Length < 2)
                    {
                        FisException fe = new FisException("PAK032", new string[] { });   //此机器Family不符合RMN条件！
                        fe.stopWF = false;
                        throw fe;
                    }
                    family2 = splitpattern[0].Trim() + " " + size;
                }
                else 
                {
                    string[] splitpattern = family.Split(' ');
                    if (splitpattern.Length >= 2)
                    {
                        family2 = splitpattern[0].Trim();
                    }
                    else
                    {
                        family2 = family.Trim().ToUpper();
                    }
                    
                }

                //使用Type=@ID AND PartNo=@spno AND Model=@family2 查询IMES_PAK..PAK_CHN_TW_Light 表存在记录
                IList<PakChnTwLightInfo> pakCTLInfoLst= new List<PakChnTwLightInfo>();
                pakCTLInfoLst =ipalletRepository.GetModelbyTypeAndPartNoAndFamily(ID, spno, family2);
                if (pakCTLInfoLst==null || pakCTLInfoLst.Count<=0)
                {
                    FisException fe = new FisException("PAK033", new string[] { });   //此China Label or Taiwan Label 与机型不匹配！
                    fe.stopWF = false;
                    throw fe;
                }

            }
            CurrentSession.AddValue(Session.SessionKeys.PartNo, spno);
	        return base.DoExecute(executionContext);
            */
        }
	}
}
