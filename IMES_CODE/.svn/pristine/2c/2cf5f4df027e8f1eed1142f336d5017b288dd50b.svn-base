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
using System.Text;
using System.Collections.Generic;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.system.util;

public partial class MaterialProcessRelation : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 18;
    private const int COL_NUM = 2;
    IProcessManager iProcessManager = ServiceAgent.getInstance().GetMaintainObjectByName<IProcessManager>(WebConstant.IProcessManager);
    private string editor;
    private string MaterialType = "MaterialType";
    private string strProcessName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //qa bug no:ITC-1281-0056
            //editor = "itc98079";//Master.userInfo.UserId; //UserInfo.UserId;//"itc98079";//
            if (!this.IsPostBack)
            {
                strProcessName = Request.QueryString["ProcessName"];
                strProcessName = StringUtil.decode_URL(strProcessName);
                editor = Request.QueryString["userName"];
                editor = StringUtil.decode_URL(editor);
                this.HiddenUserName.Value = editor;

                this.valProcessName.Text = strProcessName;
                InitLabel();
                InitMaterialProcess();
            }
            else
            {
                editor = this.HiddenUserName.Value;
            }
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

    protected void InitMaterialProcess()
    {
        try
        {
            IList<ConstValueTypeInfo> partProcesslist = iProcessManager.getallProcessbyMaterial(MaterialType);
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("MaterialType", Type.GetType("System.String"));

            if (partProcesslist != null && partProcesslist.Count != 0)
            {
                foreach (ConstValueTypeInfo temp in partProcesslist)
                {
                    dr = dt.NewRow();

                    dr["MaterialType"] = temp.value;
                    dt.Rows.Add(dr);
                }

                for (int i = partProcesslist.Count; i < DEFAULT_ROWS; i++)
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

            gdPCBProcess.DataSource = dt;
            gdPCBProcess.DataBind();
            gdPCBProcess.HeaderRow.Cells[1].Text = this.GetLocalResourceObject(Pre + "_colCustomerFamilyModel").ToString();
            gdPCBProcess.HeaderRow.Cells[0].Width = Unit.Pixel(10);
            gdPCBProcess.HeaderRow.Cells[1].Width = Unit.Pixel(70);
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

    protected void gdPCBProcess_RowDataBound(object sender, GridViewRowEventArgs e)
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
    
    protected void bindPCBProcess()
    {
        string strProcessName = valProcessName.Text;

        IList<ConstValueTypeInfo> partProcesslist = iProcessManager.getallProcessbyMaterial(MaterialType);

        IList<MaterialProcess> objPCBProcess = iProcessManager.GetMaterialProcessByProcess(strProcessName);
        //IList<PartProcessMaintainInfo> objPCBProcess = iProcessManager.getPCBProcessListByProcess(strProcessName);

        for (int i = 0; i < objPCBProcess.Count; i++)
        {
            for(int j = 0; j < partProcesslist.Count; j++)
            {
                if (partProcesslist[j].value == objPCBProcess[i].MaterialType)
                {
                    CheckBox chkTmp = (CheckBox)gdPCBProcess.Rows[j].FindControl("RowChk");
                    chkTmp.Checked = true;
                    break;
                }

            }
            
        }

    }

    protected void btnBindDataToCBL_Click(Object sender, EventArgs e)
    {
        try
        {
            bindPCBProcess();
            this.updatePanel3.Update();
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "end", "DealHideWait();", true);
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

    protected void btnOK_Click(Object sender, EventArgs e)
    {
        string strProcessName = valProcessName.Text;
        string strEditor = editor;
        string strMaterialType = "";
        //IList<MaterialProcess> arrPCBProcess = new List<MaterialProcess>();
        //MaterialProcess objPCBProcess = new MaterialProcess();

        try
        {
            
            //objPCBProcess.Process = strProcessName;
            //objPCBProcess.Editor = editor;
            for (int i = 0; i < gdPCBProcess.Rows.Count; i++)
            {

                CheckBox chkTmp = (CheckBox)gdPCBProcess.Rows[i].FindControl("RowChk");
                strMaterialType = (string)gdPCBProcess.Rows[i].Cells[1].Text;
                if (chkTmp.Checked)
                {
                    iProcessManager.AddMaterialProcess(strMaterialType, strProcessName, strEditor);
                }
                else
                {
                    iProcessManager.RemoveMaterialProcessByType(strMaterialType);
                }
            //    if (chkTmp.Checked)
            //    {
                    
            //        MaterialProcess objTmp = new MaterialProcess();
            //        objTmp.MaterialType = (string)gdPCBProcess.Rows[i].Cells[1].Text;
            //        arrPCBProcess.Add(objTmp);
            //    }
            }
            //iProcessManager.addPartProcesses(arrPCBProcess, objPCBProcess);
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
        this.updatePanel3.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "close", "onCancel();DealHideWait();", true);

    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblProcessName.Text = this.GetLocalResourceObject(Pre + "_lblProcessName").ToString();

        this.lblPCB.Text = this.GetLocalResourceObject(Pre + "_lblPCB").ToString();
        this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
        this.btnOK.Value = this.GetLocalResourceObject(Pre + "_btnOK").ToString();
        this.btnCancel.Value = this.GetLocalResourceObject(Pre + "_btnCancel").ToString();

        //setFocus();


    }

    /*
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        //errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        //scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");"); 
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }*/

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
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

}
