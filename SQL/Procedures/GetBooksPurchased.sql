CREATE PROCEDURE SP_GetBooksPurchased
    @UserId INT
AS
BEGIN
	IF EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
		SELECT b.BookId, b.BookTitle, b.smallThumbnailUrl, b.isEbook, b.webReaderLink, b.embeddable,
			b.googleBooksId, pl.[Status], pl.IsPurchased, b.isActive
		FROM Books b
		INNER JOIN PersonalLibrary pl ON b.BookId = pl.BookId
		WHERE pl.UserId = @UserId AND pl.IsPurchased = 1

    END
END


