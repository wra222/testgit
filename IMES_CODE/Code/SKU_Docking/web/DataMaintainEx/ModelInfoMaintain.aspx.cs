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
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using System.Text;

//qa bug no:ITC-1136-0039,ITC-1136-0040
//qa bug no:ITC-1136-0024,ITC-1136-0026,ITC-1136-0006,ITC-1136-0010,ITC-1136-0113

public partial class ModelInfoMaintain: IMESBasePage
{
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    private const int DEFAULT_ROWS = 18;
    IModelManager iModelManager = ServiceAgent.getInstance().GetMaintainObjectByName<IModelManager>(WebConstant.IModelManager);
    IModelManagerEx iModelManagerEx = ServiceAgent.getInstance().GetMaintainObjectByName<IModelManagerEx>(WebConstant.IModelManagerEx);
    private string editor;

    private string strModelName;

    private ArrayList changedInfoAry = null;

    public static Hashtable indexField = new Hashtable();

    static ModelInfoMaintain()
    {
        indexField.Add("Id", 0);
        indexField.Add("Name", 1);
        indexField.Add("Description", 2);
        indexField.Add("Value", 3);
        indexField.Add("Editor", 4);
        indexField.Add("Cdt", 5);
        indexField.Add("Udt", 6);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            editor = Request.QueryString["editor"];

            strModelName = Request.QueryString["modelname"];
            this.valModel.Text = strModelName;

            IList<string> models = new List<string>(strModelName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            this.Title = this.GetLocalResourceObject(Pre + "_title").ToString() + " ( Total Models Count = " + models.Count + " )";

            InitLabel();
            bindTable();
           // setColumnWidth();
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }
    }

    private void setColumnWidth()
    {
        //gdModelInfoNameList.HeaderRow.Cells[0].Width = Unit.Pixel(100);
        gdModelInfoList.HeaderRow.Cells[Convert.ToInt16(indexField["Name"])].Width = Unit.Pixel(120);

        gdModelInfoList.HeaderRow.Cells[Convert.ToInt16(indexField["Description"])].Width = Unit.Pixel(240);
        gdModelInfoList.HeaderRow.Cells[Convert.ToInt16(indexField["Value"])].Width = Unit.Pixel(360);

    }

    private DataTable MkTableByOneModel(IList<ModelInfoNameAndModelInfoValueMaintainInfo> modelinfolist)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("id");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colName").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDesc").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colValue").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

        if (modelinfolist != null && modelinfolist.Count != 0)
        {
            foreach (ModelInfoNameAndModelInfoValueMaintainInfo temp in modelinfolist)
            {
                dr = dt.NewRow();

                dr[0] = temp.Id;
                dr[1] = temp.Name;
                dr[2] = temp.Description;
                dr[3] = temp.Value;
                dr[4] = temp.Editor;
                if (temp.Cdt == DateTime.MinValue)
                {
                    dr[5] = "";
                }
                else
                {
                    dr[5] = temp.Cdt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (temp.Udt == DateTime.MinValue)
                {
                    dr[6] = "";
                }
                else
                {
                    dr[6] = temp.Udt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                
                dt.Rows.Add(dr);
            }

            for (int i = modelinfolist.Count; i < DEFAULT_ROWS; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        else
        {
            for (int i = 0; i < DEFAULT_ROWS; i++)
            {
                dr = dt.NewRow();

                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    private DataTable MkTableByModels(IList<ModelInfoNameAndModelInfoValueMaintainInfo> modelinfolist, int modelCnt, ref ArrayList changedInfoAry)
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add("id");
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colName").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colDesc").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colValue").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colEditor").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colCdt").ToString());
        dt.Columns.Add(this.GetLocalResourceObject(Pre + "_colUdt").ToString());

        Hashtable usedInfo = new Hashtable();
        ArrayList usedInfoAry = new ArrayList();

        if (modelinfolist != null && modelinfolist.Count != 0)
        {
            foreach (ModelInfoNameAndModelInfoValueMaintainInfo temp in modelinfolist)
            {
                if (usedInfo.ContainsKey(temp.Name))
                {
                    Hashtable thisInfo = (Hashtable) usedInfo[temp.Name];

                    if (thisInfo.ContainsKey(temp.Value))
                    {
                        int n = (int)thisInfo[temp.Value];
                        thisInfo[temp.Value] = ++n;
                    }
                    else
                    {
                        thisInfo.Add(temp.Value, 1);
                    }
                }
                else
                {
                    usedInfoAry.Add(temp.Name);

                    Hashtable thisInfo = new Hashtable();
                    usedInfo.Add(temp.Name, thisInfo);

                    thisInfo.Add(temp.Value, 1);
                }
            }
        }

        for (int k=0; k<usedInfoAry.Count; k++) {
            string nowName = (string) usedInfoAry[k];
            Hashtable thisInfo = (Hashtable) usedInfo[nowName];
            string nowValue = "";
            int nowCount = 0;
            bool changed = false;
            foreach (string key in thisInfo.Keys) {
                if ((nowCount <= (int)thisInfo[key]) && (!nowValue.Equals(key)))
                {
                    if (nowCount != 0)
                        changed = true;
                    nowValue = key;
                    nowCount = (int)thisInfo[key];
                }
            }
            if (!changed)
            {
                if (thisInfo.Keys.Count == 1)
                {
                    foreach (string value in thisInfo.Keys)
                    {
                        int count = (int)thisInfo[value];
                        if ((!"".Equals(value)) && (modelCnt != (int)count))
                            changed = true; // 有的model沒對此modelInfo設過value,會是空的
                    }
                }
            }

            changedInfoAry.Add(changed);

            dr = dt.NewRow();

            dr[0] = ""; //Id
            dr[1] = nowName;
            dr[2] = ""; //Description;
            dr[3] = nowValue;
            dr[4] = ""; //Editor;
            dr[5] = ""; //Cdt
            dr[6] = ""; //Udt
            dt.Rows.Add(dr);
        }

        for (int i = usedInfoAry.Count; i < DEFAULT_ROWS; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        return dt;
    }

    protected void bindTable()
    {
        try
        {
            IList<string> models = new List<string>(strModelName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));

            IList<ModelInfoNameAndModelInfoValueMaintainInfo> modelinfolist = null;
            DataTable dt = null;

            if (models.Count == 1){
                modelinfolist = iModelManager.GetModelInfoList(models[0]);
                dt = MkTableByOneModel(modelinfolist);
            }
            else{
                modelinfolist = iModelManagerEx.GetModelInfoNameAndModelInfoValueListByModels(models);
                changedInfoAry = new ArrayList();
                dt = MkTableByModels(modelinfolist, models.Count, ref changedInfoAry);
            }

            gdModelInfoList.DataSource = dt;
            gdModelInfoList.DataBind();
            setColumnWidth();

            if (models.Count > 1)
            {
                for (int i = 0; i < changedInfoAry.Count; i++)
                {
                    if ((bool)changedInfoAry[i])
                    {
                        gdModelInfoList.Rows[i].Cells[Convert.ToInt16(indexField["Value"])].ForeColor = System.Drawing.Color.Red; //.BackColor = System.Drawing.Color.Yellow;
                        gdModelInfoList.Rows[i].Cells[Convert.ToInt16(indexField["Value"])].Font.Bold = true;
                    }
                }
            }
        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;
        }
        catch (Exception ex)
        {
            //show error
            showErrorMessage(ex.Message);
            return;
        }


    }

    protected void btnRefreshModelList_Click(Object sender, EventArgs e)
    {
        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "test_clearInputs", "test_clearInputs();", true);
    }

    /// <summary>
    /// 初始化页面的静态label
    /// </summary>
    private void InitLabel()
    {
        this.lblModel.Text = this.GetLocalResourceObject(Pre + "_lblModel").ToString();
        this.lblInfo.Text = this.GetLocalResourceObject(Pre + "_lblInfo").ToString();
        //this.lblName.Text = this.GetLocalResourceObject(Pre + "_lblName").ToString();
        //this.lblValue.Text = this.GetLocalResourceObject(Pre + "_lblValue").ToString();
        //this.lblDesc.Text = this.GetLocalResourceObject(Pre + "_lblDesc").ToString();
        //this.btnDelete.Value = this.GetLocalResourceObject(Pre + "_btnDelete").ToString();
        this.btnSave.Value = this.GetLocalResourceObject(Pre + "_btnSave").ToString();
        //this.btnAdd.Value = this.GetLocalResourceObject(Pre + "_btnAdd").ToString();

        //setFocus();


    }
    protected void gd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[Convert.ToInt16(indexField["Id"])].Style.Add("display", "none");//id

        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (!String.IsNullOrEmpty(e.Row.Cells[i].Text.Trim()) && !(e.Row.Cells[i].Text.Trim().ToLower() == "&nbsp;"))
                {
                    e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                }
            }

        }
    }


    private void UpdateModelsInfo()
    {
    }

    protected void btnSave_Click(Object sender, EventArgs e)
    {
        string name = hidItem.Value;
        string value = txtValue.Text;
        string modelInfoId = hidModelInfoId.Value;

        try
        {
            IList<string> models = new List<string>(strModelName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            if (models.Count == 1)
            {
                ModelInfoMaintainInfo model = new ModelInfoMaintainInfo();

                model.Name = name;
                model.Value = value;
                model.Model = strModelName;
                model.Editor = editor;

                if (modelInfoId.Length != 0)
                {
                    model.Id = Int32.Parse(modelInfoId);
                }

                if (Int32.Parse(modelInfoId) == 0)//no id
                {
                    if (value.Length != 0)
                    {
                        modelInfoId = iModelManager.AddModelInfo(model).ToString();
                    }
                }
                else
                {
                    if (value.Length == 0)
                    {
                        iModelManager.DeleteModelInfo(model);
                        modelInfoId = "-1";
                    }
                    else
                    {
                        iModelManager.SaveModelInfo(model);
                    }
                }
            }
            else // models
            {
                if (value.Length == 0)
                {
                    iModelManagerEx.DeleteModelsInfo(name, models);
                }
                else
                {
                    IList<string> notExistedModels = new List<string>();
                    IList<string> existedModels = iModelManagerEx.GetExistedModelsFromModelInfoByModels(name, models, ref notExistedModels);

                    if (existedModels.Count > 0)
                    {
                        iModelManagerEx.UpdateModelsInfo(name, value, models);
                    }

                    for (int i=0; i<notExistedModels.Count; i++)
                    {
                        ModelInfoMaintainInfo model = new ModelInfoMaintainInfo();

                        model.Name = name;
                        model.Value = value;
                        model.Model = notExistedModels[i];
                        model.Editor = editor;

                        iModelManager.AddModelInfo(model);
                    }
                }
            }

        }
        catch (FisException ex)
        {
            showErrorMessage(ex.mErrmsg);
            return;

        }
        catch (Exception ex)
        {
            showErrorMessage(ex.Message);
            return;
        }

        bindTable();
        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "SaveComplete", "SaveComplete(\"" + modelInfoId + "\");", true);
    }



    private void showErrorMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        string oldErrorMsg = errorMsg;
        errorMsg = errorMsg.Replace("\r\n", "<br>");
        errorMsg = errorMsg.Replace("\"", "\\\"");

        oldErrorMsg = oldErrorMsg.Replace("\r\n", "\\n");
        oldErrorMsg = oldErrorMsg.Replace("\"", "\\\"");

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg + "\");");
        //scriptBuilder.AppendLine("ShowInfo(\"" + oldErrorMsg + "\");"); 
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
    }
    /*
    private void clearInputs()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("clearInputs();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.updatePanel3, typeof(System.Object), "clearInputs", scriptBuilder.ToString(), false);
    }*/

}
