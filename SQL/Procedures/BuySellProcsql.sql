
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
	Delete from buySell where sellerId = @sellerId and bookId = @bookId
	INSERT INTO PersonalLibrary (UserId, BookId, [Status], IsPurchased)
	VALUES (@buyerId, @bookId, 0, 1); 
END;



alter PROCEDURE SP_GetPurchasedBooksWithStatus1
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


alter PROCEDURE SP_GetRequestedBooksByBuyer
    @sellerId INT
AS
BEGIN
    SELECT 
        b.BookId,
        b.BookTitle,
		bs.buyerId,
		u.[name]
    FROM 
        Users u inner join buySell bs on u.id = bs.sellerId INNER JOIN Books b ON bs.bookId = b.BookId
    WHERE 
        bs.sellerId = @sellerId;
END;

exec SP_GetRequestedBooksByBuyer 0



create PROCEDURE SP_RejectRequestToBuyProcedure
    @sellerId INT,
    @buyerId INT,
    @bookId INT
AS
BEGIN
	Delete from buySell where sellerId = @sellerId AND BookId = @bookId and buyerId = @buyerId;
END;



create PROCEDURE SP_MyRequests
    @buyerId INT
AS
BEGIN
	SELECT bs.sellerId, u.[name], b.BookId, b.BookTitle FROM Users  u inner join buySell bs on u.id = sellerId INNER JOIN Books b ON bs.bookId = b.BookId
	WHERE bs.buyerId = @buyerId;
END;

-- bookId

--seller Id  - = -1 not available 
--seller Id  - = 1  available
--\\status 1 , phyiscal 



CREATE PROCEDURE SP_GetWhoOwnTheBook 
    @bookId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM PersonalLibrary WHERE BookId = @bookId AND Status = 1)
    BEGIN
        SELECT u.id AS SellerId, u.[name]
        FROM PersonalLibrary pl 
        INNER JOIN  Users u ON pl.UserId = u.id
        WHERE pl.BookId = @bookId AND pl.Status = 1;
    END
    ELSE
    BEGIN
        SELECT -1 AS SellerId;
    END
END;


exec SP_GetWhoOwnTheBook 28