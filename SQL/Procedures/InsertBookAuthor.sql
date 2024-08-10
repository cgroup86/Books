-- =============================================
-- Author:		<>
-- Create date: <10/08/2024>
-- Description:	<Insert book author>
-- =============================================
CREATE PROCEDURE SP_InsertBookAuthor
	@authorName nvarchar(200),
	@bookId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    INSERT INTO BookAuthors(BookId, AuthorName)
        VALUES (@bookId, @authorName);    
END        
GO