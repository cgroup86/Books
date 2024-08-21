alter PROCEDURE SP_UpdateBookStatus
    @UserId INT,
    @BookId INT,
    @NewStatus BIT,
	@IsEbook bit
AS
BEGIN
    IF EXISTS (SELECT 1 FROM PersonalLibrary WHERE UserId = @UserId AND BookId = @BookId)
    BEGIN
        UPDATE PersonalLibrary
        SET [Status] = @NewStatus
        WHERE UserId = @UserId AND BookId = @BookId;

		IF @NewStatus = 0 AND @IsEbook = 0
		BEGIN
        DELETE FROM buySell
        WHERE SellerId = @UserId AND BookId = @BookId;
		END
    END
	IF @@ROWCOUNT > 0
    BEGIN
        SELECT 1 AS Result;
    END
    ELSE
    BEGIN
        SELECT 0 AS Result;
    END
END