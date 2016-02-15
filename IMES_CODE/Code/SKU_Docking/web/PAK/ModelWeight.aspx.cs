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
using System.Collections;
using System.Collections.Generic;


public partial class PAK_ModelWeight : System.Web.UI.Page
{

    public String UserId;
    public String Customer;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    IModelWeight iModelWeight = ServiceAgent.getInstance().GetObjectByName<IModelWeight>(WebConstant.ModelWeightObject);
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
         //   iModelWeight = ServiceAgent.getInstance().GetObjectByName<IPakUnitWeight>(WebConstant.PakUnitWeightObject);
            
            if (!this.IsPostBack)
            {
                
                UserId = Master.userInfo.UserId;
                Customer = Master.userInfo.Customer;

                initLabel();
                BindDefect();
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

    private void BindDefect()
    {
        List<string> lst = iModelWeight.GetDefectCodeList("HOLD");
        droDefect.Items.Add("");
        foreach (string d in lst)
        {
            droDefect.Items.Add(new ListItem(d, d.Split('-')[0]));
        }
    
    }
    protected void btnGetUnitWeight_ServerClick(Object sender, EventArgs e)
    {
        string model = this.dModel.Text.Trim().ToUpper();
        string weightNum = "";
		ModelWeightDef item = new ModelWeightDef();
        try
        {
            ArrayList arr = iModelWeight.GetModelWeightByModelorCustSN(model);
            item = (ModelWeightDef)arr[0];
            weightNum = item.UnitWeight;
            //IList<string> prdLst = (IList<string>)arr[1];
            hidMessage.Value = "";
           
            this.Hidd_Model.Value = item.Model;

            if (string.IsNullOrEmpty(weightNum))
            {
                getUnitWeightComplete("False", weightNum, "");
            }
            else
            {
                //if (prdLst.Count > 0)
                //{
                //    IList<string> proIdLst = (IList<string>)arr[1];
                //    hid_PrdLsit.Value = string.Join(",", proIdLst.ToArray());
                //    string m = "Total passed machines :" + prdLst.Count + " sets";
                //    getUnitWeightComplete("True", weightNum, "HaveHold('" + m + "')");
                //}
                //else
                //{
                //    getUnitWeightComplete("True", weightNum, "NoHold()");
                //}
                getUnitWeightComplete("True", weightNum, "NoHold()");
            }



        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            initPage();
          return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            initPage();
            return;
        }
     

    }

    private void initLabel()
    {
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblStandardWeight.Text = this.GetLocalResourceObject(Pre + "_lblStandardWeight").ToString();
    }

    protected void btnHoldAndSave_ServerClick(Object sender, EventArgs e)
    {

        ModelWeightDef curitem = new ModelWeightDef();
     
        curitem.UnitWeight = this.dStandardWeight.Text.Trim();
        if (string.IsNullOrEmpty(this.Hidd_Model.Value.Trim()))
        {
            string model = this.dModel.Text.Trim().ToUpper();
            ArrayList arr = iModelWeight.GetModelWeightByModelorCustSN(model);
            curitem.Model = ((ModelWeightDef)arr[0]).Model;

            this.Hidd_Model.Value = curitem.Model;
        }
        else
        {
            curitem.Model = this.Hidd_Model.Value.Trim();
        }
        curitem.Editor = Master.userInfo.UserId;

        try
        {


            IList<string> lst = (IList<string>)hid_PrdLsit.Value.Split(',').ToList();
            string defect = hidDefect.Value;
            iModelWeight.SaveModelWeightItemAndHold(curitem, lst, "HOLD", defect);
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
        finally
        { 
        EndWaitingCoverDiv();
        }

    }
    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
      

        try
        {
            ModelWeightDef curitem = new ModelWeightDef();

            if (string.IsNullOrEmpty(this.Hidd_Model.Value.Trim()))
            {
                string model = this.dModel.Text.Trim().ToUpper();
                ArrayList arr = iModelWeight.GetModelWeightByModelorCustSN(model);
                curitem.Model = ((ModelWeightDef)arr[0]).Model;

                this.Hidd_Model.Value = curitem.Model;
            }
            else
            {
                curitem.Model = this.Hidd_Model.Value.Trim();
            }

            curitem.UnitWeight = this.dStandardWeight.Text.Trim();
            //curitem.Model = this.Hidd_Model.Value.Trim();
            curitem.Editor = Master.userInfo.UserId;

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
        finally
        {
            EndWaitingCoverDiv();
        }
      
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
    private void getUnitWeightComplete(string isOK,string weightItem,string funName)
    {
        StringBuilder scriptBuilder = new StringBuilder();
       
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("GetUnitWeightComplete(\"" + isOK + "\",\"" + weightItem + "\");" + funName);
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
            "endWaitingCoverDiv();window.setTimeout (ResetPage,100);" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "ResetPage", script, false);
    }
    private void EndWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "ResetPage", script, false);
    }
}
