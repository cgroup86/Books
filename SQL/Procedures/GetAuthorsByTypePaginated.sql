-- =============================================
-- Author:        <Cgroup86>
-- Create date: <12/08/2024>
-- Description:    <get 10 Authors per Page >
-- =============================================
CREATE PROCEDURE GetAuthorsByTypePaginated
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize;

    SELECT *
    FROM Authors
    WHERE AuthorName IS NOT NULL AND AuthorName <> ''
    ORDER BY AuthorName
    OFFSET @StartRow ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO