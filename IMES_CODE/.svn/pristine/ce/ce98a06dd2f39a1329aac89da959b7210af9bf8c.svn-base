
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:gridview for clear page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-12-10  Zhao Meili(eb)        Create            
 * 2010-01-20  Zhao Meili(eb)        Modify： ITC-932-0208 solved
 * 2010-01-22  Zhao Meili(eb)        Modify： ITC-932-0051 solved
 * 2010-02-07  Zhao Meili(eb)        Modify： ITC-932-0330  sovled
 * 2010-02-20  Zhao Meili(eb)        Modify： ITC-1122-0088、 ITC-1122-0096  sovled
 * Known issues:
 */


using System;
using System.Collections;
using System.Collections.Generic;
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
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using com.inventec.iMESWEB;
using IMES.DataModel;
using System.Text;



public partial class  GridForClear : System.Web.UI.Page
{
    //'ITC-932-0330 sovled
    private int fullRowCount   = 12;
    private  int collectionDataLength   = 15;
    private string skey = "";
    private ServiceAgent serviceAgent   = null;
    private ISession  SAsessionManager   = null;
    private ISession FAsessionManager = null;
    private ISession PAKsessionManager = null;
    //Add for Docking
    private ISession DockingsessionManager = null;
    //Add for RCTO
    private ISession RCTOsessionManager = null;
    private char[]   keySeperator ={ '|'};
    private string pre   = "";
    private string serviceConfig = "";
    private string noDescWC = "";
    private string SAServicePath = "";
    private string FAServicePath ="";
    private string PAKServicePath = "";
    private string DockingServicePath = "";
    //Add for RCTO
    private string RCTOServicePath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //' 正确的属性设置方法
        try
        {
            pre = WebCommonMethod.getConfiguration ("language").ToString();
            Response.Cache.SetNoStore();
            Response.Expires = -1;
            skey = Request.QueryString["key"].ToString().Trim();
            serviceConfig = WebCommonMethod.getConfiguration("ClearSessionService").ToString().Trim();

            if (string.IsNullOrEmpty(skey) || string.IsNullOrEmpty(serviceConfig))
            {
                InitGridView(pre, WebConstant.SessionMB);

                this.GridViewExt1.DataSource = this.getNullBomDataTable();
                this.GridViewExt1.DataBind();

            }
            else
            {
                InitGridView(pre, skey);
                this.GridViewExt1.DataSource = fillDtToFull(getSessionData(skey));
                this.GridViewExt1.DataBind();
            }
            if (!this.IsPostBack)
            {
                this.hflastDirection.Value = "";
                this.hflastSortExpression.Value = "";
            }
        }
        catch (FisException fisex)
        {
            showMessage(fisex.mErrmsg);
        }
        catch (Exception ex)
        {
            showMessage(ex.Message);
        }


    }

    private void InitGridView(string pre, string sessionkey)
    {

        if (sessionkey == WebConstant.SessionMB)
        {
            this.GridViewExt1.Columns[0].HeaderText = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_lblMB").ToString();
        }
        else if (sessionkey == WebConstant.SessionProduct)
        {
            this.GridViewExt1.Columns[0].HeaderText = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_lblProduct").ToString();
        }
        else if (sessionkey == WebConstant.SessionCommon)
        {
            this.GridViewExt1.Columns[0].HeaderText = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_lblCommon").ToString();
        }
        GridViewExt1.Columns[0].ItemStyle.Width = Unit.Pixel(160);

        this.GridViewExt1.Columns[1].HeaderText = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_colStation").ToString();
        this.GridViewExt1.Columns[2].HeaderText = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_colService").ToString();
        this.GridViewExt1.Columns[3].HeaderText = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_colline").ToString();
        this.GridViewExt1.Columns[4].HeaderText = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_colOperator").ToString();
        this.GridViewExt1.Columns[5].HeaderText = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_colCDT").ToString();

        GridViewExt1.Columns[1].ItemStyle.Width = Unit.Pixel(100);
        GridViewExt1.Columns[2].ItemStyle.Width = Unit.Pixel(160);
        GridViewExt1.Columns[3].ItemStyle.Width = Unit.Pixel(160);
    }

    private DataTable getNullBomDataTable()
    {

        DataTable dt = initBomTable();
        DataRow newRow = null;
        for (int i = 0; i < fullRowCount; i++)
        {
            newRow = dt.NewRow();
            newRow["change"] = string.Empty;
            newRow["WC"] = string.Empty;
            newRow["Service"] = string.Empty;
            newRow["PdLine"] = string.Empty;
            newRow["Operator"] = string.Empty;
            newRow["StartTime"] = string.Empty;
            newRow["StationId"] = string.Empty;
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable fillDtToFull(DataTable dt)
    {
        DataRow newRow = null;

        if (dt.Rows.Count < fullRowCount + 1)
        {
            for (int i = dt.Rows.Count; i < fullRowCount; i++)
            {
                newRow = dt.NewRow();
                newRow["change"] = string.Empty;
                newRow["WC"] = string.Empty;
                newRow["Service"] = string.Empty;
                newRow["PdLine"] = string.Empty;
                newRow["Operator"] = string.Empty;
                newRow["StartTime"] = string.Empty;
                newRow["StationId"] = string.Empty;
                dt.Rows.Add(newRow);
            }
        }
        return dt;
    }

    private DataTable getInfo(int seType, DataTable dt,  Dictionary<string, string> wcDictionary, ISession sessionManager, string ServiceName)
    {
        
  
       IList<SessionInfo> sessionList = null;
    
       DataRow newRow = null;
     
       //int i = 15;
        //newRow = dt.NewRow();
        //newRow["change"] = "chngge2" + i.ToString();
        //newRow["WC"] = "WC" + i.ToString();
        //newRow["PdLine"] = "line" + i.ToString();
        //newRow["Operator"] = "line" + i.ToString();
        //newRow["StartTime"] = "line" + i.ToString();
        //newRow["StationId"] = string.Empty;
        //dt.Rows.Add(newRow);

        //for (i = 0; i < 5; i++)
        //{
        //    newRow = dt.NewRow();
        //    newRow["change"] = "chngge2" + i.ToString();
        //    newRow["WC"] = "WC" + i.ToString();
        //    newRow["PdLine"] = "line" + i.ToString();
        //    newRow["Operator"] = "line" + i.ToString();
        //    newRow["StartTime"] = "line" + i.ToString();
        //    newRow["StationId"] = string.Empty;
        //    dt.Rows.Add(newRow);
        //}

       //sessionManager = (ISession)ServiceAgent.getInstance().GetObjectByName<ISession>(WebConstant.CommonObject, ServiceName);
    
       if (wcDictionary == null || wcDictionary.Count == 0)
       {
           IList<StationInfo> stations = sessionManager.GetAllStationInfo();
           if ((stations != null) && (stations.Count > 0))
           {
               foreach (StationInfo sta in stations)
               {
                   wcDictionary.Add(sta.StationId, sta.Descr);
               }
           }
       }
         sessionList = sessionManager.GetSessionByType((SessionType)seType);
         IList<string> noDesStaslist = new List<string>();  
        // sessionList.Add 
         if ((sessionList != null) && (sessionList.Count > 0))
         {
             foreach (SessionInfo se in sessionList)
             {

                 newRow = dt.NewRow();
                 newRow["change"] = se.SessionKey;
                 if (wcDictionary.ContainsKey(se.StationId))
                 {
                     newRow["WC"] = wcDictionary[se.StationId];//'se.wc
                 }
                 else if (!noDesStaslist.Contains(se.StationId))
                 {
                     noDesStaslist.Add(se.StationId);
                     newRow["WC"] = se.StationId;
                 }
                 newRow["Service"] = ServiceName;
                 newRow["PdLine"] = se.PdLine;
                 newRow["Operator"] = se.Operator;
                 newRow["StartTime"] = DateTimeToString(se.Cdt);
                 newRow["StationId"] = se.StationId;

                 dt.Rows.Add(newRow);
             }

             if (noDesStaslist.Count > 0)
             {
                 foreach (string nodesSta in noDesStaslist)
                 {
                     if (string.IsNullOrEmpty(noDescWC))
                     {
                         noDescWC = nodesSta;
                     }
                     else
                     {
                         noDescWC = noDescWC + "," + nodesSta;
                     }
                 }
             }
         }
         //else
         //{
         //    dt = getNullBomDataTable();
         //}



        return dt;
    }



    private DataTable getSessionData(string sessionkey)
    {
        Dictionary<string, string> wcDictionary = new Dictionary<string, string>();
        DataTable dt = initBomTable();
        string[] serviceArray = serviceConfig.Split(keySeperator);
        int seType = int.Parse(sessionkey);
        const string SA = "SA";
        const string FA = "FA";
        const string PAK = "PAK";
        //Add ForDocking 
        const string Docking = "Docking";
        //Add ForRCTO 
        const string RCTO = "RCTO";


        SAServicePath = WebCommonMethod.getConfiguration(SA + "Address").ToString().Trim() + ":" + WebCommonMethod.getConfiguration(SA + "Port").ToString().Trim();
        FAServicePath = WebCommonMethod.getConfiguration(FA + "Address").ToString().Trim() + ":" + WebCommonMethod.getConfiguration(FA + "Port").ToString().Trim();
        PAKServicePath = WebCommonMethod.getConfiguration(PAK + "Address").ToString().Trim() + ":" + WebCommonMethod.getConfiguration(PAK + "Port").ToString().Trim();
        //Add ForDocking 
        DockingServicePath = WebCommonMethod.getConfiguration(Docking + "Address").ToString().Trim() + ":" + WebCommonMethod.getConfiguration(Docking + "Port").ToString().Trim();
        //Add ForRCTO 
        RCTOServicePath = WebCommonMethod.getConfiguration(RCTO + "Address").ToString().Trim() + ":" + WebCommonMethod.getConfiguration(RCTO + "Port").ToString().Trim();

        if (serviceArray.Contains<string>(SA))
        {
            SAsessionManager = (ISession)ServiceAgent.getInstance().GetObjectByName<ISession>(WebConstant.CommonObject, SA);
            dt = getInfo(seType, dt, wcDictionary, SAsessionManager, SA);
        }
        if (serviceArray.Contains<string>(FA) && (SAServicePath != FAServicePath))
        {
            FAsessionManager = (ISession)ServiceAgent.getInstance().GetObjectByName<ISession>(WebConstant.CommonObject, FA);
            dt = getInfo(seType, dt, wcDictionary, FAsessionManager, FA);
        }
        if (serviceArray.Contains<string>(PAK) && (SAServicePath != PAKServicePath) && (PAKServicePath != FAServicePath))
        {
            PAKsessionManager = (ISession)ServiceAgent.getInstance().GetObjectByName<ISession>(WebConstant.CommonObject, PAK);
            dt = getInfo(seType, dt, wcDictionary, PAKsessionManager, PAK);
        }
        //Add for Docking
        if (serviceArray.Contains<string>(Docking) && (SAServicePath != DockingServicePath) && (DockingServicePath != FAServicePath) && (DockingServicePath != PAKServicePath))
        {
            DockingsessionManager= (ISession)ServiceAgent.getInstance().GetObjectByName<ISession>(WebConstant.CommonObject, Docking);
            dt = getInfo(seType, dt, wcDictionary, DockingsessionManager, Docking);
        }

        //Add for RCTO
        //if (serviceArray.Contains<string>(RCTO) && (SAServicePath != RCTOServicePath) && (RCTOServicePath != FAServicePath) && (RCTOServicePath != PAKServicePath) && (RCTOServicePath != DockingServicePath))
        if (serviceArray.Contains<string>(RCTO))
        {
            RCTOsessionManager = (ISession)ServiceAgent.getInstance().GetObjectByName<ISession>(WebConstant.CommonObject, RCTO);
            dt = getInfo(seType, dt, wcDictionary, RCTOsessionManager, RCTO);
        } 


        // DataTable dt = getInfo(sessionkey);


        if (!string.IsNullOrEmpty(noDescWC))
        {
            string message = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_NoDescWCExist").ToString();
            showMessage(message + noDescWC);
        }

        return dt;
    }


    private DataTable initBomTable() 
    {
    //' 获取GridView排序数据列及排序方向
    DataTable retTable   = new DataTable();
        retTable.Columns.Add("change", Type.GetType("System.String"));
        retTable.Columns.Add("WC", Type.GetType("System.String"));
        retTable.Columns.Add("Service", Type.GetType("System.String"));
        retTable.Columns.Add("PdLine", Type.GetType("System.String"));
        retTable.Columns.Add("Operator", Type.GetType("System.String"));
        retTable.Columns.Add("StartTime", Type.GetType("System.String"));
        retTable.Columns.Add("StationId", Type.GetType("System.String"));
        return retTable;

    }


    protected void gv_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e ) 
    {
        e.Row.Cells[6].Attributes.Add("style", e.Row.Cells[5].Attributes["style"] + "display:none;");
    }

    private string DateTimeToString(object dt)
    {

        if ((Convert.IsDBNull(dt)) || (dt == null))
        {
            return "";
        }
        else
        {
            return ((DateTime)dt).ToString("yyyy-MM-dd HH:mm:ss").Trim();
        }
    }

    private void showMessage(string message)
    {
       StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + message.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("parent.ShowInfo(\"" + message.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");
        this.ClientScript.RegisterStartupScript(this.GetType(), "showMessage", scriptBuilder.ToString());
    }


    protected void SortGrid(object sender, GridViewSortEventArgs e)
    {
        //' 从事件参数获取排序数据列
        //'' 假定为排序方向为“顺序”
        string sortDirection = "";
        string sortExpression = e.SortExpression;
        if (this.hflastSortExpression.Value == sortExpression)
        {
            sortDirection = "DESC";
            if (this.hflastDirection.Value == sortDirection)
            {
                sortDirection = "ASC";
            }
        }
        else
        {
            sortDirection = "ASC";
        }
        this.hflastDirection.Value = sortDirection;
        this.hflastSortExpression.Value = sortExpression;
        SortGridView(sortExpression, sortDirection);

    }

    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = null;
        if (string.IsNullOrEmpty(skey))
        {
            dt = this.getNullBomDataTable();
        }
        else
        {
            dt = getSessionData(skey);
        }

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + " " + direction;
        dt = dv.ToTable();

        //DataRow newRow = null;
        //if (dt.Rows.Count < fullRowCount + 1)
        //{
        //    for (int i = dt.Rows.Count; i < fullRowCount; i++)
        //    {
        //        newRow = dt.NewRow();
        //        newRow["change"] = string.Empty;
        //        newRow["WC"] = string.Empty;
        //        newRow["Service"] = string.Empty;
        //        newRow["PdLine"] = string.Empty;
        //        newRow["Operator"] = string.Empty;
        //        newRow["StartTime"] = string.Empty;
        //        newRow["StationId"] = string.Empty;
        //        dt.Rows.Add(newRow);
        //    }
        //}
        this.GridViewExt1.DataSource = fillDtToFull(dt);
        this.GridViewExt1.DataBind();
        InitGridView(pre, skey);
    }


}

