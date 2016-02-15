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

public partial class CommonControl_CommonInputControl : System.Web.UI.UserControl
{
    private string InputExpression;
    private bool IsCanPaste; 
    private bool IsCanUseKeyboard; 
    private bool IsProcessQuickInput; 
    private bool IsCanClear = false;
    private string EnterOrTabResFun;
    private string ReplaceExpression;
    private bool _isKeepWhitespace=false;
    private bool _IsCanInputChinese=false;
    private ServiceAgent fisService = ServiceAgent.getInstance();

    protected void Page_Load(object sender, EventArgs e)
    {
        txtInput.Attributes.Add("onkeydown", "onDateKeyDown()");
        txtInput.Attributes.Add("expression", this.InputRegularExpression);
        txtInput.Attributes.Add("keyboard", this.IsCanUseKeyboard.ToString());
        txtInput.Style.Add("ime-mode", "disabled");
        txtInput.Attributes.Add("replaceExpression", this.ReplaceRegularExpression);

        if (!IsPaste)
        {
            txtInput.Attributes.Add("onpaste", "return false;");
            txtInput.Attributes.Add("ondrop", "return false;");
        }
        else
        {
            txtInput.Attributes.Add("onpaste", "replacePaste(this)");
            txtInput.Attributes.Add("ondrop", "replacePasteDrop(this)");
        }

        txtInput.Attributes.Add("onblur", "UpperCase(this)");
        if (_IsCanInputChinese)
        {
            txtInput.Style.Add("ime-mode", "");
        }
        else
        {
            txtInput.Style.Add("ime-mode", "disabled");
        }
        if (IsProcessQuickInput)
        {
            txtInput.Attributes.Add("ProcessQuickInput", "true");
        }

        if (this.IsClear)
        {
            txtInput.Attributes.Add("isClear", "true");
        }

        if (this.EnterOrTabFun != null)
        {
            txtInput.Attributes.Add("enterOrTabFun", this.EnterOrTabFun);
        }

        if (this._isKeepWhitespace)
        {
            txtInput.Attributes.Add("keepWhitespace", "true");
        }

        string allowTime = string.Empty;

        try
        {
            allowTime = WebCommonMethod.getConfiguration("KEYIN_ALLOW_TIME");
            
            this.hidKeyAllowTime.Value = allowTime;
        }   
        catch (Exception ex)
        {
        }
    }

    public int MaxLength
    {
        set
        {
            txtInput.MaxLength = value;
        }

        get
        {
            return txtInput.MaxLength;
        }
    }

    public string Width
    {
        set
        {
            txtInput.Style.Add("width", value);
        }

        get
        {
            return string.Empty;
        }
    }

    public string Height
    {
        set
        {
            txtInput.Style.Add("height", value);
        }

        get
        {
            return string.Empty;
        }
    }

    public string CssClass
    {
        set
        {
            txtInput.CssClass = value;
        }

        get
        {
            return txtInput.CssClass;
        }
    }

    public bool IsReadOnly
    {
        set
        {
            txtInput.ReadOnly = value;
        }

        get
        {
            return txtInput.ReadOnly;
        }
    }

    public bool IsEnabled
    {
        set
        {
            txtInput.Enabled = value;
        }

        get
        {
            return txtInput.Enabled;
        }
    }
    public bool IsCanInputChinese
    {
        set
        { _IsCanInputChinese = value; }
        get
        { return _IsCanInputChinese; }
    }
    public string ReplaceRegularExpression
    {
        set
        {
            this.ReplaceExpression = value;
        }

        get
        {
            return this.ReplaceExpression;
        }
    }

    public string InputRegularExpression
    {
        set
        {
            this.InputExpression = value;
        }

        get
        {
            return this.InputExpression;
        }
    }
    
    public bool IsPaste
    {
        set
        {
            this.IsCanPaste = value;
        }

        get
        {
            return this.IsCanPaste;
        }
    }    
    
    public bool CanUseKeyboard
    {
        set
        {
            this.IsCanUseKeyboard = value;
        }

        get
        {
            return this.IsCanUseKeyboard;
        }
    } 

    public bool ProcessQuickInput
    {
        set
        {
            this.IsProcessQuickInput = value;
        }

        get
        {
            return this.IsProcessQuickInput;
        }
    } 

    public bool IsClear
    {
        set
        {
            this.IsCanClear = value;
        }

        get
        {
            return this.IsCanClear;
        }
    }
    public bool IsKeepWhitespace
    {
        set
        {
            this._isKeepWhitespace = value;
        }

        get
        {
            return this._isKeepWhitespace;
        }
    }
    public string EnterOrTabFun
    {
        set
        {
            this.EnterOrTabResFun = value;
        }

        get
        {
            return this.EnterOrTabResFun;
        }
    }


}
