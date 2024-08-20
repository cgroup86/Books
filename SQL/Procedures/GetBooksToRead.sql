CREATE PROCEDURE SP_GetBooksToRead
    @UserId INT
AS
BEGIN
    

	IF EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        SELECT b.BookId, b.BookTitle, b.smallThumbnailUrl, b.isEbook, b.webReaderLink, b.embeddable,
           b.googleBooksId, pl.[Status], pl.IsPurchased, b.isActive
		FROM Books b
		INNER JOIN PersonalLibrary pl ON b.BookId = pl.BookId
		WHERE pl.UserId = @UserId AND pl.[Status] = 0 and b.isActive = 1 -- 0 for "ToRead"

    END
END
