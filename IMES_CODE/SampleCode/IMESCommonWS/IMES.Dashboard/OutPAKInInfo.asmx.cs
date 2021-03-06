﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using IMES.LD;
using log4net;
using System.Collections.Generic;
using System.Configuration;

namespace IMES.Dashboard
{
    /// <summary>
    /// Summary description for OutPAKInInfo
    /// </summary>
    [WebService(Namespace = "http://inventec.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OutPAKInInfo : System.Web.Services.WebService
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public OutPAKIn[] GetOutPAKInInfo()
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            DateTime now = DateTime.Now;
            IList<OutPAKIn> ret = new List<OutPAKIn>();
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //1.检查传进来的参数                

                //2.获取DB中的数据
                string dbConnectStr = ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                string sqlText = @"select top 1 Period_Date,Station,OutQty,WIPQty,OKRate
                                   from dbo.PSG_IMES_InputOutput_StationTTL
                                   WHERE Station='69'
                                   ORDER BY Period_Date DESC";
                IList<OutPAKIn> OutPAKInList = SqlHelper.ExecuteReader<OutPAKIn>(dbConnectStr, CommandType.Text, sqlText);

                for (int i = 0; i <= OutPAKInList.Count - 1; ++i)
                {
                    ret.Add(new OutPAKIn
                    {
                        Period_Date = OutPAKInList[i].Period_Date,
                        Station = OutPAKInList[i].Station,
                        OutQty = OutPAKInList[i].OutQty,
                        WIPQty = OutPAKInList[i].WIPQty,
                        OKRate = OutPAKInList[i].OKRate
                    });
                }
                return ret.ToArray();

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);

                return ret.ToArray();
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
    }
}
