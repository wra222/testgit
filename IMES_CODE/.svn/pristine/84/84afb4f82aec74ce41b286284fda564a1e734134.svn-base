using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;
using IMES.Infrastructure;
//using IMES.DataModel;

public partial class Query_MastereSOPQuery : System.Web.UI.Page
{
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);        
    IFA_MastereSOPQuery intfeMastereSOP = ServiceAgent.getInstance().GetObjectByName<IFA_MastereSOPQuery>(WebConstant.IFA_MastereSOPQuery);
    
    String DBConnection = "";
    public string[] gvQueryColumnName = { "Model", "url" };
    public int[] gvQueryColumnNameWidth = { 100,  250 };

    protected void Page_Load(object sender, EventArgs e)
    {
        DBConnection = CmbDBType.ddlGetConnection();    
        if (!IsPostBack) {
            InitPage();                        
        }
    }

    protected void InitPage()
    {
       

    }

    //protected void InitPart()
    //{

    //    DataTable Model = intfeMastereSOP.GetASTPart(DBConnection);
    //    ddlPart.Items.Add(new ListItem("", "---"));
    //    for (int i = 0; i < Model.Rows.Count; i++)
    //    {
    //        ddlPart.Items.Add(new ListItem(Model.Rows[i]["PartNo"].ToString(), Model.Rows[i]["PartNo"].ToString()));
    //    }
    //}
    
    protected void lbtFreshPage_Click(object sender, EventArgs e)
    {

    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
      
              queryClick(sender, e);
         
    }
    
    public void queryClick(object sender, System.EventArgs e)
    {
        if (tbProID.Text.Trim() != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = getDataTable();
                gvQuery.DataSource = dt;
                gvQuery.DataBind();


                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Image img = new Image();
                        img.ImageUrl = dt.Rows[i]["url"].ToString();
                        img.Width = 400;
                        pl1.Controls.Add(img);

                    }
                    gvQuery.HeaderRow.Cells[0].Width = Unit.Pixel(120);
                    gvQuery.HeaderRow.Cells[1].Width = Unit.Pixel(100);
                    InitGridView();
                }
                else
                {
                    gvQuery.DataSource = null;
                    writeToAlertMessage("There is no Master label");
                    imgesop.ImageUrl = "";

                }
            }
            catch (Exception ex)
            {
                gvQuery.DataSource = null;
                writeToAlertMessage(ex.ToString());
            }
            finally
            {
                hideWait();
            }
        }
        else
        {
            try
            {
               
                    gvQuery.DataSource = null;
                    writeToAlertMessage("There is no Master label");
                    imgesop.ImageUrl = "";

                }
           
            catch (Exception ex)
            {
                gvQuery.DataSource = null;
                writeToAlertMessage(ex.ToString());
            }
            finally
            {
                hideWait();
            }
        }
    }

    private DataTable getDataTable()
    {
       
        DataTable dt = new DataTable();

        
            //ddlPart.SelectedIndex = -1;
            //ddlType.Items.Clear();
            dt = intfeMastereSOP.GetMastereSOP(DBConnection, tbProID.Text.Trim());
       

        return dt;
       
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

    }

    private void writeToAlertMessage(string errorMsg)
    {

        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        //Response.Write(scriptBuilder.ToString());
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        //scriptBuilder.AppendLine("endWaitingCoverDiv();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");
    }



    //protected void ddlPart_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DataTable Model = intfeMastereSOP.GetModel(DBConnection, ddlPart.SelectedValue);
    //    ddlType.Items.Clear();
    //    for (int i = 0; i < Model.Rows.Count; i++)
    //    {
    //        ddlType.Items.Add(new ListItem(Model.Rows[i]["Model"].ToString(), Model.Rows[i]["Model"].ToString()));
    //    }
    //}

    private void InitGridView()
    {

        for (int i = 0; i < gvQueryColumnName.Length; i++)
        {
            gvQuery.HeaderRow.Cells[i].Width = Unit.Pixel(gvQueryColumnNameWidth[i]);
        }
    }
}
