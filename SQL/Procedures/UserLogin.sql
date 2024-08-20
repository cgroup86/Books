-- =============================================
-- Author: <->
-- Create date: <1/08/2024>
-- Description:	<User login>
-- =============================================
CREATE PROCEDURE SP_UserLogin
	@Email NVARCHAR(200),
    @Password NVARCHAR(200)
AS
BEGIN
	SET @email = LOWER(@email);
    --SET NOCOUNT ON;

    -- Check if the user exists and match the email and password
    DECLARE @UserId INT;
    SELECT @UserId = id
    FROM Users
    WHERE email = @Email AND [password] = @Password AND isActive = 1;

    IF @UserId IS NULL
    BEGIN
        -- If no user found, return 0
        SELECT -1 AS UserId;
    END
    ELSE
    BEGIN
        -- If user found, return user details
        SELECT id AS UserId, [name], isAdmin, isActive
        FROM Users
        WHERE id = @UserId;
    END
END
GO