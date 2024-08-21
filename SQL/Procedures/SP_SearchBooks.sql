-- =============================================
-- Author: <->
-- Create date: <20/08/2024>
-- Description:	<Search for books using book name or author name or >
-- =============================================

alter PROCEDURE SP_SearchBooks
    @SearchType INT, 
    @SearchValue NVARCHAR(250) 
AS
BEGIN
    DECLARE @Results TABLE (
        BookId INT,
        BookTitle NVARCHAR(250),
        Price INT,
        SmallThumbnailUrl NVARCHAR(255),
		PreviewLink nvarchar(255),
        IsEbook BIT,
		IsAvailable BIT
    );

    IF @SearchType = 1
    BEGIN
        INSERT INTO @Results
        SELECT b.BookId, b.BookTitle, b.Price, b.SmallThumbnailUrl,b.previewLink, b.IsEbook, b.isAvailable
        FROM Books b
        WHERE b.BookTitle LIKE '%' + @SearchValue + '%' AND b.isActive = 1 and isAvailable = 1;
    END
    ELSE IF @SearchType = 2
    BEGIN
        INSERT INTO @Results
        SELECT DISTINCT b.BookId, b.BookTitle, b.Price, b.SmallThumbnailUrl,b.previewLink, b.IsEbook, b.isAvailable
        FROM Books b
        INNER JOIN BookAuthors ba ON b.BookId = ba.BookId
        WHERE ba.AuthorName LIKE '%' + @SearchValue + '%' AND b.isActive = 1 and isAvailable = 1;
    END
    ELSE IF @SearchType = 3
    BEGIN
        INSERT INTO @Results
        SELECT b.BookId, b.BookTitle, b.Price, b.SmallThumbnailUrl,b.previewLink, b.IsEbook, b.isAvailable
        FROM Books b
        WHERE b.[Description] LIKE '%' + @SearchValue + '%' AND b.isActive = 1 and isAvailable = 1; 
    END
    SELECT * FROM @Results;
END
