<%@ Page Language="C#" %>
<%@ Import Namespace="com.inventec.iMESWEB" %>
<%@ Import Namespace="IMES.Query.Interface.QueryIntf" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Script" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<head id="Head1" runat="server" />

<%        
    try
    {

      IPAK_WipTracking WipTracking = ServiceAgent.getInstance().GetObjectByName<IPAK_WipTracking>(WebConstant.PAK_WipTracking);
      IStation Station = ServiceAgent.getInstance().GetObjectByName<IStation>(WebConstant.Station);
      Dictionary<string, string> DicStationDescr;
        
      string shipDate = Request["shipDate"] ?? "";
      string model = Request["model"] ?? ""; ;
      string modelList = Request["modelList"] ?? ""; ;
      string line = Request["line"] ?? ""; ;
      string process = Request["process"] ?? ""; ;
      string type = Request["type"] ?? ""; ;
      string dbName = Request["dbName"] ?? "";

      string Connection = Request["connection"] ?? "";

      DicStationDescr = new Dictionary<string, string>();
      foreach (DataRow dr in Station.GetStation(Connection).Rows)
      {
          DicStationDescr.Add(dr["Station"].ToString().Trim(), dr["Descr"].ToString().Trim());
      }
        
        switch (Request["action"].ToString().ToUpper())
        {
            case "GETMAIN_WEBMETHOD":

                int headerCount = 8;
                int[] dnQty;
                string inputModel = "";
                if (model != "")
                { inputModel = model.Trim(); }
                else if (modelList.Trim() != "")
                {
                    inputModel = modelList.Trim();
                }


                DataTable dt2;
                dt2 = WipTracking.WipTrackingByDN_FA_Ex(Connection, shipDate, inputModel, line, process, type, dbName, out dnQty);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"<DIV  style='width: 98%; height: 300px;  overflow: auto;'>  
                                         <table id='mainTable'  value1='{0}' value2='{1}' 
                                        class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;table-layout:fixed;'>
                                       <tr class='iMes_grid_HeaderRowGvExt'>", dnQty[0].ToString(), dnQty[1].ToString());
                //  sb.AppendLine("<table id='mainTable' class='iMes_grid_TableGvExt' style='border-width:0px;height:1px;table-layout:fixed;'><tr class='iMes_grid_HeaderRowGvExt'>");

                //  class="FixedTables"
                int width;
                string stationDescr;
                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    width = 60;
                    if (i == 1 || i == 0)
                    { width = 150; }
                    if (i <headerCount)
                    {
                        sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt'  style='width:{1}px'>{0}</th>", dt2.Columns[i].ColumnName, width);
                    }
                    else
                    {
                        if (!DicStationDescr.TryGetValue(dt2.Columns[i].ColumnName, out stationDescr))
                        {
                            stationDescr = "";
                        }

                        sb.AppendFormat("<th class='iMes_grid_HeaderRowGvExt' style='width:{1}px' onmouseover=\"ShowDescr('{2}')\" onmouseout='UnTip()'>{0}</th>", dt2.Columns[i].ColumnName, width, stationDescr);

                    }


                }
                sb.AppendLine("</tr><tbody>");
                string modelTemp = "";
                int count = 0;
             

                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    // string classname = "";
                    // classname = "lesson" + dt2.Rows[j][1].ToString();



                    string src = "";
                    string src1 = "";
                    int cssIdx = 0;
                    string data;
                    string yellowClass = "";
                    string rowCSS;
                    //   src = @" onclick=\'GetDetail(\'{0}\',\'{1}\',\'{2}\',\'{3}\');\' " ; //station,line,model,dn
                    src1 = @" onclick='GetDetail()'"; //station,line,model,dn

                    if (dt2.Rows[j][1].ToString().Trim() != modelTemp)
                    {
                        count += 1;
                        modelTemp = dt2.Rows[j][1].ToString().Trim();
                    }
                    cssIdx = count % 2;
                    rowCSS = "row" + cssIdx.ToString();
                    // string tipCmd ;
                    sb.AppendLine("<tr class='" + rowCSS + "' onclick='ChangeRowColor()'>");
                    for (int i = 0; i < dt2.Columns.Count; i++)
                    {

                        data = dt2.Rows[j][i].ToString().Trim().Replace("&nbsp;", "");
                        if (i == 4 )
                        {
                            if (data != "0")
                            { sb.AppendFormat("<td class='dn-pak' >{0}</td>", data); }
                            else
                            {
                                sb.AppendFormat("<td style='width:150px'>{0}</td>", data);
                            }

                        }
                        else if (i >= headerCount)
                        {

                            if (data == "")
                            { yellowClass = ""; src = ""; }
                            else
                            {
                                yellowClass = "class='querycell'  ";
                                src = @" onclick='GetDetail()' onmouseover='ShowDetailDescr()' onmouseout='UnTip()'";
                            }
                            //  tipCmd = string.Format("'{0}','{1}','{2}'", dt2.Columns[i].ColumnName, dt2.Rows[j][0].ToString().Trim(), dt2.Rows[j][1].ToString().Trim());
                            sb.AppendFormat("<td {0}{1} >{2}</td>", yellowClass, src, data);

                        }
                        else
                        {
                            sb.AppendFormat("<td style='width:150px'>{0}</td>", data);
                        }

                    }
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table></div> ");
                Response.Write(sb.ToString());
                break;
            case "GET_DETAIL":
                break;        
            case "GET_EXCEL":
                break;
            default:
            break;
        }
        

    
        
    }
    catch (Exception ex)
    {
        Response.StatusCode = 500;
        Response.Write(ex.ToString());
        System.Diagnostics.Debug.WriteLine(ex.ToString());
    }         
    
%>