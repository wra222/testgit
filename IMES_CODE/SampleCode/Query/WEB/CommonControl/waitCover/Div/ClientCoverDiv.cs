using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace waitCover
{   
    public class ClientCoverDiv : WebControl
    {
        public ClientCoverDiv()
            : base(HtmlTextWriterTag.Div)
        {
        }

        #region render
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            this.Page.VerifyRenderingInServerForm(this);
            //writer.Write("<div id=\"" + ID + "\"");
            writer.Write("<div id=\"" + ClientID  + "\"");
            if (!string.IsNullOrEmpty(CssClass))
            {
                writer.Write(" class=\"" + CssClass + "\"");
            }
            writer.Write(" style='position: absolute; filter: alpha(opacity=70); display: none;background-color: #E6E6FA; width: 100%;height:100%;'>");
            //writer.Write(">");
            //writer.Write(" style='position: absolute;'>");
            writer.Write("<table id=\"wait" + ID + "\"");
            if (!string.IsNullOrEmpty(TableCssClass))
            {
                writer.Write(" class=\"" + TableCssClass + "\"");
            }            
            writer.Write(  " style='position: absolute; width: 95%; height: 95%; vertical-align: bottom;'><tr>");
            //writer.Write(">");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(ImgSrc))
            {
                if (!string.IsNullOrEmpty(ImgCssClass))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, ImgCssClass);
                }

                if (!string.IsNullOrEmpty(ImgStyle))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, ImgStyle);
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, base.ResolveUrl(ImgSrc));                
                if (!string.IsNullOrEmpty(ImgAlt))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, ImgAlt);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            if (!string.IsNullOrEmpty(Text))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Style, TxtStyle);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, TxtCssClass);               
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(Text);
                writer.RenderEndTag();
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.Write(" </tr></table></div>");
        }
        #endregion


        [
        Description("显示的图片"),
        Category("Appearance"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        ]
        public virtual string ImgSrc
        {
            set
            {
                _imgSrc = value;
            }
            get
            {
                return _imgSrc;
            }
        }
        
        [
        Description("显示的图片的说明"),
        Category("Appearance")]
        public virtual string ImgAlt
        {
            set
            {
                _imgAlt = value;
            }
            get
            {
                return _imgAlt;
            }
        }

        [BrowsableAttribute(true)]
        [DescriptionAttribute("图片的样式")]
        [CategoryAttribute("Appearance")]
        public virtual string ImgCssClass
        {
            set
            {
                _imgCssClass = value;
            }
            get
            {
                return _imgCssClass;
            }
        }

        [BrowsableAttribute(true)]
        [DescriptionAttribute("文字的样式")]
        [CategoryAttribute("Appearance")]
        public virtual string TxtCssClass
        {
            set
            {
                _txtCssClass = value;
            }
            get
            {
                return _txtCssClass;
            }
        }

        [BrowsableAttribute(true)]
        [DescriptionAttribute("图片的样式")]
        [CategoryAttribute("Appearance")]
        public virtual string ImgStyle
        {
            set
            {
                _imgStyle = value;
            }
            get
            {
                return _imgStyle;
            }
        }

        [BrowsableAttribute(true)]
        [DescriptionAttribute("文字的样式")]
        [CategoryAttribute("Appearance")]
        public virtual string TxtStyle
        {
            set
            {
                _txtStyle = value;
            }
            get
            {
                return _txtStyle;
            }
        }

        [BrowsableAttribute(true)]
        [DescriptionAttribute("表的样式")]
        [CategoryAttribute("Appearance")]
        public virtual string TableCssClass
        {
            set
            {
                _tableCssClass = value;
            }
            get
            {
                return _tableCssClass;
            }
        }

        [BrowsableAttribute(true)]
        [DescriptionAttribute("显示的文字")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Appearance")]
        public virtual string Text
        {
            set
            {
                _text = value;

            }
            get
            {
                if (string.IsNullOrEmpty(_text))
                {
                    _text = string.Empty;
                }
                return _text;
            }
        }

        protected string _txtCssClass;
        protected string _tableCssClass;

        /// <summary>
        /// 图片的css
        /// </summary>
        protected string _imgCssClass;

        /// <summary>
        /// 按钮上显示的文字
        /// </summary>
        protected string _text;

        /// <summary>
        /// 按钮上显示的图片
        /// </summary>
        protected string _imgSrc;

        /// <summary>
        /// 图片说明文字
        /// </summary>
        protected string _imgAlt;

        /// <summary>
        /// 图片的样式
        /// </summary>
        protected string _imgStyle;

        /// <summary>
        /// 
        /// </summary>
        protected string _txtStyle;

        /// <summary>
        /// keydown function
        /// </summary>
        protected string _keydownFun;

    }
}
