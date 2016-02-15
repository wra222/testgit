using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.inventec.iMESWEB;
using IMES.Infrastructure;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using com.inventec.imes.DBUtility;
//using IMES.Station.Interface.CommonIntf;
using System.Data;

public partial class Query_FA_ProductInfo : System.Web.UI.Page
{    
    private const int ProductStatus_DEFAULT_ROWS = 1;
    private const int ProductNextStation_DEFAULT_ROWS = 1;
    private const int ProductHistory_DEFAULT_ROWS = 1;
    private const int HoldHistory_DEFAULT_ROWS = 1;
    private const int ProductRepair_DEFAULT_ROWS = 1;
    private const int ProductInfo_DEFAULT_ROWS = 1;
    private const int ProductPart_DEFAULT_ROWS = 1;
    private const int ProductUnpack_DEFAULT_ROWS = 1;
    private const int ProductPartUnpack_DEFAULT_ROWS = 1;
    private const int PizzaPartUnpack_DEFAULT_ROWS = 1;
    private const int ProductChange_DEFAULT_ROWS = 1;
    private const int ProductITCND_DEFAULT_ROWS = 1;
    private const int ProductCRPart_DEFAULT_ROWS = 1;
    private const int ProductCRLog_DEFAULT_ROWS = 1;
    public string Pre = WebCommonMethod.getConfiguration(WebConstant.LANGUAGE);
    IFA_ProductInfo ProductInfo = ServiceAgent.getInstance().GetObjectByName<IFA_ProductInfo>(WebConstant.ProductInfo);
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitPage();
            this.gvFAStatus.DataSource = getNullDataTable();
            this.gvFAStatus.DataBind();
            InitGridView();

            this.gvFANextStation.DataSource = getNullDataTable_NextStation(ProductNextStation_DEFAULT_ROWS);
            this.gvFANextStation.DataBind();
            InitNextStationGridView();

            this.gvFAHistory.DataSource = getNullDataTable_History(ProductHistory_DEFAULT_ROWS);
            this.gvFAHistory.DataBind();
            InitHistoryGridView();

            this.gvHoldHistory.DataSource = getNullDataTable_HoldHistory(HoldHistory_DEFAULT_ROWS);
            this.gvHoldHistory.DataBind();
            InitHistoryGridView();

            this.gvFARepair.DataSource = getNullDataTable_Repair(ProductRepair_DEFAULT_ROWS);
            this.gvFARepair.DataBind();
            InitRepairGridView();

            this.gvFAInfo.DataSource = getNullDataTable_Info(ProductInfo_DEFAULT_ROWS);
            this.gvFAInfo.DataBind();
            InitInfoGridView();

            this.gvFAPart.DataSource = getNullDataTable_Part(ProductPart_DEFAULT_ROWS);
            this.gvFAPart.DataBind();
            InitPartGridView();

            this.gvFAUnpack.DataSource = getNullDataTable_UnPack(ProductUnpack_DEFAULT_ROWS);
            this.gvFAUnpack.DataBind();
            InitUnpackGridView();

            this.gvProductPartUnpack.DataSource = getNullDataTable_ProductPartUnPack(ProductPartUnpack_DEFAULT_ROWS);
            this.gvProductPartUnpack.DataBind();
            InitProductPartUnpackGridView();

            this.gvPizzaPartUnpack.DataSource = getNullDataTable_PizzaPartUnPack(PizzaPartUnpack_DEFAULT_ROWS);
            this.gvPizzaPartUnpack.DataBind();
            InitPizzaPartUnpackGridView();

            this.gvFAChange.DataSource = getNullDataTable_Change(ProductChange_DEFAULT_ROWS);
            this.gvFAChange.DataBind();
            InitChangeGridView();

            this.gvFAITCND.DataSource = getNullDataTable_ITCND(ProductITCND_DEFAULT_ROWS);
            this.gvFAITCND.DataBind();
            InitITCNDGridView();


            setFocus();
        }

    }
    private void InitPage()
    {
        this.lblDB.Text = this.GetLocalResourceObject(Pre + "_lblDB").ToString();
        this.lblProductID.Text = this.GetLocalResourceObject(Pre + "_lblProductID").ToString();        
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        queryClick();
    }

    public void queryClick()
    {
        try
        {
            string Connection = CmbDBType.ddlGetConnection();
            
            DataSet ds = new DataSet();            
            ds = getDataTable(hidProduct.Value.Trim());
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.gvFAStatus.DataSource = ds.Tables[0];
                this.gvFAStatus.DataBind();
                InitGridView();
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                this.gvFANextStation.DataSource = ds.Tables[1];
                this.gvFANextStation.DataBind();

                this.gvFANextStation.DataSource = null;
                this.gvFANextStation.DataSource = getNullDataTable_NextStation(ds.Tables[1].Rows.Count);
                this.gvFANextStation.DataBind();

                for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                {
                    gvFANextStation.Rows[i].Cells[0].Text = ds.Tables[1].Rows[i]["Station"].ToString();
                }

                InitNextStationGridView();
            }
            else
            {
                this.gvFANextStation.DataSource = getNullDataTable_NextStation(ProductNextStation_DEFAULT_ROWS);
                this.gvFANextStation.DataBind();
            }

            DataTable dt = new DataTable();                            //product history.............................
            //dt = iFAQuery.GetProductHistory(hidProduct.Value.Trim());            
            dt = ProductInfo.GetProductHistory(Connection, gvFAStatus.Rows[0].Cells[0].Text.Trim(), gvFAStatus.Rows[0].Cells[11].Text.Trim());
            if (dt.Rows.Count > 0)
            {
                this.gvFAHistory.DataSource = null;
                this.gvFAHistory.DataSource = getNullDataTable_History(dt.Rows.Count);
                this.gvFAHistory.DataBind();

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    gvFAHistory.Rows[i].Cells[0].Text = dt.Rows[i]["Station"].ToString() + " - " + dt.Rows[i]["StationName"].ToString();
                    gvFAHistory.Rows[i].Cells[1].Text = dt.Rows[i]["Line"].ToString();
                    gvFAHistory.Rows[i].Cells[2].Text = dt.Rows[i]["FixtureID"].ToString();
                    gvFAHistory.Rows[i].Cells[3].Text = dt.Rows[i]["Status"].ToString();
                    gvFAHistory.Rows[i].Cells[4].Text = dt.Rows[i]["ErrorCode"].ToString();
                    gvFAHistory.Rows[i].Cells[5].Text = dt.Rows[i]["Editor"].ToString();
                    gvFAHistory.Rows[i].Cells[6].Text = dt.Rows[i]["Cdt"].ToString();

                }
                InitHistoryGridView();
            }
            else
            {
                this.gvFAHistory.DataSource = getNullDataTable_History(ProductNextStation_DEFAULT_ROWS);
                this.gvFAHistory.DataBind();
            }

            DataTable dtRepairInfo = new DataTable();                  //product repair.............................................
            dtRepairInfo = ProductInfo.GetProductRepair(Connection, gvFAStatus.Rows[0].Cells[0].Text.Trim());
            if (dtRepairInfo.Rows.Count > 0)
            {
                this.gvFARepair.DataSource = null;
                this.gvFARepair.DataSource = getNullDataTable_Repair(dtRepairInfo.Rows.Count);
                this.gvFARepair.DataBind();

                for (int i = 0; i <= dtRepairInfo.Rows.Count - 1; i++)
                {
                    gvFARepair.Rows[i].Cells[0].Text = dtRepairInfo.Rows[i]["Line"].ToString();
                    gvFARepair.Rows[i].Cells[1].Text = dtRepairInfo.Rows[i]["StationName"].ToString();
                    gvFARepair.Rows[i].Cells[2].Text = dtRepairInfo.Rows[i]["Status"].ToString() == "1" ? "OK" : "NG";
                    gvFARepair.Rows[i].Cells[3].Text = dtRepairInfo.Rows[i]["Location"].ToString();
                    gvFARepair.Rows[i].Cells[4].Text = dtRepairInfo.Rows[i]["Descr"].ToString();
                    gvFARepair.Rows[i].Cells[5].Text = dtRepairInfo.Rows[i]["Cause"].ToString();
                    gvFARepair.Rows[i].Cells[6].Text = dtRepairInfo.Rows[i]["Obligation"].ToString();
                    gvFARepair.Rows[i].Cells[7].Text = dtRepairInfo.Rows[i]["Remark"].ToString();
                    gvFARepair.Rows[i].Cells[8].Text = dtRepairInfo.Rows[i]["Editor"].ToString();
                    gvFARepair.Rows[i].Cells[9].Text = dtRepairInfo.Rows[i]["Cdt"].ToString();
                }
                InitRepairGridView();
            }
            else
            {
                this.gvFARepair.DataSource = getNullDataTable_Repair(1);
                this.gvFARepair.DataBind();
            }


            DataTable dtInfo = new DataTable();            //product info.................................................
            dtInfo = ProductInfo.GetProductInfo(Connection, gvFAStatus.Rows[0].Cells[0].Text.Trim());
            if (dtInfo.Rows.Count > 0)
            {
                this.gvFAInfo.DataSource = null;
                this.gvFAInfo.DataSource = getNullDataTable_Info(dtInfo.Rows.Count);
                this.gvFAInfo.DataBind();

                for (int i = 0; i <= dtInfo.Rows.Count - 1; i++)
                {
                    gvFAInfo.Rows[i].Cells[0].Text = dtInfo.Rows[i]["InfoType"].ToString();
                    gvFAInfo.Rows[i].Cells[1].Text = dtInfo.Rows[i]["InfoValue"].ToString();
                    gvFAInfo.Rows[i].Cells[2].Text = dtInfo.Rows[i]["Editor"].ToString();
                    gvFAInfo.Rows[i].Cells[3].Text = dtInfo.Rows[i]["Cdt"].ToString();
                    gvFAInfo.Rows[i].Cells[4].Text = dtInfo.Rows[i]["Udt"].ToString();
                }
                InitInfoGridView();
            }
            else
            {
                this.gvFAInfo.DataSource = getNullDataTable_Info(1);
                this.gvFAInfo.DataBind();
            }
            DataTable holdhistroy = new DataTable();            //Hold Histroy.................................................
            holdhistroy = ProductInfo.GetProductHoldHistory(Connection, gvFAStatus.Rows[0].Cells[0].Text.Trim());
            if (holdhistroy.Rows.Count > 0)
            {
                this.gvHoldHistory.DataSource = null;
                this.gvHoldHistory.DataSource = getNullDataTable_HoldHistory(holdhistroy.Rows.Count);
                this.gvHoldHistory.DataBind();

                for (int i = 0; i <= holdhistroy.Rows.Count - 1; i++)
                {
                    gvHoldHistory.Rows[i].Cells[0].Text = holdhistroy.Rows[i]["Line"].ToString();
                    gvHoldHistory.Rows[i].Cells[1].Text = holdhistroy.Rows[i]["Family"].ToString();
                    gvHoldHistory.Rows[i].Cells[2].Text = holdhistroy.Rows[i]["Model"].ToString();
                    gvHoldHistory.Rows[i].Cells[3].Text = holdhistroy.Rows[i]["CUSTSN"].ToString();
                    gvHoldHistory.Rows[i].Cells[4].Text = holdhistroy.Rows[i]["CheckInStation"].ToString();
                    gvHoldHistory.Rows[i].Cells[5].Text = holdhistroy.Rows[i]["HoldDescr"].ToString();
                    gvHoldHistory.Rows[i].Cells[6].Text = holdhistroy.Rows[i]["Editor"].ToString();
                }
                InitInfoGridView();
            }
            else
            {
                this.gvHoldHistory.DataSource = getNullDataTable_HoldHistory(1);
                this.gvHoldHistory.DataBind();
            }

            DataTable dtPart = new DataTable();                                   //product part.......................................
            dtPart = ProductInfo.GetProductPart(Connection, gvFAStatus.Rows[0].Cells[0].Text.Trim());
            if (dtPart.Rows.Count > 0)
            {
                this.gvFAPart.DataSource = null;
                this.gvFAPart.DataSource = getNullDataTable_Part(dtPart.Rows.Count);
                this.gvFAPart.DataBind();

                for (int i = 0; i <= dtPart.Rows.Count - 1; i++)
                {
                    gvFAPart.Rows[i].Cells[0].Text = dtPart.Rows[i]["PartNo"].ToString();
                    gvFAPart.Rows[i].Cells[1].Text = dtPart.Rows[i]["PartType"].ToString();
                    gvFAPart.Rows[i].Cells[2].Text = dtPart.Rows[i]["Descr"].ToString();
                    gvFAPart.Rows[i].Cells[3].Text = dtPart.Rows[i]["PartSn"].ToString();
                    gvFAPart.Rows[i].Cells[4].Text = dtPart.Rows[i]["BomNodeType"].ToString();
                    gvFAPart.Rows[i].Cells[5].Text = dtPart.Rows[i]["CheckItemType"].ToString();
                    gvFAPart.Rows[i].Cells[6].Text = dtPart.Rows[i]["Station"].ToString();
                    gvFAPart.Rows[i].Cells[7].Text = dtPart.Rows[i]["Editor"].ToString();
                    gvFAPart.Rows[i].Cells[8].Text = dtPart.Rows[i]["Cdt"].ToString();
                }
                InitPartGridView();
            }
            else
            {
                this.gvFAPart.DataSource = getNullDataTable_Part(1);
                this.gvFAPart.DataBind();
            }

            DataTable dtUnpack = new DataTable();               //product Unpack
            dtUnpack = ProductInfo.GetProductUnpack(Connection, hidProduct.Value.Trim());
            if (dtUnpack.Rows.Count > 0)
            {
                this.gvFAUnpack.DataSource = null;
                this.gvFAUnpack.DataSource = getNullDataTable_UnPack(dtUnpack.Rows.Count);
                this.gvFAUnpack.DataBind();

                for (int i = 0; i <= dtUnpack.Rows.Count - 1; i++)
                {
                    gvFAUnpack.Rows[i].Cells[0].Text = dtUnpack.Rows[i]["ProductID"].ToString();
                    gvFAUnpack.Rows[i].Cells[1].Text = dtUnpack.Rows[i]["Model"].ToString();
                    gvFAUnpack.Rows[i].Cells[2].Text = dtUnpack.Rows[i]["PCBID"].ToString();
                    gvFAUnpack.Rows[i].Cells[3].Text = dtUnpack.Rows[i]["PCBModel"].ToString();
                    gvFAUnpack.Rows[i].Cells[4].Text = dtUnpack.Rows[i]["MAC"].ToString();
                    gvFAUnpack.Rows[i].Cells[5].Text = dtUnpack.Rows[i]["CUSTSN"].ToString();
                    gvFAUnpack.Rows[i].Cells[6].Text = dtUnpack.Rows[i]["UEditor"].ToString();
                    gvFAUnpack.Rows[i].Cells[7].Text = dtUnpack.Rows[i]["UPdt"].ToString();
                }
                InitUnpackGridView();
            }
            else
            {
                this.gvFAUnpack.DataSource = getNullDataTable_UnPack(1);
                this.gvFAUnpack.DataBind();
            }

            DataTable dtProduct_PartUnpack = new DataTable();               //Product_Part Unpack
            dtProduct_PartUnpack = ProductInfo.GetUnpackProductPart(Connection, hidProduct.Value.Trim());
            if (dtProduct_PartUnpack.Rows.Count > 0)
            {
                this.gvProductPartUnpack.DataSource = null;
                this.gvProductPartUnpack.DataSource = getNullDataTable_ProductPartUnPack(dtProduct_PartUnpack.Rows.Count);
                this.gvProductPartUnpack.DataBind();

                for (int i = 0; i <= dtProduct_PartUnpack.Rows.Count - 1; i++)
                {
                    gvProductPartUnpack.Rows[i].Cells[0].Text = dtProduct_PartUnpack.Rows[i]["ProductID"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[1].Text = dtProduct_PartUnpack.Rows[i]["PartNo"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[2].Text = dtProduct_PartUnpack.Rows[i]["PartType"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[3].Text = dtProduct_PartUnpack.Rows[i]["IECPn"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[4].Text = dtProduct_PartUnpack.Rows[i]["CustmerPn"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[5].Text = dtProduct_PartUnpack.Rows[i]["PartSn"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[6].Text = dtProduct_PartUnpack.Rows[i]["Station"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[7].Text = dtProduct_PartUnpack.Rows[i]["BomNodeType"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[8].Text = dtProduct_PartUnpack.Rows[i]["CheckItemType"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[9].Text = dtProduct_PartUnpack.Rows[i]["UEditor"].ToString();
                    gvProductPartUnpack.Rows[i].Cells[10].Text = dtProduct_PartUnpack.Rows[i]["UPdt"].ToString();
                }
                InitProductPartUnpackGridView();
            }
            else
            {
                this.gvProductPartUnpack.DataSource = getNullDataTable_ProductPartUnPack(1);
                this.gvProductPartUnpack.DataBind();
            }

            DataTable dtPizza_PartUnpack = new DataTable();               //Pizza_Part Unpack
            dtPizza_PartUnpack = ProductInfo.GetUnpackPizzaPart(Connection, hidProduct.Value.Trim());
            if (dtPizza_PartUnpack.Rows.Count > 0)
            {
                this.gvPizzaPartUnpack.DataSource = null;
                this.gvPizzaPartUnpack.DataSource = getNullDataTable_PizzaPartUnPack(dtPizza_PartUnpack.Rows.Count);
                this.gvPizzaPartUnpack.DataBind();

                for (int i = 0; i <= dtPizza_PartUnpack.Rows.Count - 1; i++)
                {
                    gvPizzaPartUnpack.Rows[i].Cells[0].Text = dtPizza_PartUnpack.Rows[i]["ProductID"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[1].Text = dtPizza_PartUnpack.Rows[i]["PizzaID"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[2].Text = dtPizza_PartUnpack.Rows[i]["PartNo"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[3].Text = dtPizza_PartUnpack.Rows[i]["PartType"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[4].Text = dtPizza_PartUnpack.Rows[i]["IECPn"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[5].Text = dtPizza_PartUnpack.Rows[i]["CustmerPn"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[6].Text = dtPizza_PartUnpack.Rows[i]["PartSn"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[7].Text = dtPizza_PartUnpack.Rows[i]["Station"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[8].Text = dtPizza_PartUnpack.Rows[i]["BomNodeType"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[9].Text = dtPizza_PartUnpack.Rows[i]["CheckItemType"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[10].Text = dtPizza_PartUnpack.Rows[i]["UEditor"].ToString();
                    gvPizzaPartUnpack.Rows[i].Cells[11].Text = dtPizza_PartUnpack.Rows[i]["UPdt"].ToString();

                }
                InitPizzaPartUnpackGridView();
            }
            else
            {
                this.gvPizzaPartUnpack.DataSource = getNullDataTable_PizzaPartUnPack(1);
                this.gvPizzaPartUnpack.DataBind();
            }

            DataTable dtChange = new DataTable();                //product Change
            dtChange = ProductInfo.GetProductChange(Connection, hidProduct.Value.Trim());
            if (dtChange.Rows.Count > 0)
            {
                this.gvFAChange.DataSource = null;
                this.gvFAChange.DataSource = getNullDataTable_Change(dtChange.Rows.Count);
                this.gvFAChange.DataBind();

                for (int i = 0; i <= dtChange.Rows.Count - 1; i++)
                {
                    gvFAChange.Rows[i].Cells[0].Text = dtChange.Rows[i]["ProductID"].ToString();
                    gvFAChange.Rows[i].Cells[1].Text = dtChange.Rows[i]["Model"].ToString();
                    gvFAChange.Rows[i].Cells[2].Text = dtChange.Rows[i]["PCBID"].ToString();
                    gvFAChange.Rows[i].Cells[3].Text = dtChange.Rows[i]["PCBModel"].ToString();
                    gvFAChange.Rows[i].Cells[4].Text = dtChange.Rows[i]["MAC"].ToString();
                    gvFAChange.Rows[i].Cells[5].Text = dtChange.Rows[i]["CUSTSN"].ToString();
                    gvFAChange.Rows[i].Cells[6].Text = dtChange.Rows[i]["Cdt"].ToString();
                }
                InitChangeGridView();
            }
            else
            {
                this.gvFAChange.DataSource = getNullDataTable_Change(1);
                this.gvFAChange.DataBind();
            }

            /*fanyang------------------------------------------------------------------------*/
            DataTable dtITCND = new DataTable();
            dtITCND = ProductInfo.GetProductITCND(Connection, hidProduct.Value.Trim());
            if (dtITCND.Rows.Count > 0)
            {
                this.gvFAITCND.DataSource = null;
                this.gvFAITCND.DataSource = getNullDataTable_ITCND(dtITCND.Rows.Count);
                this.gvFAITCND.DataBind();

                for (int i = 0; i <= dtITCND.Rows.Count - 1; i++)
                {
                    gvFAITCND.Rows[i].Cells[0].Text = dtITCND.Rows[i]["Sno"].ToString();
                    gvFAITCND.Rows[i].Cells[1].Text = dtITCND.Rows[i]["Remark"].ToString();
                    gvFAITCND.Rows[i].Cells[2].Text = dtITCND.Rows[i]["Cdt"].ToString();
                    
                }
                InitITCNDGridView();
            }
            else
            {
                this.gvFAITCND.DataSource = getNullDataTable_ITCND(1);
                this.gvFAITCND.DataBind();
            }

            //add Cleanroom Product Part
            DataTable dtCRPart = new DataTable();
            dtCRPart = ProductInfo.GetProductCRPart(Connection, hidProduct.Value.Trim());
            if (dtCRPart.Rows.Count > 0)
            {
                this.gvCRPart.DataSource = null;
                this.gvCRPart.DataSource = getNullDataTable_Part(dtCRPart.Rows.Count);
                this.gvCRPart.DataBind();

                for (int i = 0; i <= dtCRPart.Rows.Count - 1; i++)
                {
                    gvCRPart.Rows[i].Cells[0].Text = dtCRPart.Rows[i]["PartNo"].ToString();
                    gvCRPart.Rows[i].Cells[1].Text = dtCRPart.Rows[i]["PartType"].ToString();
                    gvCRPart.Rows[i].Cells[2].Text = dtCRPart.Rows[i]["Descr"].ToString();
                    gvCRPart.Rows[i].Cells[3].Text = dtCRPart.Rows[i]["PartSn"].ToString();
                    gvCRPart.Rows[i].Cells[4].Text = dtCRPart.Rows[i]["BomNodeType"].ToString();
                    gvCRPart.Rows[i].Cells[5].Text = dtCRPart.Rows[i]["CheckItemType"].ToString();
                    gvCRPart.Rows[i].Cells[6].Text = dtCRPart.Rows[i]["Station"].ToString();
                    gvCRPart.Rows[i].Cells[7].Text = dtCRPart.Rows[i]["Editor"].ToString();
                    gvCRPart.Rows[i].Cells[8].Text = dtCRPart.Rows[i]["Cdt"].ToString();
                }
                InitCRPartGridView();
            }
            else
            {
                this.gvCRPart.DataSource = getNullDataTable_CRPart(ProductCRPart_DEFAULT_ROWS);
                this.gvCRPart.DataBind();
            }


            DataTable dtCRLog = new DataTable();
            dtCRLog = ProductInfo.GetProductCRLog(Connection, hidProduct.Value.Trim());
            if (dt.Rows.Count > 0)
            {
                this.gvCRLog.DataSource = null;
                this.gvCRLog.DataSource = getNullDataTable_History(dt.Rows.Count);
                this.gvCRLog.DataBind();

                for (int i = 0; i <= dtCRLog.Rows.Count - 1; i++)
                {
                    gvCRLog.Rows[i].Cells[0].Text = dtCRLog.Rows[i]["Station"].ToString() + " - " + dt.Rows[i]["StationName"].ToString();
                    gvCRLog.Rows[i].Cells[1].Text = dtCRLog.Rows[i]["Line"].ToString();
                    gvCRLog.Rows[i].Cells[2].Text = dtCRLog.Rows[i]["FixtureID"].ToString();
                    gvCRLog.Rows[i].Cells[3].Text = dtCRLog.Rows[i]["Status"].ToString();
                    gvCRLog.Rows[i].Cells[4].Text = dtCRLog.Rows[i]["ErrorCode"].ToString();
                    gvCRLog.Rows[i].Cells[5].Text = dtCRLog.Rows[i]["Editor"].ToString();
                    gvCRLog.Rows[i].Cells[6].Text = dtCRLog.Rows[i]["Cdt"].ToString();

                }
                InitCRLogGridView();
            }
            else
            {
                this.gvCRLog.DataSource = getNullDataTable_CRLog(ProductCRLog_DEFAULT_ROWS);
                this.gvCRLog.DataBind();
            }

        }
        catch (FisException ex)
        {
            this.gvFAStatus.DataSource = getNullDataTable();
            this.gvFAStatus.DataBind();
            InitGridView();

            this.gvFANextStation.DataSource = getNullDataTable_NextStation(ProductNextStation_DEFAULT_ROWS);
            this.gvFANextStation.DataBind();
            InitNextStationGridView();

            this.gvFAHistory.DataSource = getNullDataTable_History(ProductHistory_DEFAULT_ROWS);
            this.gvFAHistory.DataBind();
            InitHistoryGridView();

            this.gvHoldHistory.DataSource = getNullDataTable_HoldHistory(HoldHistory_DEFAULT_ROWS);
            this.gvHoldHistory.DataBind();
            InitHoldHistoryGridView();

            this.gvFARepair.DataSource = getNullDataTable_Repair(ProductRepair_DEFAULT_ROWS);
            this.gvFARepair.DataBind();
            InitRepairGridView();

            this.gvFAInfo.DataSource = getNullDataTable_Info(ProductInfo_DEFAULT_ROWS);
            this.gvFAInfo.DataBind();
            InitInfoGridView();

            this.gvFAPart.DataSource = getNullDataTable_Part(ProductPart_DEFAULT_ROWS);
            this.gvFAPart.DataBind();
            InitPartGridView();

            this.gvFAUnpack.DataSource = getNullDataTable_UnPack(ProductUnpack_DEFAULT_ROWS);
            this.gvFAUnpack.DataBind();
            InitUnpackGridView();

            this.gvProductPartUnpack.DataSource = getNullDataTable_ProductPartUnPack(ProductPartUnpack_DEFAULT_ROWS);
            this.gvProductPartUnpack.DataBind();
            InitProductPartUnpackGridView();

            this.gvPizzaPartUnpack.DataSource = getNullDataTable_PizzaPartUnPack(PizzaPartUnpack_DEFAULT_ROWS);
            this.gvPizzaPartUnpack.DataBind();
            InitPizzaPartUnpackGridView();

            this.gvFAChange.DataSource = getNullDataTable_Change(ProductChange_DEFAULT_ROWS);
            this.gvFAChange.DataBind();
            InitChangeGridView();
            
            this.gvFAITCND.DataSource = getNullDataTable_ITCND(ProductITCND_DEFAULT_ROWS);
            this.gvFAITCND.DataBind();
            InitITCNDGridView();

            this.gvCRPart.DataSource = getNullDataTable_CRPart(ProductCRPart_DEFAULT_ROWS);
            this.gvCRPart.DataBind();
            InitCRPartGridView();

            this.gvCRLog.DataSource = getNullDataTable_CRLog(ProductCRLog_DEFAULT_ROWS);
            this.gvCRLog.DataBind();
            InitCRLogGridView();
                        
            showErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            this.gvFAStatus.DataSource = getNullDataTable();
            this.gvFAStatus.DataBind();
            InitGridView();

            this.gvFANextStation.DataSource = getNullDataTable_NextStation(ProductNextStation_DEFAULT_ROWS);
            this.gvFANextStation.DataBind();
            InitNextStationGridView();

            this.gvFAHistory.DataSource = getNullDataTable_History(ProductHistory_DEFAULT_ROWS);
            this.gvFAHistory.DataBind();
            InitHistoryGridView();

            this.gvHoldHistory.DataSource = getNullDataTable_HoldHistory(HoldHistory_DEFAULT_ROWS);
            this.gvHoldHistory.DataBind();
            InitHoldHistoryGridView();

            this.gvFARepair.DataSource = getNullDataTable_Repair(ProductRepair_DEFAULT_ROWS);
            this.gvFARepair.DataBind();
            InitRepairGridView();

            this.gvFAInfo.DataSource = getNullDataTable_Info(ProductInfo_DEFAULT_ROWS);
            this.gvFAInfo.DataBind();
            InitInfoGridView();

            this.gvFAPart.DataSource = getNullDataTable_Part(ProductPart_DEFAULT_ROWS);
            this.gvFAPart.DataBind();
            InitPartGridView();

            this.gvFAUnpack.DataSource = getNullDataTable_UnPack(ProductUnpack_DEFAULT_ROWS);
            this.gvFAUnpack.DataBind();
            InitUnpackGridView();

            this.gvProductPartUnpack.DataSource = getNullDataTable_ProductPartUnPack(ProductPartUnpack_DEFAULT_ROWS);
            this.gvProductPartUnpack.DataBind();
            InitProductPartUnpackGridView();

            this.gvPizzaPartUnpack.DataSource = getNullDataTable_PizzaPartUnPack(PizzaPartUnpack_DEFAULT_ROWS);
            this.gvPizzaPartUnpack.DataBind();
            InitPizzaPartUnpackGridView();

            this.gvFAChange.DataSource = getNullDataTable_Change(ProductChange_DEFAULT_ROWS);
            this.gvFAChange.DataBind();
            InitChangeGridView();

            this.gvFAITCND.DataSource = getNullDataTable_ITCND(ProductITCND_DEFAULT_ROWS);
            this.gvFAITCND.DataBind();
            InitITCNDGridView();

            this.gvCRPart.DataSource = getNullDataTable_CRPart(ProductCRPart_DEFAULT_ROWS);
            this.gvCRPart.DataBind();
            InitCRPartGridView();

            this.gvCRLog.DataSource = getNullDataTable_CRLog(ProductCRLog_DEFAULT_ROWS);
            this.gvCRLog.DataBind();
            InitCRLogGridView();

            showErrorMessage(ex.Message);
        }
        finally
        {
            hideWait();
        }
    }

    #region InitGrid Format

    private void InitGridView()            //Product Status.............................................................
    {
        int i = 100;
        int j = 70;
        gvFAStatus.HeaderRow.Cells[0].Width = Unit.Pixel(i);//Product
        gvFAStatus.HeaderRow.Cells[1].Width = Unit.Pixel(i);//Model
        gvFAStatus.HeaderRow.Cells[2].Width = Unit.Pixel(i);//Family
        gvFAStatus.HeaderRow.Cells[3].Width = Unit.Pixel(i);//MO
        gvFAStatus.HeaderRow.Cells[4].Width = Unit.Pixel(i);//UnitWeight
        //gvFAStatus.HeaderRow.Cells[4].Width = Unit.Pixel(j);//StationID
        gvFAStatus.HeaderRow.Cells[5].Width = Unit.Pixel(220);//StationID+ Station
        gvFAStatus.HeaderRow.Cells[6].Width = Unit.Pixel(j);//Status
        gvFAStatus.HeaderRow.Cells[7].Width = Unit.Pixel(j);//Line
        gvFAStatus.HeaderRow.Cells[8].Width = Unit.Pixel(i);//TestFailCount
        gvFAStatus.HeaderRow.Cells[9].Width = Unit.Pixel(i);//MAC
        gvFAStatus.HeaderRow.Cells[10].Width = Unit.Pixel(i);//ECR
        gvFAStatus.HeaderRow.Cells[11].Width = Unit.Pixel(150);//CUSTSN
        gvFAStatus.HeaderRow.Cells[12].Width = Unit.Pixel(i);//MBSN
        gvFAStatus.HeaderRow.Cells[13].Width = Unit.Pixel(i);//MBPartNo
        gvFAStatus.HeaderRow.Cells[14].Width = Unit.Pixel(150);//DeliveryNo
        gvFAStatus.HeaderRow.Cells[15].Width = Unit.Pixel(i);//PalletNo
        gvFAStatus.HeaderRow.Cells[16].Width = Unit.Pixel(i);//CartonSN
        gvFAStatus.HeaderRow.Cells[17].Width = Unit.Pixel(i);//LOC
        
        gvFAStatus.HeaderRow.Cells[18].Width = Unit.Pixel(180);//Udt

        gvFAStatus.HeaderRow.Cells[19].Width = Unit.Pixel(180);//shipdate''''''''''''''''''''''
    }

    private void InitNextStationGridView()
    {
        gvFANextStation.HeaderRow.Cells[0].Width = Unit.Pixel(180); //Station +StationName        
        //gvFANextStation.HeaderRow.Cells[1].Width = Unit.Pixel(180); //
    }
    private void InitHistoryGridView()
    {
        int i = 100;
        int j = 70;
        gvFAHistory.HeaderRow.Cells[0].Width = Unit.Pixel(220); //Station+StationName
        gvFAHistory.HeaderRow.Cells[1].Width = Unit.Pixel(j); //Line
        gvFAHistory.HeaderRow.Cells[2].Width = Unit.Pixel(j); //FixtureID
        gvFAHistory.HeaderRow.Cells[3].Width = Unit.Pixel(j); //status                
        gvFAHistory.HeaderRow.Cells[4].Width = Unit.Pixel(i); //ErrorCode
        gvFAHistory.HeaderRow.Cells[5].Width = Unit.Pixel(j); //Editor                
        gvFAHistory.HeaderRow.Cells[6].Width = Unit.Pixel(150); //Cdt                
    }
    private void InitHoldHistoryGridView()
    {
        int i = 100;
        int j = 70;
        gvFAHistory.HeaderRow.Cells[0].Width = Unit.Pixel(j); //Line
        gvFAHistory.HeaderRow.Cells[1].Width = Unit.Pixel(i); //Family
        gvFAHistory.HeaderRow.Cells[2].Width = Unit.Pixel(j); //Model
        gvFAHistory.HeaderRow.Cells[3].Width = Unit.Pixel(j); //CUSTSN                
        gvFAHistory.HeaderRow.Cells[4].Width = Unit.Pixel(i); //CheckInStation
        gvFAHistory.HeaderRow.Cells[5].Width = Unit.Pixel(300); //HoldDescr                
        gvFAHistory.HeaderRow.Cells[6].Width = Unit.Pixel(150); //Editor                
    }


    private void InitCRLogGridView()
    {
        int i = 100;
        int j = 70;
        gvCRLog.HeaderRow.Cells[0].Width = Unit.Pixel(220); //Station+StationName
        gvCRLog.HeaderRow.Cells[1].Width = Unit.Pixel(j); //Line
        gvCRLog.HeaderRow.Cells[2].Width = Unit.Pixel(j); //FixtureID
        gvCRLog.HeaderRow.Cells[3].Width = Unit.Pixel(j); //status                
        gvCRLog.HeaderRow.Cells[4].Width = Unit.Pixel(i); //ErrorCode
        gvCRLog.HeaderRow.Cells[5].Width = Unit.Pixel(j); //Editor                
        gvCRLog.HeaderRow.Cells[6].Width = Unit.Pixel(150); //Cdt                
    }


    private void InitRepairGridView()
    {
        int i = 100;
        int j = 150;
        int k = 70;
        int l = 400;

        gvFARepair.HeaderRow.Cells[0].Width = Unit.Pixel(50);//Line
        gvFARepair.HeaderRow.Cells[1].Width = Unit.Pixel(j);//StationName
        gvFARepair.HeaderRow.Cells[2].Width = Unit.Pixel(50);//Status
        gvFARepair.HeaderRow.Cells[3].Width = Unit.Pixel(j);//Location
        gvFARepair.HeaderRow.Cells[4].Width = Unit.Pixel(l);//Descr
        gvFARepair.HeaderRow.Cells[5].Width = Unit.Pixel(l);//Cause
        gvFARepair.HeaderRow.Cells[6].Width = Unit.Pixel(j);//Obligation        
        gvFARepair.HeaderRow.Cells[7].Width = Unit.Pixel(l);//Remark
        gvFARepair.HeaderRow.Cells[8].Width = Unit.Pixel(80);//Editor
        gvFARepair.HeaderRow.Cells[9].Width = Unit.Pixel(j);//Cdt

    }

    private void InitInfoGridView()
    {
        int i = 100;
        int j = 150;
        int k = 70;

        gvFAInfo.HeaderRow.Cells[0].Width = Unit.Pixel(k);//InfoType
        gvFAInfo.HeaderRow.Cells[1].Width = Unit.Pixel(j);//InfoValue
        gvFAInfo.HeaderRow.Cells[2].Width = Unit.Pixel(k);//Editor
        gvFAInfo.HeaderRow.Cells[3].Width = Unit.Pixel(i);//Cdt       
        gvFAInfo.HeaderRow.Cells[4].Width = Unit.Pixel(i);//Udt       
    }

    private void InitPartGridView()
    {
        int i = 100;
        int j = 150;
        int k = 70;

        gvFAPart.HeaderRow.Cells[0].Width = Unit.Pixel(k);//PartNo
        gvFAPart.HeaderRow.Cells[1].Width = Unit.Pixel(k);//PartType
        gvFAPart.HeaderRow.Cells[2].Width = Unit.Pixel(j);//Descr
        gvFAPart.HeaderRow.Cells[3].Width = Unit.Pixel(i);//PartSn
        gvFAPart.HeaderRow.Cells[4].Width = Unit.Pixel(k);//BomNodeType
        gvFAPart.HeaderRow.Cells[5].Width = Unit.Pixel(k);//CheckItemType
        gvFAPart.HeaderRow.Cells[6].Width = Unit.Pixel(j);//Station        
        gvFAPart.HeaderRow.Cells[7].Width = Unit.Pixel(k);//Editor
        gvFAPart.HeaderRow.Cells[8].Width = Unit.Pixel(i);//Cdt        
    }

    private void InitCRPartGridView()
    {
        int i = 100;
        int j = 150;
        int k = 70;

        gvCRPart.HeaderRow.Cells[0].Width = Unit.Pixel(k);//PartNo
        gvCRPart.HeaderRow.Cells[1].Width = Unit.Pixel(k);//PartType
        gvCRPart.HeaderRow.Cells[2].Width = Unit.Pixel(j);//Descr
        gvCRPart.HeaderRow.Cells[3].Width = Unit.Pixel(i);//PartSn
        gvCRPart.HeaderRow.Cells[4].Width = Unit.Pixel(k);//BomNodeType
        gvCRPart.HeaderRow.Cells[5].Width = Unit.Pixel(k);//CheckItemType
        gvCRPart.HeaderRow.Cells[6].Width = Unit.Pixel(j);//Station        
        gvCRPart.HeaderRow.Cells[7].Width = Unit.Pixel(k);//Editor
        gvCRPart.HeaderRow.Cells[8].Width = Unit.Pixel(i);//Cdt        
    }
    
    private void InitUnpackGridView()
    {
        int j = 150;
        gvFAUnpack.HeaderRow.Cells[0].Width = Unit.Pixel(j); //ProductID
        gvFAUnpack.HeaderRow.Cells[1].Width = Unit.Pixel(j); //Model       
        gvFAUnpack.HeaderRow.Cells[2].Width = Unit.Pixel(j); //PCBID   
        gvFAUnpack.HeaderRow.Cells[3].Width = Unit.Pixel(j); //PCBModel   
        gvFAUnpack.HeaderRow.Cells[4].Width = Unit.Pixel(j); //MAC   
        gvFAUnpack.HeaderRow.Cells[5].Width = Unit.Pixel(j); //CUSTSN   
        gvFAUnpack.HeaderRow.Cells[6].Width = Unit.Pixel(j); //UEditor   
        gvFAUnpack.HeaderRow.Cells[7].Width = Unit.Pixel(j); //UPdt   
    }

    private void InitProductPartUnpackGridView()
    {
        int j = 150;
        gvProductPartUnpack.HeaderRow.Cells[0].Width = Unit.Pixel(j); //ProductID
        gvProductPartUnpack.HeaderRow.Cells[1].Width = Unit.Pixel(j); //PartNo       
        gvProductPartUnpack.HeaderRow.Cells[2].Width = Unit.Pixel(j); //PartType   
        gvProductPartUnpack.HeaderRow.Cells[3].Width = Unit.Pixel(j); //IECPn   
        gvProductPartUnpack.HeaderRow.Cells[4].Width = Unit.Pixel(j); //CustmerPn   
        gvProductPartUnpack.HeaderRow.Cells[5].Width = Unit.Pixel(j); //PartSn   
        gvProductPartUnpack.HeaderRow.Cells[6].Width = Unit.Pixel(j); //Station   
        gvProductPartUnpack.HeaderRow.Cells[7].Width = Unit.Pixel(j); //BomNodeType 
        gvProductPartUnpack.HeaderRow.Cells[8].Width = Unit.Pixel(j); //CheckItemType 
        gvProductPartUnpack.HeaderRow.Cells[9].Width = Unit.Pixel(j); //UEditor 
        gvProductPartUnpack.HeaderRow.Cells[10].Width = Unit.Pixel(j); //UPdt 
    }

    private void InitPizzaPartUnpackGridView()
    {
        int j = 150;
        gvPizzaPartUnpack.HeaderRow.Cells[0].Width = Unit.Pixel(j); //ProductID
        gvPizzaPartUnpack.HeaderRow.Cells[1].Width = Unit.Pixel(j); //PizzaID       
        gvPizzaPartUnpack.HeaderRow.Cells[2].Width = Unit.Pixel(j); //PartNo   
        gvPizzaPartUnpack.HeaderRow.Cells[3].Width = Unit.Pixel(j); //PartType   
        gvPizzaPartUnpack.HeaderRow.Cells[4].Width = Unit.Pixel(j); //IECPn   
        gvPizzaPartUnpack.HeaderRow.Cells[5].Width = Unit.Pixel(j); //CustmerPn   
        gvPizzaPartUnpack.HeaderRow.Cells[6].Width = Unit.Pixel(j); //PartSn   
        gvPizzaPartUnpack.HeaderRow.Cells[7].Width = Unit.Pixel(j); //Station 
        gvPizzaPartUnpack.HeaderRow.Cells[8].Width = Unit.Pixel(j); //BomNodeType 
        gvPizzaPartUnpack.HeaderRow.Cells[9].Width = Unit.Pixel(j); //CheckItemType 
        gvPizzaPartUnpack.HeaderRow.Cells[10].Width = Unit.Pixel(j); //UEditor 
        gvPizzaPartUnpack.HeaderRow.Cells[11].Width = Unit.Pixel(j); //UPdt 
    }

    private void InitChangeGridView()
    {
        int j = 150;
        gvFAChange.HeaderRow.Cells[0].Width = Unit.Pixel(j); //ProductID
        gvFAChange.HeaderRow.Cells[1].Width = Unit.Pixel(j); //Model       
        gvFAChange.HeaderRow.Cells[2].Width = Unit.Pixel(j); //PCBID   
        gvFAChange.HeaderRow.Cells[3].Width = Unit.Pixel(j); //PCBModel   
        gvFAChange.HeaderRow.Cells[4].Width = Unit.Pixel(j); //MAC   
        gvFAChange.HeaderRow.Cells[5].Width = Unit.Pixel(j); //CUSTSN   
        gvFAChange.HeaderRow.Cells[6].Width = Unit.Pixel(j); //Cdt   
    }

    private void  InitITCNDGridView()
    {
        int i = 150;
        int j = 1000;
        int k = 200;
        gvFAITCND.HeaderRow.Cells[0].Width = Unit.Pixel(i); //Sno
        gvFAITCND.HeaderRow.Cells[1].Width = Unit.Pixel(j); //Remark       
        gvFAITCND.HeaderRow.Cells[2].Width = Unit.Pixel(k); //Cdt   
    }
#endregion



    #region InitDatTable Column

    private DataTable initTable()      //PrdocuctStatus...............................................
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Product", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("Family", Type.GetType("System.String"));
        retTable.Columns.Add("MO", Type.GetType("System.String"));
        retTable.Columns.Add("UnitWeight", Type.GetType("System.String"));
        //retTable.Columns.Add("StationID", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("TestFailCount", Type.GetType("System.String"));
        retTable.Columns.Add("MAC", Type.GetType("System.String"));
        retTable.Columns.Add("ECR", Type.GetType("System.String"));
        retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("MBSN", Type.GetType("System.String"));
        retTable.Columns.Add("MBPartNo", Type.GetType("System.String"));
        retTable.Columns.Add("DeliveryNo", Type.GetType("System.String"));
        retTable.Columns.Add("PalletNo", Type.GetType("System.String"));
        retTable.Columns.Add("CartonSN", Type.GetType("System.String"));
        retTable.Columns.Add("WHLocation", Type.GetType("System.String"));
        retTable.Columns.Add("Udt", Type.GetType("System.String"));

        retTable.Columns.Add("ShipDate", Type.GetType("System.String"));//shipdate''''''''''''''''''

        return retTable;
    }
   
    private DataTable initTable_History()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("FixtureID", Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("ErrorCode", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_CRLog()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("FixtureID", Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("ErrorCode", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_Repair()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Status", Type.GetType("System.String"));
        retTable.Columns.Add("Location", Type.GetType("System.String"));
        retTable.Columns.Add("Defect", Type.GetType("System.String"));
        retTable.Columns.Add("Cause", Type.GetType("System.String"));
        retTable.Columns.Add("Obligation", Type.GetType("System.String"));       
        retTable.Columns.Add("Remark", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }
    
    private DataTable initTable_NextStation()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Station", Type.GetType("System.String"));        
        return retTable;
    }


    private DataTable initTable_Info()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("InfoType", Type.GetType("System.String"));
        retTable.Columns.Add("InfoValue", Type.GetType("System.String"));        
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        retTable.Columns.Add("Udt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_Part()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("PartType", Type.GetType("System.String"));
        retTable.Columns.Add("Descr", Type.GetType("System.String"));
        retTable.Columns.Add("PartSn", Type.GetType("System.String"));
        retTable.Columns.Add("BomNodeType", Type.GetType("System.String"));
        retTable.Columns.Add("CheckItemType", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));        
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));        
        return retTable;
    }

    private DataTable initTable_CRPart()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("PartType", Type.GetType("System.String"));
        retTable.Columns.Add("Descr", Type.GetType("System.String"));
        retTable.Columns.Add("PartSn", Type.GetType("System.String"));
        retTable.Columns.Add("BomNodeType", Type.GetType("System.String"));
        retTable.Columns.Add("CheckItemType", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_Unpack()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ProductID", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("PCBID", Type.GetType("System.String"));
        retTable.Columns.Add("PCBModel", Type.GetType("System.String"));
        retTable.Columns.Add("MAC", Type.GetType("System.String"));
        retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("UEditor", Type.GetType("System.String"));
        retTable.Columns.Add("UPdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_ProductPartUnpack()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ProductID", Type.GetType("System.String"));
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("PartType", Type.GetType("System.String"));
        retTable.Columns.Add("IECPn", Type.GetType("System.String"));
        retTable.Columns.Add("CustmerPn", Type.GetType("System.String"));
        retTable.Columns.Add("PartSn", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("BomNodeType", Type.GetType("System.String"));
        retTable.Columns.Add("CheckItemType", Type.GetType("System.String"));
        retTable.Columns.Add("UEditor", Type.GetType("System.String"));
        retTable.Columns.Add("UPdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_PizzaPartUnpack()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ProductID", Type.GetType("System.String"));
        retTable.Columns.Add("PizzaID", Type.GetType("System.String"));
        retTable.Columns.Add("PartNo", Type.GetType("System.String"));
        retTable.Columns.Add("PartType", Type.GetType("System.String"));
        retTable.Columns.Add("IECPn", Type.GetType("System.String"));
        retTable.Columns.Add("CustmerPn", Type.GetType("System.String"));
        retTable.Columns.Add("PartSn", Type.GetType("System.String"));
        retTable.Columns.Add("Station", Type.GetType("System.String"));
        retTable.Columns.Add("BomNodeType", Type.GetType("System.String"));
        retTable.Columns.Add("CheckItemType", Type.GetType("System.String"));
        retTable.Columns.Add("UEditor", Type.GetType("System.String"));
        retTable.Columns.Add("UPdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_Change()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("ProductID", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("PCBID", Type.GetType("System.String"));
        retTable.Columns.Add("PCBModel", Type.GetType("System.String"));
        retTable.Columns.Add("MAC", Type.GetType("System.String"));
        retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_HoldHistory()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Line", Type.GetType("System.String"));
        retTable.Columns.Add("Family", Type.GetType("System.String"));
        retTable.Columns.Add("Model", Type.GetType("System.String"));
        retTable.Columns.Add("CUSTSN", Type.GetType("System.String"));
        retTable.Columns.Add("CheckInStation", Type.GetType("System.String"));
        retTable.Columns.Add("HoldDescr", Type.GetType("System.String"));
        retTable.Columns.Add("Editor", Type.GetType("System.String"));
        return retTable;
    }

    private DataTable initTable_ITCND()
    {
        DataTable retTable = new DataTable();
        retTable.Columns.Add("Sno", Type.GetType("System.String"));
        retTable.Columns.Add("Remark", Type.GetType("System.String"));
        retTable.Columns.Add("Cdt", Type.GetType("System.String"));
        return retTable;
    }

    #endregion


    private DataTable getNullDataTable_NextStation(int j)
    {
        DataTable dt = initTable_NextStation();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();                                                             
            newRow["Station"] = "";                                                                  
            dt.Rows.Add(newRow);                                                           
        }                                                     
        return dt;
    }

    private DataSet getDataTable(string Product)
    {
        DataSet ds = new DataSet();

        DataTable dt = initTable(); //ProductStatus
        DataTable dt1 = initTable_NextStation();//Product_NextStation

        DataRow newRow = null;
        List<NextStation> nextStationList = new List<NextStation>();
        
        string Connection = CmbDBType.ddlGetConnection();

        IMES.Query.Interface.QueryIntf.ProdutData pro = ProductInfo.ProductInfo(Connection, Product, out nextStationList);

        if (pro != null)
        {
            newRow = dt.NewRow();
            newRow["Product"] = pro.ProductID;
            newRow["Model"] = pro.Model;
            newRow["Family"] = pro.Family;
            newRow["MO"] = pro.MO;
            newRow["UnitWeight"] = pro.UnitWeight;
            //newRow["StationID"] = pro.Station;
            newRow["Station"] = pro.Station +" - "+ pro.StationDescr;
            newRow["Status"] = pro.Status == 0 ? "Fail" : "Pass";
            newRow["Line"] = pro.Line;
            newRow["TestFailCount"] = pro.TestFailCount;
            newRow["MAC"] = pro.MAC;
            newRow["ECR"] = pro.ECR;
            newRow["CUSTSN"] = pro.CustomSN;
            newRow["MBSN"] = pro.MBSN;
            newRow["MBPartNo"] = pro.MBPartNo;
            newRow["DeliveryNo"] = pro.DeliveryNo;
            newRow["PalletNo"] = pro.PalletNo;
            newRow["CartonSN"] = pro.CartonSN;
            newRow["WHLocation"] = pro.WHLocation;
            newRow["Udt"] = pro.Udt;

            newRow["ShipDate"] = pro.ShipDate;//shipdate'''''''''''''''''''''''''''''''''

            dt.Rows.Add(newRow);

            foreach (NextStation ns in nextStationList)
            {
                newRow = dt1.NewRow();
                newRow["Station"] = ns.Station +" - " + ns.Description;                
                dt1.Rows.Add(newRow);
            }

            ds.Tables.Add(dt);
            ds.Tables.Add(dt1);
        }

        return ds;
    }

    private DataTable getNullDataTable()
    {

        DataTable dt = initTable();
        DataRow newRow = null;
        for (int i = 0; i < ProductStatus_DEFAULT_ROWS; i++)
        {
            newRow = dt.NewRow();
            newRow["Product"] = "";
            newRow["Model"] = "";
            newRow["Family"] = "";
            newRow["MO"] = "";
            newRow["UnitWeight"] = "";
            //newRow["StationID"] = "";
            newRow["Station"] = ""; ;
            newRow["Status"] = "";
            newRow["Line"] = "";
            newRow["TestFailCount"] = "";
            newRow["MAC"] = "";
            newRow["ECR"] = "";
            newRow["CUSTSN"] = "";
            newRow["MBSN"] = "";
            newRow["MBPartNo"] = "";
            newRow["DeliveryNo"] = "";
            newRow["PalletNo"] = "";
            newRow["CartonSN"] = "";
            newRow["WHLocation"] = "";
            newRow["Udt"] = "";

            newRow["ShipDate"]="";                //shipdate''''''''''''''''''''''''''''''''''''

            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable_History(int j)
    {
        DataTable dt = initTable_History();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Station"] = "";
            newRow["Line"] = "";
            newRow["FixtureID"] = "";
            newRow["Status"] = "";
            newRow["ErrorCode"] = "";
            newRow["Editor"] = "";
            newRow["Cdt"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable_HoldHistory(int j)
    {
        DataTable dt = initTable_HoldHistory();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Line"] = "";
            newRow["Family"] = "";
            newRow["Model"] = "";
            newRow["CUSTSN"] = "";
            newRow["CheckInStation"] = "";
            newRow["HoldDescr"] = "";
            newRow["Editor"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable_CRLog(int j)
    {
        DataTable dt = initTable_CRLog();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Station"] = "";
            newRow["Line"] = "";
            newRow["FixtureID"] = "";
            newRow["Status"] = "";
            newRow["ErrorCode"] = "";
            newRow["Editor"] = "";
            newRow["Cdt"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable_Repair(int j)
    {
        DataTable dt = initTable_Repair();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Line"] = "";
            newRow["Station"] = "";
            newRow["Status"] = "";
            newRow["Location"] = "";
            newRow["Defect"] = "";
            newRow["Cause"] = "";
            newRow["Obligation"] = "";
            newRow["Remark"] = "";
            newRow["Editor"] = "";
            newRow["Cdt"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;

    }

    private DataTable getNullDataTable_Info(int j)
    {
        DataTable dt = initTable_Info();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["InfoType"] = "";
            newRow["InfoValue"] = "";
            newRow["Editor"] = "";
            newRow["Cdt"] = "";
            newRow["Udt"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;

    }

    private DataTable getNullDataTable_Part(int j)
    {
        DataTable dt = initTable_Part();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();             
            newRow["PartNo"] = "";                        
            newRow["PartType"] = "";                     
            newRow["Descr"] = "";                        
            newRow["PartSn"] = "";                       
            newRow["BomNodeType"] = "";           
            newRow["CheckItemType"] = "";        
            newRow["Station"] = "";              
            newRow["Editor"] = "";               
            newRow["Cdt"] = "";                 

            dt.Rows.Add(newRow);                
        }
        return dt;                           

    }

    private DataTable getNullDataTable_CRPart(int j)
    {
        DataTable dt = initTable_CRPart();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["PartNo"] = "";
            newRow["PartType"] = "";
            newRow["Descr"] = "";
            newRow["PartSn"] = "";
            newRow["BomNodeType"] = "";
            newRow["CheckItemType"] = "";
            newRow["Station"] = "";
            newRow["Editor"] = "";
            newRow["Cdt"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;

    }

    private DataTable getNullDataTable_UnPack(int j)
    {
        DataTable dt = initTable_Unpack();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["ProductID"] = "";
            newRow["Model"] = "";
            newRow["PCBID"] = "";
            newRow["PCBModel"] = "";
            newRow["MAC"] = "";
            newRow["CUSTSN"] = "";
            newRow["UEditor"] = "";
            newRow["UPdt"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable_ProductPartUnPack(int j)
    {
        DataTable dt = initTable_ProductPartUnpack();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["ProductID"] = "";
            newRow["PartNo"] = "";
            newRow["PartType"] = "";
            newRow["IECPn"] = "";
            newRow["CustmerPn"] = "";
            newRow["PartSn"] = "";
            newRow["Station"] = "";
            newRow["BomNodeType"] = "";
            newRow["CheckItemType"] = "";
            newRow["UEditor"] = "";
            newRow["UPdt"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable_PizzaPartUnPack(int j)
    {
        DataTable dt = initTable_PizzaPartUnpack();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["ProductID"] = "";
            newRow["PizzaID"] = "";
            newRow["PartNo"] = "";
            newRow["PartType"] = "";
            newRow["IECPn"] = "";
            newRow["CustmerPn"] = "";
            newRow["PartSn"] = "";
            newRow["Station"] = "";
            newRow["BomNodeType"] = "";
            newRow["CheckItemType"] = "";
            newRow["UEditor"] = "";
            newRow["UPdt"] = "";
            
       //     b.ProductID ,a.PizzaID, a.PartNo, a.PartType, a.IECPn, a.CustmerPn, 
       //a.PartSn, a.Station, a.BomNodeType, a.CheckItemType, a.UEditor, a.UPdt
            dt.Rows.Add(newRow);
        }
        return dt;
    }

    private DataTable getNullDataTable_Change(int j)
    {
        DataTable dt = initTable_Change();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["ProductID"] = "";
            newRow["Model"] = "";
            newRow["PCBID"] = "";
            newRow["PCBModel"] = "";
            newRow["MAC"] = "";
            newRow["CUSTSN"] = "";
            newRow["Cdt"] = "";
            dt.Rows.Add(newRow);
        }
        return dt;
    }
    
    private DataTable getNullDataTable_ITCND(int j)
    {
        DataTable dt = initTable_ITCND();
        DataRow newRow = null;
        for (int i = 0; i < j; i++)
        {
            newRow = dt.NewRow();
            newRow["Sno"] = "";
            newRow["Remark"] = "";
            newRow["Cdt"] = "";

            dt.Rows.Add(newRow);
        }
        return dt;

    }
    private void showErrorMessage(string errorMsg)
    {
        errorMsg = "Not Found Product Information!!";
     
        this.gvFAStatus.DataSource = getNullDataTable();
        this.gvFAStatus.DataBind();
        InitGridView();

        this.gvFANextStation.DataSource = getNullDataTable_NextStation(ProductNextStation_DEFAULT_ROWS);
        this.gvFANextStation.DataBind();
        InitNextStationGridView();

        this.gvFAHistory.DataSource = getNullDataTable_History(ProductNextStation_DEFAULT_ROWS);
        this.gvFAHistory.DataBind();
        InitHistoryGridView();

        this.gvFARepair.DataSource = getNullDataTable_Repair(ProductRepair_DEFAULT_ROWS);
        this.gvFARepair.DataBind();
        InitRepairGridView();


        this.gvFAInfo.DataSource = getNullDataTable_Info(ProductInfo_DEFAULT_ROWS);
        this.gvFAInfo.DataBind();
        InitInfoGridView();

        this.gvFAPart.DataSource = getNullDataTable_Part(ProductPart_DEFAULT_ROWS);
        this.gvFAPart.DataBind();
        InitPartGridView();

        this.gvFAUnpack.DataSource = getNullDataTable_UnPack(ProductUnpack_DEFAULT_ROWS);
        this.gvFAUnpack.DataBind();
        InitUnpackGridView();

        this.gvFAChange.DataSource = getNullDataTable_Change(ProductChange_DEFAULT_ROWS);
        this.gvFAChange.DataBind();
        InitChangeGridView();

        this.gvFAITCND.DataSource = getNullDataTable_ITCND(ProductITCND_DEFAULT_ROWS);
        this.gvFAITCND.DataBind();
        InitITCNDGridView();

        this.gvCRPart.DataSource = getNullDataTable_CRPart(ProductCRPart_DEFAULT_ROWS);
        this.gvCRPart.DataBind();
        InitCRPartGridView();

        this.gvCRLog.DataSource = getNullDataTable_CRLog(ProductCRLog_DEFAULT_ROWS);
        this.gvCRLog.DataBind();
        InitCRLogGridView();

        StringBuilder scriptBuilder = new StringBuilder();
        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("initPage();");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel3, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel4, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel5, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel6, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel7, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel8, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel9, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel10, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel11, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel14, typeof(System.Object), "showErrorMessage", scriptBuilder.ToString(), false);


    }
    
    private void setFocus()
    {
        String script = "<script language='javascript'>  getCommonInputObject().focus(); </script>";
        ScriptManager.RegisterStartupScript(this.Form, ClientScript.GetType(), "setFocus", script, false);
    }

    private void writeToAlertMessage(string errorMsg)
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("ShowMessage(\"" + errorMsg.Replace("\r\n", "<br>") + "\");");
        scriptBuilder.AppendLine("ShowInfo(\"" + errorMsg.Replace("\r\n", "\\n") + "\");");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel3, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel4, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel5, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel6, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel7, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel8, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel9, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel10, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel11, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel14, typeof(System.Object), "writeToAlertMessageAgent", scriptBuilder.ToString(), false);
   
    }

    private void hideWait()
    {
        StringBuilder scriptBuilder = new StringBuilder();

        scriptBuilder.AppendLine("<script language='javascript'>");
        scriptBuilder.AppendLine("setCommonFocus();");
        //scriptBuilder.AppendLine("endWaitingCoverDiv();");
        //scriptBuilder.AppendLine("window.setTimeout('function(){getCommonInputObject().focus();getCommonInputObject().select();}',0);");
        scriptBuilder.AppendLine("</script>");

        ScriptManager.RegisterStartupScript(this.UpdatePanel1, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel2, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel3, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel4, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel5, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel6, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel7, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel8, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel9, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel10, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel11, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
        ScriptManager.RegisterStartupScript(this.UpdatePanel14, typeof(System.Object), "hideWait", scriptBuilder.ToString(), false);
   
    }

 
}
