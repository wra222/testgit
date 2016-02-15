/*
 INVENTEC corporation (c)2011 all rights reserved. 
 Description: Unpack All by SN(Multi)
 Update: 
 Date        Name                 Reason 
 ==========  ==================   ==============================
 2013-04-16 Benson           Create 
 Known issues:
 */
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
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using System.Collections.Generic;
using log4net;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using IMES.DataModel;
using System.Text;


public partial class PAK_UnpackDN_Multi : IMESBasePage
{

    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    public const int DEFAULT_ROWS = 15;
    public const int maxSnCount=100;
    public string station;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            station = Request["Station"] ?? "";
          
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            int t = DEFAULT_ROWS + 1;
            hidDefaultCount.Value = t.ToString();
            if (!this.IsPostBack)
            {
                hidCustsnList.Value = "";
                initLabel();
                InitSnTable();
            
            }
        }
        catch (FisException ex)
        {
            writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        } 
    }
    private void InitSnTable()
    {


        DataTable retTable = new DataTable();
        retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("Result", Type.GetType("System.String"));
        retTable.Columns.Add("Error Message", Type.GetType("System.String"));

        grvSN.Columns[0].HeaderText = "CUSTSN";
        grvSN.Columns[1].HeaderText = "Result";
        grvSN.Columns[2].HeaderText = "Error Message";
        DataRow newRow;
        for (int i = 0; i < DEFAULT_ROWS; i++)
        {
            newRow = retTable.NewRow();
            newRow[0] = String.Empty;
           newRow[1] = String.Empty;
          
            newRow[2] = String.Empty;
            retTable.Rows.Add(newRow);
        }
        grvSN.DataSource = retTable;
        grvSN.DataBind();
        IniGrvWidth();
    }
    private void IniGrvWidth()
    {
        this.grvSN.HeaderRow.Cells[0].Width = Unit.Percentage(10);
        this.grvSN.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        this.grvSN.HeaderRow.Cells[2].Width = Unit.Percentage(80);
   
    }
    private void initLabel()
    {
   //     this.lblDeliveryNo.Text = this.GetLocalResourceObject(Pre + "_lblDeliveryNo").ToString();
    }
      
    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
     }
    private void CallClientFun(string funcName)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine(funcName + ";");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "funcName", scriptBuilder.ToString(), false);
    }
   
    protected void grvSN_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.Cells[1].Text == "FAIL")
                {
                    e.Row.BackColor = System.Drawing.Color.Yellow;
                }
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            IMultiUnPack iMultiUnPack = ServiceAgent.getInstance().GetObjectByName<IMultiUnPack>(WebConstant.MultiUnPack);
            string[] arr = hidCustsnList.Value.Split(',');
            List<String> lst=  arr.ToList();
            DataTable dt = iMultiUnPack.InputSnList(lst, "", UserId, station, Customer);
            var dv = dt.DefaultView;
            dv.Sort = "Result";
            dt = dv.ToTable();
            var c = dt.Rows.Count;
            if (c < DEFAULT_ROWS)
            {
                DataRow newRow;
                for (int i = 0; i <(DEFAULT_ROWS-c); i++)
                {
                    newRow = dt.NewRow();
                    newRow[0] = String.Empty;
                    newRow[1] = String.Empty;

                    newRow[2] = String.Empty;
                    dt.Rows.Add(newRow);
                }
            }
            
           grvSN.DataSource = dt;
           grvSN.Columns[0].HeaderText = "CUSTSN";
           grvSN.Columns[1].HeaderText = "Result";
           grvSN.Columns[2].HeaderText = "Error Message";
           grvSN.DataBind();
           IniGrvWidth();
           hidCustsnList.Value = "";
 
        }
        catch (FisException ex)
        {
    
              writeToAlertMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            CallClientFun("endWaitingCoverDiv(); ShowInfo('Success','green');ResetSn()");
        }

    }
}
