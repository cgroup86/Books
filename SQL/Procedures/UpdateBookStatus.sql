CREATE PROCEDURE SP_UpdateBookStatus
    @UserId INT,
    @BookId INT,
    @NewStatus BIT
AS
BEGIN
    -- Update or insert the book status for the user
    IF EXISTS (SELECT 1 FROM PersonalLibrary WHERE UserId = @UserId AND BookId = @BookId)
    BEGIN
        -- Update existing entry
        UPDATE PersonalLibrary
        SET [Status] = @NewStatus
        WHERE UserId = @UserId AND BookId = @BookId;
		select 1 as Result
    END
	ELSE 
	BEGIN
		select 0 as Result
	END
END