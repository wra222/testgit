

CREATE PROCEDURE [dbo].[sp_RebuildingIndex] as 
set nocount on 

declare @tableList TbStringList

Insert @tableList(data)
Values('ModelBOM'),('Model'),('ModelInfo'),('Part'),('PartInfo'),('PartType'),
      ('PCB'),('PCBInfo'),('PCBStatus'),('PCBStatusEx'),('PCBRepair'),
      ('Product'),('Product_Part'),('ProductInfo'),('ProductStatus'),('ProductStatusEx'),
      ('Pizza_Part'),('PizzaStatus'),('ProductAttr'),('Delivery'),('DeliveryInfo'),
      ('Delivery_Pallet'),('Pallet'),('ShipBoxDet'),
      ('COAStatus')

execute IMES_Rebuild_Index @tableList,5,20,4





GO


