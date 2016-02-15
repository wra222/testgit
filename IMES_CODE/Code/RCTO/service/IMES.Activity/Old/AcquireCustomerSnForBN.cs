/// 2010-01-07 Liu Dong(eB1-4)         Modify ITC-1103-0048

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
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//using IMES.FisObject.Common.Station;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.Common.NumControl;
using System.Transactions;
using IMES.Infrastructure.UnitOfWork;
//using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PCA.MB;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using System.Globalization;
namespace IMES.Activity
{
    /// <summary>
    /// 产生指定机型的SMT MO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseAcquireActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于001GenSMTMO.xoml
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取MO最大值;
    ///         2.产生指定机型的MO;
    ///         3.更新MO最大值;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.IsExperiment
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SMTMONO
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新NumControl
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBMORepository
    /// </para> 
    /// </remarks>
    public partial class AcquireCustomerSnForBN : BaseAcquireActivity
	{
        private static object _syncRoot_GetSeq = new object();

        public AcquireCustomerSnForBN()
		{
			InitializeComponent();
         
		}

        /// <summary>
        ///  Session 类型
        /// </summary>
        public static DependencyProperty CheckRangeProperty = DependencyProperty.Register("CheckRange", typeof(bool), typeof(AcquireCustomerSnForBN));

        /// <summary>
        /// Session 键值, MBSn/ProductID/...
        /// </summary>
        [DescriptionAttribute("CheckRange")]
        [CategoryAttribute("CheckRange Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool CheckRange
        {
            get
            {
                return ((bool)(base.GetValue(CheckRangeProperty)));
            }
            set
            {
                base.SetValue(CheckRangeProperty, value);
            }
        }
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            return base.Execute(executionContext);
        }
        private string GetCfgcodeByProduct()
        {
     
             var prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
             string s=prod.Model;
            IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

            Model _MInfo = myRepository.Find(s);
            return _MInfo.GetAttribute("CfgCode");


             
            

        }
        public void GetModelObjectByProduct()
        {
            var prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string s = prod.Model;
            IModelRepository myRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model _Mdoel = myRepository.Find(s);
            CurrentSession.AddValue(Session.SessionKeys.ModelObj, _Mdoel);
            CurrentSession.AddValue(Session.SessionKeys.ModelName, _Mdoel.ModelName);
        }
        private string GetNumString()
        {
            IConverter[] cvts = null;
            SNComposer resSnC = new SNComposer();

            cvts = new IConverter[]
                     { 
                      new ProductionSiteConverter(2,'0',ExtendSession.SessionKeys.PlantCode), 
                      new YearConverterNormal(1,'0',Session.SessionKeys.presentTime),
                      new WeekConverter(2,'0',Session.SessionKeys.presentTime,DayOfWeek.Monday),
                      new DayOfWeekConverter(Session.SessionKeys.presentTime),
                     };
            foreach (IConverter cvt in cvts)
            {
              resSnC.Add(new NumberElement(cvt, CurrentSession));
            }
            string r=resSnC.Calculate();
            return r;
        }
        private string CheckNumControlDB(string model)
        {  string result = "";
            string term = model + "-"+ GetNumString() +"%";

            string strSQL = @" select Value from dbo.NumControl where Value like @term and NoType=@type ";
             
            SqlParameter paraName = new SqlParameter("@term", SqlDbType.VarChar, 32);
           SqlParameter paraName2 = new SqlParameter("@type", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = term;
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = GetClass();
            object oEx = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData,
                                      System.Data.CommandType.Text,
                                      strSQL, paraName,paraName2);
            if (oEx != null)
            {
                string[] strTempNo = null;
                strTempNo = oEx.ToString().Split('-');
                string strNo = strTempNo[1].Substring(6, 6);
                return strNo;
                
            }
           return result;

        }
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

       
            if (CurrentSession.GetValue(Session.SessionKeys.ModelObj) == null)
            { GetModelObjectByProduct(); }
            Model modelObj = null;
        
            modelObj = (Model)CurrentSession.GetValue(Session.SessionKeys.ModelObj);
            bool IsPilotRun = false;
            string strIsTestBuild = modelObj.GetAttribute("IsTestBuild");
            if (!string.IsNullOrEmpty(strIsTestBuild) && strIsTestBuild.Trim() == "Y")
            {
                IsPilotRun = true;
            }
            
            string stStartCustSnNo ="000001";
            string strEndCustSnNo = "999999";

            string strCfg = modelObj.GetAttribute("CfgCode");
            if (string.IsNullOrEmpty(strCfg))
            {
                List<string> param = new List<string>();
                param.Add(modelObj.ModelName);
                throw new FisException("GEN044", param); 

            } 
           //  if (CheckRange) //SN需要 by  Model 定義Range -- marked at 2011/0909
           if(IsPilotRun) // Midify at 2011/09/09
            {
                 stStartCustSnNo = modelObj.GetAttribute("StartUnitID");
                 strEndCustSnNo = modelObj.GetAttribute("EndUnitID");
                 if (string.IsNullOrEmpty(stStartCustSnNo) | string.IsNullOrEmpty(strEndCustSnNo))
                 {
                     List<string> param = new List<string>();
                     param.Add(modelObj.ModelName);
                     throw new FisException("GEN045", param);
                 }
            }

            CurrentSession.AddValue(ExtendSession.SessionKeys.PlantCode, "20");
            CurrentSession.AddValue(ExtendSession.SessionKeys.CfgCode, strCfg);
            CurrentSession.AddValue(Session.SessionKeys.presentTime, DateTime.Now);
        

            IConverter[] cvts = null;
            SNComposer resSnC = new SNComposer();
            
             
         
            if (IsPilotRun)
            {
                CurrentSession.AddValue(ExtendSession.SessionKeys.TestBuild, "TestBuild");
                cvts = new IConverter[]
                     { 
                     new CommonConverter(9,'0',ExtendSession.SessionKeys.TestBuild), 
                     new SequenceConverterNormal("0123456789",6,strEndCustSnNo,stStartCustSnNo,'0') ,
                   //   new ProductionSiteConverter  (3,'0',ExtendSession.SessionKeys.CfgCode)
                   };
           }
            else
            {
                 cvts = new IConverter[]
                     { 
                      new ProductionSiteConverter(2,'0',ExtendSession.SessionKeys.PlantCode), 
                      new YearConverterNormal(1,'0',Session.SessionKeys.presentTime),
                      new WeekConverter(2,'0',Session.SessionKeys.presentTime,DayOfWeek.Monday),
                      new DayOfWeekConverter(Session.SessionKeys.presentTime),
                      new SequenceConverterNormal("0123456789",6,strEndCustSnNo,stStartCustSnNo,'0') ,
                      new ProductionSiteConverter  (3,'0',ExtendSession.SessionKeys.CfgCode)
                   };
            
            }
          
       
        
            ISequenceConverter seqCvt = null;

         
            foreach (IConverter cvt in cvts)
            { 
                if (cvt is ISequenceConverter)
                {
                    resSnC.Add(new NumberElement(cvt, "{0}"));
                    seqCvt = (ISequenceConverter)cvt;
                }
                else
                    resSnC.Add(new NumberElement(cvt, CurrentSession));
            }
            
            string preSeqStr = "";
            if (IsPilotRun) // -- Modify this area at 2011/09/09
            {
                preSeqStr = strCfg + "-" + resSnC.Calculate(); //save "200-xxxxxxxxxxxxx"  in NumControl 

            }
            else
            {
               preSeqStr=  resSnC.Calculate();
            }
           
            //if (CheckRange) -- Marked this area at 2011/09/09
            //{
            //    preSeqStr=modelObj.ModelName + "-" + resSnC.Calculate();
           
            //}
            //else
            //{
            //   preSeqStr=  resSnC.Calculate();
            
            //}
      

            SequencialNumberRanges snrs = getSequence(CurrentSession, preSeqStr, 1, false, seqCvt);
            string[] strTempNo = null;
            string strCUSTSN = null;
            string strTemp = "";
            string strPrifix = CurrentSession.GetValue(ExtendSession.SessionKeys.PlantCode) + DateTime.Now.Year.ToString().Substring(3, 1)
                                     + GetWeek() + GetDayOfWeek();
           //******************* Add this area at 2011/0909 ***********************
            if (IsPilotRun)
            {
                strTempNo = snrs.Ranges[0].NumberBegin.Split('-');
                strTemp = strTempNo[1].Replace("TestBuild", strPrifix);
                strTemp = strTemp + CurrentSession.GetValue(ExtendSession.SessionKeys.CfgCode);
                strCUSTSN = strTemp + CalculateCheckSum(strTemp);

            }
            else
            {
                strCUSTSN = snrs.Ranges[0].NumberBegin + CalculateCheckSum(snrs.Ranges[0].NumberBegin);
            
            }
            //******************* Add this area at 2011/0909 ***********************
            
            
            
           /*********** Marked this Area at 2011/09/09
            if (CheckRange)
            {
                 strTempNo = snrs.Ranges[0].NumberBegin.Split('-');
                if (IsPilotRun)
                {
                    strTemp = strTempNo[1].Replace("TestBuild", strPrifix);
                    strTemp = strTemp + CurrentSession.GetValue(ExtendSession.SessionKeys.CfgCode);
                     strCUSTSN = strTemp + CalculateCheckSum(strTemp); 
                }
                else
                {
                    
                    strCUSTSN = strTempNo[1] + CalculateCheckSum(strTempNo[1]); //strTempNo[0]: Model Name ; strTempNo[0] : SN
                }

               
            }
            else
            {
                if (IsPilotRun)
                {

                      strTemp = snrs.Ranges[0].NumberBegin.Replace("TestBuild", strPrifix);
                      strTemp = strTemp + CurrentSession.GetValue(ExtendSession.SessionKeys.CfgCode);
                      strCUSTSN = strTemp + CalculateCheckSum(strTemp);
                }
                else
                {
                         strCUSTSN= snrs.Ranges[0].NumberBegin + CalculateCheckSum(snrs.Ranges[0].NumberBegin);
                 }
          
            } */ //*******************************Marked this Area at 2011/09/09
            if (CheckIsExistingSN(strCUSTSN)>0)
            {
                List<string> param = new List<string>();
                param.Add(strCUSTSN);
                throw new FisException("GEN046", param);
            }
            CurrentSession.AddValue(Session.SessionKeys.CustSN, strCUSTSN);
            return base.DoExecute(executionContext);//default(ActivityExecutionStatus);
         }

        private int CheckIsExistingSN(string sn)
        {
         //   string strSQL = @"declare @i int,@j int  select @i= COUNT(*) from PCB where  CUSTSN=@sn ;
         //                                select  @j=COUNT(*) from Product where CUSTSN=@sn ; select @i+@j ";

            string strSQL = @" if exists(select ProductID nolock from Product nolock where  CUSTSN=@sn)  
                                           or  exists(select PCBNo nolock from PCB where CUSTSN=@sn) select  1 else  select 0";
            
            SqlParameter paraName = new SqlParameter("@sn", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = sn;
            Object obj= SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraName);
            return int.Parse(obj.ToString());
        }
        private string CalculateCheckSum(string custSN)
        {
            char[] chs = custSN.ToCharArray();
            int intCheckSum = 0;
            for (int i = 0; i < chs.Length; i++)
            {
                intCheckSum = intCheckSum + int.Parse(chs[i].ToString());
            }
            return intCheckSum.ToString().Substring(intCheckSum.ToString().Length - 1, 1);
         }
        private string GetWeek()
        {
            string result = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, 
                                         DayOfWeek.Monday).ToString();
            return result;
         }

        private string GetDayOfWeek()
        {
            string strDay = "";
            DateTime dt = DateTime.Now;
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    strDay = "1";
                    break;
                case DayOfWeek.Tuesday:
                    strDay = "2";
                    break;
                case DayOfWeek.Wednesday:
                    strDay = "3";
                    break;
                case DayOfWeek.Thursday:
                    strDay = "4";
                    break;
                case DayOfWeek.Friday:
                    strDay = "5";
                    break;
                case DayOfWeek.Saturday:
                    strDay = "6";
                    break;
                case DayOfWeek.Sunday:
                    strDay = "7";
                    break;
            }

            return strDay;
        
        }
    

        protected internal override System.Workflow.ComponentModel.Activity GetInheritedClassInst()
        {
            return this;
        }
        
        protected override string GetClass()
        { 
           return   ExtendGeneratesConstants.CustomerSNForBN;
        }
      
        protected override SequencialNumberRanges getSequence(Session sess, string preSeqStr, int quantity, bool seqRestart, ISequenceConverter seqCvt)
        {
            SequencialNumberRanges ret = new SequencialNumberRanges();

            //IMBMORepository mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
          
            string type = GetClass();
            //string subruleName = GetClass();

            lock (_syncRoot_GetSeq)
            {
              
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    try
                    {
                        SqlConnectionManager.Begin();

                        bool insOrUpd = true;
                        //string maxMo = mbmoRepository.GetMaxMO(preSeqStr);//((bool)Session.GetValue(Session.SessionKeys.IsExperiment));
                        string maxMo = numCtrlRepository.GetMaxNumber(GeneratesConstants.MappingToStandard(type), preSeqStr);//, this.Customer);//2011-09-27 不考虑Customer
                    
                        string seq = string.Empty;
                        SequencialNumberSegment cnbs = new SequencialNumberSegment();
                        if (string.IsNullOrEmpty(maxMo))
                        {
                            seq = seqCvt.NumberRule.MinNumber;
                            cnbs.NumberBegin = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, 0)));
                            cnbs.NumberEnd = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, quantity - 1)));
                            insOrUpd = true;
                       //     NumControl a = new NumControl(0,,,,,,);
                        //    numCtrlRepository.Add(a,CurrentSession.UnitOfWork); 
                        }
                        else
                        {
                            seq = this.CutOutSeq(preSeqStr, maxMo);
                            cnbs.NumberBegin = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, 1)));
                            cnbs.NumberEnd = seqCvt.Convert(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(seq, quantity)));
                            insOrUpd = false;
                            //get data
                            //numCtrlRepository.Find(
                        }

                        //mbmoRepository.SetMaxMO((bool)CurrentSession.GetValue(Session.SessionKeys.IsExperiment), GetAMBMO(sess, cnbs.NumberEnd, this.Editor, quantity));
                        //Model _MInfo = (Model)CurrentSession.GetValue(Session.SessionKeys.ModelObj);
                       // numCtrlRepository.SaveMaxNumber(GetANumControl(GeneratesConstants.MappingToStandard(type), _MInfo.ModelName /*subruleName*/, cnbs.NumberEnd, this.Customer), insOrUpd, preSeqStr);

                        numCtrlRepository.SaveMaxNumberWithOutByCustomer(GetANumControl(GeneratesConstants.MappingToStandard(type), string.Empty/*subruleName*/, cnbs.NumberEnd, this.Customer), insOrUpd, preSeqStr);//2011-09-27 不考虑Customer
                        ret.Ranges = new SequencialNumberSegment[] { (SequencialNumberSegment)cnbs };
                         
                        // Check Customer SN Range By Model

                        //Model modelObj = (Model)CurrentSession.GetValue(Session.SessionKeys.ModelObj);
                        //string strSeqNo = cnbs.NumberEnd.Substring(6, 6);
                        //string strBeginCustSnNo = modelObj.GetAttribute("StartUnitID’ ");
                        //string strEndCustSnNo = modelObj.GetAttribute("EndUnitID");
                        //if (strBeginCustSnNo == null | strEndCustSnNo == null)
                        //{
                         
                        //    throw new Exception(" The Customer SN range by MODEL not define!");
                        //}
                        //if (int.Parse(strSeqNo) < int.Parse(strBeginCustSnNo) | int.Parse(strSeqNo) > int.Parse(strEndCustSnNo))
                        //{
                        //    throw new Exception(" The Customer SN out of range of this MODEL!");
                        
                        //}
                        // Check Customer SN Range By Model

                        SqlConnectionManager.Commit();
                    }
                    catch (Exception)
                    {
                        SqlConnectionManager.Rollback();
                        throw;
                    }
                    finally
                    {
                        SqlConnectionManager.Dispose();
                        SqlConnectionManager.End();
                        scope.Complete();
                    }
             
            }
            }
            return ret;
        }

        //private IMBMO GetAMBMO(Session sess, string maxNo, string editor,int qty)
        //{
        //    IMBMO ret = new MBMO();
        //    //ret.Cdt=
        //    ret.Editor= editor;
        //    ret.Family = (string)CurrentSession.GetValue(Session.SessionKeys.FamilyName);
        //    //ret.GKey=
        //    //ret.IsDirty=
        //    //ret.Key=
        //    ret.Model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
        //    ret.MONo = maxNo;
        //    ret.PrintedQty = 0;
        //    ret.Qty = qty;
        //    ret.Remark = string.Empty;
        //    ret.Status = "1";
        //    //ret.Udt=
        //    return ret;
        //}
	}
}
