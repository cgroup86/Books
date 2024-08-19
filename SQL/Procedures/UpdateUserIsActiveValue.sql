-- =============================================
-- Author:		<Cgroup86>
-- Create date: <20/08/2024>
-- Description:	<change the values of active in User >
-- =============================================
CREATE PROCEDURE UpdateUserIsActiveValue
	-- Add the parameters for the stored procedure here
    @userId INT,
    @active BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	   IF EXISTS (SELECT 1 FROM Users WHERE id = @userId)
    BEGIN
        -- Update the specified fields
        UPDATE Users
        SET
            isActive = @active -- Assuming you want to update the active status
        WHERE id = @userId;
        
        PRINT 'User details updated successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Error: UserId not found.';
    END
END
GO

EXEC UpdateUserIsActiveValue 
    @userId = 1, 
    @active = 1