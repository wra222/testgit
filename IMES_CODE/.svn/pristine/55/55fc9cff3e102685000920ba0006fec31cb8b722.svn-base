/*
 * INVENTEC corporation: 2011 all rights reserved. 
 * Description:检查MB是不是连板的子板，且已经做了先切后测（MBCode.Type=1）的设置
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-13  Chen Xu               Create
 * Known issues:
 * TODO：
 * UC 具体业务：declare @MBSno varchar(10),@tp varchar(10)
                if CHARINDEX(SUBSTRING(@MBSno,6,1),'0123456789')= 0
                begin
                    select @tp = Type from IMES2012_GetData..MBCode nolock where MBCode = LEFT(@MBSno,2)
                    if @tp ='0'  select '该MB需先去做ICT测试，刷PCA ICT Input'
                end
 * UC Version: 6715: PAK078: 该MB需先去做ICT测试，刷ICT Input
 */

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
using IMES.FisObject.PCA.MBModel;

namespace IMES.Activity
{
    /// <summary>
    /// 检查Model是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于SA： MB SPlit
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
    ///         Model 
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
    ///         IModelRepository
    ///         IModel
    /// </para> 
    /// </remarks>
    public partial class CheckMBSnoSplit : BaseActivity
    {
        /// <summary>
        /// CheckMBSnoSplit
        /// </summary>
        public CheckMBSnoSplit()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 检查MB是不是连板的子板，且已经做了先切后测（MBCode.Type=1）的设置
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mbsno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN).ToString();
            IMBRepository imbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, false);
              
            string number = "0123456789";
            string bit = mbsno.Substring(5, 1);
            if (mbsno.Substring(5, 1) == "M")
            {
                bit = mbsno.Substring(6, 1);
            }

            if (!number.Contains(bit))  //MBSno第6位不是数字
            {
                //MBCodeDef mbcode = imbRepository.GetMBCode(mbsno.Substring(0, 2));
                MBCodeDef mbcode = new MBCodeDef();
                if (mbsno.Substring(5, 1) == "M")
                    mbcode = imbRepository.GetMBCode(mbsno.Substring(0, 3));
                else 
                    mbcode = imbRepository.GetMBCode(mbsno.Substring(0, 2));

                if (mbcode == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(mbsno);
                    FisException ex = new FisException("PAK080", errpara);  //没有找到MBSno %1 对应的PCB！
                    throw ex;
                }
                string type = mbcode.Type;        //IMES2012_GetData..MBCode 

                if (type == "1") //先切后测
                {
                    CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
                }
                else if (type == "0")
                {
                    FisException fe = new FisException("PAK078", new string[] { });  //该MB需先去做ICT测试，刷ICT Input！
                    throw fe;
                }
                else
                {
                    FisException fe = new FisException("PAK081", new string[] { });  //MBCode数据维护不全！
                    throw fe;
                }

                CurrentSession.AddValue(Session.SessionKeys.MultiQty, mbcode.Qty);
                if (mbcode.Qty <= 1)
                {
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "MB");
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, mbsno);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, mbsno);
                    string model = CurrentSession.GetValue(Session.SessionKeys.PCBModelID) as string;
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, this.Line+" " + model);
                    IList<string> newMBSnoList = new List<string>();
                    newMBSnoList.Add(mbsno);
                    CurrentSession.AddValue(Session.SessionKeys.MBSNOList, newMBSnoList);
                }
            }
            else
            {
                List<string> errpara = new List<string>();
                errpara.Add(mbsno);
                FisException ex = new FisException("CHK161", errpara);  //MB号:%1错误！
                throw ex;
            }




            return base.DoExecute(executionContext);
        }

    }
}
