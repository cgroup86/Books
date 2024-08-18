-- =============================================
-- Author: <>
-- Create date: <16/08/2024>
-- Description:	<Insert Book to personal library>
-- =============================================
CREATE PROCEDURE SP_InsertToPersonalLibrary
    @UserId INT,
    @BookId INT,
    @Status BIT,
    @IsPurchased BIT
AS
BEGIN
    -- SET NOCOUNT ON;
    -- Check if the book already exists in the user's library
    IF EXISTS (SELECT 1 FROM PersonalLibrary WHERE UserId = @UserId AND BookId = @BookId)
    BEGIN
        SELECT 0 AS Result; -- Book already exists
    END

    -- Add the book to the library
    INSERT INTO PersonalLibrary (UserId, BookId, [Status], IsPurchased)
    VALUES (@UserId, @BookId, @Status, @IsPurchased);

    -- Update the number of prints for the physical book
    IF EXISTS (SELECT 1 FROM Books WHERE BookId = @BookId AND isEbook = 0)
    BEGIN
        UPDATE Books
        SET numOfPrints = numOfPrints - 1
        WHERE BookId = @BookId;

        -- Check if the number of prints is 0, and set isAvailable to false
        IF (SELECT numOfPrints FROM Books WHERE BookId = @BookId) = 0
        BEGIN
            UPDATE Books
            SET isAvailable = 0
            WHERE BookId = @BookId;
        END
    END
    SELECT 1 AS Result;
END