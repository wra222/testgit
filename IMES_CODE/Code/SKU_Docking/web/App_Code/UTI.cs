using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IMES.Station.Interface.StationIntf;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using IMES.DataModel;

/// <summary>
/// Summary description for UTI
/// </summary>
public class UTI
{   
	public UTI()
    {
     
	}
    static public void WriteToAlertMessage(string msg,Control con)
    {  
        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + msg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + msg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(con, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    
    }
    static public void BindNullBomTable(int defaultRow,GridView gd)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
  
        //Part Type Description Part No/Item Name Qty PQty Collection Data PreCheck 
        dt.Columns.Add("Part Type");
        dt.Columns.Add("Description");
        dt.Columns.Add("Part No/Item Name");
        dt.Columns.Add("Qty");
        dt.Columns.Add("PQty") ;
        dt.Columns.Add("Collection Data");
        dt.Columns.Add("PreCheck");
        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }
        gd.DataSource = dt;
        
        gd.DataBind();
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(5);

    }
    static public void BindNullTable(int defaultRow, GridView gd, List<string> headerList)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        foreach (string s in headerList)
        {
            dt.Columns.Add(s);
        }
        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }
        gd.DataSource = dt;
        gd.DataBind();
    
    }
	
	static public MpUserInfo ConvertMpUserToObj(object obj)
    {
        Dictionary<string, object> dic = (Dictionary<string, object>)obj;
        MpUserInfo m = new MpUserInfo
        {
            UserId = (string)dic["UserId"],
            UserName = (string)dic["UserName"],
            Customer = (string)dic["Customer"],
            AccountId = (string)dic["AccountId"],
            Login = (string)dic["Login"],
            Station = (string)dic["Station"],
            SessionId=(string)dic["SessionId"],
            PCode = (string)dic["PCode"]
        };
        return m;

    }
    
}
