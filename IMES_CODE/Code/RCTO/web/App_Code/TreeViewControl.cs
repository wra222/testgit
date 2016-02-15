/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  Treeview control
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-11-30 Li, Ming-Jun(eB1)     Create 
 * 2010-01-22 Li, Ming-Jun(eB1)     Modify:ITC-1103-0118 
 * Known issues:
 */
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
using System.Xml.Linq;
using com.inventec.RBPC.Net.datamodel.intf;

namespace com.inventec.iMESWEB
{
    /// <summary>
    /// Summary description for TreeView
    /// </summary>
    public class TreeViewControl
    {
        AuthorityManager authorManager;
        DataTable treeNodes;
        UserInfo userInfo = new UserInfo();
        string _sessionId = "";

        public TreeViewControl(UserInfo ui)
        {
            authorManager = new AuthorityManager(ui);
            treeNodes = new DataTable();
            userInfo = ui;
        }

        //Populate tree node
        public void TreeNodePopulate(TreeNodeCollection nodes, IToken token, string id)
        {
            _sessionId = id;
            //Parents nodes
            treeNodes = authorManager.getAllPermission();
            treeNodes.Merge(authorManager.getAuthoritiesByToken(token));
            CreateRootNode(nodes);
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
            newNode.PopulateOnDemand = true;
            newNode.Expanded = true;
            newNode.SelectAction = TreeNodeSelectAction.Expand;

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
                String Pcode = arrDr[i]["Pcode"].ToString();
                Level = int.Parse(arrDr[i]["Level"].ToString());
                Sort = arrDr[i]["Sort"].ToString();

                TreeNode newNode = new TreeNode();
                newNode.Value = Id;

                if (Parent == "Child")
                {
                    //Modify:ITC-1103-0118
                    newNode.Text = "<img src=\"" + "./Images/" + ImageUrl + "\" border=\"0\" onclick=\"node_click(this, '" + Name + "')\" /><span onclick=\"node_click(this, '" + Name + "')\">" + Pcode + " " + Name + "</span>";
//                    newNode.ImageUrl = "./Images/" + ImageUrl;
                    NavigateUrl += "&UserId=" + userInfo.UserId + "&Customer=" + userInfo.Customer + "&UserName=" + HttpUtility.UrlEncode(userInfo.UserName) + "&AccountId=" + userInfo.AccountId + "&Login=" + userInfo.Login + "&SessionId=" + _sessionId;
                    newNode.NavigateUrl = NavigateUrl;

                    arrDr[i]["NavigateUrl"] = NavigateUrl;
                }
                else
                {
                    newNode.Text = Name;
                    newNode.NavigateUrl = "#";
                }

                node.ChildNodes.Add(newNode);

                CreateTreeNode(newNode, Sort, Level);
            }
        }

        //Get treenodes datatable
        public DataTable getTreeNodes()
        {
            return treeNodes;
        }
    }
}
