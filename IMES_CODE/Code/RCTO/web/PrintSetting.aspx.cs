using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using com.inventec.iMESWEB;
using IMES.Station.Interface.CommonIntf;
using IMES.Station.Interface.StationIntf;
using System.Collections.Generic;
using IMES.Infrastructure;

public partial class _PrintSetting : System.Web.UI.Page
{
    private int initRowsCount = 6;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public string stationHid = String.Empty ;
    public string pcode = String.Empty;
    IPrintTemplate iPrintTemplate = ServiceAgent.getInstance().GetObjectByName<IPrintTemplate>(WebConstant.CommonObject);

    ITestStation iTestStation = ServiceAgent.getInstance().GetObjectByName<ITestStation>(WebConstant.CommonObject);
    public string portNumber = WebCommonMethod.getConfiguration("PortNumber");

    protected void Page_Load(object sender, EventArgs e)
    {


            
            if (!Page.IsPostBack)
            {
                stationHid = Request["Station"];
                pcode = Request["PCode"];

                this.stationHidden.Value = stationHid;
                this.pCodeHidden.Value = pcode; 
                initLabel();
                this.gridview.DataSource = getDataTable(pcode);
                this.gridview.DataBind();
                initTableColumnHeader();
              
                

            }
            //this.gridview.Attributes.Add("ondblclick", "editCell(event)");
       

    }

   /// <summary>
   /// ��ȡ����
   /// </summary>
   /// <returns></returns>
    private DataTable getDataTable(string pcode)
    {

        DataTable dt = initTable();
        DataRow newRow;
        string value = String.Empty;
        string printer = String.Empty;
        string[] testArray;
        char[] separator = { ',' };
        string xOffset = String.Empty;
        string yOffset = String.Empty;
        string template = String.Empty;
        string ruleMode = String.Empty;
        string printMode = String.Empty;
        int j = 0;
        try
        {

            IList<string> labelTypeList = iPrintTemplate.GetPrintLabelTypeList(pcode);
           
            if (labelTypeList != null && labelTypeList.Count != 0)
            {
                foreach (string temp in labelTypeList)
                {
                    newRow = dt.NewRow();
                    newRow["LabelType"] = temp;
                    // printMode = getMode
                    printMode = iPrintTemplate.GetPrintMode(temp);
                   
                    if (printMode == "0")
                    {
                        newRow["Mode"] = "BAT";
                        newRow["PrintMode"] = "0";
                        //��¼������һ�еĴ�ӡģʽ
                        if (j == 0)
                        {
                            this.firstLineMode.Value = "0";
                        }
                    }
					else if (printMode == "3")
                    {
                        newRow["Mode"] = "BarTender";
                        newRow["PrintMode"] = printMode;
                        //记录高亮第一行的打印模式
                        if (j == 0)
                        {
                            this.firstLineMode.Value = printMode;
                        }
                    }
                    else if (printMode == "4")
                    {
                        newRow["Mode"] = "BartenderServer";
                        newRow["PrintMode"] = printMode;
                        //记录高亮第一行的打印模式
                        if (j == 0)
                        {
                            this.firstLineMode.Value = printMode;
                        }
                    }
                    else
                    {
                        newRow["Mode"] = "Template";
                        newRow["PrintMode"] = "1";
                        if (j == 0)
                        {
                            this.firstLineMode.Value = "1";
                        }
                    }
                    newRow["RuleMode"] = iPrintTemplate.GetRuleMode(temp);


                    value = getCookieValue(pcode, temp);
                    if (value != "")
                    {
                        testArray = value.Split(separator);
                        template = testArray[0];
                        xOffset = testArray[3];
                        yOffset = testArray[4];
                        printer = testArray[5];

                    }
                    else
                    {
                        template = "";
                    }

                    if (xOffset == "")
                    {
                        newRow["X"] = "10";
                    }
                    else
                    {
                        newRow["X"] = xOffset;
                    }



                    if (yOffset == "")
                    {
                        newRow["Y"] = "10";
                    }
                    else
                    {
                        newRow["Y"] = yOffset;
                    }
                    if (printer != "")
                    {
                        newRow["Printer"] = HttpUtility.UrlDecode(printer);
                    }
                    else
                    {
                        newRow["Printer"] = "";
                    }

                    newRow["Template"] = template;
                    dt.Rows.Add(newRow);
                    //if (j == 0)
                    //{
                    //    highLightNewMOList();
                    //}
                    //j++;
                }


                if (labelTypeList.Count < initRowsCount)
                {
                    for (int i = labelTypeList.Count; i < initRowsCount; i++)
                    {
                        newRow = dt.NewRow();
                        dt.Rows.Add(newRow);
                    }

                }
            }
            else
            {

                for (int i = 0; i < initRowsCount; i++)
                {
                    newRow = dt.NewRow();
                    //newRow["LabelType"] = "c" + i;

                    //if (i % 2 == 0)
                    //{
                    //    newRow["Mode"] = "BAT";
                    //    newRow["PrintMode"] = "0";
                    //    newRow["RuleMode"] = "Rule0";
                    //}
                    //else
                    //{
                    //    newRow["Mode"] = "Template";
                    //    newRow["PrintMode"] = "1";
                    //    newRow["RuleMode"] = "Rule1";
                    //}


                    //value = getCookieValue(pcode, "c" + i);
                    //if (value != "")
                    //{
                    //    testArray = value.Split(separator);
                    //    template = testArray[0];
                    //    xOffset = testArray[3];
                    //    yOffset = testArray[4];
                    //    printer = testArray[5];


                    //}


                    //if (xOffset == "")
                    //{
                    //    newRow["X"] = "10";
                    //}
                    //else
                    //{
                    //    newRow["X"] = xOffset;
                    //}



                    //if (yOffset == "")
                    //{
                    //    newRow["Y"] = "10";
                    //}
                    //else
                    //{
                    //    newRow["Y"] = yOffset;
                    //}

                    //if (printer != "")
                    //{
                    //    newRow["Printer"] = HttpUtility.UrlDecode(printer);
                    //}
                    //else
                    //{
                    //    newRow["Printer"] = "";
                    //}
                    //newRow["Template"] = template;
                   
                    dt.Rows.Add(newRow);
                }

                //for (int i = 0; i < 2; i++)
                //{
                //    newRow = dt.NewRow();
                   

                //    dt.Rows.Add(newRow);
                //}
            }

        }

        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }

        return dt;
      
    }

    private string getCookieValue(string pcode, string labelName)
    {
        HttpCookie cookie = Request.Cookies[pcode + ":" + labelName];
        if (cookie != null)
        {
          
    
            return cookie.Value.ToString();
        }
        return "";

    }
    /// <summary>
    /// ��ʼ��������
    /// </summary>
    /// <returns></returns>
    private DataTable initTable()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("LabelType", Type.GetType("System.String"));
        retTable.Columns.Add("Template", Type.GetType("System.String"));
        retTable.Columns.Add("Mode", Type.GetType("System.String"));
        retTable.Columns.Add("X", Type.GetType("System.String"));
        retTable.Columns.Add("Y", Type.GetType("System.String"));
        retTable.Columns.Add("Printer", Type.GetType("System.String"));
        retTable.Columns.Add("RuleMode", Type.GetType("System.String"));
        retTable.Columns.Add("PrintMode", Type.GetType("System.String"));
        
        return retTable;


    }

    /// <summary>
    /// ��ʼ����̬��ǩ
    /// </summary>
    
    private void initLabel()
    {
        //this.title.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblTitle");
        this.lblSettingTip1.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblSettingTip1");
        this.lblSettingTip2.Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblSettingTip2");
       
        //            <bug>
        //            BUG NO:ITC-1122-0091 
        //            REASON:��ȡstation����
        //            </bug>
        //this.station.Text = iTestStation.GeStationDescr(stationHid);
        this.btnCancel.Value = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnCanCel");
        this.btnOK.Value = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_btnOK"); 


    }

    /// <summary>
    /// ���ñ�������Ƽ����
    /// </summary>
    /// <returns></returns>
    private void initTableColumnHeader()
    {

        this.gridview.HeaderRow.Cells[0].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblLabelType");
        this.gridview.HeaderRow.Cells[1].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblTemplate");
        this.gridview.HeaderRow.Cells[2].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblMode");
        this.gridview.HeaderRow.Cells[3].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblX");
        this.gridview.HeaderRow.Cells[4].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblY");
        this.gridview.HeaderRow.Cells[5].Text = Resources.iMESGlobalDisplay.ResourceManager.GetString(Pre + "_lblPrinter");
        this.gridview.HeaderRow.Cells[0].Width = Unit.Pixel(120);
        this.gridview.HeaderRow.Cells[1].Width = Unit.Pixel(120);
        this.gridview.HeaderRow.Cells[2].Width = Unit.Pixel(80);
        this.gridview.HeaderRow.Cells[3].Width = Unit.Pixel(50);
        this.gridview.HeaderRow.Cells[4].Width = Unit.Pixel(50);
        this.gridview.HeaderRow.Cells[6].Width = Unit.Pixel(0);
        this.gridview.HeaderRow.Cells[7].Width = Unit.Pixel(0);
 


      
      
     
    }

    /// <summary>
    /// ���������Ϣ
    /// </summary>
    /// <param name="er"></param>
    private void writeToAlertMessage(String er)
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "ShowMessage('" + er + "');" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "writeToAlertMessageAgent", script, false);
    }

    /// <summary>
    /// Ϊ����м�tooltip
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewExt1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[6].Attributes.Add("style", "display:none;");
        e.Row.Cells[7].Attributes.Add("style", "display:none;");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
             
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
                if (i == 5)
                {
                    e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace(" ", "&nbsp;");
                }
                
            }
        }
    }



    /// <summary>
    ///����NewMO List�ĵ�һ��
    /// </summary>  
    private void highLightNewMOList()
    {
        String script = "<script language='javascript'>" + "\r\n" +
              "HighLightNewMOList();" + "\r\n" +
              "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "HighLightNewMOList", script, false);
    }
}
