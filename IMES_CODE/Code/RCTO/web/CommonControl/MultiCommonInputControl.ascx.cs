using System;
using com.inventec.iMESWEB;

public partial class CommonControl_MultiCommonInputControl : System.Web.UI.UserControl
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
        txtInputMulti.Attributes.Add("onkeydown", "onDateKeyDownMulti(this)");
        txtInputMulti.Attributes.Add("expression", this.InputRegularExpression);
        txtInputMulti.Attributes.Add("keyboard", this.IsCanUseKeyboard.ToString());
        txtInputMulti.Style.Add("ime-mode", "disabled");
        txtInputMulti.Attributes.Add("replaceExpression", this.ReplaceRegularExpression);

        if (!IsPaste)
        {
            txtInputMulti.Attributes.Add("onpaste", "return false;");
            txtInputMulti.Attributes.Add("ondrop", "return false;");
        }
        else
        {
            txtInputMulti.Attributes.Add("onpaste", "replacePaste(this)");
            txtInputMulti.Attributes.Add("ondrop", "replacePasteDrop(this)");
        }

        txtInputMulti.Attributes.Add("onblur", "UpperCase(this)");

        if (IsProcessQuickInput)
        {
            txtInputMulti.Attributes.Add("ProcessQuickInput", "true");
        }

        if (this.IsClear)
        {
            txtInputMulti.Attributes.Add("isClear", "true");
        }

        if (this.EnterOrTabFun != null)
        {
            txtInputMulti.Attributes.Add("enterOrTabFun", this.EnterOrTabFun);
        }

        //added by Lucy Liu
        txtInputMulti.Attributes.Add("timeStart", "");
        txtInputMulti.Attributes.Add("timeStop", "");
        txtInputMulti.Attributes.Add("readIndex", "0");
        txtInputMulti.Attributes.Add("writeIndex", "-1");
        txtInputMulti.Attributes.Add("finishInputFlag", "false");
        txtInputMulti.Attributes.Add("sInput", "");

        string allowTime = string.Empty;

        try
        {
            allowTime = WebCommonMethod.getConfiguration("KEYIN_ALLOW_TIME");

            this.hidKeyAllowTimeMulti.Value = allowTime;
        }
        catch (Exception ex)
        {
        }
    }

    public int MaxLength
    {
        set
        {
            txtInputMulti.MaxLength = value;
        }

        get
        {
            return txtInputMulti.MaxLength;
        }
    }

    public string Width
    {
        set
        {
            txtInputMulti.Style.Add("width", value);
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
            txtInputMulti.Style.Add("height", value);
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
            txtInputMulti.CssClass = value;
        }

        get
        {
            return txtInputMulti.CssClass;
        }
    }

    public bool IsReadOnly
    {
        set
        {
            txtInputMulti.ReadOnly = value;
        }

        get
        {
            return txtInputMulti.ReadOnly;
        }
    }

    public bool IsEnabled
    {
        set
        {
            txtInputMulti.Enabled = value;
        }

        get
        {
            return txtInputMulti.Enabled;
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

    public string InputValue
    {
        set
        {
            this.txtInputMulti.Text = value;
        }

        get
        {
            return this.txtInputMulti.Text;
        }
    }


}
