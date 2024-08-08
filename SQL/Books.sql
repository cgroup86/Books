CREATE TABLE Books (
    id INT IDENTITY(1,1) PRIMARY KEY , -- Identifier for the book
    title NVARCHAR(250) NOT NULL, -- The name of the book
    price INT NOT NULL, -- Price of the book
    publisher NVARCHAR(255), -- Publisher of the book
    publishedDate DATE, -- Publication date of the book
    [description] NVARCHAR(2000), -- Description of the book
    textReading bit, -- If the book is available in text format
    [pageCount] INT, -- The number of pages in the book
    averageRating INT, -- Average rating of the book
    ratingsCount INT, -- Number of ratings
    smallThumbnailUrl NVARCHAR(255), -- URL of the small thumbnail
    thumbnailUrl NVARCHAR(255), -- URL of the thumbnail
    [language] NVARCHAR(20), -- Language code of the book
    previewLink NVARCHAR(255), -- Preview link for the book
    infoLink NVARCHAR(255), -- Information link for the book
    canonicalVolumeLink VARCHAR(255), -- Canonical volume link for the book
    isEbook bit, -- True = digital, false = physical
    embeddable bit, -- If the book is embeddable
    epubIsAvailable bit, -- If the EPUB version is available
    epubDownloadLink NVARCHAR(255), -- Download link for EPUB
    pdfIsAvailable bit, -- If the PDF version is available
    pdfDownloadLink NVARCHAR(255), -- Download link for PDF
    webReaderLink NVARCHAR(255), -- Link to the web reader
    googleBooksId NVARCHAR(250), -- Google Books API ID
    etag NVARCHAR(250), -- ETag from the Google Books API
    selfLink NVARCHAR(255) -- Self link from the Google Books API
	);