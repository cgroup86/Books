-- =============================================
-- Author:		<Cgroup86>
-- Create date: <19/08/2024>
-- Description:	<change the values of active and price and number of prints in books >
-- =============================================

CREATE PROCEDURE UpdateBooksValues
    @bookId INT,
    @active BIT,
    @newPrice int,
    @numberOfPrints INT
AS
BEGIN
    -- Optionally prevent extra result sets from interfering with SELECT statements
    -- SET NOCOUNT ON;

    -- Check if the BookId exists in the table
    IF EXISTS (SELECT 1 FROM Books WHERE BookId = @bookId)
    BEGIN
        -- Update the specified fields
        UPDATE Books
        SET
            Price = @newPrice,
            numOfPrints = @numberOfPrints,
            isActive = @active -- Assuming you want to update the active status
        WHERE BookId = @bookId;
        
        PRINT 'Book details updated successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Error: BookId not found.';
    END
END
GO

