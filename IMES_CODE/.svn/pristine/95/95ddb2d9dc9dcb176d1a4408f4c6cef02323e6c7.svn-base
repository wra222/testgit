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

public partial class DataMaintain_SetPartAttribute2 : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 20;
    IPartManager ipart = (IPartManager)ServiceAgent.getInstance().GetMaintainObjectByName<IPartManager>(WebConstant.IPartManager);
    private IPartManagerEx iPartEx = (IPartManagerEx)ServiceAgent.getInstance().GetMaintainObjectByName<IPartManagerEx>(WebConstant.IPartManagerEx);
  
    Boolean isCustomerLoad;
    Boolean isFamilyLoad;
    private string partNo;
    private string partType;
    private const int COL_NUM = 6;
    private string username;
    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {

                pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
                pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
                pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
                pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
                pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
                pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
                pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
                partNo = Request.QueryString["partNo"];
                this.hidPartNo.Value = partNo;
                partType = Request.QueryString["nodeType"].ToString();
                username = Request.QueryString["username"].ToString();
              //  username = "IEC000043";
                this.HiddenUsername.Value = username;
                this.lblPartNoValue.Text = partNo;
                this.lblNodeTypeValue.Text = partType;
                initLabel();
                showListByPartNoAndType(partNo, partType);
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }

    }

    #region 数据的增删改查
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        PartInfoMaintainInfo pinfoEnt = new PartInfoMaintainInfo();
        string infoValue = this.hidInfoValue.Value.Trim().ToUpper();
        string infoType = this.hidInfoType.Value.Trim();
        string pn = this.hidPartNo.Value.ToString();
        pinfoEnt.PartNo = pn;
        pinfoEnt.InfoType = infoType;
        pinfoEnt.InfoValue = infoValue;
        pinfoEnt.Editor = this.HiddenUsername.Value;
        pinfoEnt.Cdt = DateTime.Now;
        pinfoEnt.Udt = DateTime.Now;
        iPartEx.AddPartInfoForVendorCode(pinfoEnt);
   //     itemName = replaceSpecialChart(itemName);
        showListByPartNoAndType(pn, "");
        this.UpdatePanel1.Update();
    }
    protected void btnDelete_ServerClick(object sender, EventArgs e)
    {
        string pn = this.hidPartNo.Value.ToString();
        iPartEx.DeletePartInfoByID(int.Parse(hidID.Value), pn);
        showListByPartNoAndType(pn, "");
        this.UpdatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "setNewItemValue()", true);
    }
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        PartInfoMaintainInfo pinfoEnt = new PartInfoMaintainInfo();
        string infoValue = this.hidInfoValue.Value.Trim();
        string infoType = this.hidInfoType.Value.Trim();
        string pn = this.hidPartNo.Value.ToString();
        string oldInfoValue = this.hidContent.Value.ToString();
        pinfoEnt.PartNo = pn;
        pinfoEnt.InfoType = infoType;
        pinfoEnt.InfoValue = infoValue;
        pinfoEnt.Editor = this.HiddenUsername.Value;
 
        pinfoEnt.Udt = DateTime.Now;
        if (hidCdt.Value == "" && hidUdt.Value == "")
        { iPartEx.AddPartInfoForVendorCode(pinfoEnt); }
        else
        { ipart.updatePartInfo(pinfoEnt, pn, infoType, oldInfoValue); }

        
     
         showListByPartNoAndType(pn, "");
          this.UpdatePanel1.Update();
          ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "setNewItemValue()", true);
        //添加partInfo数据
        //IList<PartInfoMaintainInfo> pinfo = new List<PartInfoMaintainInfo>();
        //PartInfoMaintainInfo pinfoEnt = new PartInfoMaintainInfo();
        //string dContent = this.txtInfoValue.Text;
        //string oldInfoValue = this.hidContent.Value.ToString();
        //string itemName = this.hidInfoValue.Value.ToString();
        //string pn = this.hidPartNo.Value.ToString();
        //pinfo = ipart.getLstPartInfo(pn, itemName, oldInfoValue);
        //if (pinfo != null && pinfo.Count > 0)
        //{
        //    pinfoEnt = pinfo.First();
        //}
        //if (dContent != "")
        //{
        //    pinfoEnt.PartNo = pn;
        //    pinfoEnt.InfoType = itemName;
        //    pinfoEnt.InfoValue = dContent;
        //    pinfoEnt.Editor = this.HiddenUsername.Value;
        //    pinfoEnt.Udt = DateTime.Now;
        //    try
        //    {
        //        if (oldInfoValue != "")
        //        {
        //            //修改数据
        //            ipart.updatePartInfo(pinfoEnt, pn, itemName, oldInfoValue);
        //        }
        //        else
        //        {
        //            //添加数据
        //            pinfoEnt.Cdt = DateTime.Now;
        //            ipart.addPartInfo(pinfoEnt);
        //        }
        //    }
        //    catch (FisException ex)
        //    {

        //        showErrorMessage(ex.mErrmsg);
        //        return;

        //    }
        //    catch (Exception ex)
        //    {
        //        showErrorMessage(ex.Message);
        //        return;
        //    }
        //    itemName = replaceSpecialChart(itemName);
        //    showListByPartNoAndType(pn, itemName);
        //    this.UpdatePanel1.Update();
        //    //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "AddUpdateComplete('" + itemName + "');", true);
        //}
        //else
        //{
        //    //删除数据
        //    ipart.deletePartInfo(itemName);
        //    showListByPartNoAndType(pn, itemName);
        //    this.UpdatePanel1.Update();
        //    //ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "resetTableHeight();", true);
        //}




    }
    #endregion


    #region gridview相关
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[5].Style["display"] = "none";
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


    private void bindTable(IList<PartInfoMaintainInfo> list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("Name");
        dt.Columns.Add("Value");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());
        dt.Columns.Add("ID");


        if (list != null && list.Count != 0)
        {
            foreach (PartInfoMaintainInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = Null2String(temp.InfoType);
                dr[1] = Null2String(temp.InfoValue);
                dr[2] = Null2String(temp.Editor);
              
                string cdt = ((System.DateTime)temp.Cdt).ToString("yyyy-MM-dd HH:mm");
                string udt = ((System.DateTime)temp.Udt).ToString("yyyy-MM-dd HH:mm");
                dr[3] = (cdt.Equals("1900-01-01 00:00")) ? "" : cdt;
                dr[4] = (udt.Equals("1900-01-01 00:00")) ? "" : udt;
                dr[5] = temp.Id.ToString();
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
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true);
    }

    private void showListByPartNoAndType(string partno, string parttype)
    {
     //   IList<PartTypeAndPartInfoValue> partInfoType = new List<PartTypeAndPartInfoValue>();
        IList<PartInfoMaintainInfo> partInfoType = iPartEx.GetPartInfoListByPartNo(partno);
        
    //    partInfoType = ipart.GetPartTypeAndPartInfoValueListByPartNo(partno, parttype);
        try
        {
            if (partInfoType != null)
            {
                bindTable(partInfoType, DEFAULT_ROWS);
                setColumnWidth();
                this.UpdatePanel1.Update();
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true);
            }
            else
            {
                bindTable(null, DEFAULT_ROWS);
                this.UpdatePanel1.Update();
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true);

            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }

    }

    private void initLabel()
    {
        this.lblPartNo.Text = this.GetLocalResourceObject(Pre + "_lblPartNoText").ToString();
        this.lblNodeType.Text = this.GetLocalResourceObject(Pre + "_lblNodeTypeText").ToString();
        this.lblAttributeList.Text = this.GetLocalResourceObject(Pre + "_lblAttributeListText").ToString();
        this.lblItemName.Text = this.GetLocalResourceObject(Pre + "_lblItemNameText").ToString();
        this.lblNodeType.Text = this.GetLocalResourceObject(Pre + "_lblPartNodeTypeText").ToString();
        this.btnClose.Value = this.GetLocalResourceObject(Pre + "_btnClose").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();

    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(15);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(22);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(22);
       // gd.HeaderRow.Cells[5].Width = Unit.Percentage(22);

    }
    #endregion


    #region 系统相关
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    #endregion

}
