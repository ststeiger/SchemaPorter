CREATE TABLE [dbo].[T_DMS_Configuration]
(
	[DMSCONF_UID] [uniqueidentifier] NOT NULL,
	[DMSCONF_Key] [nvarchar](255) NOT NULL,
	[DMSCONF_Value] [nvarchar](4000) NULL,
	[DMSCONF_Type] [nvarchar](1000) NOT NULL,
	[DMSCONF_Remark] [nvarchar](4000) NULL,
	[DMSCONF_LastModified] [datetime] NOT NULL,
	[DMSCONF_Status] [int] NOT NULL
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[T_DMS_Configuration] ADD  CONSTRAINT [uc_DMSCONF_Key] UNIQUE NONCLUSTERED 
(
	[DMSCONF_Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-----------------



CREATE UNIQUE NONCLUSTERED INDEX 
	[IX_UC_T_SYS_Ref_Layerset__LY_Status_LY_Stylizer_Must_Be_Unique_for_T_SYS_Ref_Layerset_Felder] 
ON [dbo].[T_SYS_Ref_Layerset]
(
	[LY_Stylizer] ASC
)
WHERE ([T_SYS_Ref_Layerset].[LY_Stylizer] IS NOT NULL 
AND [T_SYS_Ref_Layerset].[LY_Stylizer]<>'' 
AND [T_SYS_Ref_Layerset].[LY_Status]=(1) 
AND ([LY_Legende_View] IS NOT NULL 
AND [LY_Legende_View]<>''))

WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
