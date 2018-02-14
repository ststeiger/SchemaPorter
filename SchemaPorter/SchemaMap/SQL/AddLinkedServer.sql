
-- EXEC master.dbo.sp_dropserver @server=N'CORDB2012', @droplogins='droplogins'

IF NOT EXISTS (SELECT srv.name FROM sys.servers srv WHERE srv.server_id != 0 AND srv.name = N'RemoteDB')
BEGIN 
	EXEC master.dbo.sp_addlinkedserver 
		@server = N'RemoteDB'
	   ,@srvproduct = 'OLE DB Provider for SQL'
	   ,@provider = N'SQLNCLI'
	   ,@datasrc = 'THE_SERVER'
	   ,@catalog = 'DB_TO_COMPARE'
	;

	EXEC master.dbo.sp_addlinkedsrvlogin 
		@rmtsrvname = N'RemoteDB'
	   ,@useself = false
	   --,@locallogin = 'LocalIntegrationUser'
	   ,@locallogin = NULL 
	   ,@rmtuser = N'ApertureWebServicesDE'
	   ,@rmtpassword = N'meridian08'
	;

END 
