/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: PACosmetic
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-10-20   zhu lei            Create 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Station.Interface.StationIntf;

public partial class PAK_ReleaseProductIDHold : System.Web.UI.Page
{
    public string[] GvQueryColumnName = { "CUSTSN","ProductID","Model","PreStation","PreStatus",
                                            "PdLine","HoldStation","HoldUser","HoldTime","HoldCode",
                                            "HoldDescr"};
    public int[] GvQueryColumnNameWidth = { 45,45,50,40,40,
                                            35,45,45,65,40,
                                            50};
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IReleaseProductIDHold iReleaseProductIDHold = ServiceAgent.getInstance().GetObjectByName<IReleaseProductIDHold>(WebConstant.ReleaseProductIDHoldObject);
    private const int DEFAULT_ROWS = 6;
    public String UserId;
    public String Customer;
    public String GuidCode;
    public string HoldStationValue;
    public string[] HoldStationList;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Customer))
            {
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;
            }
            if (!this.IsPostBack)
            {
                HoldStationValue = Request["HoldStation"] ?? "";
                //HoldStationList = HoldStationValue.Split(',');
                initLabel();
                GetGotoStation();
                bindTable(null);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }
   
    private void initLabel()
    {
        this.Panel3.GroupingText = this.GetLocalResourceObject(Pre + "_pnlInputDefectList").ToString();
    }

    private ArrayList GetGotoStation()
    {
        ArrayList ret = new ArrayList();
        try
        {
            IList<ConstValueInfo> list = iReleaseProductIDHold.GetGotoStationList("GoToStationWithUnPack");
            this.cmbGotoStation.Items.Clear();
            this.cmbGotoStation.Items.Add(string.Empty);
            foreach (ConstValueInfo item in list)
            {
                ListItem Info = new ListItem();
                Info.Text = item.name;
                Info.Value = item.value;
                this.cmbGotoStation.Items.Add(Info);
            }
            return ret;
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void showErrorMessage(string errorMsg)
    {
        bindTable(null);
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private void bindTable(IList<HoldInfo> list)
    {
        DataTable dt = initTable();
        DataRow dr = null;
        string modelList = "";
        string CUSTSNList = "";
        if (list != null && list.Count != 0)
        {
            foreach (HoldInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.CUSTSN;
                CUSTSNList += temp.CUSTSN + ",";
                dr[1] = temp.ProductID;
                dr[2] = temp.Model;
                modelList += temp.Model + ",";
                dr[3] = temp.PreStation;
                dr[4] = temp.PreStatus;
                dr[5] = temp.PreLine;
                dr[6] = temp.Station;
                dr[7] = temp.HoldUser;
                dr[8] = temp.HoldTime;
                dr[9] = temp.HoldCode;
                dr[10] = temp.HoldCodeDescr;
                dt.Rows.Add(dr);
            }
            for (int i = dt.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            modelList = modelList.Substring(0, modelList.Length - 1);
            CUSTSNList = CUSTSNList.Substring(0, CUSTSNList.Length - 1);
        }
        else
        {
            dt = getNullDataTable(DEFAULT_ROWS);
        }

        gd.DataSource = dt;
        gd.DataBind();
        InitGridView();

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "Query", "modelandcustsnList('" + modelList + "','" + CUSTSNList + "');", true);
    }

    private void InitGridView()
    {
        for (int i = 0; i < GvQueryColumnNameWidth.Count(); i++)
        {
            gd.HeaderRow.Cells[i].Width = Unit.Pixel(GvQueryColumnNameWidth[i]);
        }
    }

    private DataTable getNullDataTable(int j)
    {
        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            for (int k = 0; k < 11; k++)
            {
                newRow[k] = "";
            }
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        for (int i = 0; i < GvQueryColumnName.Count(); i++)
        {
            retTable.Columns.Add(GvQueryColumnName[i], System.Type.GetType("System.String"));
        }
        return retTable;
    }
    
    protected void btnQuery_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string[] inputData = this.hidQueryValue.Value.Split(',');
            ArrayList ret = new ArrayList();
            IList<string> inputList = new List<string>();
            foreach (string item in inputData)
            {
                inputList.Add(item);
            }
            IList<string> listHoldStation = null;
            if (string.IsNullOrEmpty(this.hidHoldStationList.Value))
            {
                listHoldStation = new List<string>();
            }
            else
            {
                listHoldStation = this.hidHoldStationList.Value.Split(',').ToList();
            }

            ret = iReleaseProductIDHold.GetReleaseProductIDHoldInfo(inputList, this.hidstationId.Value, this.hideditor.Value, this.hidcustomer.Value, this.hidIsCUSTSNValue.Value, listHoldStation);
            IList<HoldInfo> holdList = (IList<HoldInfo>)ret[0];
            GuidCode = (string)ret[1];
            this.hidGuidCode.Value = GuidCode;
            bindTable(holdList);
            this.UpdatePanel1.Update();
        }
        catch (FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {

            showErrorMessage(ex.Message);
            return;
        }
       
        //showListByACAdaptorList();
        //this.updatePanel2.Update();
        //ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
    }

    
    //private void bindTable(int defaultRow)
    //{
    //    DataTable dt = new DataTable();
    //    DataRow dr = null;
    
        
    //    for (int i = 0; i < defaultRow; i++)
    //    {
    //        dr = dt.NewRow();

    //        dt.Rows.Add(dr);
    //    }

    //    gd.DataSource = dt;
    //    gd.DataBind();
    //}
}
