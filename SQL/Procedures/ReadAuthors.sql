-- =============================================
-- Author:		<Cgroup86>
-- Create date: <10/08/2024>
-- Description:	<Read Authors>
-- =============================================
CREATE PROCEDURE SP_ReadAuthors
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM [Authors];
END
GO