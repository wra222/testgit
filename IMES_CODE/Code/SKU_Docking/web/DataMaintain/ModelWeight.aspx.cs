using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMES.Maintain.Interface.MaintainIntf;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;


public partial class DataMaintain_ModelWeight : System.Web.UI.Page
{
    
    public String userName;
   
    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IModelWeight iModelWeight;

    public string pmtMessage1;
    public string pmtMessage2;
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iModelWeight = ServiceAgent.getInstance().GetMaintainObjectByName<IModelWeight>(WebConstant.MaintainModelWeightObject);
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
  
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId; //UserInfo.UserId;
                this.HiddenUserName.Value = userName;
                initLabel();
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    protected void btnGetUnitWeight_ServerClick(Object sender, EventArgs e)
    {
        string model = this.dModel.Text.Trim();
        string weightNum = "";
        try
        {
            weightNum=iModelWeight.GetUnitWeightByModel(model);
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "GetUnitWeight", "GetUnitWeightComplete('False','');DealHideWait();", true);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "GetUnitWeight", "GetUnitWeightComplete('False','');DealHideWait();", true);
            return;
        }
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "GetUnitWeight", "GetUnitWeightComplete('True','" + weightNum + "');DealHideWait();", true);

    }

    private void initLabel()
    {
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblStandardWeight.Text = this.GetLocalResourceObject(Pre + "_lblStandardWeight").ToString();
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        ModelWeightDef item = new ModelWeightDef();

        item.UnitWeight = this.dStandardWeight.Text.Trim();
        item.Model= this.dModel.Text.Trim();
        item.Editor = this.HiddenUserName.Value;

        try
        {
            iModelWeight.SaveModelWeightItem(item);
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

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "SaveComplete('True');DealHideWait();", true);
    }

    
    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        //scriptBuilder.AppendLine("DealHideWait();");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        //scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    
}
