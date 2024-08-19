USE [igroup186_test2]
GO

/****** Object:  StoredProcedure [dbo].[GetBooksPaginated]    Script Date: 19/08/2024 21:13:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Cgroup86>
-- Create date: <19/08/2024>
-- Description:	<get 10 books per Page >
-- =============================================

ALTER PROCEDURE [dbo].[GetBooksPaginated]
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Calculate the start and end row numbers for the desired page
    DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize + 1;
    DECLARE @EndRow INT = @PageNumber * @PageSize;

    -- Calculate the total number of rows in the Books table
    DECLARE @TotalRows INT;
    SELECT @TotalRows = COUNT(*)
    FROM Books
    WHERE BookId IS NOT NULL AND BookId <> '';

    -- Calculate the total number of pages
    DECLARE @TotalPages INT;
    SET @TotalPages = CEILING((1.0 * @TotalRows) / @PageSize);

    -- Use ROW_NUMBER() to assign a row number to each record and filter based on the calculated row numbers
    WITH BookPagination AS
    (
        SELECT 
            BookId,
            BookTitle,
            price,
            [description],
            smallThumbnailUrl,
            lang,
            isEbook,
            isActive,
            isAvailable,
            numOfPrints,
            ROW_NUMBER() OVER (ORDER BY BookId) AS RowNum
        FROM 
            Books
        WHERE 
            BookId IS NOT NULL AND BookId <> ''
    )
    -- Return paginated data
    SELECT 
        BookId,
        BookTitle,
        price,
        [description],
        smallThumbnailUrl,
        lang,
        isEbook,
        isActive,
        isAvailable,
        numOfPrints
    FROM 
        BookPagination
    WHERE 
        RowNum BETWEEN @StartRow AND @EndRow
    ORDER BY 
        RowNum;

    -- Return the total number of pages
    SELECT @TotalPages AS TotalPages;
END
GO


