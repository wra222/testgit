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
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;


public partial class DataMaintain_Battery : IMESBasePage 
{

    public String userName;
    
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 36;
    private IBattery iBattery;
    private const int COL_NUM = 5;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iBattery = (IBattery)ServiceAgent.getInstance().GetMaintainObjectByName<IBattery>(WebConstant.MaintainBatteryObject);

            if (!this.IsPostBack)
            {
                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();

                userName = Master.userInfo.UserId; //UserInfo.UserName;
                initLabel();
                IList<BatteryDef> dataList;
                dataList = iBattery.GetAllBatteryInfoList();
                bindTable(dataList, DEFAULT_ROWS);
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


    protected void btnBatteryChange_ServerClick(Object sender, EventArgs e)
    {
        string batteryVC = this.dBatteryVCTop.Text.Trim().ToUpper();
        ShowBatteryListByVC();
        string familyId = replaceSpecialChart(batteryVC);
        this.updatePanel1.Update();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "DealHideWait();findBattery('" + batteryVC + "');", true);
    }


    private Boolean ShowBatteryListByVC()
    {
        String batteryVC = this.dBatteryVCTop.Text.Trim().ToUpper();
        try
        {
            IList<BatteryDef> dataList;
            if (batteryVC != null && !batteryVC.Equals(""))
            {
                dataList = iBattery.GetBatteryInfoList(batteryVC);
            }else{
                 dataList = iBattery.GetAllBatteryInfoList();
            }
            if (dataList == null || dataList.Count == 0)
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


    private void ShowMessage(string p)
    {
        throw new NotImplementedException(p);
    }

    private void initLabel()
    {        
        this.lblList.Text = this.GetLocalResourceObject(Pre + "_lblListText").ToString();
        this.lblBatteryVCTop.Text = this.GetLocalResourceObject(Pre + "_lblBatteryVCTopText").ToString();
        this.lblBatteryVC.Text = this.GetLocalResourceObject(Pre + "_lblBatteryVCText").ToString();
        this.lblHssn.Text = this.GetLocalResourceObject(Pre + "_lblHssnText").ToString();


        this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(20);  //.Pixel(240);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(31);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(17);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(17);
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        BatteryDef item = new BatteryDef();
       
        string batteryVC = this.dBatteryVC.Text.Trim().ToUpper();
        string hssn = this.dHssn.Text.Trim().ToUpper();
        item.BatteryVC = batteryVC;
        item.Hssn = hssn;
        item.Editor = this.HiddenUserName.Value;


        string oldBatteryVC = this.dOldBatteryVC.Value.Trim().ToUpper();
        try
        {
            iBattery.UpdateBattery(item, oldBatteryVC);
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
        ShowBatteryListByVC();
        batteryVC = replaceSpecialChart(batteryVC);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + batteryVC + "');", true);
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        BatteryDef item = new BatteryDef();

        string batteryVC = this.dBatteryVC.Text.Trim().ToUpper();
        string hssn = this.dHssn.Text.Trim().ToUpper();
        item.BatteryVC = batteryVC;
        item.Hssn = hssn;
        item.Editor = this.HiddenUserName.Value;
        
        try
        {
            iBattery.AddBattery(item);
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
        ShowBatteryListByVC();
        batteryVC = replaceSpecialChart(batteryVC);
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "DealHideWait();AddUpdateComplete('" + batteryVC + "');", true);
    }


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        string oldBatteryVC = this.dOldBatteryVC.Value.Trim();
        try
        {
            BatteryDef item = new BatteryDef();
            item.BatteryVC = oldBatteryVC;
            item.Hssn = "";
            item.Editor = "";            
            iBattery.DeleteBattery(item);
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
        ShowBatteryListByVC();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "saveUpdate", "DealHideWait();DeleteComplete();", true);

    }

    private void bindTable(IList<BatteryDef> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_batteryVCText").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_hssnText").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        if (list != null && list.Count != 0)
        {
            foreach (BatteryDef temp in list)
            {
                dr = dt.NewRow();

                dr[0] = temp.BatteryVC;
                dr[1] = temp.Hssn;
                dr[2] = temp.Editor;
                dr[3] = temp.cdt;
                dr[4] = temp.udt;
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();iSelectedRowIndex = null;", true); 

    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');DealHideWait();");
        scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
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
