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

public partial class ChooseFromList : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string stationHid = String.Empty;
    public string pcode = String.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string sLst = Request.QueryString["l"].ToString();
            string[] lst = sLst.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string v in lst)
            {
                ListItem li = new ListItem(v, v);
                lstChoose.Items.Add(li);
            }
            if (lstChoose.Items.Count > 0)
                lstChoose.SelectedIndex = 0;

            lblTitle.Text = "";
            string title = Request.QueryString["title"] as string;
            if (!string.IsNullOrEmpty(title))
            {
                Page.Header.Title = title;
                lblTitle.Text = title;
            }
        }
    }

}
