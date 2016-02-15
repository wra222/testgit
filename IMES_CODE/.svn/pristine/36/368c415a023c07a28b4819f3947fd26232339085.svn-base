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

public partial class DataMaintain_AstDefine : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 15;
     private const int COL_NUM = 10;
    private IAstDefine iAstDefine;

    public string AstCodeAT = "";
    public string AstCodePP = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        iAstDefine = (IAstDefine)ServiceAgent.getInstance().GetMaintainObjectByName<IAstDefine>(WebConstant.ASTDEFINE);
        droNeedAssignAstSN.Attributes["onChange"] = "ChanegNeedAssignAstSN(this);";
        if (!this.IsPostBack)
        {
         
            initLabel();
            try 
            {
                BindAllAstData();
                BindAstCode();
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
    private void BindAllAstData()
    {
           IList<AstDefineInfo> lstAstDef =  iAstDefine.GetAllAstDefineInfo();
           bindTable2(lstAstDef, DEFAULT_ROWS);
           setColumnWidth();
    }
    private void BindAstCode()
    {
        droAstCode.Items.Add("");
		
		List<string> lst=  iAstDefine.GetAllAstCode("AT");
      foreach (string s in lst)
      {
          AstCodeAT += s + ",";
      }

      lst = iAstDefine.GetAllAstCode("PP");
      foreach (string s in lst)
      {
          AstCodePP += s + ",";
      }
   
      droAssignAstSNStation.Items.Add("");
      droCombineStation.Items.Add("");
      lst = iAstDefine.GetAssignAstStation();
      foreach (string s in lst)
      {
          droAssignAstSNStation.Items.Add(s);
      }
      lst = iAstDefine.GetCombineASTStation();
      foreach (string s in lst)
      {
          droCombineStation.Items.Add(s);
      }
	
    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        try
        {
            iAstDefine.DelAstDefineInfo(hidAstType.Value, hidAstCode.Value);
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
            BindAllAstData();
            updatePanel2.Update();
            HideWaitAndSetH();
        }
    }
    private AstDefineInfo GetNewAstDefine(bool isAdd)
    {
        AstDefineInfo ast = new AstDefineInfo();
        ast.AstType = droAstType.SelectedValue;
        ast.AstCode = hidSelectedAstCode.Value; //droAstCode.SelectedValue;
        ast.AstLocation = droAstLocation.SelectedValue;
        ast.NeedAssignAstSN = droNeedAssignAstSN.SelectedValue;
        ast.AssignAstSNStation = hidAssignAstSNStation.Value.Trim(); //droAssignAstSNStation.SelectedValue;
        ast.CombineStation = droCombineStation.SelectedValue;
        ast.HasCDSIAst = droHasCDSIAst.SelectedValue;
        ast.NeedPrint = droNeedPrint.SelectedValue;
        ast.NeedScanSN = droNeedScanSN.SelectedValue;
        ast.CheckUnique = droCheckUnique.SelectedValue;
        ast.HasUPSAst = dropHasUPSAst.SelectedValue;
        ast.NeedBindUPSPO = dropNeedBindUPSPO.SelectedValue;
        ast.comment = txtComment.Text;
        ast.Udt = DateTime.Now;
        if (isAdd)
        {  
          ast.Cdt = DateTime.Now;
        }
        string user= Request["UserId"]??"";
        ast.Editor = user;
        return ast;
    }
    private void HideWaitAndSetH()
    {
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "SetGrDivHeight();HideWait();ResetSelectValue();Reset();", true);
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        try
        {
            AstDefineInfo ast = GetNewAstDefine(true);
            iAstDefine.AddAstDefineInfo(ast);
          
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
            BindAllAstData();
            updatePanel2.Update();
            HideWaitAndSetH();
        }
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        try
        {
            AstDefineInfo ast = GetNewAstDefine(false);
             iAstDefine.UpdateAstDefineInfo(ast,hidAstType.Value, hidAstCode.Value);
       
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
            BindAllAstData();
            updatePanel2.Update();
            HideWaitAndSetH();
        }
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
            DateTime cdt;
            DateTime udt;
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[12].Text!="")
        {
           if( DateTime.TryParse(e.Row.Cells[14].Text,out cdt))
           {
               e.Row.Cells[14].Text=cdt.ToString("yyyy/MM/dd HH:mm");
           }
              if( DateTime.TryParse(e.Row.Cells[15].Text,out udt))
           {
               e.Row.Cells[15].Text=udt.ToString("yyyy/MM/dd HH:mm");
           
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

   
    private void bindTable2(IList<AstDefineInfo> list, int defaultRow)
    {

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("AstType");
        dt.Columns.Add("AstCode");
        dt.Columns.Add("AstLocation");
        dt.Columns.Add("NeedAssignAstSN");
        dt.Columns.Add("AssignAstSNStation");
        dt.Columns.Add("CombineStation");
        dt.Columns.Add("HasCDSIAst");
        dt.Columns.Add("NeedPrint");
        dt.Columns.Add("NeedScanSN");
        dt.Columns.Add("Comment");
		dt.Columns.Add("CheckUnique");
        dt.Columns.Add("HasUPSAst");
        dt.Columns.Add("NeedBindUPSPO");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        
        if (list != null && list.Count != 0)
        {
            foreach (AstDefineInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.AstType;
                dr[1] = temp.AstCode;
                dr[2] = temp.AstLocation;
                dr[3] = temp.NeedAssignAstSN;
                dr[4] = temp.AssignAstSNStation;
                dr[5] = temp.CombineStation;
                dr[6] = temp.HasCDSIAst;
                dr[7] = temp.NeedPrint;
                dr[8] = temp.NeedScanSN;
                dr[9] = temp.comment;
				dr[10] = temp.CheckUnique;
                dr[11] = temp.HasUPSAst;
                dr[12] = temp.NeedBindUPSPO;
                dr[13] = temp.Editor;
                dr[14] = temp.Cdt;
                dr[15] = temp.Udt;
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
        gd.HeaderRow.Cells[8].Width = Unit.Parse("100");
        gd.HeaderRow.Cells[9].Width = Unit.Parse("100");
		gd.HeaderRow.Cells[10].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[11].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[12].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[13].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[14].Width = Unit.Parse("130");
        gd.HeaderRow.Cells[15].Width = Unit.Parse("130");
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
