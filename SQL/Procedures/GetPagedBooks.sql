-- =============================================
-- Author: <>
-- Create date: <17/08/24>
-- Description:	<get pages>
-- =============================================
CREATE PROCEDURE SP_GetPagedBooks
    @IsEbook BIT,
    @PageNumber INT,
    @PageSize INT,
    @FetchTotalCount BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    -- Ensure PageNumber and PageSize are positive
    IF @PageNumber < 1 OR @PageSize < 1
    BEGIN
        RAISERROR('PageNumber and PageSize must be greater than 0.', 16, 1);
        RETURN;
    END

    -- Optional: Calculate the total number of records
    DECLARE @TotalRecords INT = NULL;
    IF @FetchTotalCount = 1
    BEGIN
        SELECT @TotalRecords = COUNT(*)
        FROM Books B
        WHERE B.IsEbook = @IsEbook;
    END

    -- Perform the pagination query
    SELECT *
    FROM (
        SELECT B.BookId, B.BookTitle, B.price, B.smallThumbnailUrl, B.previewLink, B.isEbook, B.webReaderLink, B.isAvailable,
            ROW_NUMBER() OVER (ORDER BY B.BookId) AS RowNumber
        FROM Books B
        WHERE B.IsEbook = @IsEbook and B.isActive = 1
    ) AS Result
    WHERE RowNumber BETWEEN ((@PageNumber - 1) * @PageSize) + 1 AND @PageNumber * @PageSize;

	-- Return the total number of records
    SELECT @TotalRecords AS TotalRecords;
END