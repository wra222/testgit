using System;
using System.Collections;
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
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;
public partial class FA_PilotRunMO_Add : System.Web.UI.Page
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private IPilotRunMO iPilotRunMO = ServiceAgent.getInstance().GetObjectByName<IPilotRunMO>(WebConstant.PilotRunMOObject);
    public String UserId;
    public String Customer;
    public string today;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.cmbStage.SelectedIndexChanged += new EventHandler(cmbstage_Selected);
            this.cmbFamily.SelectedIndexChanged += new EventHandler(cmbfamily_Selected);
            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;

            if (!Page.IsPostBack)
            {

                this.stationHF.Value = Request["Station"];
                //this.pCode.Value = Request["PCode"];
                InitLabel();
                initStage();
            }
            today = DateTime.Now.ToString("yyyy-MM-dd");
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }
    private void InitLabel()
    {
       
    }

    private void initStage()
    {
        try
        {
            this.cmbStage.Items.Clear();
            this.cmbStage.Items.Add(string.Empty);
            IList<string> lst = iPilotRunMO.GetAdd_Stage();
            foreach (string item in lst)
            {
                this.cmbStage.Items.Add(item);
            }
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    private void initMoType(string stage)
    {
        try
        {
            this.cmbMOType.Items.Clear();
            this.cmbMOType.Items.Add(string.Empty);
            IList<ConstValueInfo> lst = iPilotRunMO.GetAdd_MoType(stage);
            foreach (ConstValueInfo item in lst)
            {
                ListItem items = new ListItem();
                items.Text = item.name;
                items.Value = item.value;
                this.cmbMOType.Items.Add(items);
            }
            this.UpdatePanel2.Update();
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    private void initEndMoType()
    {
        try
        {
            this.cmbEndMoType.Items.Clear();
            this.cmbEndMoType.Items.Add(string.Empty);
            IList<ConstValueInfo> lst = iPilotRunMO.GetMOTypeList("PilotMoType");
            foreach (ConstValueInfo item in lst)
            {
                ListItem items = new ListItem();
                items.Text = item.name;
                items.Value = item.value;
                this.cmbEndMoType.Items.Add(items);
            }
            this.UpdatePanel3.Update();
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    private void initFamily(string stage)
    {
        try
        {
            this.cmbFamily.Items.Clear();
            this.cmbFamily.Items.Add(string.Empty);
            IList<string[]> lst = iPilotRunMO.GetAdd_Family(stage);
            foreach (string[] item in lst)
            {
                ListItem items = new ListItem();
                items.Text = item[0];
                items.Value = item[1];
                this.cmbFamily.Items.Add(items);
            }
            
            this.UpdatePanel4.Update();
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    private void initModel(string mbcode)
    {
        try
        {
            this.cmbModel.Items.Clear();
            this.cmbModel.Items.Add(string.Empty);
            IList<string> lst = iPilotRunMO.GetAdd_Model(mbcode);
            foreach (string item in lst)
            {
                this.cmbModel.Items.Add(item);
            }
            this.UpdatePanel5.Update();
        }
        catch (FisException exp)
        {
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            writeToAlertMessage(exp.Message.ToString());
        }
    }

    private void cmbstage_Selected(object sender, System.EventArgs e)
    {
        try
        {
            string stage = this.cmbStage.SelectedValue.ToString();
            initMoType(stage);
            initEndMoType();
            initFamily(stage);
            if (stage == "FA")
            {
                this.txtModel.Enabled = true;
                this.cmbModel.Enabled = false;
            }
            else
            {
                this.txtModel.Enabled = false;
                this.cmbModel.Enabled = true;
            }

        }
        catch (FisException exp)
        {
            this.txtModel.Enabled = false;
            this.cmbModel.Enabled = false;
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            this.txtModel.Enabled = false;
            this.cmbModel.Enabled = false;
            writeToAlertMessage(exp.Message.ToString());
        }
        finally
        {
            this.UpdatePanel5.Update();
            this.UpdatePanel6.Update();
        }
    }

    private void cmbfamily_Selected(object sender, System.EventArgs e)
    {
        try
        {
            string stage = this.cmbStage.SelectedValue.ToString();
            string mbcode = this.cmbFamily.SelectedValue.ToString();
            if (stage == "SA")
            {
                initModel(mbcode);
            }
        }
        catch (FisException exp)
        {
            this.txtModel.Enabled = false;
            this.cmbModel.Enabled = false;
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            this.txtModel.Enabled = false;
            this.cmbModel.Enabled = false;
            writeToAlertMessage(exp.Message.ToString());
        }
        finally
        {
            this.UpdatePanel5.Update();
            this.UpdatePanel6.Update();
        }
    }

    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        try
        {
            PilotMoInfo item = new PilotMoInfo();
            item.stage = this.cmbStage.SelectedValue.ToString();
            item.moType = this.cmbMOType.SelectedItem.Text.ToString();
            if (this.cmbStage.SelectedValue.ToString() == "SA")
            {
                item.model = this.cmbModel.SelectedValue.ToString();
            }
            else
            {
                item.model = this.txtModel.Text.Trim();
            }
            item.qty = Convert.ToInt32(this.txtQty.Text.Trim());
            item.planStartTime = Convert.ToDateTime(this.hidPlanTime.Value.ToString());
            item.partNo = this.txtPartNo.Text.Trim();
            item.vendor = this.txtVendor.Text.Trim();
            item.causeDescr = this.txtCauesDescr.Text.Trim();
            item.remark = this.txtRemark.Text.Trim();
            item.editor = this.UserId.ToString();
            string startmotype = this.cmbMOType.SelectedValue.ToString().Trim();
            string endmotype = this.cmbEndMoType.SelectedValue.ToString().Trim();

            iPilotRunMO.GenPilotRunMo(item, startmotype, endmotype, Customer);
        }
        catch (FisException exp)
        {
            this.txtModel.Enabled = false;
            this.cmbModel.Enabled = false;
            writeToAlertMessage(exp.mErrmsg);
        }
        catch (Exception exp)
        {
            this.txtModel.Enabled = false;
            this.cmbModel.Enabled = false;
            writeToAlertMessage(exp.Message.ToString());
        }
        finally
        {
            //ResetPage
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Query", "ResetPage();", true);
        }
    }

    /// <summary>
    /// 输出错误信息
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\'", string.Empty).Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }

}
