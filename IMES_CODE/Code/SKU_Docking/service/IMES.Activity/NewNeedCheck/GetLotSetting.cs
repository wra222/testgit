/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: 获取LotSetting表中PassQty
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-24  Kaisheng               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  先用当前Line查找，若不存在记录，用‘ALL’继续查找；若以上信息都不存在，则报错：“请与IE联系，维护 Lot相关设置”
 * 
 */

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Process;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于SA： PCA TestStation For Lot
    /// </para>
    /// <para>
    /// 实现逻辑：先用当前Line查找，若不存在记录，用‘ALL’继续查找；若以上信息都不存在，则报错：“请与IE联系，维护 Lot相关设置”
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
    /// IMBRepository
    /// </para> 
    /// </remarks>
    public partial class GetLotSetting : BaseActivity
	{
        /// <summary>
        /// GetLotSetting
        /// </summary>
        public GetLotSetting()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 获取LotSetting表中PassQty
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //DEBUG  ITC-1414-0095 Kaisheng 2012/06/11
            //------------------------------------------
            if (this.Station != "15")
            {
                CurrentSession.AddValue("PassQtyinlotSetting", 0);
                return base.DoExecute(executionContext);
            }
            //------------------------------------------
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            //UC update: Add @Type
            //----------------------------------------------------
            string strType = "";
            string strMBCode = "";
            bool isFru = false;
            try
            {
                isFru = (bool)CurrentSession.GetValue("bFruChecked");    
            }
            catch (FisException ex)
            {
                var strerr1 = ex.Message;
                isFru = false;
            }  
            catch (Exception e)
            {
                var strerr2 = e.Message;
                isFru = false;
            }
            //2.28.	MBCode:若第6码为’M’，则取MBSN前3码为MBCode，若第5码为’M’，则取前2码
            //CheckCode:若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码
            string strCheckCode = "";
            if (currenMB.Sn.Substring(5, 1) == "M" || currenMB.Sn.Substring(5, 1) == "B")
            {
                strMBCode = currenMB.Sn.Substring(0, 3);
                strCheckCode = currenMB.Sn.Substring(6, 1);
            }
            else if (currenMB.Sn.Substring(4, 1) == "M" || currenMB.Sn.Substring(4, 1) == "B")
            {
                strMBCode = currenMB.Sn.Substring(0, 2);
                strCheckCode = currenMB.Sn.Substring(5, 1);
            }
            else
            {
                strMBCode = currenMB.Sn.Substring(0, 2);
                strCheckCode = currenMB.Sn.Substring(5, 1);

            }
            //2.29.	MBSN子板/RCTO的判定:CheckCode为数字，则为子板，为’R’，则为RCTO
            if (strCheckCode == "R")
            {
                //为’R’，则为RCTO
                strType = "RCTO";
            }
            else if (isFru)
            {
                strType = "Fru";
            }
            else
            {
                strType = "PC";
            }
            //----------------------------------------------------
            LotSettingInfo conlotSetting = new LotSettingInfo();
            int passQtyforLine = 0;
            if (this.Line==null)
            {
                conlotSetting.line = currenMB.MBStatus.Line;
            }
            else
            {
                conlotSetting.line = this.Line;
            }
            //UC update:先用当前Line+@Type查找，若不存在记录，用‘ALL’+@Type继续查找
            conlotSetting.type = strType;
            IList<LotSettingInfo> LotSettinglst = itemRepository.GetLotSettingInfoList(conlotSetting);
            if ((LotSettinglst == null) || (LotSettinglst.Count == 0))
            {
                conlotSetting = new LotSettingInfo();
                conlotSetting.line = "ALL";
                conlotSetting.type = strType;
                LotSettinglst = itemRepository.GetLotSettingInfoList(conlotSetting);
                if ((LotSettinglst == null) || (LotSettinglst.Count == 0))
                {
                    //报错：“请与IE联系，维护 Lot 相关设置”
                    List<string> errpara = new List<string>();
                    errpara.Add(currenMB.Sn);
                    FisException ex = new FisException("CHK278", errpara);  
                    throw ex;
                }
                else
                {
                    passQtyforLine = LotSettinglst[0].passQty;
                }
            }
            else
            {
                passQtyforLine = LotSettinglst[0].passQty;
            }

            CurrentSession.AddValue("PassQtyinlotSetting", passQtyforLine);

            return base.DoExecute(executionContext);
        }
	
	}
}
