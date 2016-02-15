using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
//using IMES.Station.Interface.CommonIntf;
using System.Data;

public partial class Query_PAK_FamilyDimension : IMESQueryBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IMES.Query.Interface.QueryIntf.IFamily Family = ServiceAgent.getInstance().GetObjectByName<IMES.Query.Interface.QueryIntf.IFamily>(WebConstant.IFamilyBObject);
    public string ColumnList = "長,寬,高"; 
    IPAK_Common PAK_Common = ServiceAgent.getInstance().GetObjectByName<IPAK_Common>(WebConstant.PAK_Common);
    String DBConnection = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterMPGet(this.Master);
        DBConnection = CmbDBType.ddlGetConnection();
        try
        {
            if (!this.IsPostBack)
            {
                IniFamily();
               InitLabel();
              
             
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg,this);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message,this);
        }

    }
    private void IniFamily()
    {

        DataTable dtPno = Family.GetFamily(DBConnection);

        if (dtPno.Rows.Count > 0)
        {
            ddlFamily.Items.Add(new ListItem("", ""));
            foreach (DataRow dr in dtPno.Rows)
            {
                ddlFamily.Items.Add(new ListItem(dr["Family"].ToString().Trim(), dr["Family"].ToString().Trim()));
            }
        }
    }
    private void InitLabel()
    {

        lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
  
        lblTitle.Text = this.GetLocalResourceObject(Pre + "_lblTitle").ToString();
    }
    private void InitGridView()
    {
     
    }
  
   private void BindGrv()
   {

          DataTable dt = PAK_Common.GetFamilyInfo(DBConnection,ddlFamily.SelectedValue,txtModel.Text.Trim(),"CN");
          if (dt == null || dt.Rows.Count == 0)
          {
              BindNoData();
              EnableBtnExcel(this, false, btnExcel.ClientID);
          }
          else
          {
              BindDataToGrv(dt);
          //    gvResult.DataSource = dt;
              EnableBtnExcel(this, true, btnExcel.ClientID);
         //     gvResult.DataBind();
              //InitGridView();
          }
    
   
   
   }
    private void BindDataToGrv(DataTable dtResult)
    {
        DataTable dt = initTable(ColumnList);

        string[] data = dtResult.Rows[0][0].ToString().Trim().Split('*');

        if (data.Length != 3)
        {
            showErrorMessage("Family 長寬高定義不正確",this);
            BindNoData();
            return;
        }


        string[] colArr = ColumnList.Split(',');
        DataRow newRow = null;
        newRow = dt.NewRow();
      
        for (int i = 0; i < colArr.Length; i++)
        {
            newRow[colArr[i]] = data[i];
        }

      
        dt.Rows.Add(newRow);

        gvResult.DataSource = dt;
        gvResult.DataBind();
    }

  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
     //   beginWaitingCoverDiv(this.UpdatePanel1);     
        try
       {
         BindGrv();
        }
         catch (Exception ex)
        {
            showErrorMessage(ex.Message,this.UpdatePanel1);
          //  BindNoData();
        }
         finally
        {
             endWaitingCoverDiv(this);
        }
  
      
    }
    private void InitGrvColumnHeader()
    {
        
        
    }
    private void BindNoData()
    {
       // string  ColumnList = "長,寬,高";
        this.gvResult.DataSource = getNullDataTable(ColumnList);
        this.gvResult.DataBind();
        //InitGridView();
        //InitGrvColumnHeader();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ToolUtility t = new ToolUtility();
        t.ExportExcel(gvResult, "Excel", Page);
    }
    
}
