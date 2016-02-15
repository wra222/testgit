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
using IMES.Maintain.Interface.MaintainIntf;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure;
using IMES.DataModel;
using MaintainControl;

public partial class DataMaintain_ModelBOMRelation : System.Web.UI.Page
{
    public string userName;

    private string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);

    private IBOMNodeData iModelBOM = (IBOMNodeData)ServiceAgent.getInstance().GetMaintainObjectByName<IBOMNodeData>(WebConstant.IBOMNodeData);
    IPartTypeManagerEx iPartTypeManagerEx = ServiceAgent.getInstance().GetMaintainObjectByName<IPartTypeManagerEx>(WebConstant.IPartTypeManagerEx);

    public string pmtDel, pmtEmpty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userName = Master.userInfo.UserId;
            pmtDel = this.GetLocalResourceObject(Pre + "_pmtDel").ToString();
            pmtEmpty = this.GetLocalResourceObject(Pre + "_pmtEmpty").ToString();

            if (!this.IsPostBack)
            {
                dTree.SelectedNodeStyle.ForeColor = System.Drawing.Color.Blue;
                dTree.SelectedNodeStyle.Font.Bold = true;

                btnAdd.InnerText = "Add";
                btnSave.InnerText = "Save";
                btnDelete.InnerText = "Delete";

                InitTree();

                initLabel();
                btnSave.Disabled = true;

                selBOMNodeType.Items.Add(new ListItem("", ""));
                selBOMNodeType.Items.Add(new ListItem("PC", "PC"));
                selChildBOMNodeType.Items.Add(new ListItem("", ""));
                selChildBOMNodeType.Items.Add(new ListItem("PC", "PC"));
                IList<string> partTypes = iPartTypeManagerEx.GetBomNodeTypeList();
                for (int i = 0; i < partTypes.Count; i++)
                {
                    selBOMNodeType.Items.Add(new ListItem(partTypes[i], partTypes[i]));
                    selChildBOMNodeType.Items.Add(new ListItem(partTypes[i], partTypes[i]));
                }
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
        }
    }

    private void initLabel()
    {
        this.lblTree.Text = this.GetLocalResourceObject(Pre + "_lblTreeText").ToString();
        this.lblParentBOMNodeType.Text = this.GetLocalResourceObject(Pre + "_lblParentBOMNodeTypeText").ToString();
        this.lblBOMNodeType.Text = this.GetLocalResourceObject(Pre + "_lblBOMNodeTypeText").ToString();
        this.lblBOMNodeDescr.Text = this.GetLocalResourceObject(Pre + "_lblBOMNodeDescrText").ToString();

        this.btnAdd.InnerText = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();
        this.btnDelete.InnerText = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.InnerText = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
    }

    private void SetDDL(ref DropDownList ddl, string v)
    {
        ddl.SelectedIndex = 0;
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            if (v.Equals(ddl.Items[i].Value))
            {
                ddl.SelectedIndex = i;
                break;
            }
        }
    }

    private string GetDDL(ref DropDownList ddl)
    {
        if (ddl.SelectedItem != null)
            return ddl.SelectedItem.Value;
        return "";
    }

    private void SetDDL_BOMNodeType(string v)
    {
        SetDDL(ref selBOMNodeType, v);
    }

    private void SetDDL_ChildBOMNodeType(string v)
    {
        SetDDL(ref selChildBOMNodeType, v);
    }

    private string GetDDL_BOMNodeType()
    {
        return GetDDL(ref selBOMNodeType);
    }

    private string GetDDL_ChildBOMNodeType()
    {
        return GetDDL(ref selChildBOMNodeType);
    }

    #region 系统相关
    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();
        errorMsg = replaceSpecialChart(errorMsg);
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage('" + errorMsg.Replace("\r\n", "<br>") + "');DealHideWait();");
        scriptBuilder.AppendLine("</script>");
        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }

    private static String replaceSpecialChart(String sourceData)
    {
        sourceData = sourceData.Replace("'", "\\'");
        //sourceData = Server.HtmlEncode(sourceData);
        return sourceData;
    }

    protected static String Null2String(Object _input)
    {
        if (_input == null)
        {
            return "";
        }
        return _input.ToString().Trim();
    }

    #endregion

    protected void dTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            if (dTree.SelectedNode != null)
            {
                string bomNodeType = GetValueFromTOKEN(dTree.SelectedNode.Value);
                string childBomNodeType = dTree.SelectedNode.Text;
                SetDDL_BOMNodeType(bomNodeType);
                SetDDL_ChildBOMNodeType(childBomNodeType);
                hidOldBOMNodeType.Value = bomNodeType;
                hidOldChildBOMNodeType.Value = childBomNodeType;

                if (TopRootType.Equals(childBomNodeType))
                {
                    btnAdd.Disabled = false;
                    btnSave.Disabled = true;
                    btnDelete.Disabled = true;
                }
                else
                {
                    btnAdd.Disabled = false;
                    btnSave.Disabled = false;
                    btnDelete.Disabled = false;
                }

                IList<BOMNodeRelation> rL = iModelBOM.FindBOMNodeRelationByPair(bomNodeType, childBomNodeType);
                if (rL.Count > 0)
                {
                    BOMNodeRelation r = (BOMNodeRelation) rL[0];
                    BOMNodeDescr.Text = r.Descr;
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

    // 檢查配對是否已經存在
    private bool CheckExistPair()
    {
        IList<BOMNodeRelation> rL = iModelBOM.FindBOMNodeRelationByPair(GetDDL_BOMNodeType(), GetDDL_ChildBOMNodeType());
        if (rL.Count > 0)
        {
            showErrorMessage(this.GetLocalResourceObject(Pre + "_msgBomRelationExistPair").ToString());
            return false;
        }
        return true;
    }

    // 檢查上階是否包含新的ChildBOMNodeType
    // ChildNode can NOT be one of the parent (PC\..)
    private bool CheckChildWithinParents()
    {
        IList<string> parentTree = iModelBOM.FindBOMNodeRelationParentsByRoot(TopRootType, GetDDL_BOMNodeType());
        if ((GetDDL_BOMNodeType().Equals(GetDDL_ChildBOMNodeType())) || parentTree.Contains(GetDDL_ChildBOMNodeType()))
        {
            showErrorMessage(this.GetLocalResourceObject(Pre + "_msgBomParentPathExist").ToString());
            return false;
        }
        return true;
    }

    // 檢查Types是否包含新的BOMNodeType
    private bool CheckExistBOMNodeType()
    {
        IList<string> totalParts = iModelBOM.FindBOMNodeRelationTypes(TopRootType);
        if (!(totalParts.Contains(GetDDL_BOMNodeType()) || TopRootType.Equals(GetDDL_BOMNodeType())))
        {
            showErrorMessage(this.GetLocalResourceObject(Pre + "_msgBomTypeNotExist").ToString());
            return false;
        }
        return true;
    }

    protected void btnAdd_ServerClick(Object sender, EventArgs e)
    {
        BOMNodeRelation r = null;
        string ID = "";
        try
        {
            // 檢查配對是否已經存在
            if (!CheckExistPair())
                return;

            // 檢查上階是否包含新的ChildBOMNodeType
            if (!CheckChildWithinParents())
                return;
            
            // 檢查Types是否包含新的BOMNodeType
            if (!CheckExistBOMNodeType())
                return;

            r = new BOMNodeRelation();
            r.BOMNodeType = GetDDL_BOMNodeType();
            r.ChildBOMNodeType = GetDDL_ChildBOMNodeType();
            r.Descr = BOMNodeDescr.Text;
            r.Editor = userName;
            ID = iModelBOM.AddBOMNodeRelation(r);

            if (dTree.Nodes.Count > 0)
            {
                NodeExistInTreeNow = true;
                TreeNode n = dTree.Nodes[0];
                TreeScanParent_ChangeChild(ref n, GetDDL_BOMNodeType(), GetDDL_ChildBOMNodeType(), NodeActionMode.ADD, "", ref n);

                if (!NodeExistInTreeNow)
                {
                    DataTable result = iModelBOM.FindBOMNodeRelationByRoot(GetDDL_ChildBOMNodeType());
                    IList<TreeNode> lstTheSameParentNodes = TreeScanParent_FindNodes(dTree.Nodes[0], GetDDL_ChildBOMNodeType());
                    for (int j = 0; j < lstTheSameParentNodes.Count; j++)
                    {
                        n = lstTheSameParentNodes[j];
                        GetSubTree(ref n, GetDDL_ChildBOMNodeType(), ref result);
                    }
                }

                if (null != dTree.SelectedNode)
                {
                    dTree.SelectedNode.Selected = false;
                }
                btnSave.Disabled = true;
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

    protected void btnSave_ServerClick(Object sender, EventArgs e)
    {
        BOMNodeRelation r = null;
        string ID = "";
        try
        {
            // 檢查上階是否包含新的ChildBOMNodeType
            if (!CheckChildWithinParents())
                return;

            bool NodeChanged = false;
            if (hidOldBOMNodeType.Value.Equals(GetDDL_BOMNodeType()) && !hidOldChildBOMNodeType.Value.Equals(GetDDL_ChildBOMNodeType()))
            {
                // 檢查配對是否已經存在
                if (!CheckExistPair())
                    return;

                // 檢查Types是否包含新的BOMNodeType
                if (!CheckExistBOMNodeType())
                    return;

                iModelBOM.DeleteBOMNodeRelation(GetDDL_BOMNodeType(), hidOldChildBOMNodeType.Value);

                r = new BOMNodeRelation();
                r.BOMNodeType = GetDDL_BOMNodeType();
                r.ChildBOMNodeType = GetDDL_ChildBOMNodeType();
                r.Descr = BOMNodeDescr.Text;
                r.Editor = userName;
                ID = iModelBOM.AddBOMNodeRelation(r);

                NodeChanged = true;
            }
            else
            {
                r = new BOMNodeRelation();
                r.BOMNodeType = GetDDL_BOMNodeType();
                r.ChildBOMNodeType = GetDDL_ChildBOMNodeType();
                r.Descr = BOMNodeDescr.Text;
                r.Editor = userName;

                iModelBOM.UpdateBOMNodeRelation(r);
            }

            if ((dTree.Nodes.Count > 0) && NodeChanged)
            {
                NodeExistInTreeNow = true;
                TreeNode n = dTree.Nodes[0];
                TreeScanParent_ChangeChild(ref n, GetDDL_BOMNodeType(), hidOldChildBOMNodeType.Value, NodeActionMode.MODIFY, GetDDL_ChildBOMNodeType(), ref n);

                if (!NodeExistInTreeNow)
                {
                    DataTable result = iModelBOM.FindBOMNodeRelationByRoot(GetDDL_ChildBOMNodeType());
                    IList<TreeNode> lstTheSameParentNodes = TreeScanParent_FindNodes(dTree.Nodes[0], GetDDL_ChildBOMNodeType());
                    for (int j = 0; j < lstTheSameParentNodes.Count; j++)
                    {
                        n = lstTheSameParentNodes[j];
                        GetSubTree(ref n, GetDDL_ChildBOMNodeType(), ref result);
                    }
                }

                if (null != dTree.SelectedNode)
                {
                    dTree.SelectedNode.Selected = false;
                }
                btnSave.Disabled = true;
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

    protected void btnDelete_ServerClick(Object sender, EventArgs e)
    {
        try
        {
            iModelBOM.DeleteBOMNodeRelation(GetDDL_BOMNodeType(), GetDDL_ChildBOMNodeType());

            if (dTree.Nodes.Count > 0)
            {
                TreeNode n = dTree.Nodes[0];
                TreeScanParent_ChangeChild(ref n, GetDDL_BOMNodeType(), GetDDL_ChildBOMNodeType(), NodeActionMode.DELETE, "", ref n);
                if (null != dTree.SelectedNode)
                {
                    dTree.SelectedNode.Selected = false;
                }
                btnSave.Disabled = true;
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

    #region Tree

    private static string TopRootType = "PC";
    private static string TOKEN = ";|;";

    // ChangeMode: ADD DELETE MODIFY
    // ADD 新增 subTree
    // DELETE 刪除 subNode
    // MODIFY 修改 subNode 變subTree
    private enum NodeActionMode { ADD, DELETE, MODIFY }

    private bool NodeExistInTreeNow = true;

    private static string GetValueFromTOKEN(string s)
    {
        int p = s.IndexOf(TOKEN);
        if (p > 0)
            return s.Substring(0, p);
        else
            return "";
    }

    private string GetUniqueValue(string val)
    {
        long tick = DateTime.Now.Ticks;
        Random rnd1 = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        return val + TOKEN + (cntNode++).ToString() + rnd1.Next().ToString();
    }

    int cntNode = 0;
    // val: ParentType;|;id
    private TreeNode CreateNode(string text, string val, bool expanded)
    {
        TreeNode node = new TreeNode(); ;
        node.Text = text;
        node.Value = GetUniqueValue(val);
        node.Expanded = true;
        //node.
        return node;
    }

    private TreeNode CopyNode(TreeNode oldNode)
    {
        string val = GetValueFromTOKEN(oldNode.Value);

        TreeNode node = new TreeNode(); ;
        node.Text = oldNode.Text;
        node.Value = GetUniqueValue(val);
        node.Expanded = true;
        //node.

        foreach (TreeNode n in oldNode.ChildNodes)
        {
            node.ChildNodes.Add(CopyNode(n));
        }

        return node;
    }

    private IList<TreeNode> TreeScanParent_FindNodes(TreeNode nowNode, string findText)
    {
        IList<TreeNode> ret = new List<TreeNode>();
        if (findText.Equals(nowNode.Text))
        {
            ret.Add(nowNode);
        }
        for (int i = 0; i < nowNode.ChildNodes.Count; i++)
        {
            TreeNode sub = nowNode.ChildNodes[i];
            IList<TreeNode> retSub = TreeScanParent_FindNodes(sub, findText);
            ret = ret.Concat(retSub).ToList();
        }
        return ret;
    }

    private TreeNode TreeScanParent_FindOneNode(TreeNode nowNode, string findText)
    {
        if (findText.Equals(nowNode.Text))
        {
            return nowNode;
        }
        for (int i = 0; i < nowNode.ChildNodes.Count; i++)
        {
            TreeNode sub = nowNode.ChildNodes[i];
            TreeNode retSub = TreeScanParent_FindOneNode(sub, findText);
            if (null != retSub)
                return retSub;
        }
        return null;
    }
    
    private int TreeScanParent_ChangeChild(ref TreeNode nowNode, string TargetParentType, string TargetChildType, NodeActionMode mode,
        string NewChildType/*for MODIFY*/ , ref TreeNode TopRoot )
    {
        TreeNode nodeChanged = null;
        bool existedType = false;

        string nowType = nowNode.Text;
        int changeCnt = 0;

        for (int i = 0; i < nowNode.ChildNodes.Count; i++)
        {
            TreeNode n = nowNode.ChildNodes[i];
            changeCnt += TreeScanParent_ChangeChild(ref n, TargetParentType, TargetChildType, mode, NewChildType, ref TopRoot);

            if (TargetParentType.Equals(nowType))
            {
                if (TargetChildType.Equals(n.Text))
                {
                    nodeChanged = n;
                    existedType = true;
                    break;
                }
            }
        }
        if (TargetParentType.Equals(nowType))
        {
            if (existedType)
            {
                if (mode == NodeActionMode.MODIFY)
                {
                    //nodeChanged.Text = NewChildType;

                    //IList<TreeNode> lstTheSameParentNodes = TreeScanParent_FindNodes(dTree.Nodes[0], TargetParentType);
                    //TreeNode OneTargetNode = TreeScanParent_FindOneNode(dTree.Nodes[0], NewChildType); //TargetChildType
                    //for (int j = 0; j < lstTheSameParentNodes.Count; j++)
                    //{
                    //    // del old subNodeTree
                    //    //lstTheSameParentNodes[j].ChildNodes.Remove(nodeChanged);

                    //    if (null == OneTargetNode)
                    //    {
                    //        TreeNode nodeAdd = CreateNode(TargetChildType, TargetParentType, true);
                    //        lstTheSameParentNodes[j].ChildNodes.Add(nodeAdd);
                    //    }
                    //    else
                    //    {
                    //        string val = GetUniqueValue(nowNode.Text);
                    //        TreeNode nodeAdd = CopyNode(OneTargetNode);
                    //        nodeAdd.Value = val;
                    //        lstTheSameParentNodes[j].ChildNodes.Add(nodeAdd);
                    //    }
                    //    changeCnt++;
                    //}


                    // del old subNodeTree
                    nowNode.ChildNodes.Remove(nodeChanged);

                    TreeNode OneTargetNode = TreeScanParent_FindOneNode(dTree.Nodes[0], NewChildType);
                    if (null == OneTargetNode)
                    {
                        TreeNode nodeAdd = CreateNode(NewChildType, TargetParentType, true);
                        nowNode.ChildNodes.Add(nodeAdd);
                        NodeExistInTreeNow = false;
                    }
                    else
                    {
                        string val = GetUniqueValue(nowNode.Text);
                        TreeNode nodeAdd = CopyNode(OneTargetNode);
                        nodeAdd.Value = val;
                        nowNode.ChildNodes.Add(nodeAdd);
                    }
                    changeCnt++;

                }
                else if (mode == NodeActionMode.DELETE)
                {
                    nowNode.ChildNodes.Remove(nodeChanged);
                    changeCnt++;
                }
            }
            else
            {
                if (mode == NodeActionMode.ADD)
                {
                    //TreeNode nodeAdd = CreateNode(TargetChildType, TargetParentType, true);
                    //nowNode.ChildNodes.Add(nodeAdd);

                    //if (!"PC".Equals(TargetParentType))
                    //{
                    //    int why = 0;
                    //}

                    IList<TreeNode> lstTheSameParentNodes = TreeScanParent_FindNodes(dTree.Nodes[0], TargetParentType);
                    TreeNode OneTargetNode = TreeScanParent_FindOneNode(dTree.Nodes[0], TargetChildType);
                    for (int j = 0; j < lstTheSameParentNodes.Count; j++)
                    {
                        if (null == OneTargetNode)
                        {
                            TreeNode nodeAdd = CreateNode(TargetChildType, TargetParentType, true);
                            lstTheSameParentNodes[j].ChildNodes.Add(nodeAdd);
                            NodeExistInTreeNow = false;
                        }
                        else
                        {
                            string val = GetUniqueValue(nowNode.Text);
                            TreeNode nodeAdd = CopyNode(OneTargetNode);
                            nodeAdd.Value = val;
                            lstTheSameParentNodes[j].ChildNodes.Add(nodeAdd);
                        }
                        changeCnt++;
                    }

                }
            }
        }
        return changeCnt;
    }

    private void GetSubTree(ref TreeNode root, string type, ref DataTable result)
    {
        for (int i = 0; i < result.Rows.Count; i++)
        {
            string nowParentType = result.Rows[i]["BOMNodeType"].ToString();
            string nowChildType = result.Rows[i]["ChildBOMNodeType"].ToString();
            TreeScanParent_ChangeChild(ref root, nowParentType, nowChildType, NodeActionMode.ADD, "", ref root);
        }
    }

    private void InitTree()
    {
        try
        {
            TreeNode root = CreateNode(TopRootType, "", true);
            dTree.Nodes.Add(root);

            DataTable result = iModelBOM.FindBOMNodeRelationByRoot(TopRootType);
            GetSubTree(ref root, TopRootType, ref result);
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

}
