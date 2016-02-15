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
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using System.Text;
using System.Linq;
//qa bug no:ITC-1136-0046,ITC-1136-0047,ITC-1136-0116, ITC-1136-0115



public partial class PartType : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int DEFAULT_ROWS_PartType = 10;
    public int DEFAULT_ROWS_PartTypeAttribute = 10;
    public int DEFAULT_ROWS_SAPType = 5;
    public int DEFAULT_ROWS_Desc = 5;
    IPartTypeManager iPartTypeManager = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeManager>(WebConstant.IPartTypeManager);
    //  IPartTypeManager iPartTypeManager = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeManager>(WebConstant.IPartTypeManagerEx);
    IPartTypeManagerEx iPartTypeManagerEx = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeManagerEx>(WebConstant.IPartTypeManagerEx);
    public string editor;
    public String userName;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //      editor = UserInfo.UserId;//"itc98079";//



            if (!Page.IsPostBack)
            {

                userName = Master.userInfo.UserId;
                // userName = "IEC000043";
                this.HiddenUserName.Value = userName;

                InitLabel();
                bindPartTypeTable();
                bindAttributeTable();
                //bindMappingTable();
                bindDescTable();
            }
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
    }



    private void setPartTypeColumnWidth()
    {
        gdPartType.HeaderRow.Cells[0].Width = Unit.Pixel(10);
        gdPartType.HeaderRow.Cells[1].Width = Unit.Pixel(250);
        gdPartType.HeaderRow.Cells[2].Width = Unit.Pixel(100);
        gdPartType.HeaderRow.Cells[3].Width = Unit.Pixel(100);
        gdPartType.HeaderRow.Cells[4].Width = Unit.Pixel(100);
        gdPartType.HeaderRow.Cells[5].Width = Unit.Pixel(100);
    }

    private void setAttributeColumnWidth()
    {
        gdAttribute.HeaderRow.Cells[0].Width = Unit.Pixel(150);
        gdAttribute.HeaderRow.Cells[1].Width = Unit.Pixel(150);
        gdAttribute.HeaderRow.Cells[2].Width = Unit.Pixel(50);
        gdAttribute.HeaderRow.Cells[3].Width = Unit.Pixel(60);
        gdAttribute.HeaderRow.Cells[4].Width = Unit.Pixel(60);
    }
    /*
    private void setMappingColumnWidth()
    {
        gdSAPType.HeaderRow.Cells[1].Width = Unit.Pixel(200);
        //gdSAPType.HeaderRow.Cells[2].Width = Unit.Pixel(120);
    }*/

    private void setDescColumnWidth()
    {
        //gdDescription.HeaderRow.Cells[1].Width = Unit.Pixel(200);
        //gdDescription.HeaderRow.Cells[2].Width = Unit.Pixel(120);
    }

    protected void bindPartTypeTable()
    {
        try
        {

            //IList<PartTypeMaintainInfo> partTypeList = iPartTypeManager.GetPartTypeList();
            //IList<PartTypeMaintainInfo> partTypeList2 = iPartTypeManager.GetPartTypeList();

            IList<PartTypeMaintainInfo> partTypeList = iPartTypeManagerEx.GetPartTypeList().OrderBy(x => x.PartType).ThenBy(x => x.PartTypeGroup).ToList();
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add("ID");
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartType").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colGroup").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

            if (partTypeList != null && partTypeList.Count != 0)
            {
                foreach (PartTypeMaintainInfo temp in partTypeList)
                {
                    dr = dt.NewRow();
                    dr[0] = temp.ID;
                    dr[1] = temp.PartType;
                    dr[2] = temp.PartTypeGroup;
                    dr[3] = temp.Editor;

                    if (temp.Cdt == DateTime.MinValue)
                    {
                        dr[4] = "";
                    }
                    else
                    {
                        dr[4] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (temp.Udt == DateTime.MinValue)
                    {
                        dr[5] = "";
                    }
                    else
                    {
                        dr[5] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    dt.Rows.Add(dr);
                }

                for (int i = partTypeList.Count; i < DEFAULT_ROWS_PartType; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_PartType; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }

            gdPartType.DataSource = dt;
            gdPartType.DataBind();
            setPartTypeColumnWidth();
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
    }
    /*
    protected void bindMappingTable()
    {
        try
        {
            string PartType = txtPartType.Text;

            IList<PartTypeMappingMaintainInfo> mappingList = null;

            mappingList = iPartTypeManager.GetPartTypeMappingList(PartType);





            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add("id");
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colSAPType").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

            if (mappingList != null && mappingList.Count != 0)
            {
                foreach (PartTypeMappingMaintainInfo temp in mappingList)
                {
                    dr = dt.NewRow();

                    dr[0] = temp.ID;
                    dr[1] = temp.SAPType;
                    dr[2] = temp.Editor;

                    if (temp.Cdt == DateTime.MinValue)
                    {
                        dr[3] = "";
                    }
                    else
                    {
                        dr[3] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (temp.Udt == DateTime.MinValue)
                    {
                        dr[4] = "";
                    }
                    else
                    {
                        dr[4] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }


                    dt.Rows.Add(dr);
                }

                for (int i = mappingList.Count; i < DEFAULT_ROWS_SAPType; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_SAPType; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }
            gdSAPType.DataSource = dt;
            gdSAPType.DataBind();
            setMappingColumnWidth();
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
    }
    */
    protected void bindAttributeTable()
    {
        try
        {
            string PartType = txtPartType.Text;

            IList<PartTypeAttributeMaintainInfo> attributelist = null;

            attributelist = iPartTypeManager.GetPartTypeAttributeList(PartType);





            DataTable dt = new DataTable();
            DataRow dr = null;


            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCode").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

            if (attributelist != null && attributelist.Count != 0)
            {
                foreach (PartTypeAttributeMaintainInfo temp in attributelist)
                {
                    dr = dt.NewRow();

                    dr[0] = temp.Code;
                    dr[1] = temp.Description;
                    dr[2] = temp.Editor;

                    if (temp.Cdt == DateTime.MinValue)
                    {
                        dr[3] = "";
                    }
                    else
                    {
                        dr[3] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (temp.Udt == DateTime.MinValue)
                    {
                        dr[4] = "";
                    }
                    else
                    {
                        dr[4] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }


                    dt.Rows.Add(dr);
                }

                for (int i = attributelist.Count; i < DEFAULT_ROWS_PartTypeAttribute; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_PartTypeAttribute; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }
            gdAttribute.DataSource = dt;
            gdAttribute.DataBind();
            setAttributeColumnWidth();
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
    }
    private DataTable BuildDescNullTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("id");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        DataRow dr = null;
        for (int i = 0; i < DEFAULT_ROWS_Desc; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }
        return dt;
    }
    private DataTable BuildAttrNullTable()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCode").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
        for (int i = 0; i < DEFAULT_ROWS_PartTypeAttribute; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void bindDescTable()
    {
        try
        {
            string PartType = txtPartType.Text;

            IList<PartTypeDescMaintainInfo> desclist = null;

            desclist = iPartTypeManager.GetPartTypeDescList(PartType);





            DataTable dt = new DataTable();
            DataRow dr = null;


            dt.Columns.Add("id");
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());

            if (desclist != null && desclist.Count != 0)
            {
                foreach (PartTypeDescMaintainInfo temp in desclist)
                {
                    dr = dt.NewRow();

                    dr[0] = temp.ID;
                    dr[1] = temp.Description;

                    dt.Rows.Add(dr);
                }

                for (int i = desclist.Count; i < DEFAULT_ROWS_Desc; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_Desc; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }
            gdDescription.DataSource = dt;
            gdDescription.DataBind();
            //setDescColumnWidth();
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
    }


    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblDescDesc.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        this.lblDescList.Text = this.GetLocalResourceObject(Pre + "_lblDescList").ToString();
        this.lblCode.Text = this.GetLocalResourceObject(Pre + "_lblCode").ToString();
        this.lblGroup.Text = this.GetLocalResourceObject(Pre + "_lblGroup").ToString();
        this.lblDesc.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        //this.lblSAPType.Text = this.GetLocalResourceObject(Pre + "_lblSAPType").ToString();
        this.lblPartTypeList.Text = this.GetLocalResourceObject(Pre + "_lblPartTypeList").ToString();
        //this.lblSAPTypeList.Text = this.GetLocalResourceObject(Pre + "_lblSAPTypeList").ToString();
        this.lblPartType.Text = this.GetLocalResourceObject(Pre + "_lblPartType").ToString();
        this.lblAttribute.Text = this.GetLocalResourceObject(Pre + "_lblPartTypeAttributeList").ToString();
        this.btnDelete1.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        //this.btnDelete2.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnDelete3.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnDelete4.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave1.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        //this.btnSave2.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnSave3.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnSave4.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnAdd1.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        //this.btnAdd2.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnAdd3.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnAdd4.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();


        //setFocus();


    }



    protected void gdSAPType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");//id

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



    protected void gdAttribute_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gdPartType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");//id
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

    protected void gdDescription_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        e.Row.Cells[0].Style.Add("display", "none");//id
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

    protected void btnAddPartType_Click(Object sender, EventArgs e)
    {
        string strPartType = txtPartType.Text;
        string strPartTypeGroup = txtGroup.Text;
        string strOldPartType = hidOldPartType.Value;

        try
        {
            //  PartTypeMaintainInfo partType = new PartTypeMaintainInfo();



            iPartTypeManagerEx.AddPartType(strPartType, strPartTypeGroup, HiddenUserName.Value);
            //partType.PartType = strPartType;
            //partType.PartTypeGroup = strPartTypeGroup;
            //partType.Editor = HiddenUserName.Value;

            //

        }
        catch (FisException ex)
        {
            showErrorMessage_PartType(ex.mErrmsg, strOldPartType);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_PartType(ex.Message, strOldPartType);
            return;
        }

        bindPartTypeTable();
        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "AddSave1Complete", "AddSave1Complete(\"" + strPartType + "\");", true);
    }

    protected void btnSavePartType_Click(Object sender, EventArgs e)
    {
        string strPartType = txtPartType.Text;
        string strPartTypeGroup = txtGroup.Text;
        string strOldPartType = hidOldPartType.Value;
        string ID = hidPartTypeID.Value;
        try
        {
            PartTypeMaintainInfo partType = new PartTypeMaintainInfo();
            partType.ID = int.Parse(ID);
            partType.PartType = strPartType;
            partType.PartTypeGroup = strPartTypeGroup;
            partType.Editor = HiddenUserName.Value;

            iPartTypeManagerEx.SavePartType(partType);

        }
        catch (FisException ex)
        {
            showErrorMessage_PartType(ex.mErrmsg, strOldPartType);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_PartType(ex.Message, strOldPartType);
            return;
        }

        bindPartTypeTable();
        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "AddSave1Complete", "AddSave1Complete(\"" + strPartType + "\");", true);
    }

    protected void btnDeletePartType_Click(Object sender, EventArgs e)
    {
        string strOldPartType = hidOldPartType.Value;
        string ID = hidPartTypeID.Value;
        try
        {
            iPartTypeManagerEx.DeletePartType(ID, strOldPartType);
            gdDescription.DataSource = BuildDescNullTable();
            gdDescription.DataBind();
            gdAttribute.DataSource = BuildAttrNullTable();
            gdAttribute.DataBind();
        }
        catch (FisException ex)
        {
            showErrorMessage_PartType(ex.mErrmsg, strOldPartType);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_PartType(ex.Message, strOldPartType);
            return;
        }

        bindPartTypeTable();
        ScriptManager.RegisterStartupScript(this.updatePanel1, typeof(System.Object), "Delete1Complete", "Delete1Complete();", true);
    }
    /*
    protected void btnAddSAPType_Click(Object sender, EventArgs e)
    {
        string strSAPType = txtSAPType.Text;
        string strOldPartType = hidOldPartType.Value;
        string strSAPTypeID = hidSAPTypeID.Value;

        try
        {
            PartTypeMappingMaintainInfo SAPType = new PartTypeMappingMaintainInfo();

            SAPType.Editor = editor;
            SAPType.SAPType = strSAPType;
            SAPType.FISType = strOldPartType;

            strSAPTypeID = iPartTypeManager.AddPartTypeMapping(SAPType).ToString();

        }
        catch (FisException ex)
        {
            showErrorMessage_SAPType(ex.mErrmsg, strSAPTypeID);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_SAPType(ex.Message, strSAPTypeID);
            return;
        }

        bindMappingTable();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "AddSave2Complete", "AddSave2Complete(\"" + strSAPTypeID + "\");", true);
    }

    protected void btnSaveSAPType_Click(Object sender, EventArgs e)
    {
        string strSAPType = txtSAPType.Text;
        string strOldPartType = hidOldPartType.Value;
        string strSAPTypeID = hidSAPTypeID.Value;

        try
        {
            PartTypeMappingMaintainInfo SAPType = new PartTypeMappingMaintainInfo();

            SAPType.Editor = editor;
            SAPType.SAPType = strSAPType;
            SAPType.FISType = strOldPartType;
            SAPType.ID = Int32.Parse(strSAPTypeID);

            iPartTypeManager.SavePartTypeMapping(SAPType);

        }
        catch (FisException ex)
        {
            showErrorMessage_SAPType(ex.mErrmsg, strSAPTypeID);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_SAPType(ex.Message, strSAPTypeID);
            return;
        }

        bindMappingTable();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "AddSave2Complete", "AddSave2Complete(\"" + strSAPTypeID + "\");", true);
    }

    protected void btnDeleteSAPType_Click(Object sender, EventArgs e)
    {
        string strSAPTypeID = hidSAPTypeID.Value;

        try
        {

            iPartTypeManager.DeletePartTypeMapping(strSAPTypeID);

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

        bindMappingTable();
        ScriptManager.RegisterStartupScript(this.updatePanel2, typeof(System.Object), "Delete2Complete", "Delete2Complete();", true);
    }*/

    protected void btnAddAttribute_Click(Object sender, EventArgs e)
    {
        string strCode = txtCode.Text;
        string strDesc = txtDesc.Text;
        string strOldPartType = hidOldPartType.Value;
        string strOldCode = hidCode.Value;

        try
        {
            PartTypeAttributeMaintainInfo attribute = new PartTypeAttributeMaintainInfo();

            attribute.Code = strCode;
            attribute.Description = strDesc;
            attribute.Editor = HiddenUserName.Value;
            attribute.PartType = strOldPartType;


            iPartTypeManager.AddPartTypeAttribute(attribute);

        }
        catch (FisException ex)
        {
            showErrorMessage_Attribute(ex.mErrmsg, strOldCode);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_Attribute(ex.Message, strOldCode);
            return;
        }

        bindAttributeTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "AddSave3Complete", "AddSave3Complete(\"" + strCode + "\");", true);
    }

    protected void btnSaveAttribute_Click(Object sender, EventArgs e)
    {
        string strCode = txtCode.Text;
        string strDesc = txtDesc.Text;
        string strOldPartType = hidOldPartType.Value;
        string strOldCode = hidCode.Value;


        try
        {
            PartTypeAttributeMaintainInfo attribute = new PartTypeAttributeMaintainInfo();

            attribute.Code = strCode;
            attribute.Description = strDesc;
            // attribute.Editor = editor;
            attribute.Editor = HiddenUserName.Value;


            attribute.PartType = strOldPartType;


            iPartTypeManager.SavePartTypeAttribute(strOldCode, attribute);

        }
        catch (FisException ex)
        {
            showErrorMessage_Attribute(ex.mErrmsg, strOldCode);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_Attribute(ex.Message, strOldCode);
            return;
        }

        bindAttributeTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "AddSave3Complete", "AddSave3Complete(\"" + strCode + "\");", true);
    }

    protected void btnDeleteAttribute_Click(Object sender, EventArgs e)
    {
        string strOldPartType = hidOldPartType.Value;
        string strOldCode = hidCode.Value;
        try
        {
            iPartTypeManager.DeletePartTypeAttribute(strOldPartType, strOldCode);
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
        bindAttributeTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "Delete3Complete", "Delete3Complete();", true);
    }

    protected void btnAddDesc_Click(Object sender, EventArgs e)
    {
        string strDesc = txtDescDesc.Text;
        string strOldPartType = hidOldPartType.Value;
        string strID = hidDescDescID.Value;

        try
        {
            PartTypeDescMaintainInfo desc = new PartTypeDescMaintainInfo();

            desc.Description = strDesc;
            desc.PartType = strOldPartType;

            strID = iPartTypeManager.AddPartTypeDescription(desc).ToString();

        }
        catch (FisException ex)
        {
            showErrorMessage_Desc(ex.mErrmsg, strID);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_Desc(ex.Message, strID);
            return;
        }

        bindDescTable();
        ScriptManager.RegisterStartupScript(this.updatePanel4, typeof(System.Object), "AddSave4Complete", "AddSave4Complete(\"" + strID + "\");", true);
    }

    protected void btnSaveDesc_Click(Object sender, EventArgs e)
    {
        string strDesc = txtDescDesc.Text;
        string strOldPartType = hidOldPartType.Value;
        string strID = hidDescDescID.Value;

        try
        {
            PartTypeDescMaintainInfo desc = new PartTypeDescMaintainInfo();

            desc.Description = strDesc;
            desc.PartType = strOldPartType;
            desc.ID = Int32.Parse(strID);

            iPartTypeManager.SavePartTypeDescription(desc);

        }
        catch (FisException ex)
        {
            showErrorMessage_Desc(ex.mErrmsg, strID);
            return;
        }
        catch (Exception ex)
        {
            showErrorMessage_Desc(ex.Message, strID);
            return;
        }

        bindDescTable();
        ScriptManager.RegisterStartupScript(this.updatePanel4, typeof(System.Object), "AddSave4Complete", "AddSave4Complete(\"" + strID + "\");", true);
    }

    protected void btnDeleteDesc_Click(Object sender, EventArgs e)
    {
        string strID = hidDescDescID.Value;

        try
        {

            iPartTypeManager.DeletePartTypeDescription(strID);

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

        bindDescTable();
        ScriptManager.RegisterStartupScript(this.updatePanel4, typeof(System.Object), "Delete4Complete", "Delete4Complete();", true);
    }

    protected void btnRefreshSAPTypeAndAttributeAndDescList_Click(Object sender, EventArgs e)
    {
        bindAttributeTable();
        //bindMappingTable();
        bindDescTable();
    }





    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
        //scriptBuilder.AppendLine("clearAttributeList();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }


    private void showErrorMessage_PartType(string errorMsg, string strOldPartType)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
        scriptBuilder.AppendLine("AddSave1Complete(\"" + strOldPartType + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage1", scriptBuilder.ToString(), false);
    }

    private void showErrorMessage_SAPType(string errorMsg, string strPartTypeID)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
        scriptBuilder.AppendLine("AddSave2Complete(\"" + strPartTypeID + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage2", scriptBuilder.ToString(), false);
    }

    private void showErrorMessage_Attribute(string errorMsg, string strOldCode)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
        scriptBuilder.AppendLine("AddSave3Complete(\"" + strOldCode + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage3", scriptBuilder.ToString(), false);
    }

    private void showErrorMessage_Desc(string errorMsg, string strID)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
        scriptBuilder.AppendLine("AddSave4Complete(\"" + strID + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage4", scriptBuilder.ToString(), false);
    }

}
