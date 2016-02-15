using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Runtime.InteropServices;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using com.inventec.iMESWEB;
using IMES.Query.Interface.QueryIntf;


/// <summary>
/// Tool 的摘要描述
/// </summary>
 public class  ToolUtility
 {
     private static string _onLinesConnString;
     private static string _hisLinesConnString;
     private static IConfigDB iConfigDB = ServiceAgent.getInstance().GetObjectByName<IConfigDB>(WebConstant.ConfigDB);
     private static DBInfo objDbInfo = iConfigDB.GetDBInfo();

     public string GetTipString(string descr)
     {
         string s = @"Tip('{0}',SHADOW, true, SHADOWCOLOR, '#dd99aa',
                            FONTSIZE, '12pt',SHADOWWIDTH, 2,OFFSETY,-40,OFFSETX,-25,FADEIN,300)";
         s = string.Format(s, descr);
         return s;
     }
     
     public void ExportExcel(GridView gv,string FileName,Page pp )
    {
      //GridView gv = (GridView)Page.Form.FindControl(gvc.Replace('_', '$')); //gvResult     

        pp.Response.Clear();
        pp.Response.Buffer = true;
        pp.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
        pp.Response.Charset = "";
         pp.Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gv.AllowPaging = false;
        //gv.DataBind();
        //Change the Header Row back to white color     
        gv.HeaderRow.Style.Add("background-color", "green");
        //Apply style to Individual Cells     
        /*gv.HeaderRow.Cells[0].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[1].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[2].Style.Add("background-color", "green");
        gv.HeaderRow.Cells[3].Style.Add("background-color", "green");
         */

        /*for (int i = 0; i <= gv.Rows.Count-1; i++)
        {
            GridViewRow row = gv.Rows[i];
            //Change Color back to white         
            row.BackColor = System.Drawing.Color.White;
            //Apply text style to each Row         
            row.Attributes.Add("class", "textmode");
            //Apply style to Individual Cells of Alternating Row         
            if (i % 2 != 0)
            {
                row.Cells[0].Style.Add("background-color", "#C2D69B");
                row.Cells[1].Style.Add("background-color", "#C2D69B");
                row.Cells[2].Style.Add("background-color", "#C2D69B");
                row.Cells[3].Style.Add("background-color", "#C2D69B");
            }
        }*/
        gv.RenderControl(hw);
        //style to format numbers to string     
        //string style = @"&lt;style&gt; .textmode { mso-number-format:\@; } &lt;/style&gt;";
        //Response.Write(style);
        pp.Response.Output.Write(sw.ToString());
        pp.Response.Flush();
        pp.Response.End();
    }

     public string GetStationDescr(DataTable dt, string station)
     {
         string descr = "";
         foreach (DataRow dr in dt.Rows)
         {
             if (dr["Station"].ToString().Trim() == station)
             {
                 descr = dr["Descr"].ToString().Trim();
                 return descr;
             }
         }
         return descr;
     }
     public bool DataTableToXmlString(DataTable dataInfo, ref string dataString)
     {
         XmlSerializer xmlSer = new XmlSerializer(typeof(DataTable));
         Encoding encode = new System.Text.UTF8Encoding();
         System.IO.MemoryStream ms = new System.IO.MemoryStream();
         XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);

         xmlSer.Serialize(xtw, dataInfo);
         dataString = Encoding.UTF8.GetString(ms.ToArray()).Trim();

         xtw.Close();
         ms.Close();
         return true;
     }
     public bool XmlStringToDataTable(string dataString, ref DataTable dataInfo)
     {
         XmlSerializer ser = new XmlSerializer(typeof(DataTable));
         StringReader sr = new StringReader(dataString);
         dataInfo = (DataTable)ser.Deserialize(sr);
         sr.Close();
         return true;
     }
     
     private static byte[] GetFileByteData(String fileName)
     {

         FileStream fs = new FileStream(fileName, FileMode.Open);
         BinaryReader br = new BinaryReader(fs);

         byte[] testArray = new byte[fs.Length];
         int count = br.Read(testArray, 0, testArray.Length);

         br.Close();

         return testArray;
     }
   
     public static string GetDBConnection(string DBType , string DBName){
        string DBConnection = "";        
        _onLinesConnString = objDbInfo.OnLineConnectionString;
        _hisLinesConnString = objDbInfo.HistoryConnectionString;

        if (DBType == "OnlineDB")
        {
            DBConnection = string.Format(_onLinesConnString, DBName);
            //connString = string.Format(objDbInfo.OnLineConnectionString, dbName);
        }
        else
        {
            DBConnection = string.Format(_hisLinesConnString, DBName);
        }
        return DBConnection;

     }

}
