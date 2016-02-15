USE [HPIMES]
GO

/****** Object:  StoredProcedure [dbo].[rpt_ITCNDTS_SET_WatchFolder]    Script Date: 07/15/2013 22:56:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/****** 物件:  預存程序 dbo.rpt_ITCNDTS_SET_IMAGEDOWN_14    指令碼日期: 2004/7/1 上午 10:11:41 ******/
-- input parameter:@Attributes TbAttribute readonly
-- Output Parameter:@ErrorTexte varchar(255)
 

CREATE PROCEDURE [dbo].[rpt_ITCNDTS_SET_WatchFolder]  
	@Attributes TbAttribute readonly,
	@ErrorText  varchar(255) output
AS
    
	set nocount on;
	
	declare @ProductID varchar(32)
	declare @Now datetime = getdate()
	select @ProductID= ProductID
	  from Product with (nolock)
	  where CUSTSN in (select Value from @Attributes where Name='SN')
	if @ProductID!=''
	begin
	     MERGE ProductAttr AS target
         USING (SELECT @ProductID, Name, Value
                from  @Attributes 
                where Name !='SN') AS source (ProductID, Name, Value)
           ON (target.ProductID = source.ProductID and
               target.AttrName = source.Name)
		WHEN MATCHED THEN 
			UPDATE SET AttrValue = source.Value,
					   Udt = @Now 
		WHEN NOT MATCHED THEN	
			INSERT (AttrName, ProductID, AttrValue, Editor, Cdt, Udt)
			VALUES (source.Name,@ProductID, source.Value,'WatchFolder' ,@Now,@Now);   
	      
		set @ErrorText= 'Pass';
		return;
	end
	else
	begin
	     set @ErrorText= 'Not Exists CustomSN!!';
	     return;	      
	end
	
	set @ErrorText= 'Fail';
	


GO


