using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using System.Text;

public partial class ModelInfoMaintain: IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 18;
    IFamilyInfoEx iFamilyInfo = ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyInfoEx>(WebConstant.IFamilyInofoObjectEx);
    private string editor;

    private string strModelName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            editor = Request.QueryString["editor"];
            strModelName = Request.QueryString["modelname"];
            this.valModel.Text = strModelName;
            this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
            
            InitLabel();
            bindTable();
           // setColumnWidth();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    private void setColumnWidth()
    {
        //gdModelInfoNameList.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        gdModelInfoList.HeaderRow.Cells[1].Width = Unit.Pixel(120);
        gdModelInfoList.HeaderRow.Cells[2].Width = Unit.Pixel(210);
        gdModelInfoList.HeaderRow.Cells[3].Width = Unit.Pixel(220);
        gdModelInfoList.HeaderRow.Cells[4].Width = Unit.Pixel(100);
        gdModelInfoList.HeaderRow.Cells[5].Width = Unit.Pixel(160);
        gdModelInfoList.HeaderRow.Cells[6].Width = Unit.Pixel(160);
    }

    protected void bindTable()
    {

        try
        {

            IList<FamilyInfoDef> modelinfolist = iFamilyInfo.GetFamilyInfoList(strModelName);

            DataTable dt = new DataTable();
            DataRow dr = null;


            dt.Columns.Add("id");
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colName").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDesc").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colValue").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

            if (modelinfolist != null && modelinfolist.Count != 0)
            {
                foreach (FamilyInfoDef temp in modelinfolist)
                {
                    dr = dt.NewRow();

                    dr[0] = temp.id;
                    dr[1] = temp.name;
                    dr[2] = temp.descr;
                    dr[3] = temp.value;
                    dr[4] = temp.editor;
                    if (temp.cdt == DateTime.MinValue)
                    {
                        dr[5] = "";
                    }
                    else
                    {
                        dr[5] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (temp.udt == DateTime.MinValue)
                    {
                        dr[6] = "";
                    }
                    else
                    {
                        dr[6] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    
                    dt.Rows.Add(dr);
                }

                for (int i = modelinfolist.Count; i < DEFAULT_ROWS; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }
            gdModelInfoList.DataSource = dt;
            gdModelInfoList.DataBind();
            setColumnWidth();

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }


    }

    protected void btnRefreshModelList_Click(Object sender, EventArgs e)
    {
        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "test_clearInputs", "test_clearInputs();", true);
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblInfo.Text = this.GetLocalResourceObject(Pre + "_lblInfo").ToString();
        //this.lblName.Text = this.GetLocalResourceObject(Pre + "_lblName").ToString();
        //this.lblValue.Text = this.GetLocalResourceObject(Pre + "_lblValue").ToString();
        //this.lblDesc.Text = this.GetLocalResourceObject(Pre + "_lblDesc").ToString();
        //this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        //this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();

        //setFocus();


    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");//id

        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }




    protected void btnSave_Click(Object sender, EventArgs e)
    {
        string name = hidItem.Value;
        string value = txtValue.Text;
        string modelInfoId = hidModelInfoId.Value;

        FamilyInfoDef model = new FamilyInfoDef();

        model.name = name;
        model.value = value;
        model.family = strModelName;
        model.editor = editor;

        try
        {

            if (modelInfoId.Length != 0)
            {
                model.id = Int32.Parse(modelInfoId);
            }

            if (Int32.Parse(modelInfoId) == 0)//no id
            {
                if (value.Length != 0)
                {

                    modelInfoId = iFamilyInfo.AddSelectedFamilyInfo(model).ToString();
                }
            }
            else
            {
                if (value.Length == 0)
                {
                    //iFamilyInfo.DeleteSelectedFamilyInfo(model);
                    iFamilyInfo.DeleteFamilyInfo(model);
                    modelInfoId = "-1";
                }
                else
                {
                    iFamilyInfo.UpdateSelectedFamilyInfo(model);
                }
            }

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;

        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }

        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"" + modelInfoId + "\");", true);
    }



    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        //scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");"); 
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    /*
    private void clearInputs()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("clearInputs();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "clearInputs", scriptBuilder.ToString(), false);
    }*/

}
