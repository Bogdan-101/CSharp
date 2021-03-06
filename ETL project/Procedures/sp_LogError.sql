USE [NORTHWND]
GO
/****** Object:  StoredProcedure [dbo].[sp_LogError]    Script Date: 16.12.2020 0:28:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_LogError]
    @exception NVARCHAR(50),
    @message NVARCHAR(max),
    @time datetime2(7)
AS
    INSERT INTO dbo.Logs(Exception, Message, Time) 
	VALUES(@exception, @message, @time)

	SELECT SCOPE_IDENTITY()
