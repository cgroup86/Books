-- =============================================
-- Author: <>
-- Create date: <10/8/24>
-- Description:	<Insert Book to the data base>
-- =============================================
CREATE PROCEDURE SP_InsertBook
	-- Add the parameters for the stored procedure here
	-- @id int,
	@BookTitle NVARCHAR(250),
	@price INT,
	@description NVARCHAR(2000),
	@pageNum INT,
    @smallThumbnailUrl NVARCHAR(255),
    @lang NVARCHAR(20),
    @previewLink NVARCHAR(255),
	@isEbook bit,
	@embeddable bit,
    @webReaderLink NVARCHAR(255),
    @googleBooksId NVARCHAR(250),
	@isActive bit,
	@isAvailable bit,
	@numOfPrints int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

	-- Check if a book with the same title already exists
    IF EXISTS (SELECT 1 FROM Books WHERE BookTitle = @BookTitle)
    BEGIN
        -- If it exists, return a message and do not insert the book
        SELECT 'Book with the same title already exists' AS Message;
        RETURN;
    END
    -- Insert statements for procedure here
	Insert into Books(BookTitle, price, [description], pageNum, smallThumbnailUrl
	, lang, previewLink, isEbook, embeddable,
	webReaderLink, googleBooksId, isActive, isAvailable, numOfPrints) 
	values(@BookTitle, @price, @description, @pageNum
	, @smallThumbnailUrl, @lang, @previewLink,
	@isEbook, @embeddable,
	@webReaderLink, @googleBooksId, @isActive, @isAvailable, @numOfPrints);

	SELECT SCOPE_IDENTITY() AS NewBookID;

END
GO