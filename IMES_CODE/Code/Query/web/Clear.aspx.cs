//'/*
//' * INVENTEC corporation (c)2008 all rights reserved. 
//' * Description:clear page
//' *             
//' * Update: 
//' * Date       Name                  Reason 
//' * ========== ===================== =====================================
//' * 2008-12-10  Zhao Meili(EB2)        Create                    
//' * Known issues:
//' */

using System.Data;
using com.inventec.iMESWEB;
using System;
using System.Web.UI.WebControls;

public partial class Clear : IMESBasePage
{
    public  string pre  =  WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            initSelect();
            
            this.lblSK.Text = Resources.iMESGlobalDisplay.ResourceManager.GetObject(pre + "_lblSessKey").ToString();
        }
    }

    private void initSelect()
   {
       ListItem item = new ListItem("", "");
       this.dlType.Items .Add (item);
       item = new ListItem(Resources.iMESGlobalDisplay.ResourceManager.GetString(pre + "_lblMB"), WebConstant.SessionMB);
       this.dlType.Items.Add(item);
       item = new ListItem(Resources.iMESGlobalDisplay.ResourceManager.GetString(pre + "_lblProduct"), WebConstant.SessionProduct);
       this.dlType.Items.Add(item);
       item = new ListItem(Resources.iMESGlobalDisplay.ResourceManager.GetString(pre + "_lblCommon"), WebConstant.SessionCommon);
       this.dlType.Items.Add(item);
    }

}
