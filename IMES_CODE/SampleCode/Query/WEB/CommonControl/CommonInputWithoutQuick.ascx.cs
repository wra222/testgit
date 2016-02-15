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

public partial class CommonControl_CommonInputWithoutQuick : System.Web.UI.UserControl
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
        this.txtInput_1.Attributes.Add("onkeydown", "onDateKeyDown_1()");
        txtInput_1.Attributes.Add("expression", this.InputRegularExpression);
        txtInput_1.Attributes.Add("keyboard", this.IsCanUseKeyboard.ToString());
        txtInput_1.Style.Add("ime-mode", "disabled");
        txtInput_1.Attributes.Add("replaceExpression", this.ReplaceRegularExpression);

        if (!IsPaste)
        {
            txtInput_1.Attributes.Add("onpaste", "return false;");
            txtInput_1.Attributes.Add("ondrop", "return false;");
        }
        else
        {
            txtInput_1.Attributes.Add("onpaste", "replacePaste(this)");
            txtInput_1.Attributes.Add("ondrop", "replacePasteDrop(this)");
        }

        txtInput_1.Attributes.Add("onblur", "UpperCase(this)");

        if (IsProcessQuickInput)
        {
            txtInput_1.Attributes.Add("ProcessQuickInput", "true");
        }

        if (this.IsClear)
        {
            txtInput_1.Attributes.Add("isClear", "true");
        }

        if (this.EnterOrTabFun != null)
        {
            txtInput_1.Attributes.Add("enterOrTabFun", this.EnterOrTabFun);
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
            txtInput_1.MaxLength = value;
        }

        get
        {
            return txtInput_1.MaxLength;
        }
    }

    public string Width
    {
        set
        {
            txtInput_1.Style.Add("width", value);
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
            txtInput_1.Style.Add("height", value);
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
            txtInput_1.CssClass = value;
        }

        get
        {
            return txtInput_1.CssClass;
        }
    }

    public bool IsReadOnly
    {
        set
        {
            txtInput_1.ReadOnly = value;
        }

        get
        {
            return txtInput_1.ReadOnly;
        }
    }

    public bool IsEnabled
    {
        set
        {
            txtInput_1.Enabled = value;
        }

        get
        {
            return txtInput_1.Enabled;
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
