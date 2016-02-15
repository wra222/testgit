
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
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;

public partial class DataMaintain_ModelAssemblyCode : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 22;
     private const int COL_NUM = 10;
     private IModelAssemblyCode interfaceModelAssemblyCode;

    public string AstCodeAT = "";
    public string AstCodePP = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        interfaceModelAssemblyCode = (IModelAssemblyCode)ServiceAgent.getInstance().GetMaintainObjectByName<IModelAssemblyCode>(WebConstant.IModelAssemblyCode);
        if (!this.IsPostBack)
        {
         
            initLabel();
            try 
            {
                BindAllModelAssemblyCodeData();
           }
            catch(FisException fe)
            {
                showErrorMessage(fe.mErrmsg);
                return;
            }
            catch(Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }


        }

    }
    private void BindAllModelAssemblyCodeData()
    {
        IList<ModelAssemblyCodeInfo> lstAstDef = interfaceModelAssemblyCode.GetAllModelAssemblyCodeInfo();
           bindTable2(lstAstDef, DEFAULT_ROWS);
           setColumnWidth();
    }
   
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        try
        {
            interfaceModelAssemblyCode.DelModelAssemblyCodeInfo(hidmodel.Value.ToUpper(), hidAssemblyCode.Value.ToUpper());
        }
        catch (FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
         }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            BindAllModelAssemblyCodeData();
            updatePanel2.Update();
            HideWaitAndSetH();
        }
    }
    private ModelAssemblyCodeInfo GetNewAstDefine(bool isAdd)
    {
         string user= Request["UserId"]??"";
       
        ModelAssemblyCodeInfo ast = new ModelAssemblyCodeInfo();
        ast.Model = TXTModel.Text.ToUpper().Trim();
        ast.AssemblyCode = TXTAssemblycode.Text.ToUpper().Trim();
        ast.Revision = TXTRev.Text.ToUpper().Trim();
        ast.SupplierCode = TXTSuppliercode.Text.ToUpper().Trim();
        ast.Remark = TXTRemark.Text.Trim();
        ast.Editor = user;
        ast.Udt = DateTime.Now;
        if (isAdd)
        {
            ast.Cdt = DateTime.Now;
        }
        return ast;
    }
    private void HideWaitAndSetH()
    {
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "SetGrDivHeight();HideWait();Reset();", true);
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        try
        {
            ModelAssemblyCodeInfo ast = GetNewAstDefine(true);
            interfaceModelAssemblyCode.AddModelAssemblyCodeInfo(ast);
           
          
        }
        catch (FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
            BindAllModelAssemblyCodeData();
            updatePanel2.Update();
            HideWaitAndSetH();
        }
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        try
        {
            ModelAssemblyCodeInfo ast = GetNewAstDefine(false);
            interfaceModelAssemblyCode.UpdateModelAssemblyCodeInfo(ast, hidmodel.Value, hidAssemblyCode.Value);
             
       
        }
        catch (FisException fex)
        {
           showErrorMessage(fex.mErrmsg);
        }
        catch (System.Exception ex)
        {
           showErrorMessage(ex.Message);
        }
        finally
        {
            BindAllModelAssemblyCodeData();
            updatePanel2.Update();
            HideWaitAndSetH();
        }
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
            DateTime cdt;
            DateTime udt;
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[6].Text!="")
        {
           if( DateTime.TryParse(e.Row.Cells[6].Text,out cdt))
           {
               e.Row.Cells[6].Text=cdt.ToString("yyyy/MM/dd HH:mm");
           }
              if( DateTime.TryParse(e.Row.Cells[7].Text,out udt))
           {
               e.Row.Cells[7].Text=udt.ToString("yyyy/MM/dd HH:mm");
           
           }
        }

    }
    private void initLabel()
    {
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
    }

    protected void cmbCode_Selected(object sender, EventArgs e)
    {
       
    }

   
    private void bindTable2(IList<ModelAssemblyCodeInfo> list, int defaultRow)
    {

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Model");
        dt.Columns.Add("AssemblyCode");
        dt.Columns.Add("Revision");
        dt.Columns.Add("SupplierCode");
        dt.Columns.Add("Remark");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        if (list != null && list.Count != 0)
        {
            foreach (ModelAssemblyCodeInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.Model;
                dr[1] = temp.AssemblyCode;
                dr[2] = temp.Revision;
                dr[3] = temp.SupplierCode;
                dr[4] = temp.Remark;
                dr[5] = temp.Editor;
                dr[6] = temp.Cdt;
                dr[7] = temp.Udt;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Count.ToString();
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
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true);
    }


    private void bindTable(IList<AssetRangeDef> list, int defaultRow)
    {
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Parse("80");
        gd.HeaderRow.Cells[1].Width = Unit.Parse("120");
        gd.HeaderRow.Cells[2].Width = Unit.Parse("120");
        gd.HeaderRow.Cells[3].Width = Unit.Parse("140");
        gd.HeaderRow.Cells[4].Width = Unit.Parse("140");
        gd.HeaderRow.Cells[5].Width = Unit.Parse("140");
        gd.HeaderRow.Cells[6].Width = Unit.Parse("100");
        gd.HeaderRow.Cells[7].Width = Unit.Parse("100");      
    }

   

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("SetGrDivHeight();ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        return errorMsg;
    }
  


  
}

