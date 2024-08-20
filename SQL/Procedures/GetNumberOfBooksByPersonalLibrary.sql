-- =============================================
-- Author:		<Cgroup86>
-- Create date: <20/08/2024>
-- Description:	<get the number books by personal libary >
-- =============================================
CREATE PROCEDURE [dbo].[GetNumberOfBooksByPersonalLibrary]
    @bookId INT  -- Declare the parameter with an appropriate data type
AS
BEGIN
    -- Check if the bookId exists in the Books table
    IF EXISTS (SELECT 1 FROM Books WHERE BookId = @bookId)
    BEGIN
        -- If the bookId exists, proceed with the rest of the code
        SELECT COUNT(DISTINCT pl.BookId) AS primaryKeyCount
        FROM PersonalLibrary pl
        WHERE pl.BookId = @bookId
    END
    ELSE
    BEGIN
        -- If the bookId does not exist, return a count of 0 or handle as needed
        SELECT 0 AS primaryKeyCount;
    END
END
GO
