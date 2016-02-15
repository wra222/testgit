/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:Model Input
 *             
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-03  Chen Xu (EB1-4)      Create      
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
using com.inventec.iMESWEB;
using System.Web.Services;
using System.Web.Services.Protocols;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;


public partial class CommonControl_ModelInputControl : System.Web.UI.UserControl
{
    private IModel iModel;
    private string EnterOrTabResFun;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.txtModel.Attributes.Add("onkeydown", "OnKeyDownCheck()");
    }

   
    /// <summary>
    /// 检查用户输入的Model在Model表中是否存在
    /// </summary>
    /// <param name="model"></param>
    public void checkModelExist()
    {
        
        iModel = null;
        iModel = ServiceAgent.getInstance().GetObjectByName<IModel>(WebConstant.CommonObject);

        if (iModel != null)
        {
            string model = this.txtModel.Value.Trim().ToUpper();
            iModel.checkModel(model);
        }
      

    }

    /// <summary>
    /// 检查用户输入的Model在是否属于该Family
    /// </summary>
    /// <param name="family">family</param>
    /// <param name="model">model</param>
    public void checkModelBelongtoFamily(String family)
    {

        iModel = null;
        iModel = ServiceAgent.getInstance().GetObjectByName<IModel>(WebConstant.CommonObject);

        if (iModel != null)
        {
            string model = this.txtModel.Value.Trim().ToUpper();
            iModel.checkModelinFamily(family,model);
        }
    
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    public System.Web.UI.HtmlControls.HtmlInputText getModelObj()
    {
        return this.txtModel;
    }
   

    /// <summary>
    /// 清空内容
    /// </summary>
    public void clearContent()
    {

        //清空combobox内容
        this.txtModel.Value="";
        this.up.Update();

    }

    /// <summary>
    /// 获取内容
    /// </summary>
    public string getModelContent()
    {

        //获取内容
       return this.txtModel.Value.Trim().ToUpper() ;

    }
}