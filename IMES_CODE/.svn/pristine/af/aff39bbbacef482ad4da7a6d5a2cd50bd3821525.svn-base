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
using System;
using com.inventec.iMESWEB;
//using IMES.Station.Interface.StationIntf;
//using IMES.Station.Interface.CommonIntf;
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;


public partial class DataMaintain_ACAdaptor : IMESBasePage
{
   
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 32;
    private IACAdaptor iACAdaptor;
    private const int COL_NUM = 10;

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
        iACAdaptor = (IACAdaptor)ServiceAgent.getInstance().GetMaintainObjectByName<IACAdaptor>(WebConstant.ACADAPTORMAITAIN);
        if (!this.IsPostBack)
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre+"_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre+"_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            //need change..
            userName = Master.userInfo.UserId;
            this.HiddenUserName.Value = userName;
            //load data
            initLabel();
            //find all AC Adaptor...
            //...
            IList<IMES.DataModel.ACAdaptor> datalst=null;
            try 
            {
                datalst=iACAdaptor.GetAllAdaptorInfo();
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
        string oldAssembly = this.dOldAssemblyId.Value.Trim();
        try 
        {
            ACAdaptor adaptor = new ACAdaptor();
            adaptor.id = Convert.ToInt32(this.dOldId.Value.Trim());
            //调用删除方法.
            iACAdaptor.DeleteOneAcAdaptor(adaptor);
        }
        catch(FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(System.Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        //按照ac adaptor list加载表格中的数据.
        //...
        showListByACAdaptorList();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();DeleteComplete();HideWait();", true);
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        ACAdaptor adaptor = new ACAdaptor();
        adaptor.assemb = this.ttAssembly.Text.ToUpper().Trim();//需要转化为upperCase...
        adaptor.adppn = this.ttadppn.Text.Trim();
        adaptor.agency = this.ttAgency.Text.ToUpper().Trim();//需要转化为UpperCase...
        adaptor.supplier = this.ttSupplier.Text.Trim();
        adaptor.voltage = this.ttVoltage.Text.Trim();
        adaptor.cur = this.ttCurrent.Text.Trim();
        adaptor.editor = this.HiddenUserName.Value;

        System.DateTime cdt = DateTime.Now;;
        string timeStr = cdt.ToString();
        adaptor.cdt = cdt;
        string id = "";
        try 
        {
            
            //调用添加的方法 相同的key时需要抛出异常...
            id=iACAdaptor.AddOneAcAdaptor(adaptor);
        }
        catch(FisException fex)
        {
           
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(System.Exception ex)
        {
            
            showErrorMessage(ex.Message);
            return;
        }
        //按照ac adaptor list加载表格中的数据
        //...
        showListByACAdaptorList();
        this.updatePanel2.Update();
    //    string assemblyId = replaceSpecialChart(adaptor.assemb);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        ACAdaptor adaptor = new ACAdaptor();
        string assembly = this.ttAssembly.Text.ToUpper().Trim();
    //    string oldassembly = this.dOldAssemblyId.Value.Trim();
    //    string oldCdt = this.dOldAssemblyCdt.Value.Trim();
        //ACAdaptor oldAC = new ACAdaptor();
        //oldAC.assemb = oldassembly;
        //oldAC.cdt = Convert.ToDateTime(oldCdt);
        adaptor.assemb = assembly;
        adaptor.adppn = this.ttadppn.Text.Trim();
        adaptor.agency = this.ttAgency.Text.ToUpper().Trim();
        adaptor.supplier = this.ttSupplier.Text.Trim();
        adaptor.voltage = this.ttVoltage.Text.Trim();
        adaptor.cur = this.ttCurrent.Text.Trim();
        adaptor.editor = this.HiddenUserName.Value.Trim();
        adaptor.id = Convert.ToInt32(this.dOldId.Value.Trim());
        string id = this.dOldId.Value.Trim();
        try 
        {
                //调用更新方法1... 相同key时需要抛出异常...
            iACAdaptor.UpdateOneAcAdaptor(adaptor);

        }
        catch(FisException fex)
        {
            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch(System.Exception ex)
        {
            //IList<IMES.DataModel.ACAdaptor> datalst = iACAdaptor.GetAllAdaptorInfo();
            //bindTable(datalst, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return;
        }
        //根据ac acdaptor list的数据加载表格中的数据
        //...
        showListByACAdaptorList();
        this.updatePanel2.Update();
   //     string currentAssmebly = replaceSpecialChart(assembly);
        ScriptManager.RegisterStartupScript(this.updatePanel, typeof(System.Object), "saveUpdate", "resetTableHeight();AddUpdateComplete('" + id + "');HideWait();", true);
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[9].Attributes.Add("style", e.Row.Cells[9].Attributes["style"] + "display:none");
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
        this.lblAcAdaptorList.Text = this.GetLocalResourceObject(Pre + "_lblAcDaptor").ToString();
        this.lblAssembly.Text = this.GetLocalResourceObject(Pre + "_lblAssembly").ToString();
        this.lblAdppn.Text = this.GetLocalResourceObject(Pre + "_lblAdpPn").ToString();
        this.lblAgency.Text = this.GetLocalResourceObject(Pre + "_lblAgency").ToString();
        this.lblSupplier.Text = this.GetLocalResourceObject(Pre + "_lblSupplier").ToString();
        this.lblVoltage.Text = this.GetLocalResourceObject(Pre + "_lblVoltage").ToString();
        this.lblCurrent.Text = this.GetLocalResourceObject(Pre + "_lblCurrent").ToString();
        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre +"_btnSave").ToString();
        this.btnDel.Value = this.GetLocalResourceObject(Pre + "_btnDel").ToString();
        
    }
    private void bindTable(IList<ACAdaptor> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        //dt.Columns.Add(" ");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblAssembly").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblHPQNo").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblAgencySeries").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblSupplier").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblVoltageRating").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCurrentRating").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_tblUdt").ToString());
        dt.Columns.Add("id");
        if (list != null && list.Count != 0)
        {
            foreach (ACAdaptor temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.assemb;
                dr[1] = temp.adppn;
                dr[2] = temp.agency;
                dr[3] = temp.supplier;
                dr[4] = temp.voltage;
                dr[5] = temp.cur;
                dr[6] = temp.editor;
                dr[7] = temp.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[8] = temp.udt.ToString("yyyy-MM-dd HH:mm:ss");
                dr[9] = temp.id.ToString();
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
        
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;resetTableHeight();HideWait();", true); 
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(12);
        gd.HeaderRow.Cells[6].Width = Unit.Percentage(8);
        gd.HeaderRow.Cells[7].Width = Unit.Percentage(14);
        gd.HeaderRow.Cells[8].Width = Unit.Percentage(14);
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
    private Boolean showListByACAdaptorList()
    {
        string acadaptorlst = this.ttAcAdaptorList.Text.Trim();
        IList<ACAdaptor> adaptorLst = null;
        try
        {
            //if (acadaptorlst == "")
            {
                adaptorLst = iACAdaptor.GetAllAdaptorInfo();
            }
            //else 
            //{
            //    adaptorLst = iACAdaptor.GetAdaptorByAssembly(acadaptorlst);
            //}
            
            if(adaptorLst==null||adaptorLst.Count==0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(adaptorLst, DEFAULT_ROWS);
            }
        }
        catch(FisException fex)
        {
           
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(fex.mErrmsg);
            return false;
        }
        catch(System.Exception ex)
        {

            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;
    }

    
}
