﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using UTL.SQL;
using UTL;
namespace eBook
{
    class SQLStatement
    {
        static public List<string> GetCDSISNList(SqlConnection connect,
                                                                                Log log,
                                                                                string snoId,
                                                                                string tp)
       {
          List<string> SNList = new List<string>();
 
          SqlCommand dbCmd = connect.CreateCommand();
          dbCmd.CommandType = CommandType.StoredProcedure;
          dbCmd.CommandText = "op_CDSIDataUpdate";
          SQLHelper.createInputSqlParameter(dbCmd, "@SnoId", 10, snoId);
          SQLHelper.createInputSqlParameter(dbCmd, "@tp", 2, tp);

          log.write(LogType.Info, 0, "SQL", "GetDNList", dbCmd.CommandText);
          log.write(LogType.Info, 0, "SQL", "@SnoId", snoId);
          log.write(LogType.Info, 0, "SQL", "@tp", tp);


         SqlDataReader sdr = dbCmd.ExecuteReader();
         while (sdr.Read())
         {
            SNList.Add(sdr.GetString(0).Trim());          
         }
         sdr.Close();
         return SNList;
       }
       

        static public List<string> GetCNRSSNList(SqlConnection connect,
                                                                            Log log,
                                                                            int offsetDay)
        {
            List<string> SNList = new List<string>();

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"select distinct a.SnoId
                                                            from Special_Det a
                                                            left join ProductAttr b on (a.SnoId = b.ProductID and b.AttrName='CNRSState')
                                                            where a.Tp='CNRS' 
	                                                            and a.Udt>=dateadd(dd,@day,getdate())
	                                                            and b.AttrValue is null";

            SQLHelper.createInputSqlParameter(dbCmd, "@day", offsetDay);


            log.write(LogType.Info, 0, "SQL", "GetCNRSSNList", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@day", offsetDay.ToString());



            SqlDataReader sdr = dbCmd.ExecuteReader();

            while (sdr.Read())
            {
                SNList.Add(sdr.GetString(0).Trim());
            }
            sdr.Close();
            return SNList;
        }

        static public string GetSnoPoMo(SqlConnection connect,
                                                                        Log log,
                                                                        string productId)
        {
            string ret = null;

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = "select top 1 PO from dbo.SnoDet_PoMo (nolock) where SnoId=@SnoId";
            SQLHelper.createInputSqlParameter(dbCmd, "@SnoId", 10, productId);


            log.write(LogType.Info, 0, "SQL", "GetSnoPoMo", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@SnoId", productId);
           


            SqlDataReader sdr = dbCmd.ExecuteReader();
            while (sdr.Read())
            {
                ret=sdr.GetString(0).Trim();
            }
            sdr.Close();
            return ret;
        }

        static public void ReadCDSIXML(SqlConnection connect,
                                                              Log log,
                                                              string snoId)
        {
            List<string> SNList = new List<string>();

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = "op_ReadCDSIXML";
            SQLHelper.createInputSqlParameter(dbCmd, "@snoid", 10, snoId);
           
            log.write(LogType.Info, 0, "SQL", "GetDNList", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@snoid", snoId);

            dbCmd.ExecuteNonQuery();          
            
        }

        static public ProductInfo GetProductInfo(SqlConnection connect,
                                                                           Log log,
                                                                            string prodID)
        {
            ProductInfo Prod = null;

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"select a.CUSTSN, 
                                                                   a.Model, 
                                                                   a.MAC, 
                                                                   isnull(b.Value,'') as CNRS , 
                                                                   isnull(c.Value,'') as  OP,                                                                    
                                                                   a.MO
                                                             from Product a
                                                             left join ModelInfo b on a.Model =b.Model  and b.Name='CNRS'
                                                             left join ModelInfo c on a.Model =c.Model  and c.Name='PO'                                                               
                                                             where ProductID=@ProductID";
            SQLHelper.createInputSqlParameter(dbCmd, "@ProductID", 15, prodID);


            log.write(LogType.Info, 0, "SQL", "GetProductInfo", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@ProductID", prodID);
           

            SqlDataReader sdr = dbCmd.ExecuteReader();
            while (sdr.Read())
            {
                Prod = new ProductInfo();
                Prod.ProductID = prodID;
                Prod.CUSTSN = sdr.GetString(0).Trim();
                Prod.Model = sdr.GetString(1).Trim();
                Prod.MAC =sdr.GetString(2).Trim();
                Prod.IsCNRS = sdr.GetString(3).Trim();
                Prod.IsPO = sdr.GetString(4).Trim();              
                Prod.MOId=  sdr.GetString(5).Trim();
                Prod.PO = "";

            }
            sdr.Close();
            return Prod;
        }


        static public void WriteProductAttr(SqlConnection connect,
                                                                 Log log,
                                                                 string prodID,
                                                                 string name,
                                                                 string value,
                                                                 string editor,
                                                                 DateTime cdt)
        {
            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"if exists(select * from ProductAttr where ProductID =@ProductId and AttrName=@name)
                                                            begin
	                                                               update ProductAttr
	                                                               set   AttrValue =@value,
			                                                             Editor =@editor,
			                                                             Udt =@cdt 
	                                                               where ProductID =@ProductId and 
			                                                             AttrName=@name 
                                                            end
                                                            else  
                                                            begin
	                                                            insert ProductAttr (AttrName, ProductID, AttrValue, Editor, Cdt, Udt)
	                                                            values(@name,@ProductId,@value,@editor,@cdt,@cdt)
                                                            end";
            SQLHelper.createInputSqlParameter(dbCmd, "@ProductId", 15, prodID);
            SQLHelper.createInputSqlParameter(dbCmd, "@name", 32, name);
            SQLHelper.createInputSqlParameter(dbCmd, "@value", 255, value);
            SQLHelper.createInputSqlParameter(dbCmd, "@editor", 32, editor);
            SQLHelper.createInputSqlParameter(dbCmd, "@cdt", cdt);


            log.write(LogType.Info, 0, "SQL", "WriteProductAttr", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@ProductId", prodID);
            log.write(LogType.Info, 0, "SQL", "@name", name);
            log.write(LogType.Info, 0, "SQL", "@value", value);
            log.write(LogType.Info, 0, "SQL", "@editor", editor);
            log.write(LogType.Info, 0, "SQL", "@cdt", cdt.ToString());

            dbCmd.ExecuteNonQuery();
            
        }

        static public void WriteCDSIAST(SqlConnection connect,
                                                                 Log log,
                                                                 string prodID,
                                                                 string tp,
                                                                 string value)
        {
            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"merge CDSIAST as T
                                                        USING (select @productId as SnoId , @tp as Tp,  @value as Sno ) as S
                                                        on (S.SnoId = T.SnoId and S.Tp= T.Tp )
                                                        WHEN MATCHED THEN
                                                            UPDATE SET Sno = S.Sno
                                                        WHEN NOT MATCHED THEN
                                                            INSERT (SnoId, Tp, Sno)
                                                            VALUES (S.SnoId, S.Tp, S.Sno);";
            SQLHelper.createInputSqlParameter(dbCmd, "@productId", 15, prodID);
            SQLHelper.createInputSqlParameter(dbCmd, "@tp", 32, tp);
            SQLHelper.createInputSqlParameter(dbCmd, "@value", 255, value);
            

            log.write(LogType.Info, 0, "SQL", "WriteCDSIAST", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@productId", prodID);
            log.write(LogType.Info, 0, "SQL", "@tp", tp);
            log.write(LogType.Info, 0, "SQL", "@value", value);

            dbCmd.ExecuteNonQuery();

        }

        static public void DeleteCDSIAST(SqlConnection connect,
                                                                Log log,
                                                                string prodID)
        {
            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"delete  from CDSIAST where SnoId=@productId";

            SQLHelper.createInputSqlParameter(dbCmd, "@productId", 15, prodID);           


            log.write(LogType.Info, 0, "SQL", "DeleteCDSIAST", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@productId", prodID);            

            dbCmd.ExecuteNonQuery();

        }

        static public string  GetModelInfo(SqlConnection connect,
                                                                       Log log,
                                                                       string model,
                                                                       string name)
        {
            string ret = null;
            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = "select Value from ModelInfo where Model=@model and Name=@Name";
            SQLHelper.createInputSqlParameter(dbCmd, "@model", 15, model);
            SQLHelper.createInputSqlParameter(dbCmd, "@Name", 15, name);

            log.write(LogType.Info, 0, "SQL", "GetModelInfo", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@model", model);
            log.write(LogType.Info, 0, "SQL", "@Name", name);

            SqlDataReader sdr = dbCmd.ExecuteReader();
            while (sdr.Read())
            {
               
                ret = sdr.GetString(0).Trim();               

            }
            sdr.Close();
            return ret;
        }

        static public void ReadXMLCDSIAST( SqlConnection connect,
                                                                  Log log,
                                                                  string prodID,
                                                                   string xmlFilePath)
        {
            AssetTag tag = GetXMLAssetTag(xmlFilePath);
            DeleteCDSIAST(connect,log,prodID);
            WriteCDSIAST(connect, log, prodID, "DID", tag.DID);
            WriteCDSIAST(connect, log, prodID, "ASSET_TAG", tag.ASSET_TAG);
            WriteCDSIAST(connect, log, prodID, "HPOrder", tag.HPOrder);
            WriteCDSIAST(connect, log, prodID, "PurchaseOrder", tag.PurchaseOrder);
            WriteCDSIAST(connect, log, prodID, "FactoryPO", tag.FactoryPO);
            WriteProductAttr(connect, log, prodID, "CDSIState", "OK", "CDSI", DateTime.Now);
                                        

        }

        static public AssetTag GetXMLAssetTag(string xmlFilePath)
        {
            AssetTag assetTag =new AssetTag();
             XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);
             assetTag.DID=doc.SelectSingleNode("//INFO/DID") == null? "": doc.SelectSingleNode("//INFO/DID").InnerText;
            assetTag.HPOrder=doc.SelectSingleNode("//INFO/HPOrder") == null? "": doc.SelectSingleNode("//INFO/HPOrder").InnerText;
            assetTag.PurchaseOrder =doc.SelectSingleNode("//INFO/PurchaseOrder") == null? "": doc.SelectSingleNode("//INFO/PurchaseOrder").InnerText;
            assetTag.FactoryPO = doc.SelectSingleNode("//INFO/FactoryPO") == null? "": doc.SelectSingleNode("//INFO/FactoryPO").InnerText;
            assetTag.ASSET_TAG = doc.SelectSingleNode("//RESULT/DATA[@id='1']/VALUE") == null? "": doc.SelectSingleNode("//RESULT/DATA[@id='1']/VALUE").InnerText;
            return assetTag;
        }

        static public List<MO> GetAvailableMO(SqlConnection connect,
                                                                  Log log, 
                                                                  ProductInfo prodInfo, 
                                                                   int startDateoffset,
                                                                   int udtOffset )
        {
            List<MO> ret = new List<MO>();

            DateTime now = DateTime.Now;
            DateTime startDate = now.AddDays(startDateoffset);
            DateTime udtDate = now.AddDays(udtOffset);

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"select MO, (Qty-Print_Qty) as qty  from MO 
                                                          where Model=@model and 
                                                                     StartDate>=@StartDate and
                                                                     Qty>Print_Qty and
                                                                      Status ='H'  and  
                                                                     Udt>=@UpdateTime
                                                         order by StartDate, Udt";
            SQLHelper.createInputSqlParameter(dbCmd, "@model", 15, prodInfo.Model);
            SQLHelper.createInputSqlParameter(dbCmd, "@StartDate", 1, startDate);
            SQLHelper.createInputSqlParameter(dbCmd, "@UpdateTime", udtDate);


            log.write(LogType.Info, 0, "SQL", "GetAvailableMO", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@model", prodInfo.Model);
            log.write(LogType.Info, 0, "SQL", "@StartDate", startDate.ToString());
            log.write(LogType.Info, 0, "SQL", "@UpdateTime", udtDate.ToString());

            SqlDataReader sdr = dbCmd.ExecuteReader();
            while (sdr.Read())
            {
                MO mo = new MO();
                mo.MOId = sdr.GetString(0).Trim();
                mo.Qty = sdr.GetInt32(1);

                ret.Add(mo);
            }
            sdr.Close();
            return ret;
        }

        static public int GetAssignMOQty(SqlConnection connect,
                                                                  Log log,
                                                                  string moId)
        {
            int ret = 0;

             SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"select count(*)
                                                            from SnoDet_PoMo 
                                                          where Mo=@Mo ";
            SQLHelper.createInputSqlParameter(dbCmd, "@Mo", 32, moId);



            log.write(LogType.Info, 0, "SQL", "GetAssignMOQty", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@Mo", moId);
         


            SqlDataReader sdr = dbCmd.ExecuteReader();
            while (sdr.Read())
            {
                ret = sdr.GetInt32(0);            
            }
            sdr.Close();
            return ret;

        }


        static public List<Delivery> GetAvailableDelivery(SqlConnection connect,
                                                                                      Log log,
                                                                                       ProductInfo prodInfo,
                                                                                       int shipDateoffset)
        {
            List<Delivery> ret = new List<Delivery>();

            DateTime now = DateTime.Now;
            DateTime shipDate = new DateTime(now.Year, now.Month, now.Day);  //now.AddDays(shipDateoffset);
            DateTime shipDateEnd = shipDate.AddDays(shipDateoffset);

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
//            dbCmd.CommandText = @"select DeliveryNo, PoNo,Qty from Delivery 
//                                                          where Model=@model and 
//                                                                     ShipDate between @ShipDate and @ShipDateEnd and
//                                                                     Qty> 0 and
//                                                                     Status ='00'  
//                                                         order by ShipDate";
            dbCmd.CommandText = @"select a.DeliveryNo, a.PoNo, a.Qty, c.InfoValue as CustPo 
                                                          from Delivery a,
                                                                  SpecialOrder b,
                                                                   DeliveryInfo c   
                                                          where a.PoNo = b.FactoryPO and
                                                                     a.DeliveryNo=c.DeliveryNo and
                                                                      c.InfoType='CustPo'        and 
                                                                    b.Status in ('Created','Active') and
                                                                    a.Model=@model and 
                                                                   a.ShipDate between @ShipDate and @ShipDateEnd and
                                                                   a.Qty> 0 and
                                                                  a.Status ='00'  
                                                         order by ShipDate";
            SQLHelper.createInputSqlParameter(dbCmd, "@model", 15, prodInfo.Model);
            SQLHelper.createInputSqlParameter(dbCmd, "@ShipDate",  shipDate);
            SQLHelper.createInputSqlParameter(dbCmd, "@ShipDateEnd", shipDateEnd);
            

            log.write(LogType.Info, 0, "SQL", "GetAvailableDelivery", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@model", prodInfo.Model);
            log.write(LogType.Info, 0, "SQL", "@ShipDate", shipDate.ToString());
            log.write(LogType.Info, 0, "SQL", "@ShipDateEnd", shipDateEnd.ToString());

     

            SqlDataReader sdr = dbCmd.ExecuteReader();
            while (sdr.Read())
            {
                Delivery delivery =new Delivery();
                delivery.DeliveryNo =sdr.GetString(0).Trim();
                delivery.PO =sdr.GetString(1).Trim();
                delivery.Qty =sdr.GetInt32(2);
                delivery.CustPo = sdr.GetString(3).Trim();
                delivery.HpPo = delivery.PO;
                delivery.isCustPo = false;
                ret.Add(delivery);
            }
            sdr.Close();
            return ret;
        }

        static public List<Delivery> GetAvailableDeliveryByCustPo(SqlConnection connect,
                                                                                                Log log,
                                                                                                ProductInfo prodInfo,
                                                                                                 int shipDateoffset)
        {
            List<Delivery> ret = new List<Delivery>();

            DateTime now = DateTime.Now;
            DateTime shipDate = new DateTime(now.Year, now.Month, now.Day);  //now.AddDays(shipDateoffset);
            DateTime shipDateEnd = shipDate.AddDays(shipDateoffset);

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            //            dbCmd.CommandText = @"select DeliveryNo, PoNo,Qty from Delivery 
            //                                                          where Model=@model and 
            //                                                                     ShipDate between @ShipDate and @ShipDateEnd and
            //                                                                     Qty> 0 and
            //                                                                     Status ='00'  
            //                                                         order by ShipDate";
            dbCmd.CommandText = @"select a.DeliveryNo, b.FactoryPO, a.Qty , a.PoNo 
                                                          from Delivery a,
                                                               DeliveryInfo c,  
                                                                SpecialOrder b 
                                                          where a.DeliveryNo =c.DeliveryNo and
                                                                c.InfoType='CustPo'        and   
                                                                c.InfoValue= b.FactoryPO and
                                                                b.Status in ('Created','Active') and
                                                                a.Model=@model and 
                                                                a.ShipDate between @ShipDate and @ShipDateEnd and
                                                                   a.Qty> 0 and
                                                                  a.Status ='00'  
                                                         order by ShipDate";

            SQLHelper.createInputSqlParameter(dbCmd, "@model", 15, prodInfo.Model);
            SQLHelper.createInputSqlParameter(dbCmd, "@ShipDate", shipDate);
            SQLHelper.createInputSqlParameter(dbCmd, "@ShipDateEnd", shipDateEnd);


            log.write(LogType.Info, 0, "SQL", "GetAvailableDelivery", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@model", prodInfo.Model);
            log.write(LogType.Info, 0, "SQL", "@ShipDate", shipDate.ToString());
            log.write(LogType.Info, 0, "SQL", "@ShipDateEnd", shipDateEnd.ToString());



            SqlDataReader sdr = dbCmd.ExecuteReader();
            while (sdr.Read())
            {
                Delivery delivery = new Delivery();
                delivery.DeliveryNo = sdr.GetString(0).Trim();
                delivery.PO = sdr.GetString(1).Trim();
                delivery.Qty = sdr.GetInt32(2);
                delivery.CustPo = delivery.PO;
                delivery.HpPo = sdr.GetString(3).Trim();
                delivery.isCustPo = true;
                ret.Add(delivery);
            }
            sdr.Close();
            return ret;
        }

        static public int GetAssignPOQty(SqlConnection connect,
                                                                 Log log,
                                                                 string DeliveryNo,
                                                                 string PoNo)
        {
            int ret = 0;

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"select count(*)
                                                            from SnoDet_PoMo 
                                                          where PO=@PO or
                                                                     Delivery=@DeliveryNo";
            SQLHelper.createInputSqlParameter(dbCmd, "@PO", 32, PoNo);
            SQLHelper.createInputSqlParameter(dbCmd, "@DeliveryNo", 32, DeliveryNo);





            log.write(LogType.Info, 0, "SQL", "GetAssignPOQty", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@PO", PoNo);
            log.write(LogType.Info, 0, "SQL", "@DeliveryNo", DeliveryNo);


            SqlDataReader sdr = dbCmd.ExecuteReader();
            while (sdr.Read())
            {
                ret = sdr.GetInt32(0);
            }
            sdr.Close();
            return ret;

        }        

        static public void InsertCNRSPoMo(SqlConnection connect,
                                                                Log log,
                                                                CDSIPO poInfo)
        {
            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"insert dbo.SnoDet_PoMo(SnoId, Mo, PO, POItem, Delivery, PLT, BoxId, Remark, Editor, Cdt, Udt)
                                                            values(@SnoId, @Mo, @PO, '00001', @Delivery, '', '', '', 'CNRS', GETDATE(), GETDATE())";

            SQLHelper.createInputSqlParameter(dbCmd, "@SnoId", 15, poInfo.ProductID);
            SQLHelper.createInputSqlParameter(dbCmd, "@Mo", 32, poInfo.MOId);
            SQLHelper.createInputSqlParameter(dbCmd, "@PO", 32, poInfo.PO);
            SQLHelper.createInputSqlParameter(dbCmd, "@Delivery", 32, poInfo.DeliveryNo);


            log.write(LogType.Info, 0, "SQL", "InsertCNRSPoMo", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@SnoId", poInfo.ProductID);
            log.write(LogType.Info, 0, "SQL", "@Mo", poInfo.MOId);
            log.write(LogType.Info, 0, "SQL", "@PO", poInfo.PO);
            log.write(LogType.Info, 0, "SQL", "@Delivery", poInfo.DeliveryNo);

            dbCmd.ExecuteNonQuery();

        }



        static public bool  AssignMO(SqlConnection connect,
                                                       Log log,
                                                        AppConfig config,
                                                        ProductInfo prodInfo, 
                                                       CDSIPO poInfo)
        {
            bool selectedMo = false;

            List<MO> moList=  GetAvailableMO(connect,
                                                                           log,
                                                                           prodInfo,
                                                                            config.MOStartDateOffSetDay,
                                                                            config.MOUdtOffSetDay);
            if (moList.Count == 0) return selectedMo;
            
            int availableQty=0;
           
            foreach (MO mo in moList)
            {
                availableQty = mo.Qty- GetAssignMOQty(connect, log, mo.MOId);
                if (availableQty > 0)
                {
                   
                    poInfo.MOId = mo.MOId;
                    selectedMo = true;
                    break;
                }
            }

            return selectedMo;

        }

        static public bool AssignPO(SqlConnection connect,
                                                      Log log,
                                                      AppConfig config,
                                                      ProductInfo prodInfo, 
                                                      CDSIPO poInfo)
        {

            bool selectedPo = false;

            List<Delivery> poList = GetAvailableDelivery(connect,
                                                                                     log,
                                                                                     prodInfo,
                                                                                      config.ShipDateOffSetDay);
            if (poList.Count == 0)
            {
                poList = GetAvailableDeliveryByCustPo(connect,
                                                                                                     log,
                                                                                                     prodInfo,
                                                                                                      config.ShipDateOffSetDay);
            }

            if (poList.Count == 0)    return selectedPo;

            int availableQty = 0;

            foreach (Delivery po in poList)
            {
                availableQty = po.Qty - GetAssignPOQty(connect, log, po.DeliveryNo, po.PO);
                if (availableQty > 0)
                {                    
                    poInfo.PO= po.PO;
                    poInfo.DeliveryNo = po.DeliveryNo;
                    poInfo.DeliveryQty = po.Qty;
                    poInfo.RemainQty = availableQty;
                    poInfo.CustPo = po.CustPo;
                    poInfo.HpPo = po.HpPo;
                    poInfo.isCustPo = po.isCustPo;
                    selectedPo = true;
                    break;
                }
            }

            return selectedPo;
        }

        static public SpecialOrder GetSpecialOrder(SqlConnection connect,
                                                                 Log log,                                                                
                                                                 string  factoryPO)
        {

            SpecialOrder ret = null;

            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"select  Category, AssetTag, Qty, Status, Remark
                                                            from SpecialOrder
                                                            where  FactoryPO=@FactoryPO";
            SQLHelper.createInputSqlParameter(dbCmd, "@FactoryPO", 32, factoryPO);





            log.write(LogType.Info, 0, "SQL", "GetSpecialOrder", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@FactoryPO", factoryPO);
           


            SqlDataReader sdr = dbCmd.ExecuteReader();
            if (sdr!=null  && sdr.Read())
            {
                ret = new SpecialOrder()
                {
                    FactoryPO = factoryPO,
                    Category = sdr.GetString(0).Trim(),
                    AssetTag = sdr.GetString(1).Trim(),
                    Qty = sdr.GetInt32(2),
                    Status = sdr.GetString(3).Trim(),
                    Remark = sdr.GetString(4)
                };
               
            }
            sdr.Close();

            return ret;
        }


        static public void UpdateSpecialOrderStatus(SqlConnection connect,
                                                                                Log log,                                                                               
                                                                                string factoryPO,
                                                                                string status)
        {

            
            SqlCommand dbCmd = connect.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = @"update SpecialOrder
                                                           set Status=@Status,
                                                               Udt=GETDATE()
                                                         where  FactoryPO=@FactoryPO";
            SQLHelper.createInputSqlParameter(dbCmd, "@FactoryPO", 32, factoryPO);
            SQLHelper.createInputSqlParameter(dbCmd, "@Status", 16, status);

            log.write(LogType.Info, 0, "SQL", "UpdateSpecialOrderStatus", dbCmd.CommandText);
            log.write(LogType.Info, 0, "SQL", "@FactoryPO", factoryPO);
            log.write(LogType.Info, 0, "SQL", "@Status", status);

            dbCmd.ExecuteNonQuery();            
           
        }
        

    }

    public class ProductInfo
    {
        public string ProductID;
        public string Model;
        public string CUSTSN;
        public string MAC;
        public string IsCNRS;
        public string IsPO;
        public string MOId;
        public string PO;
    }

    public class CDSIPO{
        public string ProductID;
        public string MOId;
        public string PO;
       
        public string DeliveryNo;
        public int DeliveryQty;
        public int RemainQty;
        //public int DemainQty;
        public string CustPo;
        public string HpPo;
        public bool isCustPo = false;
       
    }

    public class SpecialOrder 
    {
        public string FactoryPO;
        public string Category;
        public string AssetTag;
        public int Qty;
        public string Status;
        public string Remark;   
       

    }

    public class Delivery
    {
        public string DeliveryNo;
        public string PO;
        public int Qty;
        public string CustPo;
        public string HpPo;
        public bool isCustPo = false;
    }

     public class MO
    {
        public string MOId;
        public int Qty;
    }

    public class AssetTag
    {
        public string DID;
        public string HPOrder;
        public string PurchaseOrder;
        public string FactoryPO;
        public string ASSET_TAG;      
    }


   
}
