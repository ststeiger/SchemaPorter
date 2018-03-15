
CREATE TABLE [dbo].[_LoLz](
	[LOL_Name] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_Vorname] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_User] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_Status] [int] NULL,
	[_LOL_Label]  AS (case when [LOL_Name] like [LOL_Vorname] then [LOL_Name] 
when [LOL_Vorname] like '' then [LOL_Name] else isnull(upper(left([LOL_Vorname],(1)))+'. ','')+[LOL_Name] end) PERSISTED
);







CREATE TABLE [dbo].[_LoLz](
	[LOL_Name] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_Vorname] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_User] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_Status] [int] NULL,
	[_LOL_Label]  AS (case when [LOL_Name] like [LOL_Vorname] then [LOL_Name] 
when [LOL_Vorname] like '' then [LOL_Name] else isnull(upper(left([LOL_Vorname],(1)))+'. ','')+[LOL_Name] end) PERSISTED
);


CREATE TABLE [dbo].[_LoLz1](
	[LOL_Name] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_Vorname] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_User] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LOL_Status] [int] NULL,
	[_LOL_Label1]  AS (dbo.fn_PadLeft([LOL_Name], 25, 'a')) PERSISTED
);






DROP FUNCTION IF EXISTS [dbo].[fn_PadLeft]
GO




-- =========================================
-- Author:		Stefan Steiger
-- ALTER date:	10.08.2011
-- Description:	PadLeft
-- =========================================
CREATE FUNCTION [dbo].[fn_PadLeft]
(
	@str varchar(50),
	@digits integer,
	@pad char(1) 
)
RETURNS varchar(50)
WITH SCHEMABINDING
AS
BEGIN
	DECLARE 
	@result varchar(50),
	@dif integer,
	@i integer,
	@s varchar(50)
	
	IF LEN(@str) < @digits
		SET @dif = @digits - LEN(@str)
	ELSE
		SET @dif = 0
	
	SET @s = ''
	
	SET @i = 0
	WHILE @i < @dif
	BEGIN
		SET @s = @s + @pad 	
		SET @i = @i + 1
	END
	
	SET @result = @s + @str
	
	RETURN @result
END




GO




