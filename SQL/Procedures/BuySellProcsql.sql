
-- does exist = remove from the table
-- doesn't exist = add to the table
CREATE PROCEDURE SP_AddRemoveRequestToBuyProcedure
	@sellerId INT,
    @buyerId INT,
    @bookId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM buySell WHERE sellerId = @sellerId AND buyerId = @buyerId AND  bookId = @bookId)
    BEGIN
        DELETE FROM buySell WHERE sellerId = @sellerId AND buyerId = @buyerId AND bookId = @bookId;
    END
    ELSE
    BEGIN
        INSERT INTO buySell (sellerId, buyerId, bookId) VALUES (@sellerId, @buyerId, @bookId);
    END
END;




alter PROCEDURE SP_AcceptRequestToBuyProcedure
    @sellerId INT,
    @buyerId INT,
    @bookId INT
AS
BEGIN
    DELETE FROM PersonalLibrary WHERE UserId = @sellerId AND BookId = @bookId;
	Delete from buySell where sellerId = @sellerId and buyerId = @buyerId and bookId = @bookId
	INSERT INTO PersonalLibrary (UserId, BookId, [Status], IsPurchased)
	VALUES (@buyerId, @bookId, 0, 1);
END;



CREATE PROCEDURE SP_GetPurchasedBooksWithStatus1
	@userID int
AS
BEGIN
    SELECT 
        pl.UserId,
		u.[name],
        pl.BookId,
        b.BookTitle
		
    FROM 
        Users u inner join  PersonalLibrary pl on u.id = pl.UserId INNER JOIN  Books b ON pl.BookId = b.BookId
    WHERE pl.[Status] = 1 AND pl.IsPurchased = 1 and b.isActive = 1 and u.id <> @userID;
END;
exec SP_GetPurchasedBooksWithStatus1 1 


CREATE PROCEDURE SP_GetRequestedBooksByBuyer
    @sellerId INT
AS
BEGIN
    SELECT 
        b.BookId,
        b.BookTitle,
		bs.buyerId
    FROM 
        Users u inner join buySell bs on u.id = bs.sellerId INNER JOIN Books b ON bs.bookId = b.BookId
    WHERE 
        bs.sellerId = @sellerId;
END;

exec SP_GetRequestedBooksByBuyer 1