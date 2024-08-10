-- =============================================
-- Author:		<>
-- Create date: <10/08/2024>
-- Description:	<Insert book category>
-- =============================================
CREATE PROCEDURE SP_InsertToBookCategory
	@category nvarchar(150),
	@bookId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    INSERT INTO BookCategoryB(BookId, CategoryNameB)
        VALUES (@bookId, @category);    
END        
GO