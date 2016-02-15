/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/ITCNDCheckProduct
 * UI:CI-MES12-SPEC-FA-UI ITCND Check.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC ITCND Check.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-11   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 2)检查机型是否被QC Hold住
* 检查Model.Status=0 提示“此机型被QC Hold”
* 3)检查机器是否被QC Hold住
* 检查Product.State=’H’ 提示“此机器被QC Hold”
* 4）if exists (select * from ProductInfo where a.ProductID=@PrdId InfoType='Exp') and InfoValue<>’’)
* 提示“測試機器，請勿出貨”
* 注：原来逻辑的QASpcList和LocalMaintain（Tp='Exp'）未使用
* 5)if exists(select Model from TSModel  ( nolock )  where Model=@Model and Mark='0')
* select '0','該機型不需要做ICTnD Check.'
* 5）ProductLog 表latest record WC=66, IsPass=0
* 提示“ITCND Fail, 請重新測試”
* 6）if not exist(select Model from TSModel  ( nolock )  where Model=@Model and Mark='1')
* ProductLog 表latest record WC=66, IsPass=1的记录的Udt后存在EPIA或SLAM记录WC in(73,74)记录，则提示“EPIA後,請重新DownLoad Image”
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Activity
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ITCND Check
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
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
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class ITCNDCheckProduct : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public ITCNDCheckProduct()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //TODO 1
            ProductLog testLog = new ProductLog();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (product == null)
            {
                throw new NullReferenceException("Product in session is null");
            }
            Model model = product.ModelObj;
            if (model == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(product.ProId);
                throw new FisException("CHK804", errpara);
            }
            if (model.Status == "0")
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK802", errpara);
            }
            if(product.State == "H")
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK802", errpara);
            }
            string value = (string)product.GetExtendedProperty("Exp");
            if (!String.IsNullOrEmpty(value))
            {
                List<string> errpara = new List<string>();                
                throw new FisException("CHK803", errpara);                         
            }

            IList<TsModelInfo> tsInfo = null;

            TsModelInfo cond = new TsModelInfo();
            cond.model = product.Model;
            cond.mark = "0";
            tsInfo = productRepository.GetTsModelList(cond);
            if(tsInfo != null && tsInfo.Count != 0)
            {
                List<string> errpara = new List<string>();                
                throw new FisException("CHK805", errpara);           
            }
            //
            
            IList<ProductLog> allLogs = new List<ProductLog>();


            cond.model = product.Model;
            cond.mark = "1";
            tsInfo = productRepository.GetTsModelList(cond);
            if(tsInfo == null || tsInfo.Count == 0)
            {
                allLogs = product.ProductLogs;
                IList<ProductLog> logs = new List<ProductLog>();
                ProductLog maxLog = new ProductLog();
                bool bExist = false;
                if(allLogs != null && allLogs.Count != 0)
                {
                    foreach(ProductLog temp in allLogs)
                    {
                        if(temp.Station == "66" && temp.Status == StationStatus.Pass)
                        {
                            bExist = true;
                            if(temp.Cdt.CompareTo(maxLog.Cdt) > 0)
                            {
                                maxLog = temp;
                            }
                        }
                        //else if(temp.Station == "73" || temp.Station == "74")
                        else if(temp.Station == "6A")
                        {
                            logs.Add(temp);
                        }
                    }
                    if (bExist == true)
                    {
                        foreach (ProductLog temp in logs)
                        {
                            if (temp.Cdt.CompareTo(maxLog.Cdt) > 0)
                            {
                                List<string> errpara = new List<string>();
                                throw new FisException("CHK807", errpara);
                            }
                        }
                    }
                }                
            }

            //UC 7
            allLogs = product.ProductLogs;
            if (allLogs != null && allLogs.Count > 0)
            {
                ProductLog maxLog = new ProductLog();
                foreach (ProductLog temp in allLogs)
                {
                    if (temp.Cdt.CompareTo(maxLog.Cdt) > 0)
                    {
                        maxLog = temp;
                    }
                }
                if (maxLog.Station == "66" && maxLog.Status == StationStatus.Fail)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK806", errpara);
                }

                if (!(maxLog.Station == "66" && maxLog.Status == StationStatus.Pass))
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK845", errpara);
                }
            }

            //UC 8
            //ITC-1360-1787
            string[] code = new string[2];
            code[0] = product.Model;
            code[1] = product.CUSTSN;
            bool isHold = false;
            isHold = productRepository.CheckExistItcndCheckQcHold("1", code);
            if (isHold == true)
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK847", errpara);
            }

            //UC 9
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IList<MoBOMInfo> mbinfo = new List<MoBOMInfo>();
            mbinfo = bomRepository.GetPnListByModelAndBomNodeTypeAndDescr(product.Model, "P1", "ECOA");
            if (mbinfo != null && mbinfo.Count > 0)
            {
                DateTime win8Time;
                DateTime logTime;
                //('P/N','KEY','HASH')
                string[] infotypes = new string[3];
                infotypes[0] = "P/N";
                infotypes[1] = "KEY";
                infotypes[2] = "HASH";
                win8Time = productRepository.GetNewestCdtFromProductInfo(product.ProId, infotypes);
                logTime = productRepository.GetNewestCdtFromProductLog(product.ProId, "66", 1);

                if (win8Time == DateTime.MinValue || logTime == DateTime.MinValue)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK900", errpara);
                }

                if (DateTime.Compare(win8Time, logTime.AddMinutes(1)) > 0 ||
                    DateTime.Compare(logTime, win8Time.AddMinutes(1)) > 0)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK901", errpara);
                }




                //UC10
                //最新的P/N：@ImgPN 
                //（ProductInfo.InfoValue Condtion: Uppder(ProductInfo.InfoType) = ‘P/N’ and ProductID=[ProductID] order by Udt desc）
                IList<string> infoTypes = new List<string>();
                infoTypes.Add("P/N");
                IList<IMES.FisObject.FA.Product.ProductInfo> proInfos = new List<IMES.FisObject.FA.Product.ProductInfo>();
                proInfos = productRepository.GetProductInfoListUpperCaseItemType(product.ProId, infoTypes);

                //获取BOM的ECOA的PN：@BOMPN （select b.PartNo from ModelBOM a (nolock), Part b (nolock) 
                //where a.Material = @Model
                //and a.Component = b.PartNo
                //and b.BomNodeType = 'P1'
                //and b.Descr LIKE 'ECOA%'）

                //若@ImgPN与@BOMPN不相等，则报错：“ImageDownload 失败，ECOA PN 错误
                bool bCompare = false;
                if (proInfos != null && proInfos.Count > 0)
                {
                    foreach (MoBOMInfo temp in mbinfo)
                    {
                        if (temp.component == proInfos[0].InfoValue)
                        {
                            bCompare = true;
                            break;
                        }
                    }
                }
                if (bCompare == false)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK927", errpara);
                }
            }

            

            return base.DoExecute(executionContext);
        }
    }
}