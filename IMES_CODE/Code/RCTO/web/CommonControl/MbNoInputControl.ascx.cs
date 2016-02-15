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

    private ServiceAgent fisService = ServiceAgent.getInstance();

    protected void Page_Load(object sender, EventArgs e)
    {
        txtInput1.Attributes.Add("onkeydown", "onDateKeyDown2()");
        txtInput1.Attributes.Add("onkeypress", "keyPress2()");
        txtInput1.Attributes.Add("expression", this.InputRegularExpression);
        txtInput1.Attributes.Add("keyboard", this.IsCanUseKeyboard.ToString());
        txtInput1.Style.Add("ime-mode", "disabled");
        txtInput1.Attributes.Add("replaceExpression", this.ReplaceRegularExpression);

        if (!IsPaste)
        {
            txtInput1.Attributes.Add("onpaste", "return false;");
            txtInput1.Attributes.Add("ondrop", "return false;");
        }
        else
        {
            txtInput1.Attributes.Add("onpaste", "replacePaste(this)");
            txtInput1.Attributes.Add("ondrop", "replacePasteDrop(this)");
        }

        txtInput1.Attributes.Add("onblur", "UpperCase(this)");

        if (IsProcessQuickInput)
        {
            txtInput1.Attributes.Add("ProcessQuickInput", "true");
        }

        if (this.IsClear)
        {
            txtInput1.Attributes.Add("isClear", "true");
        }

        if (this.EnterOrTabFun != null)
        {
            txtInput1.Attributes.Add("enterOrTabFun", this.EnterOrTabFun);
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
            txtInput1.MaxLength = value;
        }

        get
        {
            return txtInput1.MaxLength;
        }
    }

    public string Width
    {
        set
        {
            txtInput1.Style.Add("width", value);
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
            txtInput1.Style.Add("height", value);
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
            txtInput1.CssClass = value;
        }

        get
        {
            return txtInput1.CssClass;
        }
    }

    public bool IsReadOnly
    {
        set
        {
            txtInput1.ReadOnly = value;
        }

        get
        {
            return txtInput1.ReadOnly;
        }
    }

    public bool IsEnabled
    {
        set
        {
            txtInput1.Enabled = value;
        }

        get
        {
            return txtInput1.Enabled;
        }
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
