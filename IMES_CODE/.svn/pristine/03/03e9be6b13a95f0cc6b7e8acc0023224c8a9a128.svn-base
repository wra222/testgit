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

public partial class ModelInfoNameMaintain: IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 18;
    //IFamilyInfoName iFamilyInfoName = ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyInfoName>(WebConstant.IFAMILYINFOObject);
    IFamilyInfoEx iFamilyInfoName = ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyInfoEx>(WebConstant.IFamilyInofoObjectEx);
    private string editor;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            editor = Request.QueryString["editor"];
            this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();

            InitLabel();
            bindTable();
            //setColumnWidth();
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
        gdModelInfoNameList.HeaderRow.Cells[1].Width = Unit.Pixel(100);
        
        gdModelInfoNameList.HeaderRow.Cells[2].Width = Unit.Pixel(120);
        gdModelInfoNameList.HeaderRow.Cells[3].Width = Unit.Pixel(200);
        gdModelInfoNameList.HeaderRow.Cells[4].Width = Unit.Pixel(250);
        gdModelInfoNameList.HeaderRow.Cells[5].Width = Unit.Pixel(250);
    }

    protected void bindTable()
    {

        try
        {
            IList<FamilyInfoNameEx> modelinfoNamelist = iFamilyInfoName.GetFamilyInfoName();

            DataTable dt = new DataTable();
            DataRow dr = null;


            dt.Columns.Add("id");
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colInfoName").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDesc").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

            if (modelinfoNamelist != null && modelinfoNamelist.Count != 0)
            {
                foreach (FamilyInfoNameEx temp in modelinfoNamelist)
                {
                    dr = dt.NewRow();

                    dr[0] = "";
                    dr[1] = temp.Name;
                    dr[2] = temp.Description;
                    dr[3] = temp.Editor;
                    if (temp.Cdt == DateTime.MinValue)
                    {
                        dr[4] = "";
                    }
                    else
                    {
                        dr[4] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (temp.Udt == DateTime.MinValue)
                    {
                        dr[5] = "";
                    }
                    else
                    {
                        dr[5] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    dt.Rows.Add(dr);
                }

                for (int i = modelinfoNamelist.Count; i < DEFAULT_ROWS; i++)
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

            gdModelInfoNameList.DataSource = dt;
            gdModelInfoNameList.DataBind();
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
        this.lblInfo.Text = this.GetLocalResourceObject(Pre + "_lblInfo").ToString();
        this.lblName.Text = this.GetLocalResourceObject(Pre + "_lblName").ToString();
        this.lblDesc.Text = this.GetLocalResourceObject(Pre + "_lblDesc").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();

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


    protected void btnAdd_Click(Object sender, EventArgs e)
    {
        string name = txtName.Text;
        string desc = txtDesc.Text;
        bool bExist = false;

        try
        {
            if ("".Equals(name))
            {
                showErrorMessage(this.GetLocalResourceObject(Pre + "_msgAddSave").ToString());
                return;
            }

            IList<FamilyInfoNameEx> modelinfoNamelist = iFamilyInfoName.GetFamilyInfoName();
            if (modelinfoNamelist != null && modelinfoNamelist.Count != 0)
            {
                foreach (FamilyInfoNameEx temp in modelinfoNamelist)
                {
                    if (name.Equals(temp.Name))
                    {
                        bExist = true;
                        break;
                    }
                }
            }
            if (bExist)
            {
                showErrorMessage(this.GetLocalResourceObject(Pre + "_msgNameExist").ToString());
                return;
            }

            FamilyInfoNameEx model = new FamilyInfoNameEx();
            model.Name = name;
            model.Description = desc;
            model.Editor = editor;
            iFamilyInfoName.AddFamilyInfoName(model);
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
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"\");", true);

    }

    protected void btnSave_Click(Object sender, EventArgs e)
    {
        string name = txtName.Text.Trim();
        string desc = txtDesc.Text;
        string modelInfoNameId = hidModelInfoNameId.Value;
        bool bExist = false;

        try
        {
            if ("".Equals(name))
            {
                showErrorMessage(this.GetLocalResourceObject(Pre + "_msgAddSave").ToString());
                return;
            }

            if (!hidModelInfoNameId.Value.Equals(name))
            {
                IList<FamilyInfoNameEx> modelinfoNamelist = iFamilyInfoName.GetFamilyInfoName();
                if (modelinfoNamelist != null && modelinfoNamelist.Count != 0)
                {
                    foreach (FamilyInfoNameEx temp in modelinfoNamelist)
                    {
                        if (name.Equals(temp.Name))
                        {
                            bExist = true;
                            break;
                        }
                    }
                }
                if (bExist)
                {
                    showErrorMessage(this.GetLocalResourceObject(Pre + "_msgNameExist").ToString());
                    return;
                }
            }

            FamilyInfoNameEx model = new FamilyInfoNameEx();
            model.Name = name;
            model.Description = desc;
            model.Editor = editor;
            iFamilyInfoName.UpdateSelectedFamilyInfoName(model, hidModelInfoNameId.Value);
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
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"" + modelInfoNameId + "\");", true);
    }




    protected void btnDelete_Click(Object sender, EventArgs e)
    {
        try
        {
            iFamilyInfoName.DeleteSelectedFamilyInfoName(hidModelInfoNameId.Value);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
        }
        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"\");", true);
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
