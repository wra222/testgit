using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using com.inventec.system.util;

public partial class DataMaintain_ModelBOMParent : System.Web.UI.Page
{
    public String currentPartNo=null;
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IBOMNodeData iModelBOM;
    public string showParentFlag;
    private const int DEFAULT_ROWS = 17;
    private const int COL_NUM = 2;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            iModelBOM = (IBOMNodeData)ServiceAgent.getInstance().GetMaintainObjectByName<IBOMNodeData>(WebConstant.IBOMNodeData);
            if (!this.IsPostBack)
            {
                this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
                currentPartNo = Request.QueryString["currentPartNo"];
                currentPartNo = StringUtil.decode_URL(currentPartNo);
                showParentFlag = Request.QueryString["flag"];

                //macRangeCode = "001";
                initLabel();

                //MACInfoDef info = iModelBOM.GetMACInfo(macRangeCode);
                //ShowInfo(info);
                bindTable(null, DEFAULT_ROWS);                
                setColumnWidth();
                ShowTableList(showParentFlag);
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

    private Boolean ShowTableList(string flag)
    {
        try
        {
            string code = this.dCurrentPartNo.Text.Trim();
            DataTable  dataList;
            if (flag == "AllParent")
            {
                dataList = iModelBOM.GetAllParentInfo(code);
            }
            else
            {
                dataList = iModelBOM.GetParentInfo(code);
            }
            if (dataList == null || dataList.Rows.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataList, DEFAULT_ROWS);
            }

        }
        catch (FisException ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;

    }
    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }
    protected void btnParent_ServerClick(Object sender, EventArgs e)
    {
        ShowTableList("Parent");
    }

    protected void btnAllParent_ServerClick(Object sender, EventArgs e)
    {
        ShowTableList("AllParent"); 
    }

    //SELECT distinct a.Material, b.Descr, c.Flag, c.ApproveDate  
    private void bindTable(DataTable list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDescription").ToString());
        dt.Columns.Add("Flag");
        dt.Columns.Add("ApproveDate");


        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < list.Rows[i].ItemArray.Count(); j++)
                {
                    dr[j] = Null2String(list.Rows[i][j]);
                }
                dt.Rows.Add(dr);
            }

            for (int i = list.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Rows.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.hidRecordCount.Value = "";
        }

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(40);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(60);

    }

    private void initLabel()
    {
        this.lblPmt.Text = this.GetLocalResourceObject(Pre + "_lblPmt").ToString();
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblList").ToString();
        this.dCurrentPartNo.Text = currentPartNo;

        this.btnParent.InnerText = this.GetLocalResourceObject(Pre + "_btnParent").ToString();
        this.btnAllParent.InnerText = this.GetLocalResourceObject(Pre + "_btnAllParent").ToString();
        this.btnOK.InnerText = this.GetLocalResourceObject(Pre + "_btnOK").ToString();

    }



    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }


    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }

    }
}
