-- =============================================
-- Author: <>
-- Create date: <9/8/24>
-- Description:	<Insert category to the data base>
-- =============================================
CREATE PROCEDURE SP_InsertCategory
	-- Add the parameters for the stored procedure here
	-- @id int,
	@name nvarchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into [CategoryB] ([CategoryNameB]) values(@name);
END
GO
