USE [igroup186_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_readCourses]    Script Date: 8/11/2024 3:06:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Author:        <Cgroup86>
-- Create date: <13/06/2024>
-- Description:	<Get all the courses from Courses table>
-- =============================================
CREATE PROCEDURE SP_ReadBooks
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	select B.BookId, B.BookTitle, B.price, B.[description], B.previewLink, B.isEbook, B.webReaderLink, BA.AuthorName, B.isActive, B.thumbnailUrl, , 
		from Books B inner join BookAuthors BA on B.BookId = BA.BookId;
END
GO



