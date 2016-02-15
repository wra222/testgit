using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Text;
using Inventec.HPEDITS.XmlCreator.Database;

public partial class HP_UpLoadFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCT_Click(object sender, EventArgs e)
    {
        if (FuCT.PostedFile.FileName != "")
        {

            string TruncateCTsql = "EXEC sp_Upload_HPFiles_Dean2D @vchCmd='TRUNCATE_CT_TABLE', @vchSet=''";
            DAO.sqlCmd(Constant.S_EDITSConnStr, TruncateCTsql);
            
            string filepath = FileUpload(FuCT);
            DataSet ds = ImportCSV(filepath);
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    StringBuilder vchSet = new StringBuilder();
                    vchSet.Append(Method.BuildXML(dr[0].ToString().Trim(), "COND_PRIORITY"));
                    vchSet.Append(Method.BuildXML(dr[1].ToString().Trim(), "REGION"));
                    vchSet.Append(Method.BuildXML(dr[2].ToString().Trim(), "ORDER_TYPE"));
                    vchSet.Append(Method.BuildXML(dr[3].ToString().Trim(), "SALES_CHAN"));
                    vchSet.Append(Method.BuildXML(dr[4].ToString().Trim(), "SHIP_MODE"));
                    vchSet.Append(Method.BuildXML(dr[5].ToString().Trim(), "SHIP_CAT_TYPE"));
                    vchSet.Append(Method.BuildXML(dr[6].ToString().Trim(), "INTL_CARRIER"));
                    vchSet.Append(Method.BuildXML(dr[7].ToString().Trim(), "MAND_CARRIER_ID"));
                    vchSet.Append(Method.BuildXML(dr[8].ToString().Trim(), "TRANS_SERV_LEVEL"));
                    vchSet.Append(Method.BuildXML(dr[9].ToString().Trim(), "PREF_GATEWAY"));
                    vchSet.Append(Method.BuildXML(dr[10].ToString().Trim(), "SUB_REGION"));
                    vchSet.Append(Method.BuildXML(dr[11].ToString().Trim(), "SHIP_TO_COUNTRY_CODE"));
                    vchSet.Append(Method.BuildXML(dr[12].ToString().Trim(), "AREA_GROUP_ID"));
                    vchSet.Append(Method.BuildXML(dr[13].ToString().Trim(), "SHIP_TO_STATE"));
                    vchSet.Append(Method.BuildXML(dr[14].ToString().Trim(), "SHIP_REF"));
                    vchSet.Append(Method.BuildXML(dr[15].ToString().Trim(), "CUSTOMER_ID"));
                    vchSet.Append(Method.BuildXML(dr[16].ToString().Trim(), "SHIP_TO_ID"));
                    vchSet.Append(Method.BuildXML(dr[17].ToString().Trim(), "MULTI_LINE_ID"));
                    vchSet.Append(Method.BuildXML(dr[18].ToString().Trim(), "HP_PN"));
                    vchSet.Append(Method.BuildXML(dr[19].ToString().Trim(), "HP_EXCEPT"));
                    vchSet.Append(Method.BuildXML(dr[20].ToString().Trim(), "PHYS_CONSOL"));
                    vchSet.Append(Method.BuildXML(dr[21].ToString().Trim(), "EDI_DISTRIB_CHAN"));
                    vchSet.Append(Method.BuildXML(dr[22].ToString().Trim(), "EDI_INTL_CARRIER"));
                    vchSet.Append(Method.BuildXML(dr[23].ToString().Trim(), "EDI_PL_CODE"));
                    vchSet.Append(Method.BuildXML(dr[24].ToString().Trim(), "EDI_MFG_CODE"));
                    vchSet.Append(Method.BuildXML(dr[25].ToString().Trim(), "EDI_TRANS_SERV_LEVEL"));
                    vchSet.Append(Method.BuildXML(dr[26].ToString().Trim(), "EDI_LANG_CODE"));
                    vchSet.Append(Method.BuildXML(dr[27].ToString().Trim(), "EDI_DOC_SET_NUMBERCODE"));
                    vchSet.Append(Method.BuildXML(dr[28].ToString().Trim(), "DOC_SET_NUMBER"));
                    vchSet.Append(Method.BuildXML(dr[29].ToString().Trim(), "SOURCE_LANG_FORMAT_CODE"));


                    string InsertCTsql = "EXEC sp_Upload_HPFiles_Dean2D @vchCmd='INSERT_CT_TABLE', @vchSet='" + vchSet.ToString() + "'";
                    DAO.sqlCmd(Constant.S_EDITSConnStr, InsertCTsql);
                }
                 
                   string sScript = @"<script language='javascript'>alert('CTFile Upload Successfully!!');</script>";
                    ClientScript.RegisterStartupScript(Page.GetType(), "", sScript);
                }
            catch(Exception ex)
            {
                Response.Write(ex);
            }
            
            
        }
    }
    public string FileUpload(FileUpload fu)
    {
        string backupFile = "";
        if (fu.PostedFile.FileName != "")
        {
            backupFile = fu.PostedFile.FileName.Substring(fu.PostedFile.FileName.LastIndexOf(@"\") + 1);//RT_EDITS_DOC_SET_CONTENTS_20080319.csv

            if (backupFile != "")
            {
                if (backupFile.Substring(0, 2) == "CT")
                {
                    fu.PostedFile.SaveAs(ConfigurationManager.AppSettings["FilePath"] + @"\CT\" + backupFile);//
                }
                else if (backupFile.Substring(0,2)=="RT")
                {
                    fu.PostedFile.SaveAs(ConfigurationManager.AppSettings["FilePath"] + @"\RT\" + backupFile);//
                }
                else// new second RT
                {
                    fu.PostedFile.SaveAs(ConfigurationManager.AppSettings["FilePath"] + @"\NSRT\" + backupFile);//
                }
            }
        }
        else
        {
            backupFile = "";
        }

        return backupFile;
    }

    private DataSet ImportCSV(string filepath)
    {
        OdbcConnection cnCSV = null;
        OdbcDataAdapter daCSV = null;
        DataSet ds = new DataSet();
        string strConnectionString = "";
        ////****Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=檔案位置;Extensions=asc,csv,tab,txt;Persist Security Info=False;
        if (filepath.Substring(0, 2) == "CT")
        {
            strConnectionString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" +
                ConfigurationSettings.AppSettings["FilePath"] + @"\CT\" +
                ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
        }
        else if (filepath.Substring(0,2)=="RT")
        {
            strConnectionString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" +
               ConfigurationSettings.AppSettings["FilePath"] + @"\RT\" +
               ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
        }
        else// new second RT
        {
            strConnectionString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" +
               ConfigurationSettings.AppSettings["FilePath"] + @"\NSRT\" +
               ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
        }

                  
        cnCSV = new OdbcConnection(strConnectionString);
        cnCSV.Open();
        OdbcCommand cmdSelect = new OdbcCommand(@"SELECT * FROM [" + filepath + "]", cnCSV); //command
        daCSV = new System.Data.Odbc.OdbcDataAdapter();

        daCSV.SelectCommand = cmdSelect;  //excute sql command
        DataTable dtCSV = new DataTable();//a table to save data
        daCSV.Fill(dtCSV);
        ds.Tables.Add(dtCSV);
        daCSV = null;
        cnCSV.Close();
        return ds;
    }
    protected void btnRT_Click(object sender, EventArgs e)
    {
        string xltfilepath = "";
        string pdffilepath = "";
        if (FuRT.PostedFile.FileName != "")
        {

            string TruncateCTsql = "EXEC sp_Upload_HPFiles_Dean2D @vchCmd='TRUNCATE_RT_TABLE', @vchSet=''";
            DAO.sqlCmd(Constant.S_EDITSConnStr, TruncateCTsql);

            string filepath = FileUpload(FuRT);
            int versionindex = filepath.Trim().IndexOf(".");
            string version = filepath.Trim().Substring(versionindex - 4,4);
            DataSet ds = ImportCSV(filepath);
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    StringBuilder vchSet = new StringBuilder();
                    vchSet.Append(Method.BuildXML(dr[0].ToString().Trim(), "DOC_SET_NUMBER"));
                    vchSet.Append(Method.BuildXML(dr[1].ToString().Trim(), "DOC_CAT"));
                    vchSet.Append(Method.BuildXML(dr[2].ToString().Trim(), "XSL_TEMPLATE_NAME"));
                    vchSet.Append(Method.BuildXML(dr[3].ToString().Trim(), "SCHEMA_FILE_NAME"));
                    vchSet.Append(Method.BuildXML(dr[4].ToString().Trim(), "IMAGE_FILES"));
                    vchSet.Append(Method.BuildXML(dr[5].ToString().Trim(), "STRING_ID_FLAG"));
                    vchSet.Append(Method.BuildXML(version,"VERSION"));

                    xltfilepath = ConfigurationManager.AppSettings["HPFilePath"] + dr[2].ToString().Trim();
                    //xltfilepath = "\\\\10.99.252.204\\OUT\\" + dr[2].ToString().Trim();
                    /* if (!File.Exists(xltfilepath))
                     {
                         string sScript1 = @"<script language='javascript'>alert('not find the file of "+ dr[2].ToString().Trim() + "!!');</script>";
                         ClientScript.RegisterStartupScript(Page.GetType(), "", sScript1);
                         return;
                     }
                     if (dr[4].ToString().Trim() != "")
                     {
                         pdffilepath = ConfigurationManager.AppSettings["HPFilePath"] + dr[4].ToString().Trim();
                         if (!File.Exists(pdffilepath))
                         {
                             string sScript1 = @"<script language='javascript'>alert('not find the file of " + dr[4].ToString().Trim() + "!!');</script>";
                             ClientScript.RegisterStartupScript(Page.GetType(), "", sScript1);
                             return;
                         }
                     }
                    string CheckRTsql = "select DOC_SET_NUMBER from [PAK.PAKCT] nolock where DOC_SET_NUMBER = '" + dr[0].ToString().Trim() + "'";
                    String[] CheckRow = DAO.sqlCmdArr(Constant.S_EDITSConnStr, CheckRTsql);
                    if (CheckRow[0].Equals("")) 
                    {
                        string sScript1 = @"<script language='javascript'>alert('RT Table not Match the CT Table: " + dr[0].ToString().Trim() + " !!');</script>";
                        ClientScript.RegisterStartupScript(Page.GetType(), "", sScript1);
                        return;
                    } */
                    string InsertRTsql = "EXEC sp_Upload_HPFiles_Dean2D @vchCmd='INSERT_RT_TABLE', @vchSet='" + vchSet.ToString() + "'";
                    DAO.sqlCmd(Constant.S_EDITSConnStr, InsertRTsql);

                }
                   string sScript = @"<script language='javascript'>alert('RT_File Upload Successfully!!');</script>";
                   ClientScript.RegisterStartupScript(Page.GetType(), "", sScript);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

    }
            //-------------------add  new second rt-------
    protected void btnNSRT_Click(object sender, EventArgs e)
    {
        if (FuNSRT.PostedFile.FileName != "")
        {
            string TruncateCTsql = "EXEC sp_Upload_HPFiles_DEan2D @vchCmd='TRUNCATE_NSRT_TABLE', @vchSet=''";
            DAO.sqlCmd(Constant.S_EDITSConnStr, TruncateCTsql);

            string filepath = FileUpload(FuNSRT);
            int versionindex = filepath.Trim().IndexOf(".");
            string version = filepath.Trim().Substring(versionindex - 4, 4);            
            DataSet ds = ImportCSV(filepath);
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    StringBuilder vchSet = new StringBuilder();
                    vchSet.Append(Method.BuildXML(dr[0].ToString().Trim(), "XSL_TEMPLATE_NAME"));
                    vchSet.Append(Method.BuildXML(dr[1].ToString().Trim(), "STRING_ID"));
                    vchSet.Append(Method.BuildXML(dr[2].ToString().Trim(), "SUB_STRING_ID"));
                    vchSet.Append(Method.BuildXML(dr[3].ToString().Trim(), "SUB_STRING_SEQUENCE"));
                    vchSet.Append(Method.BuildXML(dr[4].ToString().Trim(), "SUB_STRING_MAX_OCCURENCE"));
                    vchSet.Append(Method.BuildXML(dr[5].ToString().Trim(), "DATA_ELEMENT"));
                    vchSet.Append(Method.BuildXML(dr[6].ToString().Trim(), "DATA_ELEMENT_SEQUENCE"));
                    vchSet.Append(Method.BuildXML(dr[7].ToString().Trim(), "DATA_ELEMENT_RANGE_START"));
                    vchSet.Append(Method.BuildXML(dr[8].ToString().Trim(), "DATA_ELEMENT_RANGE_END"));
                    vchSet.Append(Method.BuildXML(dr[9].ToString().Trim(), "ENCODING"));
                    vchSet.Append(Method.BuildXML(dr[10].ToString().Trim(), "ENCODER"));
                    vchSet.Append(Method.BuildXML(dr[11].ToString().Trim(), "DELIMITER"));
                    //vchSet.Append(Method.BuildXML(dr[12].ToString().Trim(), "STRING_PREFIX"));
                    vchSet.Append(Method.BuildXML(dr[12].ToString().Replace(@"'",@"''").Trim(), "STRING_PREFIX"));
                    vchSet.Append(Method.BuildXML(dr[13].ToString().Trim(), "STRING_SUFFIX"));
                    vchSet.Append(Method.BuildXML(dr[14].ToString().Trim(), "SUB_STRING_PREFIX"));
                    vchSet.Append(Method.BuildXML(dr[15].ToString().Trim(), "SUB_STRING_SUFFIX"));
                    vchSet.Append(Method.BuildXML(dr[16].ToString().Trim(), "DATA_ELEMENT_PREFIX"));
                    vchSet.Append(Method.BuildXML(dr[17].ToString().Trim(), "DATA_ELEMENT_SUFFIX"));
                    vchSet.Append(Method.BuildXML(version, "VERSION"));                                 

                    string InsertRTsql = "EXEC sp_Upload_HPFiles_Dean2D @vchCmd='INSERT_NSRT_TABLE', @vchSet='" + vchSet.ToString() + "'";
                    DAO.sqlCmd(Constant.S_EDITSConnStr, InsertRTsql);

                }
                string sScript = @"<script language='javascript'>alert('New Second RT_File Upload Successfully!!');</script>";
                ClientScript.RegisterStartupScript(Page.GetType(), "", sScript);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }
    }
    
}
