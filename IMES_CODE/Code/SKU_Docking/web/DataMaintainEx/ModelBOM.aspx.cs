﻿/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: ModelBOM Maintain default page
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ================ =====================================
 * * Known issues:
 * 
 */
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using log4net;
using com.inventec.iMESWEB;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure;
using System.Text;
using IMES.DataModel;
using MaintainControl;
using System.Collections;
using System.Collections.Generic;

public partial class DataMaintain_ModelBOM : System.Web.UI.Page
{
    public String userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 14;
    private IBOMNodeData iModelBOM;
    private const int COL_NUM = 11;

    public string pmtMessage1;
    public string pmtMessage2;
    public string pmtMessage3;
    public string pmtMessage4;
    public string pmtMessage5;
    public string pmtMessage6;
    public string pmtMessage7;
    public string pmtMessage8;
    public string pmtMessage9;
    public string pmtMessage10;
    public string pmtMessage11;
    public string pmtMessage12;
    public string pmtMessage13;
    public string pmtMessage14;
    public string pmtMessage15;


    public string pmtModel1;
    public string pmtModel2;
    public int idIndex;

    IPartTypeManagerEx iPartTypeManagerEx = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeManagerEx>(WebConstant.IPartTypeManagerEx);
    IPartManagerEx iPartManagerEx = ServiceAgent.getInstance().GetMaintainObjectByName<IPartManagerEx>(WebConstant.IPartManagerEx);

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            pmtMessage1 = this.GetLocalResourceObject(Pre + "_pmtMessage1").ToString();
            pmtMessage2 = this.GetLocalResourceObject(Pre + "_pmtMessage2").ToString();
            pmtMessage3 = this.GetLocalResourceObject(Pre + "_pmtMessage3").ToString();
            pmtMessage4 = this.GetLocalResourceObject(Pre + "_pmtMessage4").ToString();
            pmtMessage5 = this.GetLocalResourceObject(Pre + "_pmtMessage5").ToString();
            pmtMessage6 = this.GetLocalResourceObject(Pre + "_pmtMessage6").ToString();
            pmtMessage7 = this.GetLocalResourceObject(Pre + "_pmtMessage7").ToString();
            pmtMessage8 = this.GetLocalResourceObject(Pre + "_pmtMessage8").ToString();
            pmtMessage9 = this.GetLocalResourceObject(Pre + "_pmtMessage9").ToString();
            pmtMessage10 = this.GetLocalResourceObject(Pre + "_pmtMessage10").ToString();
            pmtMessage11 = this.GetLocalResourceObject(Pre + "_pmtMessage11").ToString();
            pmtMessage12 = this.GetLocalResourceObject(Pre + "_pmtMessage12").ToString();
            pmtMessage13 = this.GetLocalResourceObject(Pre + "_pmtMessage13").ToString();
            pmtMessage14 = this.GetLocalResourceObject(Pre + "_pmtMessage14").ToString();
            pmtMessage15 = this.GetLocalResourceObject(Pre + "_pmtMessage15").ToString();

            InitClientFunc();
            if (!this.IsPostBack)
            {
                userName = Master.userInfo.UserId;
                initLabel();
                bindTable(null, DEFAULT_ROWS);
                setColumnWidth();
                idIndex = 0;
                SaveTreeIdIndex();
                
                //initTreeView();
                GetPartTypeList();
            }
            //this.dTree.Attributes.Add("onclick", "OnTreeNodeChange");
            idIndex = Int32.Parse(this.dTreeIdIndexMax.Value);
            iModelBOM = (IBOMNodeData)ServiceAgent.getInstance().GetMaintainObjectByName<IBOMNodeData>(WebConstant.IBOMNodeData);
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

    private void SaveTreeIdIndex()
    {
        this.dTreeIdIndexMax.Value = idIndex.ToString();
    }

    private void initLabel()
    {
        this.lblModelPN.Text= this.GetLocalResourceObject(Pre + "_lblModelPN").ToString();
        this.lblBOMTree.Text=this.GetLocalResourceObject(Pre + "_lblBOMTree").ToString();

        this.btnShowBOM.InnerText=this.GetLocalResourceObject(Pre + "_btnShowBOM").ToString();
        this.btnSaveAs.InnerText=this.GetLocalResourceObject(Pre+"_btnSaveAs").ToString();
        this.btnSaveAsDummy.InnerText = "Save As Dummy";
        this.btnApproveModel.InnerText=this.GetLocalResourceObject(Pre+"_btnApproveModel").ToString();
        this.btnRefreshMOBOM.InnerText=this.GetLocalResourceObject(Pre+"_btnRefreshMOBOM").ToString();
        this.btnFind.InnerText=this.GetLocalResourceObject(Pre+"_btnFind").ToString();

        pmtModel1=this.GetLocalResourceObject(Pre+"_pmtModel1").ToString();
        pmtModel2=this.GetLocalResourceObject(Pre+"_pmtModel2").ToString();

        this.btnGroupNo.InnerText=this.GetLocalResourceObject(Pre+"_btnGroupNo").ToString();
        this.btnExp.InnerText=this.GetLocalResourceObject(Pre+"_btnExp").ToString();
        this.btnRep.InnerText=this.GetLocalResourceObject(Pre+"_btnRep").ToString();
        this.btnParent.InnerText=this.GetLocalResourceObject(Pre+"_btnParent").ToString();
        this.btnAllParent.InnerText=this.GetLocalResourceObject(Pre+"_btnAllParent").ToString();
        this.btnToExcel.InnerText=this.GetLocalResourceObject(Pre+"_btnToExcel").ToString();

        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.InnerText = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        
        this.lblPartNo.Text = this.GetLocalResourceObject(Pre + "_lblPartNo").ToString();
        this.lblQty.Text = this.GetLocalResourceObject(Pre + "_lblQty").ToString();
        this.lblPriority.Text = this.GetLocalResourceObject(Pre + "_lblPriority").ToString();
        //this.lblType.Text = this.GetLocalResourceObject(Pre + "_lblType").ToString();

        this.lblPartType.Text = this.GetLocalResourceObject(Pre + "_lblPartTypeText").ToString();
    }

    protected void btnTreeNodeChange_ServerClick(Object sender, EventArgs e)
    {
        string value = this.dSelectedTreeNodeId.Value.Trim();
        myTreeNode itemNode = findTreeNode(value);
        if (itemNode == null)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "HideWait();", true);
            return;
        }
        
        List<string> idList=new List<string> ();
        for (int i = 0; i < itemNode.ChildNodes.Count; i++)
        {
            string id=((myTreeNode)itemNode.ChildNodes[i]).Id;
            idList.Add(id);
        }
        ShowListByTreeNode(idList);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "HideWait();ShowRowEditInfo();", true);
        this.updatePanel1.Update();
        this.updatePanel2.Update();

    }

    private Boolean ShowListByTreeNode(List<string> idList)
    {
       
        try
        {
            DataTable dataList;
            dataList = iModelBOM.GetSubModelBOMList(idList);
            if (dataList == null || dataList.Rows.Count == 0)
            {
                bindTable(null, DEFAULT_ROWS);
            }
            else
            {
                bindTable(dataList, DEFAULT_ROWS);
            }
        }
        catch (FisException ex)
        {
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.mErrmsg);
            return false;
        }
        catch (Exception ex)
        {
            //show error
            bindTable(null, DEFAULT_ROWS);
            showErrorMessage(ex.Message);
            return false;
        }
        return true;

    }

    //根据树的node的value查找节点，包括根节点
    private myTreeNode findTreeNode(string value)
    {
        List<myTreeNode> findResult = ((MaintainTreeViewExt)this.dTree).FindMatchTreeNodeList("");
        for (int i = 0; i < findResult.Count(); i++)
        {
            if (findResult[i].Value == value)
            {
                return (myTreeNode)findResult[i];
            }
        }
        if (this.dTree.Nodes.Count>0 && this.dTree.Nodes[0].Value == value)
        {
            return (myTreeNode)this.dTree.Nodes[0];
        }
        return null;
    }

    //根据树的node的current值查找节点，包括根节点
    //返回所有符合条件的节点
    private List<myTreeNode> findTreeNodeByPartNo(string value)
    {
        List<myTreeNode> result = new List<myTreeNode>();

        List<myTreeNode> findResult = ((MaintainTreeViewExt)this.dTree).FindMatchTreeNodeList("");
        for (int i = 0; i < findResult.Count(); i++)
        {
            if (((myTreeNode)findResult[i]).Current == value)
            {
                result.Add( (myTreeNode)findResult[i]);
            }
        }
        if (this.dTree.Nodes.Count>0 && ((myTreeNode)this.dTree.Nodes[0]).Current == value)
        {
             result.Add((myTreeNode)this.dTree.Nodes[0]);
        }
        return result;
    }

    //取得和显示树
    protected void btnPartNoChange_ServerClick(Object sender, EventArgs e)
    {
        string ModelPartNo = this.dModelPN.Text.Trim().ToUpper();
        InputMaterialInfoDef inputModelPartInfo;

        try
        {
            //检查是否合法的model、part
            inputModelPartInfo = iModelBOM.GetMaterialInfo(ModelPartNo);
            this.dOldPartNo.Value = ModelPartNo;
            SetApproveState(inputModelPartInfo);
            
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
        InitTreeView(inputModelPartInfo);
        bindTable(null, DEFAULT_ROWS);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "HideWait();initSelectTreeNode();DeleteComplete();", true);
        this.updatePanel.Update();
        this.updatePanel3.Update();
        this.updatePanel4.Update();
        this.updatePanel1.Update();
        this.updatePanel2.Update();
        
    }

    private void SetApproveState(InputMaterialInfoDef modelPartInfo)
    {
        if (modelPartInfo.IsModel == "True")
        {
            if (modelPartInfo.BOMApproveDate == "")
            {
                this.btnApproveModel.Disabled = false;
                this.lblApprove.Text = "";
            }
            else
            {
                this.btnApproveModel.Disabled = true;
                this.lblApprove.Text = this.GetLocalResourceObject(Pre + "_pmtModel1").ToString() + " " + modelPartInfo.Editor + " " + this.GetLocalResourceObject(Pre + "_pmtModel2").ToString() + " " + modelPartInfo.BOMApproveDate + ".";
            }
        }
        else
        {
            this.btnApproveModel.Disabled = true;
            this.lblApprove.Text = "";
        }

    }

    //操作过此函数后，最终需要调SaveTreeIdIndex
    private String GetTreeIdIndex()
    {                        
        idIndex=idIndex+1;
        return idIndex.ToString();
    }
    //操作过此函数后，最终需要调SaveTreeIdIndex
    private String GetNodeText(string current, string desc, string value)
    {
        string description = desc.Trim();

        if (description != "")
        {
            description = "(" + description + ")";
        }
        string text = current + description;

        string partNo=replaceSpecialChart(current);
        //return text;
        return "<span id=\"dNode" + value + "\" class='Maintain_TreeNodeNomal' onclick=\"OnTreeNodeChange('" + value + "','" + partNo + "')\">" + text + "</span>";

    }

   
    private void InitTreeView(InputMaterialInfoDef rootModelPartInfo)
    {
        //取得的顺序
        //public string id;
        //public string parent;
        //public string curent;
        //public string desc;
        //public bool isPart;
        //public bool isModel;
        //public int level;
        try
        {
            this.dTree.Nodes.Clear();
            myTreeNode root = GetTreeNodeByData(rootModelPartInfo.PartNo);
            root.IsModel = rootModelPartInfo.IsModel; 
            root.IsPart = rootModelPartInfo.IsPart;   
            root.Desc = rootModelPartInfo.Description;
            //////
            this.dTree.Nodes.Add(root);
            this.dTree.ExpandAll();
            SaveTreeIdIndex();
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

    private myTreeNode GetTreeNodeByData(string PartNo)
    {
        myTreeNode root = new myTreeNode();
        ExtendDictionary<String, myTreeNode> treeNodeInfo = new ExtendDictionary<string, myTreeNode>();
        DataTable result = iModelBOM.GetTreeTable(PartNo);

        for (int i = 0; i < result.Rows.Count; i++)
        {

            string id = Null2String(result.Rows[i][0]);
            string parent = Null2String(result.Rows[i][1]);
            string current = Null2String(result.Rows[i][2]);
            string desc = Null2String(result.Rows[i][7] + ", " + result.Rows[i][8] + ", " + result.Rows[i][3]); //BomNodeType, PartType, Descr


            myTreeNode currentNode = (myTreeNode)treeNodeInfo.Get(current);
            myTreeNode parentNode = (myTreeNode)treeNodeInfo.Get(parent);

            //处理子节点
            if (currentNode == null)
            {
                currentNode = new myTreeNode();
                currentNode.Desc = desc;
                currentNode.Id = id;
                currentNode.Current = current;
                currentNode.Value = GetTreeIdIndex();
                currentNode.Text = GetNodeText(currentNode.Current, currentNode.Desc, currentNode.Value);
                currentNode.NavigateUrl = GetURLString(currentNode.Value);
                treeNodeInfo.Add(currentNode.Current, currentNode);

            }
            else
            {
                //没有当过子节点
                if (currentNode.Id == null || currentNode.Id == "")
                {
                    currentNode.Desc = desc;
                    currentNode.Id = id;
                    currentNode.Current = current;
                    currentNode.Text = GetNodeText(currentNode.Current, currentNode.Desc, currentNode.Value);
                }
                else
                {

                    currentNode = CloneTreeNode(currentNode);
                    //新拷贝的节点拥有不同的value和id
                    currentNode.Id = id;
                }

            }

            //处理父节点
            if (parentNode == null)
            {
                parentNode = new myTreeNode();
                parentNode.Current = parent;
                parentNode.Value = GetTreeIdIndex();
                parentNode.NavigateUrl = GetURLString(parentNode.Value);
                treeNodeInfo.Add(parentNode.Current, parentNode);
            }

            parentNode.ChildNodes.Add(currentNode);
        }

        //根节点加入树
        if (treeNodeInfo.ContainsKey(PartNo))
        {
            root = (myTreeNode)treeNodeInfo.Get(PartNo);
        }
        root.Current = PartNo;
        root.Id = "";   
        root.Material = "";         //
        root.Value = GetTreeIdIndex();
        root.NavigateUrl = GetURLString(root.Value);
        root.Text = GetNodeText(root.Current, root.Desc, root.Value);
        return root;
    }

    private string GetURLString(string value)
    {
        return "javascript:OnSelectNode('" + value + "')";
    }

    // 将当前节点装入子节点的信息
    //targetNode, 当前节点
    //srcNode，被拷贝的节点
    //返回 当前节点
    private myTreeNode CloneTreeNode(myTreeNode targetNode, myTreeNode srcNode)
    {
        targetNode.ChildNodes.Clear();

        //判断找到子节点就返回
        string rootValue = srcNode.Value;
        //返回结果
        srcNode.IsCountStack = false;

        rtInfo getInfo = DealNodeInfo(srcNode, targetNode, rootValue);
        myTreeNode tmpSrcNode = getInfo.srcNode;
        myTreeNode tmpTargetNode = getInfo.targetNode;

        while (tmpSrcNode != null)
        {
            getInfo = DealNodeInfo(tmpSrcNode, tmpTargetNode, rootValue);
            tmpSrcNode = getInfo.srcNode;
            tmpTargetNode = getInfo.targetNode;
        }

        return targetNode;
    }


    //深度克隆节点
    private myTreeNode CloneTreeNode(myTreeNode node)
    {
        //返回结果
        myTreeNode resultNode = copyOneNode(node);
        return CloneTreeNode(resultNode, node);
    }

    //复制节点
    private myTreeNode copyOneNode(myTreeNode node)
    {

        if (node == null)
        {
            return new myTreeNode();
        }
        myTreeNode result = new myTreeNode();
        result.Id = node.Id;
        //result.Material = node.Material;
        result.Current = node.Current;
        result.Desc = node.Desc;
        //result.IsModel = node.IsModel;
        // result.IsPart = node.IsPart;      
        //!!!
        result.IsCountStack = false;
        result.CountStack = new Stack();
        result.Value = GetTreeIdIndex();
        result.NavigateUrl = GetURLString(result.Value);
        result.Text = GetNodeText(result.Current, result.Desc, result.Value);

        return result;
    }

    private rtInfo DealNodeInfo(myTreeNode node, myTreeNode stNode, string rootValue)
    {
        rtInfo result = new rtInfo();
        if (node.IsCountStack == false)
        {
            for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
            {
                node.CountStack.Push((myTreeNode)node.ChildNodes[i]);
            }
        }

        if (node.CountStack.Count > 0)
        {
            myTreeNode current = (myTreeNode)node.CountStack.Pop();
            myTreeNode treenode = copyOneNode(current);
            stNode.ChildNodes.Add(treenode);
            current.IsCountStack = false;
            result.srcNode = current;
            result.targetNode = treenode;
            //return current;
        }
        else
        {
            if (node.Parent == null || node.Value ==rootValue)
            {
                result.srcNode = null;
                result.targetNode = null;
                //return null;
            }
            else
            {
                ((myTreeNode)node.Parent).IsCountStack = true;
                //current = node.Parent;
                result.srcNode = ((myTreeNode)node.Parent);
                result.targetNode = ((myTreeNode)stNode.Parent);
            }
        }
        return result;
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Pixel(5); //Unit.Percentage(10);
        gd.HeaderRow.Cells[1].Width = Unit.Pixel(30);
        gd.HeaderRow.Cells[2].Width = Unit.Pixel(10);
        gd.HeaderRow.Cells[3].Width = Unit.Pixel(45);
        gd.HeaderRow.Cells[4].Width = Unit.Pixel(55);
        gd.HeaderRow.Cells[5].Width = Unit.Pixel(5);
        gd.HeaderRow.Cells[6].Width = Unit.Pixel(40);
        gd.HeaderRow.Cells[7].Width = Unit.Pixel(10);
        gd.HeaderRow.Cells[8].Width = Unit.Pixel(10);
        gd.HeaderRow.Cells[9].Width = Unit.Pixel(20);
        gd.HeaderRow.Cells[10].Width = Unit.Pixel(20);
    }

    private void bindTable(DataTable list, int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemBomNodeType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemDesc").ToString());

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemGroupNo").ToString());
        dt.Columns.Add("Flag");

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_lstItemUdt").ToString());

        dt.Columns.Add("Id");
        dt.Columns.Add("isChecked");

        if (list != null && list.Rows.Count != 0)
        {
            for (int i = 0; i < list.Rows.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < list.Rows[i].ItemArray.Count(); j++)
                {

                    if (list.Rows[i][j].GetType() == typeof(DateTime))
                    {
                        dr[j] = ((System.DateTime)list.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        dr[j] = Null2String(list.Rows[i][j]);
                    }

                }
                //dr[list.Rows[i].ItemArray.Count()] = "False";
                dt.Rows.Add(dr);
            }

            for (int i = list.Rows.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            this.hidRecordCount.Value = list.Rows.Count.ToString();
        }
        else
        {
            for (int i = 0; i < defaultRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            this.hidRecordCount.Value = "";
        }

        gd.DataSource = dt;
        gd.DataBind();
        setColumnWidth();

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "newIndex", "iSelectedRowIndex = null;", true); 
        
    }

    protected void btnExp_ServerClick(Object sender, EventArgs e)
    {
        this.dTree.ExpandAll();
        //List<string> idList = new List<string>();
        //ShowListByTreeNode(idList);
        bindTable(null, DEFAULT_ROWS);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "initSelectTreeNode();DeleteComplete();", true);
        this.updatePanel.Update();
        this.updatePanel1.Update();
        this.updatePanel2.Update();
    }

    protected void btnRep_ServerClick(Object sender, EventArgs e)
    {
        this.dTree.ExpandAll();
        if (this.dTree.Nodes.Count==0)
        {
            return;
        }
        for (int i = 0; i < this.dTree.Nodes[0].ChildNodes.Count; i++)
        {
            this.dTree.Nodes[0].ChildNodes[i].CollapseAll();
        }
        bindTable(null, DEFAULT_ROWS);
        //List<string> idList = new List<string>();
        //ShowListByTreeNode(idList);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "initSelectTreeNode();DeleteComplete();", true);
        this.updatePanel.Update();
        this.updatePanel1.Update();
        this.updatePanel2.Update();
    }

    protected void btnShowBOM_ServerClick(Object sender, EventArgs e)
    {

    }

    protected void btnApproveModel_ServerClick(Object sender, EventArgs e)
    {
        /*
        string partNo = this.dOldPartNo.Value.Trim();
        try
        {
            iModelBOM.ApproveModel(partNo, this.HiddenUserName.Value.Trim());
            InputMaterialInfoDef inputModelPartInfo;
            inputModelPartInfo = iModelBOM.GetMaterialInfo(partNo);
            if (inputModelPartInfo.IsModel == "False")
            {
                //14 
                showErrorMessage(pmtMessage14);
                //showErrorMessage("这个Model目前在数据库中已经不存在");
            }
            SetApproveState(inputModelPartInfo);
            this.updatePanel3.Update();
            this.updatePanel4.Update();
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
        */
    }


    protected void btnGroupNo_ServerClick(Object sender, EventArgs e)
    {
        List<string> idList = GetCheckedIdList();
        string itemId = idList[0];

        //找树
        string value = this.dSelectedTreeNodeId.Value.Trim();
        myTreeNode itemNode = findTreeNode(value);
        if (itemNode == null)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "HideWait();", true);
            return;
        }
        ChangeNodeInfoDef parent = new ChangeNodeInfoDef();
        parent.NodeName = itemNode.Current;

        try
        {            
            iModelBOM.SetNewAlternativeGroup(idList, parent, this.HiddenUserName.Value.Trim());
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

        List<string> nodeIdList = new List<string>();
        for (int i = 0; i < itemNode.ChildNodes.Count; i++)
        {
            string id = ((myTreeNode)itemNode.ChildNodes[i]).Id;
            nodeIdList.Add(id);
        }
        ShowListByTreeNode(nodeIdList);

        String clientFunc = "HideWait();AddUpdateComplete('" + itemId + "');";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", clientFunc, true);
        this.updatePanel1.Update();
        this.updatePanel2.Update();
    }

    public void InitClientFunc()
    {
        string script =
          "function LoadEvent()" +
          "{{" +
          " try" +
          " {{" +
          " var elem = document.getElementById('{0}_SelectedNode');" +
          " if(elem != null )" +
          " {{" +
          " var node = document.getElementById(elem.value);" +
          " if(node != null)" +
          " {{" +
          " node.scrollIntoView(true);" +
          " {1}.scrollLeft = 0;" +
          " }}" +
         " }}" +
        " }}" +
        " catch(oException)" +
        " {{}}" +
        "}}";

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadEvent", String.Format(script, this.dTree.ClientID, this.updatePanel.ClientID), true);
    }

    // 檢查配對是否已經存在; BOMNodeRelation
    private bool CheckExistPair(string ParentPartNo, string ChildPartNo)
    {
        string ParentType = "", ChildType = "";
        // 檢查 BOMNodeRelation
        if (!iModelBOM.ChekkMatchingBOMRelation(ParentPartNo, ChildPartNo, ref ParentType, ref ChildType))
        {
            // 取得 Children types
            IList<BOMNodeRelation> availChild = iModelBOM.FindBOMNodeRelationChild(ParentType);
            IList<string> availChildTypes = new List<string>();
            for (int i = 0; i < availChild.Count; i++)
            {
                availChildTypes.Add(availChild[i].ChildBOMNodeType);
            }

            StringBuilder s = new StringBuilder();
            s.AppendLine(this.GetLocalResourceObject(Pre + "_msgNowParentType").ToString() + ParentType);
            s.AppendLine(this.GetLocalResourceObject(Pre + "_msgNowChildType").ToString() + ChildType);
            s.AppendLine(this.GetLocalResourceObject(Pre + "_msgBOMNodeRelationNotExist").ToString());
            if (availChildTypes.Count == 0)
                s.AppendLine(this.GetLocalResourceObject(Pre + "_msgBOMNodeRelationNoChildType").ToString());
            else
            {
                s.AppendLine(this.GetLocalResourceObject(Pre + "_msgBOMNodeRelationChildTypes").ToString() + string.Join(",", availChildTypes.ToArray()));
            }

            showErrorMessage(s.ToString());
            return false;
        }
        return true;
    }

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        String clientFunc = "HideWait();";
        try
        {
            ModelBOMInfoDef item = new ModelBOMInfoDef();
            item.Component = this.dPartNo.Text.ToUpper().Trim();
            item.ID =this.dOldId.Value.Trim();
            item.Priority = "";
            item.Editor =this.HiddenUserName.Value.Trim();
            item.Quantity=this.dQty.Text.Trim();
            item.Flag = this.dPriority.Text.Trim();

            //找树
            string value = this.dSelectedTreeNodeId.Value.Trim();
            myTreeNode itemNode = findTreeNode(value);
            if (itemNode == null)
            {
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "HideWait();", true);
                return;
            }
            ChangeNodeInfoDef parent =new ChangeNodeInfoDef ();
            parent.NodeName=itemNode.Current;

            //父节点新取得的信息
            myTreeNode parentNodeFromDatabase;
            

            // 檢查配對是否已經存在; BOMNodeRelation
            if (!CheckExistPair(itemNode.Current, item.Component))
                return;

            iModelBOM.SaveModelBOM(item,parent);
            //取得新保存节点父节点的树信息
            //父节点的信息，itemNode.Current
            parentNodeFromDatabase = GetTreeNodeByData(itemNode.Current);


            //update tree         
            //找到树中所有与当前父节点PartNo同名的节点
            List<myTreeNode> findResult=findTreeNodeByPartNo(itemNode.Current);
            for (int i = 0; i < findResult.Count; i++)
            {
                //将父节点新的信息深拷贝到其中
                CloneTreeNode(findResult[i], parentNodeFromDatabase); 
            }
            //选择当前的展开节点，展开那个节点
            this.dTree.ExpandAll();
            itemNode.Select();

            //////
            //判断新改的节点是否在显示中
            Boolean isNewNodeInShow = false;
            List<string> idList = new List<string>();
            for (int i = 0; i < itemNode.ChildNodes.Count; i++)
            {
                string id = ((myTreeNode)itemNode.ChildNodes[i]).Id;
                idList.Add(id);
                ///////
                //如果当前的子节点没被加到原来的父节点上，说明不符合显示条件
                if (item.ID == id)
                {
                    isNewNodeInShow = true;
                }
                //////
            }
            ShowListByTreeNode(idList);
            SaveTreeIdIndex();
            string treeNodeValue = itemNode.Value;
            
            clientFunc = clientFunc+" HighLightTreeNode('" + treeNodeValue + "');AddUpdateComplete('" + item.ID + "');";
            if (isNewNodeInShow == false)
            {

                //15 
                //clientFunc = clientFunc + "alert('" + "已经成功修改节点，新改的数据不在可被显示的范围内" + "')";
                //clientFunc = clientFunc + "alert('" + pmtMessage15 + "');";
            }
        
            clientFunc = clientFunc + "showSelectedNode();";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", clientFunc, true);
            this.updatePanel.Update();
            this.updatePanel1.Update();
            this.updatePanel2.Update();
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

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        String clientFunc = "HideWait();";
        try
        {
            ModelBOMInfoDef item = new ModelBOMInfoDef();
            item.Component = this.dPartNo.Text.ToUpper().Trim();
            item.ID = "0";
            item.Priority = this.dPriority.Text.Trim();
            item.Editor = this.HiddenUserName.Value.Trim();
            item.Quantity = this.dQty.Text.Trim();

            //找树
            string value = this.dSelectedTreeNodeId.Value.Trim();
            myTreeNode itemNode = findTreeNode(value);
            if (itemNode == null)
            {
                ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "HideWait();", true);
                return;
            }
            ChangeNodeInfoDef parent = new ChangeNodeInfoDef();
            parent.NodeName = itemNode.Current;

            //父节点新取得的信息
            myTreeNode parentNodeFromDatabase;
            TreeNodeDef addNodeInfo;


            // 檢查配對是否已經存在; BOMNodeRelation
            if (!CheckExistPair(itemNode.Current, item.Component))
                return;

            addNodeInfo = iModelBOM.AddModelBOM(item, parent);
            item.ID = addNodeInfo.id;
            //取得新保存节点父节点的树信息
            //父节点的信息，itemNode.Current
            parentNodeFromDatabase = GetTreeNodeByData(itemNode.Current);

            //CacheUpdate for BOM
            iModelBOM.CacheUpdate_ForBOM(parent.NodeName);
            

            //update tree         
            //找到树中所有与当前父节点PartNo同名的节点
            List<myTreeNode> findResult = findTreeNodeByPartNo(itemNode.Current);
            for (int i = 0; i < findResult.Count; i++)
            {
                //将父节点新的信息深拷贝到其中
                CloneTreeNode(findResult[i], parentNodeFromDatabase);
            }
            //选择当前的展开节点，展开那个节点
            this.dTree.ExpandAll();
            itemNode.Select();

            //判断新改的节点是否在显示中
            Boolean isNewNodeInShow = false;

            List<string> idList = new List<string>();
            for (int i = 0; i < itemNode.ChildNodes.Count; i++)
            {
                string id = ((myTreeNode)itemNode.ChildNodes[i]).Id;
                idList.Add(id);
                ///////
                //如果当前的子节点没被加到原来的父节点上，说明不符合显示条件
                if (item.ID == id)
                {
                    isNewNodeInShow = true;
                }
                //////
            }
            ShowListByTreeNode(idList);
            SaveTreeIdIndex();
            string treeNodeValue = itemNode.Value;
            

            clientFunc =clientFunc+ " HighLightTreeNode('" + treeNodeValue + "');AddUpdateComplete('" + item.ID + "');";
            if (isNewNodeInShow == false)
            {
                //15 
                //clientFunc = clientFunc + "alert('" + pmtMessage15 + "');";
                //clientFunc = clientFunc + "alert('" + "已经成功添加节点，新改的数据不在可被显示的范围内" + "');";
            }
            clientFunc = clientFunc + "showSelectedNode();";
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", clientFunc, true); 
            this.updatePanel.Update();
            this.updatePanel1.Update();
            this.updatePanel2.Update();
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


    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {

        List<string> idList = GetCheckedIdList();

        //找树
        string value = this.dSelectedTreeNodeId.Value.Trim();
        myTreeNode itemNode = findTreeNode(value);
        if (itemNode == null)
        {
            ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", "HideWait();", true);
            return;
        }
        ChangeNodeInfoDef parent = new ChangeNodeInfoDef();
        parent.NodeName = itemNode.Current;

        //父节点新取得的信息
        myTreeNode parentNodeFromDatabase; 
        try
        {
            iModelBOM.Delete(idList, parent, this.HiddenUserName.Value.Trim());
            //取得新保存节点父节点的树信息
            //父节点的信息，itemNode.Current
            parentNodeFromDatabase = GetTreeNodeByData(itemNode.Current);

            //CacheUpdate for BOM
            iModelBOM.CacheUpdate_ForBOM(parent.NodeName);
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

        //update tree         
        //找到树中所有与当前父节点PartNo同名的节点
        List<myTreeNode> findResult = findTreeNodeByPartNo(itemNode.Current);
        for (int i = 0; i < findResult.Count; i++)
        {
            //将父节点新的信息深拷贝到其中
            CloneTreeNode(findResult[i], parentNodeFromDatabase);
        }
        //选择当前的展开节点，展开那个节点
        this.dTree.ExpandAll();
        itemNode.Select();
        ///////

        List<string> nodeIdList = new List<string>();
        for (int i = 0; i < itemNode.ChildNodes.Count; i++)
        {
            string id = ((myTreeNode)itemNode.ChildNodes[i]).Id;
            nodeIdList.Add(id);
        }
        ShowListByTreeNode(nodeIdList);
        SaveTreeIdIndex();
        string treeNodeValue = itemNode.Value;
        String clientFunc = "";
        clientFunc = clientFunc + "HideWait();";
        clientFunc = clientFunc + "HighLightTreeNode('" + treeNodeValue + "');DeleteComplete();";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "saveUpdate", clientFunc, true); 
        this.updatePanel.Update();
        this.updatePanel1.Update();
        this.updatePanel2.Update();

    }

    private List<string> GetCheckedIdList()
    {
        List<string> idList = new List<string>();

        string[] checkStatus=this.gdCheckBoxStatusArray.Value.Split(",".ToCharArray());
        for (int i = 0; i < checkStatus.Length; i++)
        {
            
            //CheckBox cb = (CheckBox)this.gd.Rows[i].Cells[0].FindControl("gdCheckBox");
            //if (cb.Checked)
            //if (this.gd.Rows[i].Cells[10].Text == "True")
            //{
                string id = checkStatus[i].Trim();
                idList.Add(id);
            //}
        }
        return idList;
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
        scriptBuilder.AppendLine("HideWait();");        
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');");
        scriptBuilder.AppendLine("ShowInfo('" + errorMsg.Replace("\r\n", "\\n") + "');");
        //scriptBuilder.AppendLine("ShowRowEditInfo();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        for (int i = COL_NUM; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "display:none");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text.Trim()==""|| e.Row.Cells[1].Text.Trim() == "&nbsp;")
            {
                 CheckBox cb =((CheckBox)e.Row.Cells[0].FindControl("gdCheckBox"));
                 cb.Style.Add("display", "none");
            }

            for (int i = 0; i < COL_NUM; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

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

    private void GetPartTypeList()
    {
        IList<PartTypeMaintainInfo> lst = iPartTypeManagerEx.GetPartTypeList();

        this.CmbMaintainPartTypeAll.Items.Clear();
        this.CmbMaintainPartTypeAll.Items.Add(new ListItem("", ""));

        foreach (PartTypeMaintainInfo value in lst)
        {
            ListItem item = new ListItem(value.PartType, value.PartType);
            CmbMaintainPartTypeAll.Items.Add(item);
        }
        //this.updatePanel_PartType.Update();
    }

    protected void CmbMaintainPartTypeAll_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetPartListByPartType(CmbMaintainPartTypeAll.SelectedValue);
    }
    private void GetPartListByPartType(string partType)
    {
        try
        {
            this.CmbMaintainPart.Items.Clear();
            this.CmbMaintainPart.Items.Add(new ListItem("", ""));
            IList<PartDef> lst = iPartManagerEx.GetPartListByPartType(partType, 100);
            if (lst!=null)
            foreach (PartDef p in lst)
            {
                if (!string.IsNullOrEmpty(p.partNo))
                {
                    ListItem item = new ListItem(p.partNo + " (BomNodeType:" + p.bomNodeType + ")", p.partNo);
                    CmbMaintainPart.Items.Add(item);
                }
            }
            this.updatePanel_PartType.Update();

        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
        }
    }

    protected void btnChangedPartTypeAllIndex_ServerClick(Object sender, EventArgs e)
    {
        CmbMaintainPartTypeAll.SelectedIndex = Convert.ToInt32(changedPartTypeAllIndex.Value);
        CmbMaintainPartTypeAll_SelectedIndexChanged(null, null);
        for (int i = 0; i < CmbMaintainPart.Items.Count; i++)
        {
            if (CmbMaintainPart.Items[i].Value.Equals(dPartNo.Text))
            {
                CmbMaintainPart.SelectedIndex = i;
                break;
            }
        }
    }

}

