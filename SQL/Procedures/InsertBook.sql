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
	@publisher NVARCHAR(255),
	@publishedDate nvarchar(20),
	@description NVARCHAR(2000),
	@pageNum INT,
	@averageRating INT,
	@ratingsCount INT, 
    @smallThumbnailUrl NVARCHAR(255),
    @thumbnailUrl NVARCHAR(255),
    @lang NVARCHAR(20),
    @previewLink NVARCHAR(255),
    @infoLink NVARCHAR(255), 
    @canonicalVolumeLink nVARCHAR(255),
	@isEbook bit,
	@embeddable bit,
    @epubIsAvailable bit,
    @epubDownloadLink NVARCHAR(255),
    @pdfIsAvailable bit, 
    @pdfDownloadLink NVARCHAR(255),
    @webReaderLink NVARCHAR(255),
	@textReading bit, 
	@photoReading bit,
    @googleBooksId NVARCHAR(250),
    @etag NVARCHAR(250),
    @selfLink NVARCHAR(255) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	-- SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into Books(BookTitle, price, publisher, publishedDate, [description], pageNum, averageRating, ratingsCount, smallThumbnailUrl
	, thumbnailUrl, lang, previewLink, infoLink, canonicalVolumeLink, isEbook, embeddable,
	epubIsAvailable, epubDownloadLink, pdfIsAvailable, pdfDownloadLink, webReaderLink, textReading,
	photoReading, googleBooksId, etag, selfLink) values(@BookTitle, @price, @publisher, @publishedDate
	, @description, @pageNum, @averageRating, @ratingsCount, @smallThumbnailUrl, @thumbnailUrl,
	@lang, @previewLink, @infoLink, @canonicalVolumeLink, @isEbook, @embeddable, @epubIsAvailable,
	@epubDownloadLink, @pdfIsAvailable, @pdfDownloadLink, @webReaderLink, @textReading, @photoReading,
	@googleBooksId, @etag, @selfLink);

	SELECT SCOPE_IDENTITY() AS NewBookID;

END
GO