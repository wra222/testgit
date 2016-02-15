// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-02   Du Xuan (itc98066)          create
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
    /// Wimax Label / WWAN ID Label 的列印条件及方法
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
    public partial class CheckWimaxLabelPrint : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckWimaxLabelPrint()
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
            IList<IBOMNode> bomNodeList = (IList<IBOMNode>)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            String curCode = (String)CurrentSession.GetValue(Session.SessionKeys.MBCode);


            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            List<string> errpara = new List<string>();

            Model curModel = modelRep.Find(curProduct.Model);

            //SELECT @esn='',@imei='',@aiccid='',@siccid='',@pno='',@family='',@meid='',@pcid='',@wwan='',@LabelNO1='',@LabelNO2='',@wimax=''
            //SELECT @ZM_wwan='',@wwanzmode='',@SID=''
            string esn = string.Empty;
            string imei = string.Empty;
            string aiccid = string.Empty;
            string siccid = string.Empty;
            string pno = string.Empty;
            string family = string.Empty;
            string meid = string.Empty;
            string pcid = string.Empty;
            string wwan = string.Empty;
            string LabelNO1 = string.Empty;
            string LabelNO2 = string.Empty;
            string wimax = string.Empty;
            string zm_wwan = string.Empty;
            string wwanzmode = string.Empty;
            string sid = string.Empty;
            string mn = string.Empty;

            //2.取Model (IMES_FA..Product.Model)/ Family (IMES_GetData..Model.Family)并赋值给@pno / @family
            pno = curProduct.Model;
            family = curModel.FamilyName;

            //3.如果ModelBOM 中Model 的直接下阶中存在Part No = '60WIMAX00001' 的Part，则置@wimax='Y'，
            //并取Product 的ESN （IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'ESN'）赋值给变量@esn
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(pno);
            IList<IBOMNode> bomList = curBom.FirstLevelNodes;

            foreach (IBOMNode node in bomList)
            {
                if (node.Part.PN == "60WIMAX00001")
                {
                    wimax = "Y";
                    break;
                }
            }
            

            //4.如果ModelBOM 中Model 的直接下阶中不存在Part No = '60WIMAX00001' 的Part，
            //则取Product 的ESN （IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'ESN'）的后8位赋值给变量@esn

            esn = (string)curProduct.GetExtendedProperty("ESN");
            if (wimax != "Y")
            {
                if (!string.IsNullOrEmpty(esn))
                {
                    if (esn.Length >= 8)
                    {
                        esn = esn.Substring(esn.Length - 8, 8);
                    }
                }
                else
                {
                    esn = "";
                }
            }

            //5.取Product 的IMEI（IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'IMEI'） / MEID (（IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'MEID'）) / AICCID （IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'AICCID'）/ SICCID （IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'SICCID'）/ PCID （IMES_FA..ProductInfo.InfoValue，Condition: InfoType = 'PCID'）
            //分别赋值给变量 @imei / @meid / @aiccid / @siccid / @pcid
            imei = (string)curProduct.GetExtendedProperty("IMEI");
            meid = (string)curProduct.GetExtendedProperty("MEID");
            aiccid = (string)curProduct.GetExtendedProperty("AICCID");
            siccid = (string)curProduct.GetExtendedProperty("SICCID");
            pcid = (string)curProduct.GetExtendedProperty("PCID");

            //6.取Model 的MN1 （IMES_GetData..ModelInfo.Value，Condition: Name = 'MN1'）/ SID （IMES_GetData..ModelInfo.Value，Condition: Name = 'SID'）属性
            //赋值给变量@MN, @SID
            mn = curModel.GetAttribute("MN1");
            sid = curModel.GetAttribute("SID");

            //7.	Insert #wwan
            //将ModelBOM 中Model 直接下阶的所有Descr like 'WWAN%' 的Part 的Part No 插入到#wwan
            //Remark：
            //#wwan 定义如下
            //CREATE TABLE #wwan (SPno char(20) )

            IList <string> wwanList = new List<string>();
             foreach (IBOMNode node in bomList)
            {
                if (!string.IsNullOrEmpty(node.Part.Descr) && node.Part.Descr.Length >= 4)
                {
                    if (node.Part.Descr.Substring(0,4)=="WWAN")
                    {
                        wwanList.Add(node.Part.PN);
                    }
                }
                //8.将ModelBOM 中Model 直接下阶中BomNodeType 为'PL'，
                //Descr='WWANID LABEL 1' 的Part 的Part No 赋值给@LabelNO1
                if (node.Part.BOMNodeType == "PL" && node.Part.Descr == "WWANID LABEL 1" )
                {
                    LabelNO1 = node.Part.PN;
                }
                //9.将ModelBOM 中Model 直接下阶中BomNodeType 为'PL'，Descr='WWANID LABEL 2' 
                //的Part 的Part No 赋值给@LabelNO2
                if (node.Part.BOMNodeType == "PL" && node.Part.Descr == "WWANID LABEL 2" )
                {
                    LabelNO2 = node.Part.PN;
                }
            }
            //10.如果#wwan 无记录并且@wimax=''，则表明无需列印WWAN ID Label 或者Wimax Label，退出
            if (wwanList.Count == 0 && string.IsNullOrEmpty(wimax))
            {
                CurrentSession.AddValue("WLabel", "");
                return base.DoExecute(executionContext);
            }

            //11.	按照如下方法，赋值变量@wwan / @ZM_wwan
            //参考方法：
            //IF EXISTS (SELECT * FROM #wwan WHERE SPno like 'BM2WL%')  
            //  BEGIN
	        //  SELECT @wwan=RTRIM(SPno) FROM #wwan WHERE SPno like 'BM2WL%'
	        //  SELECT @ZM_wwan=@wwan
            //  END
            //ELSE
            //  BEGIN
	        //  SELECT @wwan=RTRIM(SPno) FROM #wwan
            //END
            if (wwanList.Count > 0)
            {
                wwan = wwanList[0];
                foreach (string str in wwanList)
                {
                    if (str.Substring(0, 5) == "BM2WL")
                    {
                        wwan = str.TrimEnd();
                        zm_wwan = wwan;
                        break;
                    }
                }
            }
            //12.如果@wwan<>'' AND @wwan IS NOT NULL，
            //则取ModelBOM 中@wwan 直接下阶中Part No 为6 位的Part 中的任意一个的AS 属性
            //（IMES_GetData..PartInfo.InfoValue，Condition: InfoType = 'AS'）赋值给@wwanzmode
            if (!string.IsNullOrEmpty(wwan))
            {
                IHierarchicalBOM tmpBom = bomRep.GetHierarchicalBOMByModel(wwan);
                IList<IBOMNode> tmpBomList = tmpBom.FirstLevelNodes;

                foreach(IBOMNode node in tmpBomList)
                {
                    if (node.Part.PN.Length ==6)
                    {
                        wwanzmode =node.Part.GetAttribute("AS");
                        break;
                    }
                }
            }

            //检查列印WWAN Label 需要的Image D/L 数据是否上传
            //===========================================================================
            //DECLARE @wwanKP varchar(100)
            //DECLARE @wwanKPAS varchar(10)
            //DECLARE @LabelMEID varchar(10)
            //DECLARE @LabelIMEI varchar(10)
            //DECLARE @LabelICCID varchar(10)
            //DECLARE @LabelESN varchar(10)
            string wwanKP = "";
            string wwanKPAS = "";
            //string labelMEID = "";
            //string labelIMEI = "";
            //string labelICCID = "";
            //string labelESN = "";
            //IF @wwan<>''
            //BEGIN
            if (!string.IsNullOrEmpty(wwan))
            {
                //SELECT @wwanKP = PartNo FROM Product_Part NOLOCK WHERE ProductID = @pid AND PartType = 'WWAN'	
                IList<ProductPart> list = new List<ProductPart>();
                ProductPart wcond = new ProductPart();
                wcond.ProductID = curProduct.ProId;
                wcond.PartType = "WWAN";
                list = productRep.GetProductPartList(wcond);
                wwanKP = list[0].PartID;
                //SELECT @wwanKPAS = InfoValue FROM PartInfo NOLOCK WHERE PartNo = @wwanKP AND InfoType = 'AS'
                //SELECT @wwanKPAS = LEFT(@wwanKPAS, CHARINDEX('-', @wwanKPAS)-1)
                wwanKPAS = partRep.GetPartInfoValue(wwanKP, "AS");
                string[] splitpattern = wwanKPAS.Split('-');
                if (splitpattern.Length > 0)
                {

                    wwanKPAS = splitpattern[0];
                }
                else
                {
                    wwanKPAS = "";
                }

                //IF NOT EXISTS(SELECT * FROM HP_WWANLabel NOLOCK 
                //WHERE LEFT(ModuleNo, CHARINDEX('-', ModuleNo)-1) = @wwanKPAS)
                //      SELECT '0','Please maintain HP_WWANLabel!'
                //      RETURN
                //ELSE
                //      SELECT @LabelESN = LabelESn, @LabelICCID= LabelICCID, @LabelMEID = LabelMEID, @LabelIMEI = LabelIMEI
                //      FROM HP_WWANLabel NOLOCK 	WHERE LEFT(ModuleNo, CHARINDEX('-', ModuleNo)-1) = @wwanKPAS
                IList<HpWwanlabelInfo> hpList = bomRep.GetHpWwanlabelInfoByModuleNoPrefix(wwanKPAS);
                if (hpList.Count <= 0)
                {
                    errpara.Add(this.Key);//Please maintain HP_WWANLabel!
                    throw new FisException("PAK152", errpara);
                }
                HpWwanlabelInfo hpInfo = hpList[0];

                //IF @LabelESN = 'Y'
                //      IF @esn = ''
                //          SELECT '0', 'ESN missing!'
                //          RETURN
                if (hpInfo.labelESn == "Y")
                {
                    if (string.IsNullOrEmpty(esn))
                    {
                        errpara.Add(this.Key);//ESN missing!
                        throw new FisException("PAK153", errpara);
                    }
                }
                //IF @LabelIMEI = 'Y'
                //      IF @imei = ''
                //              SELECT '0', 'IMEI missing!'
                //              RETURN
                if (hpInfo.labelIMEI == "Y")
                {
                    if (string.IsNullOrEmpty(imei))
                    {
                        errpara.Add(this.Key);//IMEI missing!
                        throw new FisException("PAK154", errpara);
                    }
                }
                //IF @LabelMEID = 'Y'
                //      IF @meid = ''
                //              SELECT '0', 'MEID missing!'
                //
                if (hpInfo.labelMEID == "Y")
                {
                    if (string.IsNullOrEmpty(meid))
                    {
                        errpara.Add(this.Key);//MEID missing!
                        throw new FisException("PAK155", errpara);
                    }
                }
                //IF @LabelICCID = 'Y'
                //      IF @siccid = ''
                //              SELECT '0', 'SICCID missing!'
                //              RETURN
                if (hpInfo.labelICCID == "Y")
                {
                    if (string.IsNullOrEmpty(siccid))
                    {
                        errpara.Add(this.Key);//SICCID missing!
                        throw new FisException("PAK156", errpara);
                    }
                }

            }
            //===========================================================================

            //14.如果(@family='DIABLO 2.1' or @family='DIABLO 2.0' or @family = 'HARBOUR 1.1')AND @pcid=''，
            //则报告错误：“PCID missing!”后，退出
            if ((family == "DIABLO 2.1" || family =="DIABLO 2.0" || family =="HARBOUR 1.1")&& (pcid == ""))
            {
                FisException fe = new FisException("PAK108", new string[] { });   //PCID missing!
                throw fe;
            }

            //15.如果@wimax<>''，则需要列印的是Wimax Label，
            //此时要求弹出对话框提示用户：“Please print Wimax Label!”后，再列印Wimax Label；
            //否则，列印的是WWAN ID Label，无需提示
            //Wimax Label 和WWAN ID Label使用同一个Batch File 列印

            if (wimax != "")
            {
                CurrentSession.AddValue("WLabel", "Wimax Label");
            }
            else
            {
                CurrentSession.AddValue("WLabel", "WWAN ID Label");
            }

	        return base.DoExecute(executionContext);
        }
	}
}
