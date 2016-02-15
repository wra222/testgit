using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.DataModel;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;


public partial class PAK_ModelWeight : System.Web.UI.Page
{

    public String UserId;
    public String Customer;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
   
    IPakUnitWeight iModelWeight;
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            iModelWeight = ServiceAgent.getInstance().GetObjectByName<IPakUnitWeight>(WebConstant.PakUnitWeightObject);
            
            if (!this.IsPostBack)
            {
                
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

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
        string model = this.dModel.Text.Trim().ToUpper();
        string weightNum = "";
		ModelWeightDef item = new ModelWeightDef();
        try
        {
            item=iModelWeight.GetModelWeightByModelorCustSN(model);
			weightNum =item.UnitWeight;
            this.Hidd_Model.Value = item.Model;

            if (string.IsNullOrEmpty(weightNum))
            {
                getUnitWeightComplete("False", weightNum);
            }
            else getUnitWeightComplete("True",weightNum);

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            initPage();
                        
          //  ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "GetUnitWeight", "GetUnitWeightComplete('False','');DealHideWait();", true);
            
            return;
        }
        catch (Exception ex)
        {

            showErrorMessage(ex.Message);
            initPage();
           // ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "GetUnitWeight", "GetUnitWeightComplete('False','');DealHideWait();", true);
            return;
        }
       //   ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "GetUnitWeight", "GetUnitWeightComplete('True','" + weightNum + "');DealHideWait();", true);

    }

    private void initLabel()
    {
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblStandardWeight.Text = this.GetLocalResourceObject(Pre + "_lblStandardWeight").ToString();
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        ModelWeightDef curitem = new ModelWeightDef();

        curitem.UnitWeight = this.dStandardWeight.Text.Trim();
        curitem.Model = this.Hidd_Model.Value.Trim();
        curitem.Editor = Master.userInfo.UserId;

        try
        {
            iModelWeight.SaveModelWeightItem(curitem);

            saveComplete();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            initPage();
            
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            initPage();
        }

      //  ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "SaveComplete('True');DealHideWait();", true);
    }

    
    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    /// <summary>
    /// 保存成功
    /// <returns></returns>
    private void saveComplete()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (SaveComplete,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "SaveComplete", script, false);
    }

    /// <summary>
    /// 保存成功
    /// <returns></returns>
    private void getUnitWeightComplete(string isOK,string weightItem)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("GetUnitWeightComplete(\"" + isOK + "\",\"" + weightItem + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "para_transfer", scriptBuilder.ToString(), false);
    }

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    /// <summary>
    /// 清空页面
    /// <returns></returns>
    private void initPage()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "window.setTimeout (ResetPage,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "ResetPage", script, false);
    }
}
