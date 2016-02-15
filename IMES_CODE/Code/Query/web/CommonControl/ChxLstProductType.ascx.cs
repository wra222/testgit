using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonControl_ChxLstProductType : System.Web.UI.UserControl
{
    private bool _isVertical;
    private bool _isHide;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        { 
            if(_isVertical)
            {
                chxPrd.RepeatDirection = RepeatDirection.Vertical;
            }
            chxPrd.Visible = !_isHide;
        }
    }
    public bool IsHide
    {
        get { return _isHide; }
        set { _isHide = value; }
    }
    public bool IsVertical
    {
        get { return _isVertical; }
        set { _isVertical = value; }
    }
    public string GetCheckList()
    {
        var s = chxPrd.Items.Cast<ListItem>()
                   .Where(item => item.Selected)
                   .Aggregate("", (current, item) => current + (item.Text + ","));
        s=s.Length>0?s.Substring(0,s.Length - 1):s;
   
        return s;
    
    }
}
