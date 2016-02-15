using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using Inventec.HPEDITS.XmlCreator;
using Inventec.HPEDITS.XmlCreator.Database;


/// <summary>
/// Summary description for EDITS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class EDITS : System.Web.Services.WebService
{

    public EDITS()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /*[WebMethod]
    public EDITSResponse BoxShipLabel(string serialNO, string boxID)
    //public string BoxShipLabel(string serialNO, string boxID)
    {
        //string FilePath=@"D:\hpedits\uploadfiles\aa.xml";

        EDITSResponse response = new EDITSResponse();
        BoxLabelXmlCreator xmlCreator = new BoxLabelXmlCreator();
        string whereClause="WHERE SERIAL_NUM='"+serialNO.Trim()+"'";
        List<string> fields=new List<string>();
        fields.Add("InternalID");
        DataTable readTable=DBFactory.PopulateTempTable("[PAK_PackkingData]",whereClause,fields);
        if (readTable.Rows.Count > 0)
        {
            string internalID = readTable.Rows[0]["InternalID"].ToString();
            string concatenatedID = internalID.Trim() + @"/" + boxID.Trim();
            //string concatenatedID = internalID.Trim() + @"/" + serialNO.Trim();
            xmlCreator.LoadBoxLabelDatabaseData(concatenatedID);
            XmlDataDocument dataDocument = xmlCreator.GetXML(true);
            response.XML = dataDocument.InnerXml;
            //xmlCreator.WriteXml(FilePath, true);
            //return internalID.Trim();
        }
        //return "";
        return response;
    }*/

    [WebMethod]
    public string PackListLabel(string FilePH, string Dn)
    {
        XmlCreator xmlCreator = null;
        xmlCreator = new PackListXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadData(Dn.ToString()+@"/"+"");
            xmlCreator.WriteXml(FilePH, false);
            
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }   

    [WebMethod]
    public string PackListShipmentLabel(string FilePH, string Dn)
    {
        XmlCreator xmlCreator = null;
        xmlCreator = new PackListShipmentXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadData(Dn.ToString()+ @"/"+"" );
            xmlCreator.WriteXml(FilePH, false);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod]
    public string BoxShipToLabel(string FilePH, string Dn, string SerialNum)
    {
        BoxLabelXmlCreator xmlCreator = null;
        xmlCreator = new BoxLabelXmlCreator(); 
        string res = null;
        try
        {
            xmlCreator.LoadBoxLabelDatabaseData(Dn.ToString().Trim() + @"/" + SerialNum.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod]
    public string BoxShipToShipmentLabel(string FilePH, string Dn, string SerialNum)
    {
        BoxLabelShipmentXmlCreator xmlCreator = null;
        xmlCreator = new BoxLabelShipmentXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadBoxLabelDatabaseData(Dn.ToString().Trim() + @"/" + SerialNum.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod]
    public string PalletALabel(string FilePH, string Dn,string Pallet)
    {
        PalletAXmlCreator xmlCreator = null;
        xmlCreator = new PalletAXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadPalletADatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod]
    public string PalletAShipmentLabel(string FilePH, string Dn, string Pallet)
    {
        PalletAShipmentXmlCreator xmlCreator = null;
        xmlCreator = new PalletAShipmentXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadPalletADatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod]
    public string PalletBLabel(string FilePH, string Dn, string Pallet)
    {
        PalletBXmlCreator xmlCreator = null;
        xmlCreator = new PalletBXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadPalletBDatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod]
    public string PalletBShipmentLabel(string FilePH, string Dn, string Pallet)
    {
        PalletBShipmentXmlCreator xmlCreator = null;
        xmlCreator = new PalletBShipmentXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadPalletBDatabaseData(Dn.ToString().Trim() + @"/" + Pallet.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod]
    public string WayBillList(string FilePH, string Dn)
    {
        XmlCreator xmlCreator = null;
        //xmlCreator = new WayBillXmlCreator();
        xmlCreator = new WayBillShipmentXmlCreator();

        string res = null;
        try
        {
            xmlCreator.LoadData(Dn.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod]
    public string WayBillShipmentList(string FilePH, string Dn)
    {
        XmlCreator xmlCreator = null;
       // xmlCreator = new WayBillShipmentXmlCreator();
        xmlCreator = new WayBillShipmentXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadData(Dn.ToString().Trim());
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;
    }

    [WebMethod(Description = "Master_Waybill")]
    public string MasterWaybillList(string FilePH, string Dn)
    {
        MasterWaybillShipmentXmlCreator xmlCreator = null;
        xmlCreator = new MasterWaybillShipmentXmlCreator();
        string res = null;
        try
        {
            xmlCreator.LoadData(Dn.ToString().Trim() + @"/" + "");
            xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;

    }
    [WebMethod(Description = "QVC_packList")]
    public string PackingShipment_QVCLabel(string FilePH, string Dn)
    {
        PackListShipmentXmlCreator_QVC xmlCreator = null;
        xmlCreator = new PackListShipmentXmlCreator_QVC();
        string res = null;
        try
        {
            xmlCreator.LoadData(Dn.ToString().Trim() + @"/" + "", FilePH);
            //xmlCreator.WriteXml(FilePH, true);
            res = Dn;
        }
        catch (Exception err)
        {
            res = err.Message.ToString();
        }
        return res;

    }
}

[Serializable]
public class EDITSResponse
{
    public string DOC_SET_NUMBER;
    public string XML;
    public EDITSResponse()
    {
        this.DOC_SET_NUMBER = string.Empty;
        this.XML = string.Empty;
    }
    public EDITSResponse(string docSetNumber, string xml)
    {
        this.DOC_SET_NUMBER = docSetNumber;
        this.XML = xml;
    }
}



