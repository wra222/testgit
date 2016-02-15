
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
using IMES.Infrastructure;
using log4net;
using System.Text;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
//using IMES.FisObject;
//using IMES.FisObject.PAK.Carton;
using System.IO;
public partial class FA_LotCollectionforStorage : System.Web.UI.Page
{  
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    ICollectionMaterialLot iCollectionMaterialLot = ServiceAgent.getInstance().GetMaintainObjectByName<ICollectionMaterialLot>(WebConstant.CollectionMaterialLotObject);
    public String userId;
    public String customer;
    public String station;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmbCollectCPU.InnerDropDownList.ID = "drpCPU";
        customer = Master.userInfo.Customer;
        userId = Master.userInfo.UserId;
        hidMaterialID.Value = cmbCollectCPU.InnerDropDownList.ClientID;
        hidRecevingID.Value = cmbCollectStage.InnerDropDownList.ClientID;
        if (!this.IsPostBack)
        {
            cmbCollectStage.InnerDropDownList.Attributes.Add("onchange", "ShowStage(this);");
            InitLabel();
       }
    }
    private void InitLabel()
    {
       this.lbDataEntry.Text = this.GetLocalResourceObject(Pre + "_lbDataEntry").ToString();
    }
      

    [System.Web.Services.WebMethod]
    public static  void Save(string boxId,string specNo,string lotNo,int qty,string material,string receving,string user)
    {
        try
        {
            ICollectionMaterialLot iCollectionMaterialLot = ServiceAgent.getInstance().GetMaintainObjectByName<ICollectionMaterialLot>(WebConstant.CollectionMaterialLotObject);
            iCollectionMaterialLot.AddMaterialBox(boxId, specNo, lotNo, qty, material, receving, "Ready", user);
        }
        catch (Exception ex)
        {
            throw ex;
        }
      
    }
    [System.Web.Services.WebMethod]
    public static void CheckBoxId(string boxId)
    {
        try
        {
            ICollectionMaterialLot iCollectionMaterialLot = ServiceAgent.getInstance().GetMaintainObjectByName<ICollectionMaterialLot>(WebConstant.CollectionMaterialLotObject);
            iCollectionMaterialLot.CheckBoxId(boxId);
        }
        catch (FisException ex)
        {
            throw new Exception(ex.mErrmsg);
        }
        catch (Exception ex)
        {
            throw ex;
        }
 

    }
}
