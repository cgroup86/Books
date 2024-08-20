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
    SET NOCOUNT ON;  

    SET @Email = LOWER(@Email);  

    DECLARE @UserId INT;
	DECLARE @UserName nvarchar(200);
    DECLARE @StoredPassword NVARCHAR(200);
    DECLARE @IsAdmin BIT;
    DECLARE @IsActive BIT;

	SELECT @UserId = id,
		   @UserName = [name],
           @StoredPassword = [password],
           @IsAdmin = isAdmin,
           @IsActive = isActive
    FROM Users
    WHERE email = @Email;

    -- Check if the user exists
    IF @UserId IS NULL
    BEGIN
        -- User does not exist
        SELECT UserId = -1, [Name] = '', IsAdmin = 0, IsActive = 0;
    END
    ELSE
    BEGIN
        IF @StoredPassword = @Password
        BEGIN
            SELECT @UserId AS UserId, 
                   @UserName AS [Name],
                   @IsAdmin AS IsAdmin,
                   @IsActive AS IsActive;
        END
        ELSE
        BEGIN
            SELECT UserId = -2, [Name] = '', IsAdmin = 0, IsActive = 0;
        END
    END
END
GO
