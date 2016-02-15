Create PROCEDURE  [dbo].[IMES_HTTPRequest]        
	@URI varchar(2000) = '',             
	@methodName varchar(50) = 'GET',
	@requestBody VarChar(MAX)='',
	@responseText varchar(8000) output 
AS 
	SET NOCOUNT ON 
	IF    @methodName = '' 
	BEGIN       
		select FailPoint = 'Method Name must be set'       
		return 
	END 
	set   @responseText = 'FAILED' 
	DECLARE @objectID int 
	DECLARE @hResult int 
	DECLARE @source varchar(255), @desc varchar(255)  
	EXEC @hResult = sp_OACreate 'MSXML2.ServerXMLHTTP', @objectID OUT 
	IF @hResult <> 0  
	BEGIN       
		EXEC sp_OAGetErrorInfo @objectID, @source OUT, @desc OUT       
		SELECT hResult = convert(varbinary(4), @hResult),                    
				source = @source,                    
				description = @desc,                    
				FailPoint = 'Create failed',                    
				MedthodName = @methodName        
		goto destroy        
		return 
	END 
	print 'Create Http'
	-- open the destination URI with Specified method  
	EXEC @hResult = sp_OAMethod @objectID, 'open', null, 
								@methodName, @URI, 'false',@requestBody
	IF @hResult <> 0  
	BEGIN       
		EXEC sp_OAGetErrorInfo @objectID, @source OUT, @desc OUT       
		SELECT hResult = convert(varbinary(4), @hResult),              
			   source = @source,              
			   description = @desc,              
			   FailPoint = 'Open failed',              
			   MedthodName = @methodName        
		goto destroy        
		return 
	END
	print 'Open Http'
	-- send the request
	IF @requestBody=''
		EXEC @hResult = sp_OAMethod @objectID, 'send'
	ELSE  --SOAP Request
	BEGIN
		-- set request headers  
		EXEC @hResult = sp_OAMethod @objectID, 'setRequestHeader', null, 'Content-Type', 'text/xml;charset=UTF-8' 
		IF @hResult <> 0  
		BEGIN       
			EXEC sp_OAGetErrorInfo @objectID, @source OUT, @desc OUT       
			SELECT hResult = convert(varbinary(4), @hResult),              
					source = @source,              
					description = @desc,              
					FailPoint = 'SetRequestHeader failed',              
					MedthodName = @methodName        
			goto destroy        
			return 
		END 
		-- set soap action  
		EXEC @hResult = sp_OAMethod @objectID, 'setRequestHeader', null, 'SOAPAction', 'POST'  
		IF @hResult <> 0  
		BEGIN       
			EXEC sp_OAGetErrorInfo @objectID, @source OUT, @desc OUT       
			SELECT hResult = convert(varbinary(4), @hResult),              
				   source = @source,              
				   description = @desc,              
				   FailPoint = 'SetRequestHeader failed',              
				   MedthodName = @methodName        
			goto destroy        
			return 
		END 
		declare @len int 
		set @len = len(@requestBody)  
		EXEC @hResult = sp_OAMethod @objectID, 'setRequestHeader', null, 'Content-Length', @len  
		IF @hResult <> 0  
		BEGIN       
			EXEC sp_OAGetErrorInfo @objectID, @source OUT, @desc OUT       
			SELECT hResult = convert(varbinary(4), @hResult),              
					source = @source,              
					description = @desc,              
					FailPoint = 'SetRequestHeader failed',              
					MedthodName = @methodName        
			goto destroy        
			return 
		END  
		EXEC @hResult = sp_OAMethod @objectID, 'send',NULL,@requestBody 
	END
		  
	IF    @hResult <> 0  
	BEGIN       
		EXEC sp_OAGetErrorInfo @objectID, @source OUT, @desc OUT       
		SELECT hResult = convert(varbinary(4), @hResult),              
				source = @source,              
				description = @desc,              
				FailPoint = 'Send failed',              
				MedthodName = @methodName        
		goto destroy        
		return 
	END 
	-- Get response text  
	exec sp_OAGetProperty @objectID, 'responseText', @responseText out 
	IF @hResult <> 0  
	BEGIN       
		EXEC sp_OAGetErrorInfo @objectID, @source OUT, @desc OUT       
		SELECT  hResult = convert(varbinary(4), @hResult),              
				source = @source,              
				description = @desc,              
				FailPoint = 'ResponseText failed',              
				MedthodName = @methodName        
		goto destroy        
		return 
	END 
destroy:        
	exec sp_OADestroy @objectID  
	SET NOCOUNT OFF   


GO
