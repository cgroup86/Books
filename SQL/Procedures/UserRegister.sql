-- =============================================
-- Author: <->
-- Create date: <1/08/2024>
-- Description:	<User Register>
-- =============================================
CREATE PROCEDURE SP_UserRegister
	-- Add the parameters for the stored procedure here
	    @name NVARCHAR(200),
	    @email NVARCHAR(200),
		@password NVARCHAR(200)
AS
BEGIN

    -- Check if the email already exists
    IF EXISTS (SELECT 1 FROM Users WHERE email = @email)
    BEGIN
        SELECT 0 AS Result; -- Email exists
    END
    ELSE
    BEGIN
        -- Insert new user
        INSERT INTO Users (name, email, password, isAdmin, isActive)
        VALUES (@name, @email, @password, 0, 1);

        SELECT 1 AS Result; -- Registration successful
    END
END
GO