using System;
using System.Data;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using System.Web.UI;
using IMES.Maintain.Interface.MaintainIntf;
using System.Web.UI.WebControls;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using IMES.DataModel;

public partial class CheckItemTypeRuleforTestREDialog : System.Web.UI.Page
{

    private ICheckItemTypeListMaintain iCheckItemTypeListMaintain;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        try
        {
            iCheckItemTypeListMaintain = (ICheckItemTypeListMaintain)ServiceAgent.getInstance().GetMaintainObjectByName<ICheckItemTypeListMaintain>(WebConstant.CheckItemTypeListObject);
            initCmbTestObject();
            initCmbTestCondition();
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


    private void initCmbTestObject()
    {
        this.cmdTestObject.Items.Clear();
        this.cmdTestObject.Items.Add(string.Empty);
        IList<ConstValueInfo> list = iCheckItemTypeListMaintain.GetConstValueList();
        IList<ListItem> inneritem = new List<ListItem>();
        foreach (ConstValueInfo NameItem in list)
        {
            ListItem item = new ListItem();
            item.Text = NameItem.name;
            item.Value = NameItem.value;
            inneritem.Add(item);
            this.cmdTestObject.Items.Add(item);
        }
        this.cmdTestObject.SelectedIndex = 0;
    }

    private void initCmbTestCondition()
    {
        this.cmbTestCondition.Items.Clear();
        this.cmbTestCondition.Items.Add(string.Empty);
    }

    protected void betTest_ServerClick(Object sender, EventArgs e)
    {
        string ret = "N";
        try
        {
            string objectType = this.hidObjectType.Value;
            string objectTxt = this.txtTestObject.Value.Trim();
            string conditionType = this.hidConditionType.Value;
            string conditionTxt = this.txtTestCondition.Value.Trim();
            string expression = this.txtExpression.Value.Trim();

            bool a = iCheckItemTypeListMaintain.CheckTextExpression(objectTxt, objectType, conditionTxt, conditionType, expression);
            if (a)
            {
                ret = "Y";
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "checkReg", "ResultAnws('" + ret + "')", true);
    }


    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }
}
