using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.inventec.template;
using com.inventec.template.structure;
using log4net;
using System.IO;
using Seagull.BarTender.Print;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;

namespace IMES.BartenderPrinter
{  
    public class BartenderLabelMgt : MarshalByRefObject, LabelManager
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string DPI = "DPI";
        private static string BartenderTemplateFolder = "BartenderTemplateFolder";
        private static string TempFolder = "TempFolder";

        #region LabelManager Members

        public List<ImageInfo> getImageListForPreview(string printTemplateName, 
                                                                            List<ParamInfo> inputParas)
        {
            throw new NotImplementedException();
        }

        public List<ImageInfo> getImageListForPrint(string printTemplateName, 
                                                                          List<DataSetConnInfo> dataSetConnList, 
                                                                          List<ParamInfo> inputParas, 
                                                                          bool isPrinttingRoom, 
                                                                          string dpi)
        {
            string methodName = "getImageListForPrint";
            logger.DebugFormat("BEGIN: {0}()", methodName);
          
            try
            {
                int dpiValue =300;
                if (!string.IsNullOrEmpty(dpi))
                {
                    dpiValue = int.Parse(dpi);
                }
                else
                {
                    dpiValue = int.Parse(ConfigurationManager.AppSettings[DPI] ?? "300");
                }
                return getImgeInfo(printTemplateName, inputParas, dpiValue);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public List<ImageInfo> getImageListForPrint(string printTemplateName, 
                                                                            List<DataSetConnInfo> dataSetConnList, 
                                                                            List<ParamInfo> inputParas, 
                                                                            bool isPrinttingRoom)
        {
            string methodName = "getImageListForPrint";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                int dpi = int.Parse(ConfigurationManager.AppSettings[DPI] ?? "300");
                return getImgeInfo(printTemplateName, inputParas, dpi);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public List<ImageInfo> getImageListForPrintBySinglePara(string printTemplateName, 
                                                                                            List<DataSetConnInfo> dataSetConnList, 
                                                                                            string inputPara, 
                                                                                            bool isPrinttingRoom)
        {
            throw new NotImplementedException();
        }

        public List<ImageInfo> getImageListForPrintBySinglePara(string printTemplateName, 
                                                                                             List<DataSetConnInfo> dataSetConnList, 
                                                                                            string inputPara, 
                                                                                            bool isPrinttingRoom, 
                                                                                            string dpi)
        {
            throw new NotImplementedException();
        }
        #endregion

        private List<ImageInfo> getImgeInfo(string printTemplateName, List<ParamInfo> inputParas, int dpi)
        {
            string methodName = "getImgeInfo";
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<ImageInfo> infoList = new List<ImageInfo>();
                string btwFolder = ConfigurationManager.AppSettings[BartenderTemplateFolder];
                string tempFolder = ConfigurationManager.AppSettings[TempFolder];
                string[] printNameList = printTemplateName.Split(new char[] { '~', ';', ',' });
                if (printNameList.Length < 1)
                {
                    throw new Exception("btw template name is wrong format : " + printTemplateName);
                }

                string btwFileName = printNameList[0];
                string spName = printNameList[1];
                IList<SqlParameter> sqlParameterList = new List<SqlParameter>();
                foreach (ParamInfo info in inputParas)
                {
                    sqlParameterList.Add(SqlHelper.CreateSqlParameter(info.ParamName,info.Values[0]));
                }
                logger.DebugFormat("execute spName: {0}", spName);
                IList<NameValue> nameValueList= ExecuteSP(spName, sqlParameterList);              
               
                IList<string> bmpStrList = BartenderUTL.Producer(dpi, btwFolder, btwFileName, tempFolder, nameValueList);
                List<com.inventec.template.structure.ImageInfo> imgeInfoList = bmpStrList.Select(x => new ImageInfo
                {
                    Image = null,
                    ImageHeightPX = 0,
                    ImagePixPerM = 0,
                    ImageWidthPX = 0,
                    ImageString = x
                }).ToList();

                return imgeInfoList;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
         
        }

        private IList<NameValue> ExecuteSP(string spName, IList<SqlParameter> paramList)            
        {
            try
            {
                IList<NameValue> ret = new List<NameValue>();

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_IMES,
                                                                                                    System.Data.CommandType.StoredProcedure,
                                                                                                     spName,
                                                                                                     paramList.ToArray()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(SQLReader.ToObject<NameValue>(sqlR));
                        }
                    }
                }

                return ret;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }


   
}
