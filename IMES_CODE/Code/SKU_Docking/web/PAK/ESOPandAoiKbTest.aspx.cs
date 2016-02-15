/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for LabelLightGuide Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Label Light Guide.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Label Light Guide.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-19  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Data;
using System.Web.Services;
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
using IMES.DataModel;
using System.Text;
using System.Globalization;
using System.Linq;
public partial class PAK_ESOPandAoiKbTest : IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    public String UserId;
    public String Customer;
	private const int DEFAULT_ROWS = 6;

    IESOPandAoiKbTest iESOPandAoiKbTest = ServiceAgent.getInstance().GetObjectByName<IESOPandAoiKbTest>(WebConstant.IESOPandAoiKbTest);

	public string JpgPath = ConfigurationManager.AppSettings["RDS_Server_FALabel"].ToString();
    protected string isTestAOI = WebCommonMethod.getConfiguration("IsTestAOI");
    /*
     * Answer to: ITC-1360-0578
     * Description: Make com setting configurable.
    */
    [WebMethod]
    public static IList<string> getCommSetting(string hName, string editor)
    {
        IList<string> ret = new List<string>();
        try
        {
            IList<COMSettingInfo> wsiList = ServiceAgent.getInstance().GetObjectByName<IESOPandAoiKbTest>(WebConstant.IESOPandAoiKbTest).getCommSetting(hName, editor);
        
            ret.Add(WebConstant.SUCCESSRET);
            ret.Add(wsiList[0].commPort.ToString());
            ret.Add(wsiList[0].baudRate);
            ret.Add(wsiList[0].rthreshold.ToString());
            ret.Add(wsiList[0].sthreshold.ToString());
            ret.Add(wsiList[0].handshaking.ToString());
        }
        catch (FisException e)
        {
            ret.Add(e.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ret;
    }
    public struct S_ESOPandAoiKbTestResult
    {
        public ProductInfo PrdInfo;
        public string IsNeedAOI;
        public string AoiAddr;
        public string KbPn;
        public string LabelPn;
        
    }
    [Serializable]
    public struct S_Arr
    {
        public string Name;
        public string Value;
        public string Descr;
    }

    [WebMethod]
    public static ArrayList[] GetAOIParr()
    {
          IConstValue   iConstValue = ServiceAgent.getInstance().GetObjectByName<IConstValue>(WebConstant.CommonObject);
          IList<ConstValueInfo> lstAoiConst = iConstValue.GetConstValueListByType("AOIServerIP", "ID");
          ArrayList arr = new ArrayList();
          foreach (ConstValueInfo cf in lstAoiConst)
          {
              S_Arr s = new S_Arr() { Name = cf.name, Value = cf.value, Descr = cf.description };
              arr.Add(s);
          }
          ArrayList[] arrP = new ArrayList[2];
          arrP[0] = new ArrayList();
          arrP[1] = new ArrayList();

          IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
          IList<DefectInfo> defectList = iDefect.GetDefectList("PRD");
          arrP[0].Add(arr);
          arrP[1].Add(defectList);
      //    arr.Add(lstAoiConst);
        //  arr.Add(defectList);
          return arrP;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmbPdLine.InnerDropDownList.AutoPostBack = true;
            cmbLC.InnerDropDownList.AutoPostBack = true;
          //  GetAOIParr();
			chkQuery.AutoPostBack = true;
			chkQuery.CheckedChanged += new EventHandler(chkQuery_CheckedChanged);

           btnUpdateUI.ServerClick += new EventHandler(btnUpdateUI_ServerClick);
           
           btnExit.ServerClick += new EventHandler(btnExit_ServerClick);
		   btnCheckPart.ServerClick += new EventHandler(btnCheckPart_ServerClick);
           cmbPdLine.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_SelectedIndexChanged);
          //  cmbLC.InnerDropDownList.SelectedIndexChanged += new EventHandler(cmbPdLine_SelectedIndexChanged);

            UserId = Master.userInfo.UserId;
            Customer = Master.userInfo.Customer;
            if (!Page.IsPostBack)
            {
                InitLabel();

                clearUI();
				setColumnWidth();
				
                this.cmbPdLine.Station = Request["Station"];
                this.cmbPdLine.Customer = Master.userInfo.Customer;
                this.cmbPdLine.Stage = "FA";
                this.hidStation.Value = Request["Station"];
                this.upHidden.Update();
				
				string sMax = "javascript:maximizeImage(this);return true;";
				string sMin = "javascript:minimizeImage(this); return true;";
				ShowImage0.Attributes["onMouseOver"] = sMax;
				ShowImage0.Attributes["onMouseOut"] = sMin;
				ShowImage1.Attributes["onMouseOver"] = sMax;
				ShowImage1.Attributes["onMouseOut"] = sMin;
				ShowImage2.Attributes["onMouseOver"] = sMax;
				ShowImage2.Attributes["onMouseOut"] = sMin;
				ShowImage3.Attributes["onMouseOver"] = sMax;
				ShowImage3.Attributes["onMouseOut"] = sMin;
				ShowImage4.Attributes["onMouseOver"] = sMax;
				ShowImage4.Attributes["onMouseOut"] = sMin;
				ShowImage5.Attributes["onMouseOver"] = sMax;
				ShowImage5.Attributes["onMouseOut"] = sMin;
				ShowImage6.Attributes["onMouseOver"] = sMax;
				ShowImage6.Attributes["onMouseOut"] = sMin;
				ShowImage7.Attributes["onMouseOver"] = sMax;
				ShowImage7.Attributes["onMouseOut"] = sMin;
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
    }
    
    private void chkQuery_CheckedChanged(object sender, System.EventArgs e)
    {
        callNextInput();
    }

    private void cmbPdLine_SelectedIndexChanged(object sender, System.EventArgs e)
    {
       this.upHidden.Update();
        showInfo("");
       clearUI();
       callNextInput();
    }

    private void btnExit_ServerClick(object sender, System.EventArgs e)
    {
        clearUI();
        if (!string.IsNullOrEmpty(hidProdId.Value))
        { iESOPandAoiKbTest.Cancel(hidProdId.Value); }
     
    }

    private void ClearJpg(){
		ShowImage0.ImageUrl = "";
		ShowImage1.ImageUrl = "";
		ShowImage2.ImageUrl = "";
		ShowImage3.ImageUrl = "";
		ShowImage4.ImageUrl = "";
		ShowImage5.ImageUrl = "";
		ShowImage6.ImageUrl = "";
		ShowImage7.ImageUrl = "";
	}
	private void SetJpg(int cnt, string file){
		file = JpgPath + file + ".jpg";
		switch(cnt){
			case 0:
				ShowImage0.ImageUrl = file; break;
			case 1:
				ShowImage1.ImageUrl = file; break;
			case 2:
				ShowImage2.ImageUrl = file; break;
			case 3:
				ShowImage3.ImageUrl = file; break;
			case 4:
				ShowImage4.ImageUrl = file; break;
			case 5:
				ShowImage5.ImageUrl = file; break;
			case 6:
				ShowImage6.ImageUrl = file; break;
			case 7:
				ShowImage7.ImageUrl = file; break;
		}
	}
    [System.Web.Services.WebMethod]
 //   public static void Save(string custsn,IList<string> arr)
    public static void Save(string custsn,string defectCode)
    {
        IList<string> lst=new List<string>();
        if(defectCode!="")
        {lst.Add(defectCode);}
        IESOPandAoiKbTest aoiObj = ServiceAgent.getInstance().GetObjectByName<IESOPandAoiKbTest>(WebConstant.IESOPandAoiKbTest);
        aoiObj.Save(custsn, lst);
    }
    [System.Web.Services.WebMethod]
    public static void AOICallBack(string sn, string editor, string station,
                                                string line, string customer, string result, string errorCode, string errorDesc)
    {
        IESOPandAoiKbTest aoiObj = ServiceAgent.getInstance().GetObjectByName<IESOPandAoiKbTest>(WebConstant.IESOPandAoiKbTest);
         aoiObj.AOICallBack(sn, editor, station, line, customer, result, errorCode, errorDesc);
   }

    private void btnCheckPart_ServerClick(object sender, System.EventArgs e)
	{
		try
        {
            if (hidWantData.Value != "0")
			{
				string pn = iESOPandAoiKbTest.InputASTLabel(hidProdId.Value, hidInputPart.Value);
                //updateTable(pn, hidInputPart.Value);
                hidInputPart.Value = "";
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
        finally
        {
            this.upHidden.Update();
            endWaitingCoverDiv();
            //if (hidWantData.Value == "1") disableCombox();
            callNextInput();
        }
		
	}
    
	private void btnUpdateUI_ServerClick(object sender, System.EventArgs e)
    {
		bool isQuery = this.chkQuery.Checked;
        try
        {
            clearUI();
            
            string code = cmbLC.InnerDropDownList.SelectedValue;
            IList<WipBuffer> wbList = iESOPandAoiKbTest.getBomData(hidModel.Value, code); //txtModel.Text
            IList<string> lnList = new List<string>();
            int cntJpg = 0;
            foreach (WipBuffer ele in wbList)
            {
                lnList.Add(ele.LightNo);
                
                string PartNo = ele.PartNo;
                if (cntJpg < 8)
                {
                    SetJpg(cntJpg, PartNo);
                    cntJpg++;
                }
            }
            setLight(lnList);
			transLight();

			string[] bomPartNoItems = hidBomPartNoItems.Value.Split(new string[] { "●" }, StringSplitOptions.RemoveEmptyEntries);
			int cntBomParts = 0;
            foreach (string bi in bomPartNoItems)
            {
                if (cntJpg < 8)
                {
                    SetJpg(cntJpg, bi);
                    cntJpg++;
                }
				cntBomParts++;
            }
			
			hidWantData.Value = cntBomParts.ToString();
			
            JpgUp.Update();
            
            if (string.Compare(isTestAOI, "TRUE", true) == 0)
            { hidIsAOILine.Value = "Y"; }

            if (hidIsAOILine.Value == "Y")
            {
        //        callAOI();
             }
            else
            {
                hidAoiAddr.Value = "";
                hidKbPn.Value = "";
                hidLabelPn.Value = "";
            }
            upHidden.Update();
            if (this.chkQuery.Checked)
            {
				UpdateJsVar("custsn", "");
                showInfo("Query finished!", "green");
            }
            else if (cntBomParts == 0 && hidIsAOILine.Value == "Y")
			{
                UpdateJsVarAndSave("custsn", this.txtCPQS.Text);
			}
            else
			{
				//UpdateJsVar("custsn", this.txtCPQS.Text);
                showInfo("Please scan Part to Check or 9999!", "green");
			}
            // //0:ProductInfo 1:need aoi 2:addr 3:kb pn 4:label pn;
			
			if (isQuery)
				this.chkQuery.Enabled = true;
			else
				this.chkQuery.Enabled = false;
            upChkQuery.Update();
        }
        catch (FisException ee)
        {
            clearUI();
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
        finally
        {
            endWaitingCoverDiv();
			
            callNextInput();
        }
        //finally
        //{
        //    this.upHidden.Update();
        //    endWaitingCoverDiv();
        //    if (this.chkQuery.Checked != true)
        //    {
        //        upChkQuery.Update();
        //    }
        //    if (chcekEPIA == "EPIA")
        //    {
        //        showInfo("EPIA 機器");
        //    }
        //    callNextInput();
        //}
    }

    private void InitLabel()
    {
        this.lblPdLine.Text = this.GetLocalResourceObject(Pre + "_lblPdLine").ToString();
        this.lblProId.Text = this.GetLocalResourceObject(Pre + "_lblProId").ToString();
        this.lblLC.Text = this.GetLocalResourceObject(Pre + "_lblLC").ToString();
        this.lblCPQS.Text = this.GetLocalResourceObject(Pre + "_lblCPQS").ToString();
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        //this.lblTableTitle.Text = this.GetLocalResourceObject(Pre + "_lblTableTitle").ToString();
        this.lblDataEntry.Text = this.GetLocalResourceObject(Pre + "_lblDataEntry").ToString();
        this.chkQuery.Text = this.GetLocalResourceObject(Pre + "_chkQuery").ToString();

        setPdLineCombFocus();
    }

    private void clearUI()
    {
        try
        {
            bindTable(DEFAULT_ROWS);

            ClearJpg();
			JpgUp.Update();

            this.chkQuery.Enabled = true;
        }
        catch (FisException ee)
        {
            writeToAlertMessage(ee.mErrmsg);
        }
        catch (Exception ex)
        {
            writeToAlertMessage(ex.Message);
        }
    }

    void setLight(IList<string> lnList)
    {
        IList<int> lightList = new List<int>();
        for (int i = 0; i < 192; i++)
        {
            lightList.Add(0);
        }

        for (int j = 0; j < lnList.Count; j++)
        {
            int temp;
            if (int.TryParse(lnList[j], out temp))
            {
                lightList[temp-1] = 1;
            }
        }

        string result = "";
        for (int k = 0; k < 24; k++)
        {
            int tempLight8 = 0;
            for (int m = 0; m < 8; m++)
            {
                tempLight8 = tempLight8 + (int)(lightList[k * 8 + m] * Math.Pow(2d, (double)m));
            }
            result = result + "," + tempLight8;
        }
        result = result.TrimStart(new char[] { ',' });

        hidData2Send.Value = result;
        return;
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n").Replace("\"", "\\\"") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanelAll, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
    }
    private void Save()
    {
        String script = "<script language='javascript'>Save(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "Save", script, false);
    }

    private void callNextInput()
    {
        String script = "<script language='javascript'>CallNextInput(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callNextInput", script, false);
    }
    private void callAOI()
    {
        String script = "<script language='javascript'>CallAOI(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callAOI", script, false);
    }
    private void showInfo(string info)
    {
        String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + "\"); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }
    private void showInfo(string info,string color)
    {
       // String script = "<script language='javascript'> ShowInfo(\"" + info.Replace("\"", "\\\"") + ",'"+color+  "\"); </script>";
        String script = "<script language='javascript'> ShowInfo('{0}','{1}') </script>";
        script = string.Format(script,info, color);
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "clearInfo", script, false);
    }
    private void setPdLineCombFocus()
    {
        String script = "<script language='javascript'>  setPdLineCmbFocus(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "setPdLineCmbFocus", script, false);
    }

    private void transLight()
    {
        String script = "<script language='javascript'>transLight();</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "transLight", script, false);
    }
    //  ShowInfo("print success!", "green");
    private void callNextInputAndSuccess()
    {

        String script = "<script language='javascript'>callNextInput();ShowInfo('Success!', 'green'); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "callNextInput", script, false);
    }
    private void endWaitingCoverDiv()
    {
        String script = "<script language='javascript'>" + "\r\n" +
            "endWaitingCoverDiv();" + "\r\n" +
            "</script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "endWaitingCoverDiv", script, false);
    }

    [System.Web.Services.WebMethod]
    public static ArrayList GetDefectList()
    {
        IDefect iDefect = ServiceAgent.getInstance().GetObjectByName<IDefect>(WebConstant.CommonObject);
        IList<DefectInfo> defectList = iDefect.GetDefectList("PRD");
        ArrayList arr = new ArrayList();
        arr.Add(defectList);
        return arr;
    }

	private void UpdateJsVar(string varName, string varValue)
    {
        String script = "<script language='javascript'> " + varName + "='" + varValue + "'; </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "updateJsVar", script, false);
    }
    private void UpdateJsVarAndSave(string varName, string varValue)
    {
		//  " + varName + "='" + varValue + "';
        String script = "<script language='javascript'> Save(); </script>";
        ScriptManager.RegisterStartupScript(this.updatePanelAll, ClientScript.GetType(), "updateJsVar", script, false);
    }
	
	private void bindTable(int defaultRow)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartNo").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPartType").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDescription").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colPQty").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCollection").ToString());

        for (int i = 0; i < defaultRow; i++)
        {
            dr = dt.NewRow();

            dt.Rows.Add(dr);
        }

        gd.DataSource = dt;
        gd.DataBind();
    }

    private void setColumnWidth()
    {
        gd.HeaderRow.Cells[0].Width = Unit.Percentage(38);
        gd.HeaderRow.Cells[1].Width = Unit.Percentage(10);
        gd.HeaderRow.Cells[2].Width = Unit.Percentage(26);
        gd.HeaderRow.Cells[3].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[4].Width = Unit.Percentage(5);
        gd.HeaderRow.Cells[5].Width = Unit.Percentage(16);
    }

}
