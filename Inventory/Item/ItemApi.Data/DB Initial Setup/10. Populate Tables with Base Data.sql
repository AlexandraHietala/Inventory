-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [SEInventory]

GO

-- [app].[Items]
INSERT INTO [app].[Items] ([STATUS],[TYPE],[BRAND],[SERIES],[NAME],[DESCRIPTION],[FORMAT],[SIZE],[YEAR],[PHOTO],[CREATED_BY],[LAST_MODIFIED_BY]) VALUES ('OWNED','BLIND','Tokidoki','Unicornos','Prima Donna','The OG','FIGURE','REGULAR',NULL,NULL,'AHIETALA','AHIETALA')

--[app].[ItemComments]
INSERT INTO [app].[ItemComments] ([ITEM_ID],[COMMENT],[CREATED_BY],[LAST_MODIFIED_BY]) VALUES (1,'This is the first chase Unicorno ever! She is very rare and valuable.','AHIETALA','AHIETALA')

