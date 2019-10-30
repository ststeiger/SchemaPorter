
SELECT * FROM master.dbo.sysprocesses




IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fu_SCHEMA_GetDataTypeName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
EXECUTE(N'
CREATE FUNCTION [dbo].[fu_SCHEMA_GetDataTypeName]( )
RETURNS nvarchar(MAX)
AS BEGIN RETURN NULL; END');
END


GO 



-- =============================================
-- Author:		Stefan Steiger
-- Create date: 30.10.2019
-- Description:	Funktion fu_SCHEMA_GetDataTypeName
--              
-- =============================================
ALTER FUNCTION [dbo].[fu_SCHEMA_GetDataTypeName]
(
	 @in_engine_type nvarchar(256) -- ms/pg/my/fb/sl/ora/...
	,@in_data_type nvarchar(256)
	,@in_max_length int 
	,@in_precision int 
	,@in_scale int 
	,@in_datetime_precision int 
)
	RETURNS nvarchar(MAX) 
AS
BEGIN
	DECLARE @out_pfad nvarchar(MAX); 
		
		
	-- Return the result of the function
	RETURN @out_pfad; 
END


GO

