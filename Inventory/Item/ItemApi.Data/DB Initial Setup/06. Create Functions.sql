-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [SEInventory]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE FUNCTION [app].[fnSplit_OnPipe]
(
   @list varchar(max)
)
RETURNS TABLE
AS

   RETURN 
   (
	   SELECT [SplitData] = CONVERT(INT, [SplitData])
       FROM
       (
           SELECT [SplitData] = [x].[i].value('(./text())[1]', 'INT')
           FROM
           (
               SELECT [compiled] = CONVERT(XML, '<i>' + REPLACE(@list, '|', '</i><i>') + '</i>').query('.')
           ) AS [xml]
           CROSS APPLY
           [compiled].nodes('i') AS [x]([i])
       ) AS [split]
       WHERE [SplitData] IS NOT NULL
   );

   GO

 -----------------------------------------------------------

CREATE FUNCTION [app].[fnSplit_KeyValuePairs]
(
   @list varchar(max)
)
RETURNS TABLE
AS

   RETURN 
   (
       SELECT [Key], [Value]
       FROM
       (
           SELECT [Key] = [y].[k].value('.', 'VARCHAR(max)'), [Value] = [z].[v].value('.', 'VARCHAR(max)')
           FROM
           (
               SELECT [compiled] = CONVERT(XML, REPLACE('<items><i><k>' + REPLACE(@list, '||', '</v></i><i><k>') + '</v></i></items>', '::', '</k><v>')).query('.')
           ) AS [xml]
           CROSS APPLY
            [compiled].nodes('items/i') AS [items]([i])
		   CROSS APPLY
           [items].[i].nodes('k') AS [y]([k])
		   CROSS APPLY
           [items].[i].nodes('v') AS [z]([v])
       ) AS [split]
       WHERE [Key] IS NOT NULL
   );

GO

 -----------------------------------------------------------
-- Can't remember what these do..
--CREATE FUNCTION [app].[fnSplit_IDNote]
--(
--   @list varchar(max)
--)
--RETURNS TABLE
--AS

--   RETURN 
--   (
--       SELECT [ID], [Note]
--       FROM
--       (
--           SELECT [ID] = [y].[id].value('.', 'VARCHAR(max)'), [Note] = [z].[note].value('.', 'VARCHAR(max)')
--           FROM
--           (
--               SELECT [compiled] = CONVERT(XML, REPLACE('<items><i><id>' + REPLACE(@list, '||', '</note></i><i><id>') + '</note></i></items>', '::', '</id><note>')).query('.')
--           ) AS [xml]
--           CROSS APPLY
--           [compiled].nodes('items/i') AS [items]([i])
--		   CROSS APPLY
--           [items].[i].nodes('id') AS [y]([id])
--		   CROSS APPLY
--           [items].[i].nodes('note') AS [z]([note])
--       ) AS [split]
--       WHERE [ID] IS NOT NULL
--   );

--GO

 -----------------------------------------------------------

--CREATE FUNCTION [app].[fnSplit_IDIDNote]
--(
--   @list varchar(max)
--)
--RETURNS TABLE
--AS

--   RETURN 
--   (
--       SELECT [ID1], [ID2], [Note]
--       FROM
--       (
--           SELECT [ID1] = [x].[id1].value('.', 'VARCHAR(max)'), [ID2] = [y].[id2].value('.', 'VARCHAR(max)'), [Note] = [z].[note].value('.', 'VARCHAR(max)')
--           FROM
--           (
--               SELECT [compiled] = CONVERT(XML, REPLACE('<items><i><id1>' + REPLACE(REPLACE(@list,'\\','</id1><id2>'), '||', '</note></i><i><id1>') + '</note></i></items>', '::', '</id2><note>')).query('.')
--           ) AS [xml]
--           CROSS APPLY
--           [compiled].nodes('items/i') AS [items]([i])
--		   CROSS APPLY
--           [items].[i].nodes('id1') AS [x]([id1])
--		   CROSS APPLY
--		   [items].[i].nodes('id2') AS [y]([id2])
--		   CROSS APPLY
--           [items].[i].nodes('note') AS [z]([note])
--       ) AS [split]
--       WHERE [ID1] IS NOT NULL
--   );

--GO

 -----------------------------------------------------------