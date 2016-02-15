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

public partial class StationInfoAttribute : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 18;
    //IFamilyInfoEx iFamilyInfo = ServiceAgent.getInstance().GetMaintainObjectByName<IFamilyInfoEx>(WebConstant.IFamilyInofoObjectEx);
    IStation2 iStation = ServiceAgent.getInstance().GetMaintainObjectByName<IStation2>(WebConstant.MaintainStationObject);
    private string editor;
    private IConstValueType iConstValueType;
    private string strStationName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iConstValueType = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueType>(WebConstant.ConstValueTypeObject);
            editor = Request.QueryString["editor"];
            strStationName = Request.QueryString["StationName"];
            this.valModel.Text = strStationName;
            this.Title = this.GetLocalResourceObject(Pre + "_title").ToString();
            
            InitLabel();
            initCodeType();
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
        gdModelInfoList.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        gdModelInfoList.HeaderRow.Cells[1].Width = Unit.Pixel(120);
        gdModelInfoList.HeaderRow.Cells[2].Width = Unit.Pixel(80);
        gdModelInfoList.HeaderRow.Cells[3].Width = Unit.Pixel(220);
        gdModelInfoList.HeaderRow.Cells[4].Width = Unit.Pixel(100);
        gdModelInfoList.HeaderRow.Cells[5].Width = Unit.Pixel(160);
        gdModelInfoList.HeaderRow.Cells[6].Width = Unit.Pixel(160);
    }

    protected void bindTable()
    {

        try
        {
            IList<StationAttrDef> Stationinfolist = iStation.GetStationAttr(strStationName);
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("Station");
            dt.Columns.Add("AttrName");
            dt.Columns.Add("AttrValue");
            dt.Columns.Add("Descr");
            dt.Columns.Add("Editor");
            dt.Columns.Add("Cdt");
            dt.Columns.Add("Udt");
            if (Stationinfolist != null && Stationinfolist.Count != 0)
            {
                foreach (StationAttrDef temp in Stationinfolist)
                {
                    dr = dt.NewRow();
                    dr[0] = temp.Station;
                    dr[1] = temp.AttrName;
                    dr[2] = temp.AttrValue;
                    dr[3] = temp.Descr;
                    dr[4] = temp.Editor;
                    if (temp.Cdt == DateTime.MinValue)
                    {
                        dr[5] = "";
                    }
                    else
                    {
                        dr[5] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (temp.Udt == DateTime.MinValue)
                    {
                        dr[6] = "";
                    }
                    else
                    {
                        dr[6] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    dt.Rows.Add(dr);
                }
                for (int i = Stationinfolist.Count; i < DEFAULT_ROWS; i++)
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
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btndel.Value = this.GetLocalResourceObject(Pre + "_btndel").ToString();
    }

    public void initCodeType()
    {
        IList<ConstValueTypeInfo> lstConstValueType = iConstValueType.GetConstValueTypeList("StationAttributeName");
        this.drpattrName.Items.Clear();
        this.drpattrName.Items.Add(string.Empty);
        ListItem item = null;
        if (lstConstValueType.Count != 0)
        {
            foreach (ConstValueTypeInfo temp in lstConstValueType)
            {
                item = new ListItem();
                item.Text = temp.value;
                item.Value = temp.value;
                this.drpattrName.Items.Add(item);
            }
            this.drpattrName.SelectedIndex = 0;
        }



        //if (iConstValueType != null)
        //{
        //    IList<ConstValueTypeInfo> lstConstValueType = null;

        //    lstConstValueType = iConstValueType.GetConstValueTypeList("StationAttributeName");

        //    if (lstConstValueType != null && lstConstValueType.Count != 0)
        //    {
        //        initControl(lstConstValueType);
        //    }
        //    else
        //    {
        //        initControl(null);
        //    }
        //}
        //else
        //{
        //    initControl(null);
        //}
    }

    //private void initControl(IList<ConstValueTypeInfo> lstConstValueType)
    //{
    //    ListItem item = null;

    //    this.drpattrName.Items.Clear();
    //    this.drpattrName.Items.Add(string.Empty);

    //    if (lstConstValueType != null)
    //    {
    //        foreach (ConstValueTypeInfo temp in lstConstValueType)
    //        {
    //            item = new ListItem();
    //            item.Text = temp.value;
    //            item.Value = temp.value;
    //            this.drpattrName.Items.Add(item);
    //        }
    //    }
    //}

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[0].Style.Add("display", "none");//id

        
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

    protected void btnDelete_Click(Object sender, EventArgs e)
    {
        string name = hidattrname.Value.Trim();
        try
        {
            iStation.DeleteStationAttr(strStationName, name);
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
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"" + "-1" + "\");", true);
    }


    protected void btnSave_Click(Object sender, EventArgs e)
    {
        string name = hidattrname.Value.Trim();
        string value = txtattrvalue.Text.Trim(); ;
        string descr = txtdesce.Text.Trim();
        StationAttrDef StationDef = new StationAttrDef();
        //FamilyInfoDef model = new FamilyInfoDef();
        StationDef.Station = strStationName;
        StationDef.AttrName = name;
        StationDef.AttrValue = value;
        StationDef.Descr = descr;
        StationDef.Editor = editor;
        

        try
        {

            String Check = iStation.GetStationAttrValue(strStationName, name);
            if (Check == "" || Check == null)
            {
                iStation.AddStationAttr(StationDef);
            }
            else
            {
                iStation.UpdateStationAttr(StationDef);
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
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"" + "-1" + "\");", true);
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
