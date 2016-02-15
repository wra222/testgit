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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using System.Text;
using IMES.Station.Interface.StationIntf;
using System.Web.Services;


public partial class SA_MaterialRequest : IMESBasePage
{
    private const int DEFAULT_ROWS = 15;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
    IMaterialRequest imaterialrequest = ServiceAgent.getInstance().GetObjectByName<IMaterialRequest>(WebConstant.SAMBRepairControl);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        this.cmdstage.AutoPostBack = true;
        this.cmdstage.SelectedIndexChanged += new EventHandler(cmdstage_Selected);
        UserId = Master.userInfo.UserId;
        Customer = Master.userInfo.Customer;
        if (!this.IsPostBack)
        {
        
            string[] stagelist = (Request["StageList"]?? "SA,FA,PAK").Split(',','~');
            try
            {
                this.cmdstage.Items.Add("");
                foreach(string stage in stagelist)
                {
                    
                        this.cmdstage.Items.Add(stage);
                   
                }
                
                bindTable2(null, 10);
                
            }
          
            catch (Exception ee)
            {
                showErrorMessage(ee.Message);
                return;
            }


        }
   
    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }
    private string replaceSpecialChart(string errorMsg)
    {
        errorMsg = errorMsg.Replace("'", "\\'");
        return errorMsg;
    }
    protected void cmdstage_Selected(object sender, EventArgs e)
    {
        string stage = this.cmdstage.SelectedValue.Trim();
        this.cmbPdLine.Customer = Customer;
        this.cmbPdLine.Stage = stage;
        this.cmbPdLine.refresh();
    }
    protected void btnAdd_ServerClick(object sender, EventArgs e)
    {
        try
        {
           
            string stage = cmdstage.SelectedValue.Trim().ToUpper();
            string partno = txtpartno.Text.Trim().ToUpper();
            string family = txtfamily.Text.Trim().ToUpper();
            string model = txtmodel.Text.Trim().ToUpper();
            string line = cmbPdLine.InnerDropDownList.SelectedValue.Trim();
            string loction = txtlocation.Text.Trim().ToUpper();
            string qty = txtqty.Text.Trim();
            string remark = txtremark.Text.Trim();
            MBRepairControlInfo info = new MBRepairControlInfo();
            info.PartNo = partno;
            info.MaterialType = "Component";
            info.Stage = stage;
            info.Family = family;
            info.PCBModelID = model;
            info.Line = line;
            info.Location = loction;
            info.Qty = Convert.ToInt32(qty);
            info.Status = "Waiting";
            info.Remark = remark.Trim();
            info.Editor = UserId;
            info.Cdt = DateTime.Now;
            info.Udt = DateTime.Now;
            imaterialrequest.AddMBRepairControl(info);
            bindgd("Waiting");
            showSuccessMessage("保存成功");
        }
        catch (FisException fex)
        {

            showErrorMessage(fex.mErrmsg);
            return;
        }
        catch (System.Exception ex)
        {
            showErrorMessage(ex.Message);
        }
        finally
        {
          
        }
    }
    protected void btquery_ServerClick(object sender, EventArgs e)
    {
      
       bindgd("Waiting");
    }
    protected void btdel_ServerClick(object sender, EventArgs e)
    {
        string id = this.hidDeleteID.Value.ToString();
        if (string.IsNullOrEmpty(id))
        {
            throw new FisException("This ID is not Exists");
        }
        int delID = int.Parse(id);
      IList<MBRepairControlInfo> retquery= imaterialrequest.GeMaterialRequest(new MBRepairControlInfo { ID = delID });
      if (retquery.Where(x => x.Status != "Waiting").Any())
      {
          showErrorMessage("此 ID" + delID + "状态非Waiting 不允许删除，请重新Query!");
          return;
      }
      else
      {
          imaterialrequest.DelMBRepairControl((new MBRepairControlInfo { ID = delID }));
          bindgd("Waiting");
          ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "Delete", "DeleteComplete();", true);
      }
    }
    protected void bindgd(string status)
    {
        IList<MBRepairControlInfo> ControlInfo = imaterialrequest.GeMaterialRequest(new MBRepairControlInfo { Status = status, MaterialType = "Component" });
        bindTable2(ControlInfo.OrderByDescending(x=>x.Udt).ToList() , 10);
    }
    private void bindTable2(IList<MBRepairControlInfo> list, int defaultRow)
    {

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add("ID");
        dt.Columns.Add("PartNo");
        dt.Columns.Add("Stage");
        dt.Columns.Add("Family");
        dt.Columns.Add("PCBModelID");
        dt.Columns.Add("Line");
        dt.Columns.Add("Location");
        dt.Columns.Add("Qty");
        dt.Columns.Add("Status");
        dt.Columns.Add("Loc");
        dt.Columns.Add("AssignUser");
        dt.Columns.Add("Remark");
        dt.Columns.Add("Editor");
        dt.Columns.Add("Cdt");
        dt.Columns.Add("Udt");
        if (list != null && list.Count != 0)
        {
            foreach (MBRepairControlInfo temp in list)
            {
                dr = dt.NewRow();
                dr[0] = temp.ID;
                dr[1] = temp.PartNo;
                dr[2] = temp.Stage;
                dr[3] = temp.Family;
                dr[4] = temp.PCBModelID;
                dr[5] = temp.Line;
                dr[6] = temp.Location;
                dr[7] = temp.Qty;
                dr[8] = temp.Status;
                dr[9] = temp.Loc;
                dr[10] = temp.AssignUser;
                dr[11] = temp.Remark;
                dr[12] = temp.Editor;
                dr[13] = temp.Cdt;
                dr[14] = temp.Udt;
                dt.Rows.Add(dr);
            }

            for (int i = list.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            
        }

        gd.DataSource = dt;
        gd.DataBind();
        this.UpdatePanel1.Update();
        setColumnWidth();
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true);
    }
    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Parse("30");
        gd.HeaderRow.Cells[1].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[2].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[3].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[4].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[5].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[6].Width = Unit.Parse("90");
        gd.HeaderRow.Cells[7].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[8].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[9].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[10].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[11].Width = Unit.Parse("100");
        gd.HeaderRow.Cells[12].Width = Unit.Parse("50");
        gd.HeaderRow.Cells[13].Width = Unit.Parse("130");
        gd.HeaderRow.Cells[14].Width = Unit.Parse("130");
     
    }
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("SetGrDivHeight();ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    private void showSuccessMessage(string msg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        msg = replaceSpecialChart(msg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("onSaveSucess('" + msg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }



}
