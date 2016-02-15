/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PrintShiptoCartonLabelImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-03-29   208014            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
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
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.CartonSSCC;


namespace IMES.Activity
{
    /// <summary>
    /// 保存产生的 Carton SSCC
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         071PrintShiptoCartonLabel
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         调用CartonSSCC sq
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.CartonSN
    ///        
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
    ///        
    /// </para> 
    /// </remarks>

    public partial class GenerateCartonSSCC : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateCartonSSCC()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 保存产生的 Carton SSCC
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            string currentCartonSN =(string)CurrentSession.GetValue(Session.SessionKeys.Carton);
            currentCartonSN = currentCartonSN.Trim();
                        
            ////-------------------------  SSCC 的編碼規則 ------------------------------
            
            //DECLARE @cn varchar(20)                   --Carton No 
            //DECLARE @GS1 char(7),						--GS1，現在固定為’‘
            //@CartonSN char(9),						--Carton號的右側位
            //@data char(16),							--由GS1和CartonSN組成
            //@data1 bigint,							--@data轉化為int型之後的數
            //@sum int,									--@data1按位數和一定規則的和
            //@digit int,								--@data1的各个位置的数值
            //@i char(1),								--控制变量，决定乘数是还是
            //@multiplier int,							--乘数（或）
            //@checkdigit1 int,							--校验码，SSCC的最后一位
            //@checkdigit char(1),						--校验码的char型
            //@SSCC char(20)							--最终的SSCC号

            string GS1="1928686";
            string CartonSN = currentCartonSN.Substring(currentCartonSN.Length-9, 9);

            string data = GS1 + CartonSN;

            Int64 data1 = Int64.Parse(data);
            string i = "1";
            int sum = 0;
            int multiplier =0;
            string checkdigit = null;

            while ((data1 / 10) > 0)
            {
                if (i == "1")
                {
                    multiplier = 3;
                    i = "0";
                }
                else if (i == "0")
                {
                    multiplier = 1;
                    i = "1";
                }

                Int64 digit = data1 % 10;
                sum =(int)(sum + digit * multiplier);
                data1 = data1 / 10;
             }

            sum =(int)(sum + data1 * 1);
            int checkdigit1 =(int)(10 - sum % 10);
            if (checkdigit1 == 10)
            {
                checkdigit = "0";
            }
            else
            {
                checkdigit=Convert.ToString(checkdigit1);
            }
            string SSCC = "0" + data + checkdigit;

            ////-------------------------  SSCC 的編碼規則 ------------------------------

            CartonSSCC curerntCartonSSCC = new CartonSSCC();
            curerntCartonSSCC.CartonSN = currentCartonSN;
            curerntCartonSSCC.SSCC = SSCC;
            curerntCartonSSCC.Editor = this.Editor;
            curerntCartonSSCC.Cdt = DateTime.Now;


            ICartonSSCCRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<ICartonSSCCRepository, CartonSSCC>();
           
            currentRepository.Add(curerntCartonSSCC, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);

        }

        
	}
}
