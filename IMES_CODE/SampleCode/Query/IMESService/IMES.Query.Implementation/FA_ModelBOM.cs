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
    public class FA_ModelBOM: MarshalByRefObject, IFA_ModelBOM
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string ModelName,string ModelNode,bool Root)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                if (Root)
                {
                    SQLText = @"SELECT @Model,@Model,'','','0' ";
                }
                else
                {

                    SQLText = @"SELECT DISTINCT Component,Material,
                                BomNodeType,
                                PartType,
                                [Level] 
                                FROM [dbo].[fn_ExpandBom](@Model)  
                                WHERE Material=@ModelNode ";
                                //ORDER BY 5 ";
                }

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@Model", 50, ModelName),
                                                 SQLHelper.CreateSqlParameter("@ModelNode", 50, ModelNode));
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

        public DataTable GetModelBOM(string Connection, string Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            try
            {
             string SQLText = "";
             SQLText = @"SELECT DISTINCT Material, Component, BomNodeType, PartType, [Level] 
                         FROM [dbo].[fn_ExpandBom](@Model) "; 
        

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@Model", 50, Model));
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

        public DataTable GetModelInfo(string Connection, string ModelName) {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT Model,Name,Value,Editor,Udt");
                sb.AppendLine(string.Format("FROM ModelInfo WHERE Model = '{0}'", ModelName));                  
            
                return SQLHelper.ExecuteDataFill(Connection,System.Data.CommandType.Text,sb.ToString());
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

        public DataTable GetPartInfo(string Connection, string PartNo) {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT 0 AS id,PartNo,'Part_Descr' AS InfoType,Descr,Editor,Udt ");
                sb.AppendLine(string.Format("FROM Part WHERE PartNo = '{0}'", PartNo));
                sb.AppendLine("UNION");
                sb.AppendLine("SELECT ROW_NUMBER()OVER(ORDER BY InfoType),PartNo,InfoType,InfoValue,Editor,Udt ");
                sb.AppendLine(string.Format("FROM PartInfo WHERE PartNo = '{0}' ",PartNo));

                return SQLHelper.ExecuteDataFill(Connection, System.Data.CommandType.Text, sb.ToString());
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
