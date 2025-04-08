USE [StarryEdenUser]
GO

EXEC [app].[spGetUsers]
EXEC [app].[spGetUser] 1
EXEC [app].[spGetRoles]
EXEC [app].[spGetRole] 1000
EXEC [app].[spAddUser] 'Alexandra', 'salt', 'hash', 9999, 'AHIETALA'
EXEC [app].[spGetAuth] 2
EXEC [app].[spUpdateUser] 2, 'Alexandra', 'salt', 'hash', 1000, 'AHIETALA' 
EXEC [app].[spRemoveUser] 2, 'AHIETALA'
EXEC [app].[spGetUserHistory] 2

