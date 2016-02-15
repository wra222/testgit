using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using IMES.Infrastructure;
using log4net;
using System.Reflection;
using System.Data;

namespace IMES.Query.Implementation
{
    public class PAK_DockSnQuery : MarshalByRefObject, IPAK_DockSnQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetDockSnQueryResult(string Connection, String CUSTSN, String Model, Boolean NumType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName); 

            try
            {
                StringBuilder sb = new StringBuilder();

                if (NumType)
                {
                    sb.AppendLine(@"delete a
                                   from [192.168.147.6,998].HPIMES.dbo.DKSn a,Product_Part b 
                                   where a.CUSTSN=b.PartSn 
                                   and b.CheckItemType='DockingSN'
                                   and b.PartSn LIKE 'CNU%'
                                   select * from [192.168.147.6,998].HPIMES.dbo.DKSn");
                    if (CUSTSN.Length < 16)
                    {
                        sb.AppendLine(string.Format("where CUSTSN = '{0}'", CUSTSN));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("where CUSTSN in(select value from dbo.fn_split('{0}',','))", CUSTSN));
                    }
                }
                else
                {
                    if (Model.Length < 16)
                    {
                        sb.AppendLine(string.Format(@"delete a
                                   from [192.168.147.6,998].HPIMES.dbo.DKSn a,Product_Part b 
                                   where a.CUSTSN=b.PartSn 
                                   and b.CheckItemType='DockingSN'
                                   and b.PartSn LIKE 'CNU%'
                                   select * from [192.168.147.6,998].HPIMES.dbo.DKSn where Model ='{0}'", Model));
                    }
                    else
                    {
                        sb.AppendLine(string.Format(@"delete a
                                   from [192.168.147.6,998].HPIMES.dbo.DKSn a,Product_Part b 
                                   where a.CUSTSN=b.PartSn 
                                   and b.CheckItemType='DockingSN'
                                   and b.PartSn LIKE 'CNU%'
                                   select * from [192.168.147.6,998].HPIMES.dbo.DKSn where Model in(select value from dbo.fn_split('{0}',','))", Model));
                    }
                }


                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText);
            }
            catch (Exception e)
            {

                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
    }
}
