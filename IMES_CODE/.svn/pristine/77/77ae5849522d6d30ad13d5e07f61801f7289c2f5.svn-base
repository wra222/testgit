CREATE PROCEDURE [dbo].[IMES_Rebuild_HPIMES_Index] as 
set nocount on 

declare @tableList TbStringList

Insert @tableList(data)
Values('ModelBOM'),('Model'),('ModelInfo'),('Part'),('PartInfo'),('PartType'),
      ('PCB'),('PCBInfo'),('PCBStatus'),('PCBStatusEx'),('PCBRepair'),
      ('Product'),('Product_Part'),('ProductInfo'),('ProductStatus'),('ProductStatusEx'),
      ('Pizza_Part'),('PizzaStatus'),('ProductAttr'),('Delivery'),('DeliveryInfo'),
      ('Delivery_Pallet'),('Pallet'),('ShipBoxDet'),
      ('COAStatus'),('Carton'),('CartonStatus'),('Carton_Product'),
      ('Delivery_Carton'),('CartonInfo'),
      ('ProductPlan'),('MO')


execute IMES_Rebuild_Index @tableList,5,20,4

GO


CREATE PROCEDURE [dbo].[IMES_Rebuild_HPIMESLog_Index] as 
set nocount on 
declare @tableList TbStringList

Insert @tableList(data)
Values('PCBLog'),('PCBTestLog'),('ProductTestLog'),('ProductLog'),
      ('PrintLog'),('RePrintLog'),('QCStatus'),('COALog')
      

execute IMES_Rebuild_Index @tableList,5,20,4


GO
