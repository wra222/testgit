using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckBaseTime : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckBaseTime()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product newProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<ConstValueInfo> sataionlist = partRep.GetConstValueListByType("CheckBaseTimeDLL");//根据Station 选择ConstValue维护的Type
            List<string> typelist = sataionlist.Where(x => x.name == this.Station).Select(p => p.value).ToList();
            if (typelist != null && typelist.Count > 0)
           {
                    string type = typelist[0];
                    IList<ConstValueInfo> basefamilylist = partRep.GetConstValueListByType(type);
                    //先根据机型查询维护，再用Family

                    var basechecktime = (from p in basefamilylist
                                         where p.name == newProduct.Model
                                         select p.value).ToList();
                    if (basechecktime == null || basechecktime.Count == 0)
                    {
                        basechecktime = (from p in basefamilylist
                                         where p.name == newProduct.Family
                                    select p.value).ToList();
                    }
                    if (basechecktime == null || basechecktime.Count == 0)
                     {
                            throw new FisException("CQCHK0026", new string[] { type+" Model/Family:"+newProduct.Model });
                     }
                    string checklogtime=basechecktime[0];// CKBT<4
                    var stationList = checklogtime.Split(new char[] { '>', '<' });
                    if (stationList.Length!=2)
                    {
                        throw new FisException("CQCHK0026", new string[] { type + " Maintain Error:" + checklogtime });
                    }
                    bool isgreater = checklogtime.IndexOf(">") > 0;
                    string ckbtstation = stationList[0];
                    string ckbttime = stationList[1];
                    int   toleranceAbs;
                    if (!int.TryParse(ckbttime, out toleranceAbs))
                    {
                        throw new FisException("CQCHK0026", new string[] { type + " LogTime Maintain Error:" + stationList[1] });
                    }

                    ProductLog logckbt = productRepository.GetLatestLogByWcAndStatus(newProduct.ProId, ckbtstation, 1);
                    if (null == logckbt)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(ckbtstation);
                        throw new FisException("CHK1008", errpara); // 無記錄
                    }
                    DateTime logtimeckbt = logckbt.Cdt;

                    if (isgreater)//维护的是大于
                    {
                        if (Math.Abs((DateTime.Now - logtimeckbt).TotalMinutes) < toleranceAbs)//小于了维护的时间需要报错：时间不足
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(type);
                            errpara.Add("the time not enough");
                            throw new FisException("CQCHK1126", errpara); // 不足
                        }
                    }
                    else//维护的是小于
                    {
                        if (Math.Abs((DateTime.Now - logtimeckbt).TotalMinutes) > toleranceAbs)//大于了维护的时间需要报错：时间超时
                        {
                            List<string> errpara = new List<string>();
                            errpara.Add(type);
                            errpara.Add(" Over time");
                            throw new FisException("CQCHK1126", errpara); // 超时
                        }
                    }
                      


                }

            
           
            return base.DoExecute(executionContext);
        }
	}
}
