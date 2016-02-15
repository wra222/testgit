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
using com.inventec.RBPC.Net.intf;
using System.Linq;



public partial class LabelSetting : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public int DEFAULT_ROWS_LabelType = 36;
    public int DEFAULT_ROWS_LabelTemplate = 5;
    private IPermissionManager permissionManager = RBPCAgent.getRBPCManager<IPermissionManager>();
    private string strSecondNodePCA = "PCA";
    private string strSecondNodeFA = "FA";
    private string strSecondNodePacking = "Packing";
    private string strSecondNodeDocking = "Docking";
    private string strSecondNodeRCTO = "RCTO";
    private string strSecondNodeCleanRoom = "CleanRoom";
    private string strChildNodePCA = "OPSA";
    private string strChildNodeFA = "OPFA";
    private string strChildNodePacking = "OPPA";
    private string strChildNodeDocking = "OPDK";
    private string strChildNodeRCTO = "OPRT";
    private string strChildNodeCleanRoom = "OPCR";

    IConstValueMaintain iConstValue = ServiceAgent.getInstance().GetMaintainObjectByName<IConstValueMaintain>(WebConstant.ConstValueMaintainObject);
    ILabelSettingManager iLabelSettingManager = ServiceAgent.getInstance().GetMaintainObjectByName<ILabelSettingManager>(WebConstant.ILabelSettingManager);
    public string editor;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            editor = Master.userInfo.UserId;
            this.hiddenUsername.Value = editor;
            if (!Page.IsPostBack)
            {
                InitLabel();
                bindLabelTypeTable();
                bindLabelTemplateTable();
                InitPrintModeSelect();
                InitRuleModeSelect();
                TreeView1.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                TreeNodePopulate(TreeView1.Nodes);
                TreeView1.ExpandAll();
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

    #region 初始化各下拉控件
    protected void InitPrintModeSelect()
    {
        string value1 = this.GetLocalResourceObject(Pre + "_CmbPrintModeItemValue1").ToString();
        string value2 = this.GetLocalResourceObject(Pre + "_CmbPrintModeItemValue2").ToString();
        string value3 = this.GetLocalResourceObject(Pre + "_CmbPrintModeItemValue3").ToString();
		string valueBartender = this.GetLocalResourceObject(Pre + "_CmbPrintModeItemValueBartender").ToString();
        string valueBartendersrv = this.GetLocalResourceObject(Pre + "_CmbBartenderSRV").ToString();
        selPrintMode.Items.Add(new ListItem(value1, "0"));
        selPrintMode.Items.Add(new ListItem(value2, "1"));
        selPrintMode.Items.Add(new ListItem(value3, "2"));
		selPrintMode.Items.Add(new ListItem(valueBartender, "3"));
        selPrintMode.Items.Add(new ListItem(valueBartendersrv, "4"));
    }

    protected void InitRuleModeSelect()
    {
        string value1 = this.GetLocalResourceObject(Pre + "_CmbRuleModeItemValue1").ToString();
        string value2 = this.GetLocalResourceObject(Pre + "_CmbRuleModeItemValue2").ToString();
        string value3 = this.GetLocalResourceObject(Pre + "_CmbRuleModeItemValue3").ToString();
        selRuleMode.Items.Add(new ListItem(value1, "0"));
        selRuleMode.Items.Add(new ListItem(value2, "1"));
        selRuleMode.Items.Add(new ListItem(value3, "2"));
    }


    #endregion


    #region  gridview数据绑定与页面控件初始化
    /// <summary>
    /// 获取LabelType数据列表并绑定到gridview
    /// </summary>
    protected void bindLabelTypeTable()
    {
        try
        {
            IList<LabelTypeMaintainInfo> labelTypeList = iLabelSettingManager.getLableTypeList();
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("printmodevalue");
            dt.Columns.Add("rulemodevalue");
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colLabelType").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPrintMode").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colRuleMode").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

            if (labelTypeList != null && labelTypeList.Count != 0)
            {
                foreach (LabelTypeMaintainInfo temp in labelTypeList)
                {
                    dr = dt.NewRow();
                    dr[0] = temp.PrintMode;
                    dr[1] = temp.RuleMode;
                    dr[2] = temp.LabelType;
                    if (temp.PrintMode == 0)//Bat:0
                    {
                        dr[3] = this.GetLocalResourceObject(Pre + "_CmbPrintModeItemValue1").ToString();
                    }
                    else if (temp.PrintMode == 1)
                    {//moban:1
                        dr[3] = this.GetLocalResourceObject(Pre + "_CmbPrintModeItemValue2").ToString();
                    }
					else if (temp.PrintMode == 3)
                    {
                        dr[3] = this.GetLocalResourceObject(Pre + "_CmbPrintModeItemValueBartender").ToString();
                    }
                    else if (temp.PrintMode == 4)
                    {
                        dr[3] = this.GetLocalResourceObject(Pre + "_CmbBartenderSRV").ToString();
                    }
                    else
                    {
                        dr[3] = this.GetLocalResourceObject(Pre + "_CmbPrintModeItemValue3").ToString();
                    }
                    if (temp.RuleMode == 0)//MO:0
                    {
                        dr[4] = this.GetLocalResourceObject(Pre + "_CmbRuleModeItemValue1").ToString();
                    }
                    else if (temp.RuleMode == 1)//PO:1
                    {
                        dr[4] = this.GetLocalResourceObject(Pre + "_CmbRuleModeItemValue2").ToString();
                    }
                    else //Part
                    {
                        dr[4] = this.GetLocalResourceObject(Pre + "_CmbRuleModeItemValue3").ToString();
                    }
                    dr[5] = temp.Description;
                    dr[6] = temp.Editor;

                    if (temp.Cdt == DateTime.MinValue)
                    {
                        dr[7] = "";
                    }
                    else
                    {
                        dr[7] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (temp.Udt == DateTime.MinValue)
                    {
                        dr[8] = "";
                    }
                    else
                    {
                        dr[8] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    dt.Rows.Add(dr);
                }

                for (int i = labelTypeList.Count; i < DEFAULT_ROWS_LabelType; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_LabelType; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }

            gdLabelType.DataSource = dt;
            gdLabelType.DataBind();
            setColumnWidthLabelType();
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
    /// 获取lbelTemplate列表并绑定到gridview
    /// </summary>
    protected void bindLabelTemplateTable()
    {
        try
        {
            string strLabelType = txtLabelType.Text;

            IList<PrintTemplateMaintainInfo> printTemplateList = null;
            printTemplateList = iLabelSettingManager.getPrintTemplateListByLableType(strLabelType);
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colTemplateName").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPiece").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colSPName").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colLayout").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
            dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());
            if (printTemplateList != null && printTemplateList.Count != 0)
            {
                foreach (PrintTemplateMaintainInfo temp in printTemplateList)
                {
                    dr = dt.NewRow();

                    dr[0] = temp.TemplateName;
                    dr[1] = temp.Piece;
                    dr[2] = temp.SpName;
                    if (temp.Layout == 0)
                    {
                        dr[3] = "Portrait";
                    }
                    else
                    {
                        dr[3] = "Landscape";
                    }
                    dr[4] = temp.Description;
                    dr[5] = temp.Editor;

                    if (temp.Cdt == DateTime.MinValue)
                    {
                        dr[6] = "";
                    }
                    else
                    {
                        dr[6] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss"); //temp.Cdt; 
                    }

                    if (temp.Udt == DateTime.MinValue)
                    {
                        dr[7] = "";
                    }
                    else
                    {
                        dr[7] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    dt.Rows.Add(dr);
                }

                for (int i = printTemplateList.Count; i < DEFAULT_ROWS_LabelTemplate; i++)
                {
                    dt.Rows.Add(dt.NewRow());
                }
            }
            else
            {
                for (int i = 0; i < DEFAULT_ROWS_LabelTemplate; i++)
                {
                    dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }
            gdLabelTemplate.DataSource = dt;
            gdLabelTemplate.DataBind();
            setColumnWidthLabelTemplate();
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
        //this.lblRelatedFunction.Text = this.GetLocalResourceObject(Pre + "_lblRelatedFunction").ToString();
        this.lblLabelType.Text = this.GetLocalResourceObject(Pre + "_lblLabelType").ToString();
        this.lblPrintMode.Text = this.GetLocalResourceObject(Pre + "_lblPrintMode").ToString();
        this.lblRuleMode.Text = this.GetLocalResourceObject(Pre + "_lblRuleMode").ToString();
        this.lblDescription1.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        this.lblLabelTypeList.Text = this.GetLocalResourceObject(Pre + "_lblLabelTypeList").ToString();
        this.lblTemplateName.Text = this.GetLocalResourceObject(Pre + "_lblTemplateName").ToString();
        this.lblPiece.Text = this.GetLocalResourceObject(Pre + "_lblPiece").ToString();
        this.lblSPName.Text = this.GetLocalResourceObject(Pre + "_lblSPName").ToString();
        this.lblDescription3.Text = this.GetLocalResourceObject(Pre + "_lblDescription").ToString();
        this.lblLabelTemplateList.Text = this.GetLocalResourceObject(Pre + "_lblLabelTemplateList").ToString();
        this.lblRelatedFunction.Text = this.GetLocalResourceObject(Pre + "_lblRelatedFunction").ToString();
        this.btnDelete1.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnDelete3.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave1.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnSave2.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnSave3.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        this.btnRuleSetting.Value = this.GetLocalResourceObject(Pre + "_btnRuleSetting").ToString();
        this.lblLayout.Text = this.GetLocalResourceObject(Pre+"_lblLayout").ToString();


    }


    /// <summary>
    /// gridview的template数据绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gdLabelTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
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


    /// <summary>
    /// labeltype的gridview数据绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gdLabelType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Style.Add("display", "none");//printmode
        e.Row.Cells[1].Style.Add("display", "none");//rulemode

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

    private void setColumnWidthLabelType()
    {
        gdLabelType.HeaderRow.Cells[2].Width = Unit.Percentage(19);
        gdLabelType.HeaderRow.Cells[3].Width = Unit.Percentage(8);
        gdLabelType.HeaderRow.Cells[4].Width = Unit.Percentage(8);
        gdLabelType.HeaderRow.Cells[5].Width = Unit.Percentage(15);
        gdLabelType.HeaderRow.Cells[6].Width = Unit.Percentage(10);
        gdLabelType.HeaderRow.Cells[7].Width = Unit.Percentage(20);
        gdLabelType.HeaderRow.Cells[8].Width = Unit.Percentage(20);
    }

    private void setColumnWidthLabelTemplate()
    {
        gdLabelTemplate.HeaderRow.Cells[0].Width = Unit.Percentage(17);
        gdLabelTemplate.HeaderRow.Cells[1].Width = Unit.Percentage(5);
        gdLabelTemplate.HeaderRow.Cells[2].Width = Unit.Percentage(18);
        gdLabelTemplate.HeaderRow.Cells[3].Width = Unit.Percentage(7);
        gdLabelTemplate.HeaderRow.Cells[4].Width = Unit.Percentage(10);
        gdLabelTemplate.HeaderRow.Cells[5].Width = Unit.Percentage(8);
        gdLabelTemplate.HeaderRow.Cells[6].Width = Unit.Percentage(15);
        gdLabelTemplate.HeaderRow.Cells[7].Width = Unit.Percentage(15);
    }


    /// <summary>
    /// 绑定tree
    /// </summary>
    private void bindTree()
    {
        try
        {
            //清空所有checkbox
            for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
            {
                TreeView1.CheckedNodes[i].Checked = false;
                i--;
            }
            TreeView1.Nodes[0].Checked = false;

            if (hidLabelType.Value.Equals(""))
            {
                return;
            }
            //取得高亮group对应的所有的permission
            IList<PCodeLabelTypeMaintainInfo> lstPCode = iLabelSettingManager.getPCodeByLabelType(hidLabelType.Value);
            IList<ConstValueInfo> Listfortree = iConstValue.GetConstValueListByType("LabelSettingFunction");


            //循环permission，如果有permission name与treeview节点相同，则check该节点
            if (lstPCode != null && lstPCode.Count != 0)
            {
                foreach (PCodeLabelTypeMaintainInfo temp in lstPCode)
                {
                    TreeNode tmp = null;
                    foreach (ConstValueInfo items in Listfortree)
                    {
                        if (temp.PCode.IndexOf(items.value) == 0)
                        {
                            tmp = TreeView1.FindNode("Operation/" + items.name + "/" + temp.PCode);
                        }
                    }


                    //if (temp.PCode.IndexOf(strChildNodePCA) == 0)
                    //{
                    //    tmp = TreeView1.FindNode("Operation/" + strSecondNodePCA + "/" + temp.PCode);
                    //}
                    //else if (temp.PCode.IndexOf(strChildNodeFA) == 0)
                    //{
                    //    tmp = TreeView1.FindNode("Operation/" + strSecondNodeFA + "/" + temp.PCode);
                    //}
                    //else if (temp.PCode.IndexOf(strChildNodePacking) == 0)
                    //{
                    //    tmp = TreeView1.FindNode("Operation/" + strSecondNodePacking + "/" + temp.PCode);
                    //}
                    //else if (temp.PCode.IndexOf(strChildNodeDocking) == 0)
                    //{
                    //    tmp = TreeView1.FindNode("Operation/" + strSecondNodeDocking + "/" + temp.PCode);
                    //}
                    //else if (temp.PCode.IndexOf(strChildNodeRCTO) == 0)
                    //{
                    //    tmp = TreeView1.FindNode("Operation/" + strSecondNodeRCTO + "/" + temp.PCode); 
                    //}
                    //else 
                    //{
                    //    tmp = TreeView1.FindNode("Operation/" + strSecondNodeCleanRoom + "/" + temp.PCode);
                    //}




                    if (tmp != null)
                    {
                        tmp.Checked = true;
                        if (!tmp.Parent.Equals(null))
                        {
                            tmp.Parent.Checked = true;
                            for (int j = 0; j < tmp.Parent.ChildNodes.Count; j++)
                            {
                                if (!tmp.Parent.ChildNodes[j].Checked)
                                {
                                    tmp.Parent.Checked = false;
                                    break;
                                }
                            }

                            if (tmp.Parent.Checked == true)
                            {
                                tmp = tmp.Parent;
                                if (!tmp.Parent.Equals(null))
                                {
                                    tmp.Parent.Checked = true;
                                    for (int j = 0; j < tmp.Parent.ChildNodes.Count; j++)
                                    {
                                        if (!tmp.Parent.ChildNodes[j].Checked)
                                        {
                                            tmp.Parent.Checked = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
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

    #endregion


    #region 增删改查方法

    /// <summary>
    /// labelType的保存按钮按下时触发的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveLabelType_Click(Object sender, EventArgs e)
    {
        try
        {
            LabelTypeMaintainInfo tmpLabelTypeInfo = new LabelTypeMaintainInfo();


            tmpLabelTypeInfo.LabelType = txtLabelType.Text;
            tmpLabelTypeInfo.PrintMode = Int32.Parse(selPrintMode.SelectedValue);
            tmpLabelTypeInfo.RuleMode = Int32.Parse(selRuleMode.SelectedValue);
            tmpLabelTypeInfo.Description = txtDescription1.Text;
            tmpLabelTypeInfo.Editor = editor;

            //如果txtLabelType内容没变， 表示将来需要通过hidLabelType保存
            if (hidLabelType.Value == txtLabelType.Text)
            {
                iLabelSettingManager.SaveLabelType(tmpLabelTypeInfo);
            }
            //否则，做新增
            else
            {
                iLabelSettingManager.AddLabelType(tmpLabelTypeInfo);
            }


            bindLabelTypeTable();
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
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Add1Complete", "Add1Complete(\"" + txtLabelType.Text + "\"); resetTableHeight();", true);
    }

    /// <summary>
    /// labelType的删除按钮按下时触发的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteLabelType_Click(Object sender, EventArgs e)
    {
        string strLabelType = hidLabelType.Value;
        try
        {

            iLabelSettingManager.DeleteLabelType(strLabelType);

            bindLabelTypeTable();

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
        this.updatePanel1.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Delete1Complete", "Delete1Complete();resetTableHeight();", true);
    }


    /// <summary>
    /// lableTempLate的保存按钮按下时触发的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveLabelTemplate_Click(Object sender, EventArgs e)
    {
        try
        {
            PrintTemplateMaintainInfo tmpPrintTemplateInfo = new PrintTemplateMaintainInfo();
            tmpPrintTemplateInfo.LabelType = hidLabelType.Value;
            tmpPrintTemplateInfo.Piece = Int32.Parse(txtPiece.Text);
            tmpPrintTemplateInfo.SpName = txtSPName.Text;
            tmpPrintTemplateInfo.TemplateName = txtTemplateName.Text;
            if (this.drpLayout.SelectedIndex == 0)
            {
                tmpPrintTemplateInfo.Layout = 0;
            }
            else
            {
                tmpPrintTemplateInfo.Layout = 1;
            }
            tmpPrintTemplateInfo.Description = txtDescription3.Text;
            tmpPrintTemplateInfo.Editor = editor;
            //如果Template内容没变， 表示将来需要通过hidTemplateName保存
            if (hidTemplateName.Value == txtTemplateName.Text)
            {
                iLabelSettingManager.SavePrintTemplate(tmpPrintTemplateInfo);
            }
            //否则，做新增
            else
            {
                iLabelSettingManager.AddPrintTemplate(tmpPrintTemplateInfo);
            }


            bindLabelTemplateTable();
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
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Add3Complete", "Add3Complete(\"" + txtTemplateName.Text + "\"); resetTableHeight();DealHideWait();", true);
    }

    /// <summary>
    /// labelTemplate的删除按钮按下时触发的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteLabelTemplate_Click(Object sender, EventArgs e)
    {
        string strTemplateName = hidTemplateName.Value;
        try
        {

            iLabelSettingManager.DeletePrintTemplate(strTemplateName);

            bindLabelTemplateTable();

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
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Delete3Complete", "Delete3Complete(); resetTableHeight();", true);
    }

    /// <summary>
    /// functionTree保存按钮按下时触发的事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnSaveTree_Click(Object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
            {
                if (TreeView1.CheckedNodes[i].ChildNodes.Count > 0)
                {
                    TreeView1.CheckedNodes.RemoveAt(i);
                    i--;
                }

            }

            if (TreeView1.CheckedNodes.Count == 0)
            {
                //return;
            }

            IList<string> lstCheckedNode = new List<string>();
            for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
            {
                lstCheckedNode.Add(TreeView1.CheckedNodes[i].Value);
            }

            PCodeLabelTypeMaintainInfo pCodeLabelType = new PCodeLabelTypeMaintainInfo();
            pCodeLabelType.LabelType = hidLabelType.Value;
            iLabelSettingManager.SavePCode(lstCheckedNode, pCodeLabelType);
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
        this.updatePanel1.Update();
        this.updatePanel2.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Add1Complete", "Add1Complete(\"" + txtLabelType.Text + "\"); resetTableHeight();DealHideWait();", true);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRefreshLabelTemplateListAndTree_Click(Object sender, EventArgs e)
    {
        bindTree();
        bindLabelTemplateTable();
        this.updatePanel2.Update();
        this.updatePanel3.Update();
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "Delete3Complete", "Delete3Complete();", true);

    }

    #endregion


    #region Node相关处理
    //Populate tree node
    public void TreeNodePopulate(TreeNodeCollection nodes)
    {
        //Parents nodes
        //treeNodes = authorManager.getAllPermission();

        //treeNodes = authMgr.GetAllPermissionList();
        CreateRootNode(nodes);
        //Children nodes
        //DataTable treeNodeChildren = new DataTable();
        //treeNodeChildren = authMgr.getAllPermission();
        //CreateTreeNode(treeNodeChildren, node);
    }

    //Create root node
    private void CreateRootNode(TreeNodeCollection nodes)
    {
        String Name = "Operation";
        TreeNode rootNode = new TreeNode(Name, Name);
        rootNode.NavigateUrl = "#";
        nodes.Add(rootNode);

        IList<ConstValueInfo> Listfortree = iConstValue.GetConstValueListByType("LabelSettingFunction");
        var s = (from t in Listfortree
                 orderby t.description
                 select t).ToList();

        foreach (ConstValueInfo items in s)
        {
            CreateSecondNode(items.name, items.value, rootNode);
        }
        


        

        //CreateSecondNode(strSecondNodePCA, strChildNodePCA, rootNode);
        //CreateSecondNode(strSecondNodeFA, strChildNodeFA, rootNode);
        //CreateSecondNode(strSecondNodePacking, strChildNodePacking, rootNode);
        //CreateSecondNode(strSecondNodeDocking, strChildNodeDocking, rootNode);
        //CreateSecondNode(strSecondNodeRCTO, strChildNodeRCTO, rootNode);
        //CreateSecondNode(strSecondNodeCleanRoom, strChildNodeCleanRoom, rootNode);

    }

    //strOP="OPSA","OPFA","OPPA" = SqlName
    public void CreateSecondNode(String treeName, String SqlName, TreeNode parent)
    {
        TreeNode newNode = new TreeNode(treeName, treeName);
        newNode.NavigateUrl = "#";
        parent.ChildNodes.Add(newNode);
        CreateTreeNode(SqlName, newNode);
    }

    //Create tree node
    public void CreateTreeNode(String SqlName, TreeNode node)
    {
        DataTable dtTreeNodes = permissionManager.GetPermissionByOP(SqlName);

        foreach (DataRow dr in dtTreeNodes.Select("","name"))
        {
            string name = dr["name"].ToString();
            string descr = dr["descr"].ToString();

            TreeNode newNode = new TreeNode(name, descr);
            newNode.PopulateOnDemand = false;
            newNode.NavigateUrl = "#";
            node.ChildNodes.Add(newNode);
        }

    }

    #endregion


    #region 一些系统方法

    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    #region 原页面的方法
    //private void showErrorMessage_Add1Complete(string errorMsg, string strLabelType)
    //{
    //    StringBuilder scriptBuilder = new StringBuilder();

    //    string oldErrorMsg = errorMsg;
    //    errorMsg = errorMsg.Replace("\r\n", "<br>");
    //    errorMsg = errorMsg.Replace("\"", "\\\"");

    //    oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
    //    oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

    //    scriptBuilder.AppendLine("<script language='javascript'>");
    //    scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
    //    scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
    //    scriptBuilder.AppendLine("Add1Complete(\"" + strLabelType + "\");");
    //    scriptBuilder.AppendLine("</script>");

    //    ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage1", scriptBuilder.ToString(), false);
    //}

    //private void showErrorMessage_Add3Complete(string errorMsg, string strLabelType)
    //{
    //    StringBuilder scriptBuilder = new StringBuilder();

    //    string oldErrorMsg = errorMsg;
    //    errorMsg = errorMsg.Replace("\r\n", "<br>");
    //    errorMsg = errorMsg.Replace("\"", "\\\"");

    //    oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
    //    oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

    //    scriptBuilder.AppendLine("<script language='javascript'>");
    //    scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
    //    scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
    //    scriptBuilder.AppendLine("Add3Complete(\"" + strLabelType + "\");");
    //    scriptBuilder.AppendLine("</script>");

    //    ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage1", scriptBuilder.ToString(), false);
    //}

    //private void showErrorMessage_SAPType(string errorMsg, string strPartTypeID)
    //{
    //    StringBuilder scriptBuilder = new StringBuilder();

    //    string oldErrorMsg = errorMsg;
    //    errorMsg = errorMsg.Replace("\r\n", "<br>");
    //    errorMsg = errorMsg.Replace("\"", "\\\"");

    //    oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
    //    oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

    //    scriptBuilder.AppendLine("<script language='javascript'>");
    //    scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
    //    scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");");
    //    scriptBuilder.AppendLine("AddSave2Complete(\"" + strPartTypeID + "\");");
    //    scriptBuilder.AppendLine("</script>");

    //    ScriptManager.RegisterStartupScript(this.updatePanel5, typeof(System.Object), "showErrorMessage2", scriptBuilder.ToString(), false);
    //}
    #endregion

    private static String replaceSpecialChart(String sourceData)
    {
        if (sourceData == null)
        {
            return "";
        }
        sourceData = sourceData.Replace("'", "\\'");
        return sourceData;
    }
    #endregion

}
