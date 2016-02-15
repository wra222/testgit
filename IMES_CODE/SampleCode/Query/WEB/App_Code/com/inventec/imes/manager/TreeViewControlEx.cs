using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using com.inventec.iMESWEB;


namespace com.inventec.imes.manager
{
    /// <summary>
    /// Summary description for TreeView
    /// </summary>
    public class TreeViewControlEx
    {
        AuthorityManager authMgr;
        DataTable treeNodes;

        public TreeViewControlEx()
        {
            authMgr = new AuthorityManager();
            treeNodes = new DataTable();
        }

        public TreeViewControlEx(UserInfo userInfo)
        {
            authMgr = new AuthorityManager(userInfo);
            treeNodes = new DataTable();
        }

        public void setTreeNodes(DataTable dt)
        {
            treeNodes = dt;
        }

        //Populate tree node
        public void TreeNodePopulate(TreeNodeCollection nodes)
        {
            //Parents nodes
            treeNodes = authMgr.GetAllPermissionList();
            CreateRootNode(nodes);
            //Children nodes
            //DataTable treeNodeChildren = new DataTable();
            //treeNodeChildren = authMgr.getAllPermission();
            //CreateTreeNode(treeNodeChildren, node);
        }

        //Create root node
        private void CreateRootNode(TreeNodeCollection nodes)
        {
            DataRow[] arrDr = treeNodes.Select("Level = '0'");

            String Id = arrDr[0]["Id"].ToString();
            String Name = arrDr[0]["Name"].ToString();
            String Sort = arrDr[0]["Sort"].ToString();

            TreeNode newNode = new TreeNode(Name, Id);
            newNode.NavigateUrl = "#";
            //newNode.PopulateOnDemand = true;
            //newNode.Expanded = true;

            nodes.Add(newNode);

            CreateTreeNode(newNode, Sort, 0);
        }

        //Create tree node
        public void CreateTreeNode(TreeNode node, String Sort, int Level)
        {
            DataRow[] arrDr = treeNodes.Select("Level = '" + (Level + 1).ToString()  + "' and Sort like '" + Sort + "%'");

            for (int i = 0; i < arrDr.Length; i++)
            {
                String Id = arrDr[i]["Id"].ToString();
                String Name = arrDr[i]["Name"].ToString();
                String NavigateUrl = arrDr[i]["NavigateUrl"].ToString();
                String ImageUrl = arrDr[i]["ImageUrl"].ToString();
                String Parent = arrDr[i]["Parent"].ToString();
                Level = int.Parse(arrDr[i]["Level"].ToString());
                Sort = arrDr[i]["Sort"].ToString();

                String objNodeValue = Sort + "&" + arrDr[i]["Level"].ToString();

                TreeNode newNode = new TreeNode(Name, Id);
                if (Parent == "Child")
                {
                    newNode.PopulateOnDemand = false;
                }
                    /*
                else
                {
                    newNode.PopulateOnDemand = true;
                }
                     */
                newNode.NavigateUrl = "#";
                //newNode.ImageUrl = "1";
                //newNode.ImageToolTip = objNodeValue;
                //newNode.Expanded = true;

                node.ChildNodes.Add(newNode);

                CreateTreeNode(newNode, Sort, Level);
            }
        }
    }
}
