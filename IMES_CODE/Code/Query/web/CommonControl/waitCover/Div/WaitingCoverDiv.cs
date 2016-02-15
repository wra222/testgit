using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace waitCover
{
    public class WaitingCoverDiv : ClientCoverDiv
    {

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterQuitScript();
        }

        internal void RegisterQuitScript()
        {
            #region ���뿪���ȴ��͹رյȴ��Ľű�
            if (AutoScript)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("WaitingCoverDiv"))
                {
                   string script=JavaScriptConstant.WaitingCoverDiv.Replace(JavaScriptConstant.CLIENT_ID, ClientID);
                   if (string.IsNullOrEmpty(KeyDownFun))
                   {
                       KeyDownFun = "";
                   }
                    script = script.Replace(JavaScriptConstant.KeyDownFun, KeyDownFun);
                    Page.ClientScript.RegisterClientScriptBlock(
                        this.GetType(),
                        "WaitingCoverDiv",                        
                        script);
                }
            }
            #endregion
        }

        [
        Description("��ʾ��ͼƬ"),
        Category("Appearance"),
        Editor("System.Web.UI.Design.UrlEditor", typeof(System.Drawing.Design.UITypeEditor)),
        ]
        public override string ImgSrc
        {
            set
            {
                _imgSrc = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_imgSrc))
                {
                    return "~/Images/wait_animated.gif";
                }
                return _imgSrc;
            }
        }

        [
        Description("��ʾ��ͼƬ��˵��"),
        Category("Appearance")]
        public override string ImgAlt
        {
            set
            {
                _imgAlt = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_imgAlt))
                {
                    return Resource.WaitingAlt;
                }
                return _imgAlt;
            }
        }

        [
        Description("��ʾ��ͼƬ��˵��"),
        Category("Appearance")]
        public override string Text
        {
            set
            {
                _text = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_text))
                {
                    return Resource.WaitingTxt;
                }
                return _text;
            }
        }     

        [
      Description("��ʾ��ͼƬ����ʽ"),
      Category("Appearance")]
        public override string ImgStyle
        {
            set
            {
                _imgStyle = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_imgCssClass))
                {
                    return "width:50%; text-align:right; vertical-align:center;";
                }
                return _imgStyle;
            }
        }

        [
        Description("��ʾ�����ֵ���ʽ"),
        Category("Appearance")]
        public override string TxtStyle
        {
            set
            {
                _txtStyle = value;
            }
            get
            {
                if (string.IsNullOrEmpty(_txtStyle))
                {
                    return "width:50%; font-size: 16pt; font-family: Verdana; font-weight: normal; color:Black; vertical-align:center;";
                }
                return _txtStyle;
            }
        }

        [
        Description("�Ƿ��Զ����ɽű�"),
        Category("Behavior")]
        public virtual bool AutoScript
        {
            set
            {
                _autoScript = value;
            }
            get
            {

                return _autoScript;
            }
        }

        [
       Description("key down function"),
       Category("Behavior")]
        public virtual string  KeyDownFun
        {
            set
            {
                _keydownFun = value;
            }
            get
            {

                return _keydownFun;
            }
        }

        protected bool _autoScript = true;

    }

}
