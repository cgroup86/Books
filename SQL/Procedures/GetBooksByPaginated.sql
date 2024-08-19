-- =============================================
-- Author:        <Cgroup86>
-- Create date: <19/08/2024>
-- Description:    <get 10 Authors per Page >
-- =============================================
CREATE PROCEDURE GetBooksByPaginated
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize;

    SELECT *
    FROM Books
    WHERE BookId IS NOT NULL AND BookId <> ''
    ORDER BY BookId
    OFFSET @StartRow ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO