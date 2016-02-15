using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace MaintainControl
{

    public class myTreeNode : TreeNode
    {

        private Boolean isCountStack;

        public Boolean IsCountStack
        {
            get { return isCountStack; }
            set { isCountStack = value; }
        }
        //ChildStack();
        private Stack countStack;

        public Stack CountStack
        {
            get { return countStack; }
            set { countStack = value; }
        }

        //value 是个自增加的id
        //text 是current和desc的组合

        //ModelBOM的id;
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        //待用
        private string material; //material, parent CODE 

        public string Material
        {
            get { return material; }
            set { material = value; }
        }
        
        //
        private string current;  //component， 因为text是个组合

        public string Current
        {
            get { return current; }
            set { current = value; }
        }

        private string desc;

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private string isPart;

        public string IsPart
        {
            get { return isPart; }
            set { isPart = value; }
        }
        private string isModel;

        public string IsModel
        {
            get { return isModel; }
            set { isModel = value; }
        }


        public myTreeNode()
            : base()
        {
            this.countStack = new Stack();
            this.isCountStack = false;
            this.desc = "";
            this.isModel="False";
            this.isPart="False";
            this.id = "";
            this.material = "";
            this.current = "";
        }

        public myTreeNode(string text, string value, string desc)
            : base(text, value)
        {
            this.desc = desc;
        }

        protected override object SaveViewState()
        {
            object[] arrState = new object[9];
            arrState[0] = base.SaveViewState();
            arrState[1] = this.isModel;
            arrState[2] = this.isPart;
            arrState[3] = this.desc;
            arrState[4] = this.isCountStack;
            arrState[5] = this.id;
            arrState[6] = this.material;
            arrState[7] = this.current;
            arrState[8] = DeepClone(this.countStack);
            return arrState;
        }


        public static object DeepClone(object source)
        {
            if (source == null)
            {
                return null;
            }
            Object objectReturn = null;
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, source);
                    stream.Position = 0;
                    objectReturn = formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    //log记录
                }
            }
            return objectReturn;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] arrState = savedState as object[];

                this.isModel = (string)arrState[1];
                this.isPart = (string)arrState[2];
                this.desc = (string)arrState[3];
                this.isCountStack = (bool)arrState[4];
                this.id = (string)arrState[5];
                this.material = (string)arrState[6];
                this.current = (string)arrState[7];
                this.countStack = (Stack)DeepClone(arrState[8]);                
                base.LoadViewState(arrState[0]);
            }
        }

    }


    [ToolboxData("<{0}:MaintainTreeViewExt runat=server></{0}:MaintainTreeViewExt>")]
    public class MaintainTreeViewExt: TreeView
    {
        public MaintainTreeViewExt()
        {
            //
            // TODO: Add constructor logic here
            //
        }
       
        protected override TreeNode CreateNode()
        {
            return new myTreeNode();
        }


        //树上的节点实际是以current为标识
        //matchKey==""时,找全部的节点,先叶子后根
        public List<myTreeNode> FindMatchTreeNodeList(string matchKey)
        {
            myTreeNode rootNode = (myTreeNode)this.Nodes[0];
            return FindMatchTreeNodeList(rootNode, matchKey);
        }

        //树上的节点实际是以current为标识
        //matchKey==""时,找全部的节点
        public List<myTreeNode> FindMatchTreeNodeList(myTreeNode rootNode, string matchKey)
        {
            List<myTreeNode> result = new List<myTreeNode>();

            rootNode.IsCountStack = false;
            //当前点
            myTreeNode getData = DealFindMatchNode(rootNode, result, matchKey, rootNode.Value );

            //getData = DealNodeInfo(node);
            while (getData != null)
            {
                getData = DealFindMatchNode(getData, result, matchKey, rootNode.Value);
            }
            return result;
        }

        private myTreeNode DealFindMatchNode(myTreeNode node, List<myTreeNode> findResult, string matchKey, string rootNodeValue)
        {
            myTreeNode result;

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
                current.IsCountStack = false;
                result = current;
            }
            else
            {
                if (node.Parent == null || node.Value == rootNodeValue)
                {
                    result = null;
                }
                else
                {

                    ((myTreeNode)node.Parent).IsCountStack = true;
                    if (matchKey == "" || (matchKey != "" && node.Current == matchKey))
                    {
                        findResult.Add(node);
                    }
                    result = (myTreeNode)node.Parent;

                }
            }
            return result;
        }
    }

    public class ExtendDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {

        public void Add(TKey key, TValue value)
        {
            if (!base.ContainsKey(key))
            {
                try
                {
                    base.Add(key, value);
                }
                catch
                {

                }
            }
        }

        public Object Get(TKey key)
        {
            if (base.ContainsKey(key))
            {
                return base[key];
            }
            return null;
        }
    }

    public class rtInfo
    {
        public myTreeNode srcNode = new myTreeNode();
        public myTreeNode targetNode = new myTreeNode();
    }
}
