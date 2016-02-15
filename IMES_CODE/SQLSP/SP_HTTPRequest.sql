Create PROCEDURE  [dbo].[SP_HTTPRequest]        
	@URI varchar(2000) = '',             
	@methodName varchar(50) = '',        
	@requestBody varchar(8000) = '',        
	@SoapAction varchar(255),        
	@UserName nvarchar(100), -- Domain\UserName or UserName        
	@Password nvarchar(100),        
	@responseText varchar(8000) output 
as 
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
	-- open the destination URI with Specified method  
	EXEC @hResult = sp_OAMethod @objectID, 'open', null, 
								@methodName, @URI, 'false', 
								@UserName, @Password 
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
	EXEC @hResult = sp_OAMethod @objectID, 'setRequestHeader', null, 'SOAPAction', @SoapAction  
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
	/*  -- if you have headers in a table called RequestHeader you can go through them with this  
	DECLARE @HeaderKey varchar(500), @HeaderValue varchar(500)  
	DECLARE RequestHeader CURSOR LOCAL FAST_FORWARD  FOR       
		SELECT      HeaderKey, HeaderValue        
		FROM RequestHeaders        
		WHERE       Method = @methodName  
		OPEN RequestHeader  FETCH NEXT FROM RequestHeader  
		INTO @HeaderKey, @HeaderValue  
		WHILE @@FETCH_STATUS = 0
		  
		BEGIN       --select @HeaderKey, @HeaderValue, @methodName        
			EXEC @hResult = sp_OAMethod @objectID, 'setRequestHeader', null, @HeaderKey, @HeaderValue        
			IF @hResult <> 0        
			BEGIN             
				EXEC sp_OAGetErrorInfo @objectID, @source OUT, @desc OUT             
				SELECT  hResult = convert(varbinary(4), @hResult),                    
						source = @source,                    
						description = @desc,                    
						FailPoint = 'SetRequestHeader failed',                    
						MedthodName = @methodName              
				goto destroy              
				return       
			END       
			FETCH NEXT FROM RequestHeader        
			INTO @HeaderKey, @HeaderValue  
		END 
		CLOSE RequestHeader  
		DEALLOCATE RequestHeader  */  
		-- send the request  
		EXEC @hResult = sp_OAMethod @objectID, 'send', null, @requestBody  
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
		declare @statusText varchar(1000), @status varchar(1000)  
		-- Get status text  
		exec sp_OAGetProperty @objectID, 'StatusText', @statusText out 
		exec sp_OAGetProperty @objectID, 'Status', @status out 
		select @status, @statusText, @methodName  
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