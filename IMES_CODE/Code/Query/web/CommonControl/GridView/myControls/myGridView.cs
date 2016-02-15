/*
 * INVENTEC corporation (c)2008 all rights reserved. 
 * Description:gridviewExt control
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2008-10-10  Zhao Meili(EB2)        Create 
 * 2009-02-01  Zhao Meili(EB2)        Modify:ITC-932-0034 solved
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.ComponentModel;
using myControls.Constant;
using System.Configuration;

namespace myControls
{

    [ToolboxData(@"<{0}:GridViewExt  
    runat=""server""
    AutoGenerateColumns=""false""
    GvExtWidth=""95%""
    GvExtHeight=""200px""
    AutoHighlightScrollByValue =""false"" 
    GetTemplateValueEnable=""false"" 
    SetTemplateValueEnable=""false"" 
    OnGvExtRowClick=""""  
    OnGvExtRowDblClick="""" 
    HiddenColCount=0
    >
   </{0}:GridViewExt>")
      //ParseChildren(true),
    //PersistChildren(false)
    ]
    public class GridViewExt : GridView
    {
        #region  variant members
        //key column
        private string _keyCol;
        //scrollTop Postition
        private string _scrollTopPosition;
        //div height
        private string _gvDivHeight;
        //div width
        private string _gvDivWidth;
        //set scroll by value or not
        private bool _setScrollAutoByValue;
        //set onclick event script name
        private string _onGvExtRowClick;
        //set ondblclick event script name
        private string _onGvExtRowDblClick;
        //whether support set Template controls Value
        private bool _setTemplateValue;
        //whether support set Template controls Value
        private bool _getTemplateValue;
        //hidden columns count
        private int _hiddenColcount;
        #endregion

        #region override method
        public GridViewExt()
        {
            this.PreRender += new EventHandler(GridViewExt_PreRender);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string tablecss = this.CssClass;
            writer.Write("<div id='div_" + ClientID + "' align =\"center\" style=\"overflow: auto; width: "
                  + GvExtWidth + "; height: "
                  + GvExtHeight + ";position :relative ;\" ");
           // writer.Write("class='" + tablecss + "'>");
            writer.Write(">");
            base.Attributes.Add("style", "table-layout:fixed;");
           // base.Style.Add(HtmlTextWriterStyle.TextAlign, "center"); 
            //this.CssClass = ""; 
            this.CellPadding = 0;
            this.BorderWidth = Unit.Pixel(0);
            this.HiddenColCount = 0; 
            base.Render(writer);
            writer.Write("</div>");           
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            if ((this.AllowPaging) && ((this.PagerSettings.Mode == PagerButtons.Numeric) || (this.PagerSettings.Mode == PagerButtons.NumericFirstLast)) && (e.Row.RowType == DataControlRowType.Pager))
            {
                ControlCollection cls = e.Row.Controls[0].Controls[0].Controls[0].Controls;
                string nextPagesText = retStringValue(ConfigurationManager.AppSettings.Get("gvExtPaging-NextPages"));
                string previousPagesText = retStringValue(ConfigurationManager.AppSettings.Get("gvExtPaging-PreviousPages"));
                string beforeIndexText = retStringValue(ConfigurationManager.AppSettings.Get("gvExtPaging-BeforeIndex"));
                string afterIndexText = retStringValue(ConfigurationManager.AppSettings.Get("gvExtPaging-AfterIndex"));
                bool indexTextChanged = true;
                if (string.IsNullOrEmpty(beforeIndexText) && string.IsNullOrEmpty(afterIndexText))
                {
                    indexTextChanged = false;
                }
                for (int i = 0; i < cls.Count; i++)
                {
                    if (cls[i].Controls[0].GetType().ToString().Trim() == "System.Web.UI.WebControls.DataControlPagerLinkButton")
                    {
                        LinkButton lb = (LinkButton)cls[i].Controls[0];
                        if ((lb.Text == "...") && (i > 1))
                        {
                            lb.Text = nextPagesText;
                        }
                        else if (lb.Text == "...")
                        {
                            lb.Text = previousPagesText;
                        }
                        else if (!indexTextChanged)
                        {
                            continue;
                        }
                        else if ((this.PagerSettings.Mode == PagerButtons.Numeric) || ((this.PagerSettings.Mode == PagerButtons.NumericFirstLast) && (lb.Text != this.PagerSettings.LastPageText) && (lb.Text != this.PagerSettings.FirstPageText)))
                        {
                            lb.Text = beforeIndexText + lb.Text + afterIndexText;
                        }
                    }
                    else if (indexTextChanged && (cls[i].Controls[0].GetType().ToString().Trim() == "System.Web.UI.WebControls.Label"))
                    {
                        Label lbl = (Label)cls[i].Controls[0];
                        lbl.Text = beforeIndexText + lbl.Text + afterIndexText;
                    }

                }

                TableCell mycell = new TableCell();
                TextBox txt = new TextBox();
                txt.Text = (this.PageIndex+1).ToString() ;
                txt.ID = "pageIndexTxt";
                txt.CssClass = "common_grid_PagerGotoText"; 
                HyperLink hl = new HyperLink();
                hl.Text = "GoTo";
                hl.NavigateUrl = "javascript:GoToPageByIndex('" + this.ClientID.Replace("_", "$") + "','" + e.Row.ClientID + "_" + txt.ID + "'," + this.PageCount + ")";
               mycell.Controls.Add(hl);
               mycell.Controls.Add(txt);
     
                TableCell hidCell = new TableCell();
                hidCell.Width = Unit.Pixel(10);

               // e.Row.Cells[0].ColumnSpan = this.Columns.Count - this.HiddenColCount;

                Label totallbl = new Label();
                totallbl.Text = getSpacebyCount(5) + "Total " + this.PageCount + " Pages " + getSpacebyCount(2);
                mycell.Controls.Add(totallbl);
                e.Row.Cells[0].Controls[0].Controls[0].Controls.Add(hidCell);
                e.Row.Cells[0].Controls[0].Controls[0].Controls.Add(mycell);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                char[] sep ={ '_' };
                string[] gvClientID = e.Row.ClientID.Split(sep);

                int index = int.Parse(gvClientID[gvClientID.Length - 1].Substring(3)) - 2;
                e.Row.ID = index.ToString(); 
                e.Row.Attributes.Add("id", e.Row.ClientID);
                e.Row.Attributes.Add("index", index.ToString());
                if (!String.IsNullOrEmpty(OnGvExtRowClick))
                {
                    e.Row.Attributes.Add("onclick", OnGvExtRowClick);
                    e.Row.Attributes["style"] = "Cursor:hand" ;
                  
                }
                if (!String.IsNullOrEmpty(OnGvExtRowDblClick))
                {
                    e.Row.Attributes.Add("ondblclick", OnGvExtRowDblClick);
                }
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Wrap = false;                    
                }
                
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    //ITC-932-0034 solved
                    e.Row.Cells[i].Attributes.Add("style", e.Row.Cells[i].Attributes["style"] + "position: relative;top:expression(this.offsetParent.scrollTop-1);");
                    e.Row.Cells[i].CssClass = base.HeaderStyle.CssClass;
                }
            }

            base.OnRowDataBound(e);
            
        }


        private string getSpacebyCount(int count)
        {
            string str = "";
            for (int i = 0; i < count; i++)
            {
                str += "&nbsp;";
            }
            return  System.Web.HttpUtility.HtmlDecode(str);
        }

        private string  retStringValue(object obj)
        {
            if (obj == null)
            {
                return String.Empty;
            }
            else
            {
                return obj.ToString(); 
            }
        }


        #endregion

        #region gridview property

        /// <summary>
        /// by itc207013,set highlightrow position
        /// </summary>
        public virtual string HighLightRowPosition
        {
            get
            {
                if (_scrollTopPosition == null)
                {
                    _scrollTopPosition = ConstantForControls.ScrollDefaultPosition ;
                }
                return _scrollTopPosition;
            }
            set
            {
                _scrollTopPosition = value;
            }
        }

        /// <summary>
        ///  by itc207013, set gridview height
        /// </summary>
        public virtual string GvExtHeight
        {
            get
            {
                if (_gvDivHeight == null)
                {
                    _gvDivHeight = "200px";
                }
                return _gvDivHeight;
            }
            set
            {
                _gvDivHeight = value;
            }
        }

        /// <summary>
        ///  by itc207013,set gridview width
        /// </summary>
        public virtual string GvExtWidth
        {
            get
            {
                if (_gvDivWidth == null)
                {
                    _gvDivWidth = "95%";
                }
                return _gvDivWidth;
            }
            set
            {
                _gvDivWidth = value;
            }
        }

        /// <summary>
        ///  by itc207013, set auto highlight and scroll
        /// </summary>
        public virtual bool AutoHighlightScrollByValue
        {
            get
            {
                return _setScrollAutoByValue;
            }
            set
            {
                _setScrollAutoByValue = value;
            }
        }

        /// <summary>
        /// by itc207013,set gridview on row click event
        /// </summary>
        public virtual string  OnGvExtRowClick
        {
            get
            {
                if (_onGvExtRowClick == null)
                {
                    _onGvExtRowClick = "";
                }
                return _onGvExtRowClick;
            }
            set
            {
                _onGvExtRowClick = value;
            }
        }

        /// <summary>
        /// by itc207013,set gridview on row double click event
        /// </summary>
        public virtual string  OnGvExtRowDblClick
        {
            get
            {
                if (_onGvExtRowDblClick == null)
                {
                    _onGvExtRowDblClick = "";
                }
                return _onGvExtRowDblClick;
            }
            set
            {
                _onGvExtRowDblClick = value;
            }
        }

        /// <summary>
        /// by itc207013,set template value method load or not
        /// </summary>
        public virtual bool SetTemplateValueEnable
        {
            get
            {
                return _setTemplateValue;
            }
            set
            {
                _setTemplateValue = value;
            }
        }

        /// <summary>
        /// by itc207013,get template value method load or not
        /// </summary>
        public virtual bool GetTemplateValueEnable
        {
            get
            {
                return _getTemplateValue;
            }
            set
            {
                _getTemplateValue = value;
            }
        }

        public virtual int HiddenColCount
        {
            get
            {
                return _hiddenColcount;
            }
            set
            {
                _hiddenColcount = value;
            }
        }
        #endregion

        /// <summary>
        ///  by itc207013
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scriptContent"></param>
        private void registerScript(string name, string scriptContent)
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered(name))
            {
                Page.ClientScript.RegisterClientScriptBlock(
                    this.GetType(),
                    name, scriptContent
                    );
            }
        }


        /// <summary>
        /// GridViewExt_PreRender
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void GridViewExt_PreRender(object sender, EventArgs e)
        {
            #region register script
            bool includeCommonJs = false;
            string scriptContent = "";

            if (AutoHighlightScrollByValue)
            {

                //replace clientID
                scriptContent = ConstantForControls.JSSetScrollTopByColValueForGvExt.Replace("var gvClientID='ForReplace';", "var gvClientID='" + ClientID + "';");
                //replace highlight position
                scriptContent = scriptContent.Replace("var fixPosition='1';", "var fixPosition='" + HighLightRowPosition + "';") ;
                //replace function name
                scriptContent = scriptContent.Replace("function setScrollTopForGvExt", "function setScrollTopForGvExt_" + ClientID);
                scriptContent = scriptContent.Replace("setCvExtRowSelected", "setCvExtRowSelected_" + ClientID);
                scriptContent = scriptContent.Replace("setRowNonSelected", "setRowNonSelected_" + ClientID);
                scriptContent = scriptContent.Replace("setRowSelectedByIndex", "setRowSelectedByIndex_" + ClientID);
                ////replace alternating Row css name
                //scriptContent = scriptContent.Replace("var alterRowCssNameForGvExt = 'ForPeplace';", "var alterRowCssNameForGvExt = '" + alterRowCss + "';");
                ////replace row css name
                //scriptContent = scriptContent.Replace("var rowCssNameForGvExt ='ForPeplace';", "var rowCssNameForGvExt ='" + rowCss + "';");
                ////replace selected row css name
                //scriptContent = scriptContent.Replace("var selectedRowCssNameForGvExt = 'ForPeplace';", "var selectedRowCssNameForGvExt ='" + selectRowCss + "';");
              
                this.registerScript("JsAutoHighlightScrollByValueForGvExt" + ClientID, scriptContent);


            }
            if (GetTemplateValueEnable)
            {
                scriptContent = ConstantForControls.JSgetTemplateCellValue.Replace("var gvClientID='ForReplace';", "var gvClientID='" + ClientID + "';");
                scriptContent = scriptContent.Replace("function getTemplateCellValueFromCvExt", "function getTemplateCellValueFromCvExt_" + ClientID);
                this.registerScript("JsgetTemplateCellValueForGvExt" + ClientID, scriptContent);
            }
            if (SetTemplateValueEnable)
            {
                scriptContent = ConstantForControls.JSsetTemplateCellValue.Replace("var gvClientID='ForReplace';", "var gvClientID='" + ClientID + "';");
                scriptContent = scriptContent.Replace("function setTemplateCellValueToCvExt", "function setTemplateCellValueToCvExt_" + ClientID);
                this.registerScript("JssetTemplateCellValueForGvExt" + ClientID, scriptContent);
            }

            if ((!includeCommonJs) && (GetTemplateValueEnable || SetTemplateValueEnable || AutoHighlightScrollByValue))
            {
                scriptContent = ConstantForControls.JsForGvExtRevokeMethod.Replace("setCvExtRowSelected(", "setCvExtRowSelected_" + ClientID + "(");

                this.registerScript("JsForGvExtRevokeMethodForGvExt" + ClientID, scriptContent);
                includeCommonJs = true;
            }

            string rowCss = this.RowStyle.CssClass;
            string alterRowCss = this.AlternatingRowStyle.CssClass;
            string clickEvent=this.OnGvExtRowClick ;
            string dblclickEvent = this.OnGvExtRowDblClick;

            if (alterRowCss == "")
            {
                alterRowCss = rowCss;
            }
            string selectRowCss = this.SelectedRowStyle.CssClass;

            scriptContent = ConstantForControls.JSsetCvExtRowSelected.Replace("var gvClientID='ForReplace';", "var gvClientID='" + ClientID + "';");
            //replace alternating Row css name
            scriptContent = scriptContent.Replace("var alterRowCssNameForGvExt = 'ForReplace';", "var alterRowCssNameForGvExt = '" + alterRowCss + "';");
            //replace row css name
            scriptContent = scriptContent.Replace("var rowCssNameForGvExt ='ForReplace';", "var rowCssNameForGvExt ='" + rowCss + "';");
            //replace selected row css name
            scriptContent = scriptContent.Replace("var selectedRowCssNameForGvExt = 'ForReplace';", "var selectedRowCssNameForGvExt ='" + selectRowCss + "';");
            scriptContent = scriptContent.Replace("function setCvExtRowSelected", "function setCvExtRowSelected_" + ClientID);
            scriptContent = scriptContent.Replace("setRowSelectedByIndex", "setRowSelectedByIndex_" + ClientID);
            scriptContent = scriptContent.Replace("function AddCvExtRowToBottom", "function AddCvExtRowToBottom_" + ClientID);
            if (!string.IsNullOrEmpty(clickEvent))
            {
                scriptContent = scriptContent.Replace(" var onclickEvent = 'ForReplace';", " var onclickEvent = '" + clickEvent + "';");
            }
            else
            {
                scriptContent = scriptContent.Replace(" var onclickEvent = 'ForReplace';", " var onclickEvent = '';");
            }
            if (!string.IsNullOrEmpty(dblclickEvent))
            {
                scriptContent = scriptContent.Replace("var ondblclickEvent = 'ForReplace';", "var ondblclickEvent = '" + dblclickEvent + "';");
            }
            else
            {
                scriptContent = scriptContent.Replace("var ondblclickEvent = 'ForReplace';", "var ondblclickEvent = '';");
            }
            scriptContent = scriptContent.Replace("function ChangeCvExtRowByIndex", "function ChangeCvExtRowByIndex_" + ClientID);
            scriptContent = scriptContent.Replace("function setRowNonSelected", "function setRowNonSelected_" + ClientID);


            this.registerScript("JssetCvExtRowSelectedForGvExt" + ClientID, scriptContent);
            #endregion
            
        }

    }
}
