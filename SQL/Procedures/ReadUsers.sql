-- =============================================
-- Author: <->
-- Create date: <1/08/2024>
-- Description:	<Read Users>
-- =============================================
CREATE PROCEDURE SP_ReadUsers
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [Users];
END
GO