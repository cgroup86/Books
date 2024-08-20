-- =============================================
-- Author:<Cgroup86>
-- Create date: <20/08/2024>
-- Description:<get the number books by personal libary >
-- =============================================
ALTER PROCEDURE GetNumberOfBooksByPersonalLibary
-- Add the parameters for the stored procedure here
@bookId
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
--SET NOCOUNT ON;

    -- Insert statements for procedure here
   IF EXISTS (SELECT 1 FROM Books WHERE bookId = @bookId)
    BEGIN
SELECT COUNT(DISTINCT pl.BookId) AS primaryKeyCount
FROM PersonalLibrary pl
JOIN Books b on pl.BookId = b.BookId
    END
END
GO
EXEC GetNumberOfBooksByPersonalLibary 