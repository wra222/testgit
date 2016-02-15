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
//using IMES.FisObject.PCA.EcrVersion;

public partial class DataMaintain_FRUMBVersionMaintain : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IFRUMBVersion iFRUMBVersion;
    private const int COL_NUM = 8;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;

    protected void Page_Load(object sender, EventArgs e)
    {
        iFRUMBVersion = (IFRUMBVersion)ServiceAgent.getInstance().GetMaintainObjectByName<IFRUMBVersion>(WebConstant.MaintainFRUMBVersionManagerObject);
        userName = Master.userInfo.UserId;
        if (!this.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
      //      pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
      
            //need change..
          
            this.HiddenUserName.Value = userName;
            //load data
            initLabel();
            initSelect();
            //find all AC Adaptor...
            //...
            IList<FruMBVerInfo> datalst = null;
            try 
            {
                string code = this.cmbCodeSelect.SelectedValue.ToString().Trim();
               
               datalst = iFRUMBVersion.GetFruMBVer(code);
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
            bindTable(datalst, DEFAULT_ROWS);

        }

    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(this.dOldId.Value.Trim());
            iFRUMBVersion.RemoveFruMBVer(id);
      //      iFRUMBVersion.DeleteAssetRangeItem(id);
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
        //按照ac adaptor list加载表格中的数据.
        initCodeTopSelect("");
    //    showListByAssetRangeList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }

   

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {

        string id = "";
      FruMBVerInfo info = new FruMBVerInfo()  {
                editor = this.HiddenUserName.Value.Trim(),
                partNo = this.ttPartNo.Text.Trim(),
                remark = this.ttRemark.Text.Trim(),
                ver = this.ttVer.Text.Trim(),
                mbCode = this.ttMBCode.Text.Trim(),
                cdt = DateTime.Now,
                udt = DateTime.Now,
            };
        try
        {
           
          
            iFRUMBVersion.InsertFruMBVer(info);
           
            
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
        //按照ac adaptor list加载表格中的数据
        //...
        initCodeTopSelect(info.partNo);
        showListByAssetRangeList();
        this.updatePanel2.Update();
        //    string assemblyId = replaceSpecialChart(adaptor.assemb);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
       
        string id = this.dOldId.Value.Trim();
        FruMBVerInfo info = new FruMBVerInfo()
        {
            editor = this.HiddenUserName.Value.Trim(),
            partNo = this.ttPartNo.Text.Trim(),
            remark = this.ttRemark.Text.Trim(),
            ver = this.ttVer.Text.Trim(),
            mbCode = this.ttMBCode.Text.Trim(),
            cdt = DateTime.Now,
            udt = DateTime.Now,
            id = Convert.ToInt32(this.dOldId.Value.Trim())
        };
        try
        {
         
            iFRUMBVersion.UpdateFruMBVer(info);
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
        //根据ac acdaptor list的数据加载表格中的数据
        //...
        initCodeTopSelect(info.partNo);
         showListByAssetRangeList();
        updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + -1 + "');HideWait();", true);
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
         e.Row.Cells[7].Attributes.Add("style", e.Row.Cells[6].Attributes["style"] + "display:none");
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
    private void initLabel()
    {
        //this.lblAssetRangeList.Text = this.GetLocalResourceObject(Pre + "_lblAssetRangeList").ToString();
        this.lblPartNo.Text = "Part No:";
        //this.lblMBCode.Text = this.GetLocalResourceObject(Pre + "_lblMBCode").ToString();
        //this.lblVer.Text = this.GetLocalResourceObject(Pre + "_lblVer").ToString();
        //this.lblRemark.Text = this.GetLocalResourceObject(Pre + "_lblRemark").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
       this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
    }

    private void initSelect()
    {
        initCodeTopSelect("");
    }

    private void initCodeTopSelect(string SelectValue)
    {
        IList<string> lstPartNo = iFRUMBVersion.GetPartNoInFruMBVer();
        this.cmbCodeSelect.Items.Clear();
        if (lstPartNo.Count != 0)
        {
            foreach (string item in lstPartNo)
            {
                this.cmbCodeSelect.Items.Add(item.Trim());
            }
            if (SelectValue == "")
            {
                IList<FruMBVerInfo> datalst = null;
                this.cmbCodeSelect.SelectedIndex = 0;
                string code = this.cmbCodeSelect.SelectedValue.ToString().Trim();
                datalst = iFRUMBVersion.GetFruMBVer(code);
                bindTable(datalst, DEFAULT_ROWS);
            }
            else
            {
                this.cmbCodeSelect.SelectedValue = SelectValue;
            }

        }
        this.updatePanel1.Update();
        this.updatePanel2.Update();
    }

    protected void cmbCode_Selected(object sender, EventArgs e)
    {
        IList<FruMBVerInfo> datalst = null;
        try
        {
            string Code = this.cmbCodeSelect.SelectedValue.ToString().Trim();
            datalst = iFRUMBVersion.GetFruMBVer(Code);
            bindTable(datalst, DEFAULT_ROWS);
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
    }

    protected void Code_Input(object sender, EventArgs e)
    {
        IList<FruMBVerInfo> datalst = null;
        try
        {
            string Code = this.hidCodeInput.Value.ToString().Trim();
            datalst = iFRUMBVersion.GetFruMBVer(Code);
            if (datalst.Count != 0)
            {
                bindTable(datalst, DEFAULT_ROWS);
                initCodeTopSelect(Code);
            }
            else 
            {
                initCodeTopSelect("");
                this.updatePanel1.Update();
                this.updatePanel2.Update();
                showErrorMessage("Cant find that match Code...");
              

            }
        }
        catch (FisException ee)
        {
            showErrorMessage(ee.mErrmsg);
            bindTable(null, DEFAULT_ROWS);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            bindTable(null, DEFAULT_ROWS);
        }
    }

    private void bindTable(IList<FruMBVerInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add("PartNo");
        dt.Columns.Add("MBCode");
        dt.Columns.Add("Ver");
        dt.Columns.Add("Remark");
    
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        dt.Columns.Add("ID");
        if (list != null && list.Count != 0)
        {
            foreach (FruMBVerInfo temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.partNo;
                dr[1] = temp.mbCode;
                dr[2] = temp.ver;
                dr[3] = temp.remark;
                dr[4] = temp.editor;
                dr[5] = temp.cdt;
                dr[6] = temp.udt;
                dr[7] = temp.id;
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
        gd.GvExtHeight = dTableHeight.Value;
        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true);
    }

    private void setColumnWidth()
    {

        gd.HeaderRow.Cells[3].Width = Unit.Percentage(18);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(18);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(18);

    }

   

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');HideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return errorMsg;
    }
    private Boolean showListByAssetRangeList()
    {
    //    string acadaptorlst = this.ttAcAdaptorList.Text.Trim();
        IList<FruMBVerInfo> dataLst = null;
        try
        {
       
            string Code = this.hidCode.Value.ToString();
         
            dataLst = iFRUMBVersion.GetFruMBVer(Code);
          

            if (dataLst == null || dataLst.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataLst, DEFAULT_ROWS);
            }
        }
        catch (FisException fex)
        {
           
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
            return false;
        }
        catch (System.Exception ex)
        {

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }

   
}
