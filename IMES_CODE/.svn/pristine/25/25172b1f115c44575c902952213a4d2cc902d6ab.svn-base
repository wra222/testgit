
CREATE FUNCTION [dbo].[fn_UnBindDNQtyByCarton](
	@DeliveryNo        varchar(32),
	@FullDNQty         int,
	@FullCartonQty     int 	
)
RETURNS TABLE
AS RETURN
     select RemainQty = @FullDNQty - ISNull(SUM(a.Qty),0),
           CartonQty = case when (@FullDNQty - ISNull(SUM(a.Qty),0)) < @FullCartonQty then 
								  (@FullDNQty - ISNull(SUM(a.Qty),0))
                            else
                                 @FullCartonQty
                       end             
    from Delivery_Carton a 
	inner join Carton b on a.CartonSN= b.CartonSN 
    where a.DeliveryNo = @DeliveryNo and
          b.Status in ('Reserve','Full','Partial')
    
GO

Create FUNCTION [dbo].[fn_GetPalletCartonQty](
	@DeliveryNo  varchar(32)
)
RETURNS TABLE
AS RETURN
   select TotalCartonQty = isnull(SUM(DeliveryQty),0)                                             
     from Delivery_Pallet
     where DeliveryNo= @DeliveryNo  
GO    


CREATE FUNCTION [dbo].[fn_GetDNQty](
	@PalletNo        varchar(32)
)
RETURNS TABLE
AS RETURN
    select DNQty=COUNT(distinct d.DeliveryNo), 
          TotalDeliveryQty=isnull(SUM(d.DeliveryQty),0) 
     from Delivery_Pallet d
	where  d.PalletNo= @PalletNo
	
GO

Create FUNCTION [dbo].[fn_GetCartonQty](
	@PalletNo        varchar(32)
)
RETURNS TABLE
AS RETURN
    select CartonQty=COUNT(1)
	 from Carton c
	where c.PalletNo = @PalletNo and
	      c.Status in ('Reserve','Full','Partial') 
	     
GO	
    


            
    
   
   











GO


