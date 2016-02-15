/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-21   Du.Xuan               Create   
* ITC-1414-0069  重复累加
* Known issues:
* TODO：
* 
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.PartSn;
using IMES.FisObject.Common.Part;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 產生CT NO號相关逻辑
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Carton NO
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         更新Product.CUSTSN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
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
    ///         Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateRCTOCTNo : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateRCTOCTNo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 產生Carton NO號相关逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        { 
            string ctno = (string)CurrentSession.GetValue("CTNO");
            int qty = (int)CurrentSession.GetValue("QTY");
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //获取Part.PartNo 和 Part.Descr
            //条件：Part.PartNo = PartInfo.PartNo and PartInfo.InfoValue=Left([CT No],5)
            //限制：Top 1
            //若Part.PartNo不存在，则报错：“错误的CT No”

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            PartInfo cond = new PartInfo();
            cond.PN = ctno.Substring(0, 5);
            IList<PartInfo> partList = partRep .GetPartInfoList(cond);                 
            PartInfo partinfo = partList[0];
            IPart part = partRep.Find(partinfo.PN);
                

            //Generate New CT No
            //方法：原[CT No] + 流水码
            //数量：Qty
            //流水码规则：
            //3位
            //31进制：0123456789BCDFGHJKLMNPQRSTVWXYZ
            //从000开始
            // 若为’ZZZ’，则报错：“流水号已用完”
            //[CT No]为前缀，若[CT No]改变，则流水码从000开始
            try
            {
                string newCtno = "";
                string maxnum = "";
                string prestr = "";
                bool addflag = false;
    
                prestr = ctno;
                // 自己管理事务开始
                //SqlTransactionManager.Begin();
                //IUnitOfWork uof = new UnitOfWork();//使用自己的UnitOfWork

                //从NumControl中获取流水号
                //GetMaxNumber
                INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
                MathSequenceWithCarryNumberRule marc = new MathSequenceWithCarryNumberRule(3, "0123456789BCDFGHJKLMNPQRSTVWXYZ");

                NumControl maxObj = numControl.GetMaxNumberObj("CTNO", prestr + "{0}");
                if (maxObj != null)
                    maxnum = maxObj.Value;

                if (string.IsNullOrEmpty(maxnum))
                {
                    maxnum = "000";
                    addflag = true;
                }
                else
                {

                    string numstr = maxnum.Substring(maxnum.Length - 3);
                    long diff = marc.CalculateDifference(numstr,"ZZZ");
                    if (numstr.ToUpper() == "ZZZ" || diff < qty)
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }

                    numstr = marc.IncreaseToNumber(numstr, 1);
                    maxnum = numstr;
                }

                newCtno = ctno + maxnum.ToUpper();

                NumControl item = null;// new NumControl();
                if (addflag)
                {
                    item = new NumControl();
                    item.NOType = "CTNO";
                    item.Value = newCtno;
                    item.NOName = "";
                    item.Customer = "HP";
                }
                else
                {
                    item = maxObj;
                    item.Value = newCtno;
                }

                int count = 1;
                IPrintLogRepository printRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
                IList<string> ctnoList = new List<string>(); 
                while(count <= qty)
                {
                    ctnoList.Add(item.Value);
                    numControl.SaveMaxNumber(item, addflag);

                    //Insert PartSN
                    //IECSN：产生的18位CT No
                    //IECPn：Step2中获取的Part.PartNo
                    //PartType：Step2中获取的Part.Descr
                    //VendorSN：输入的15码的[CT No]
                    IPartSnRepository partsnRep = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
                    //PartSn(string iecSn, string iecPn, string type, string vendorSn, string vendorDCode, string vCode, string pn151, string editor, string dateCode);
                    PartSn partSn = new PartSn(newCtno, part.PN, part.Descr,ctno,"","","",this.Editor,"");
                    partsnRep.Add(partSn,CurrentSession.UnitOfWork);

                    //Insert PrintLog
                    //Name=’RCTO CT Label’
                    //BegNo=Begin [CT No]
                    //EndNo=End [CT No]
                    //Descr=[Qty]
                    var log = new PrintLog
                    {
                        Name = "RCTO CT Label",
                        BeginNo = newCtno,
                        EndNo = newCtno,
                        Descr = Convert.ToString(qty),
                        Editor = this.Editor
                    };
                    printRep.Add(log, CurrentSession.UnitOfWork);

                    //get next
                    count++;
                    addflag = false;
                    maxnum = marc.IncreaseToNumber(maxnum, 1);
                    newCtno = ctno + maxnum.ToUpper();
                    item = maxObj;
                    item.Value = newCtno;
                }             
                //uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值
                //SqlTransactionManager.Commit();//提交事物，释放行级更新锁 
             
                CurrentSession.AddValue("CTNOList", ctnoList);
                
            }
            catch (Exception)
            {
                //SqlTransactionManager.Rollback();
                throw;
            }
            finally
            {
                //SqlTransactionManager.Dispose();
                //SqlTransactionManager.End();
            }
            
            return base.DoExecute(executionContext);
        }

    }
}
