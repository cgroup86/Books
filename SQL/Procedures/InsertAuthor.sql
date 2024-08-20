-- =============================================
-- Author: <>
-- Create date: <8/8/24>
-- Description:	<Insert author to the data base>
-- =============================================
CREATE PROCEDURE SP_InsertAuthor
	-- Add the parameters for the stored procedure here
	-- @id int,
	@name nvarchar(200),
	@topWork nvarchar(200),
	@workCount int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [Authors] ([AuthorName], [topWork], [workCount]) VALUES(@name, @topWork, @workCount);
END
GO