use [igroup186_test2]
go
-- begin of the sp's
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- create the table of the question 
CREATE TABLE Survey (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Question NVARCHAR(255) NOT NULL,
    AnswerA NVARCHAR(255) NOT NULL,
    AnswerB NVARCHAR(255) NOT NULL,
    AnswerC NVARCHAR(255) NOT NULL,
    AnswerD NVARCHAR(255) NOT NULL
);
go

-- questions : 
-- 1  - who is the auther of this book __ 
-- 2 - who is the auther of the book __
-- 3 - what is the price of the book ___
-- 4 - what is the page number of the book __
-- 5 - which book of this books is in this catgory __





-- 1 - who is the auther of the book __
create PROCEDURE spGenerateQuestion1
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @BookId as int;
	declare @Answer as nvarchar(200)
	-- get random book id and random author name
	select top 1 @BookId = [BookId], @Answer = [AuthorName] from BookAuthors where [AuthorName] != '' order by  NEWID()

	-- return the results ther first one is the correct one always
	 select 'Who is the auther of the book ' + [BookTitle] +'?' as [Question], @Answer as [CorrectAnswer] from Books where [BookId] = @BookId 
	 select top 3 [AuthorName] as [WrongAnswers] from BookAuthors where [AuthorName] != '' and [AuthorName] != @Answer order by  NEWID()

END
GO


-- use the sp:
EXEC spGenerateQuestion1;
go



-- create a view for the join table of the authers and the books table (used in some sp questions)
CREATE VIEW books_authers AS
SELECT b.*,ba.AuthorName as [AuthorName]
FROM Books as b join BookAuthors as ba on b.BookId = ba.BookId
go

-- which book is for the auther __
create PROCEDURE spGenerateQuestion2
AS
BEGIN
	SET NOCOUNT ON;

	declare @Auther as nvarchar(200);
	declare @AnswerTitle as nvarchar(250)
	declare @AnswerImage as nvarchar(255)

	-- get random book id and random author name
	select top 1 @Auther = [AuthorName], @AnswerTitle = [BookTitle], @AnswerImage = [smallThumbnailUrl] from [books_authers] where [AuthorName] != '' order by  NEWID()

	-- return the results ther first one is the correct one always
	 select 'Which book is for this auther ' + @Auther +'?' as [Question], @AnswerTitle as [CorrectAnswerTitle],  @AnswerImage as [CorrectAnswerImage]
	 select top 3 [BookTitle] as [WrongAnswersTitle], [smallThumbnailUrl] as [WrongAnswersImage]  from [books_authers] where [AuthorName] != '' and [AuthorName] != @Auther order by  NEWID()

END
GO

EXEC spGenerateQuestion2;
go



-- what is the price of the book
create PROCEDURE spGenerateQuestion3
AS
BEGIN
	SET NOCOUNT ON;


	declare @Book as nvarchar(200);
	declare @Answer as int

	-- get random book id and random author name
	select top 1 @Book = [BookTitle], @Answer = [price] from [Books] where [BookTitle] != '' and [price] != 0 order by  NEWID()

	-- return the results ther first one is the correct one always
	 select 'What is the price of the book ' + @Book +'?' as [Question], @Answer as [CorrectAnswer]
	 select top 3 [price] as [WrongAnswers]  from [Books] where [BookTitle] != '' and [price] != 0 and [BookTitle] != @Book order by  NEWID()

END
GO

EXEC spGenerateQuestion3;
go


--  what is the number of the pages for this book
create PROCEDURE spGenerateQuestion4
AS
BEGIN
	SET NOCOUNT ON;


	declare @Book as nvarchar(200);
	declare @Answer as int

	-- get random book id and random author name
	select top 1 @Book = [BookTitle], @Answer = [pageNum] from [Books] where [BookTitle] != '' and [pageNum] != 0 order by  NEWID()

	-- return the results ther first one is the correct one always
	 select 'What is the price of the book ' + @Book +'?' as [Question], @Answer as [CorrectAnswer]
	 select top 3 [pageNum] as [WrongAnswers]  from [Books] where [BookTitle] != '' and [pageNum] != 0 and [BookTitle] != @Book order by  NEWID()

END
GO
EXEC spGenerateQuestion4;
go



-- create a view for the join table of the authers and the books table (used in some sp questions)
CREATE VIEW books_category AS
SELECT b.*,c.CategoryNameB as [CategoryNameB]
FROM Books as b join BookCategoryB as c on b.BookId = c.BookId
go


--  which book of this books is in this catgory

alter PROCEDURE spGenerateQuestion5
AS
BEGIN
	SET NOCOUNT ON;

	declare @Category as nvarchar(150);
	declare @AnswerTitle as nvarchar(250)
	declare @AnswerImage as nvarchar(255)


	-- get random book id and random author name
	select top 1 @AnswerTitle = [BookTitle],@AnswerImage = [smallThumbnailUrl], @Category = [CategoryNameB] from books_category where [BookTitle] != '' and [smallThumbnailUrl] != '' and [CategoryNameB] != '' order by  NEWID()

	-- return the results ther first one is the correct one always
	 select 'Which book of this books is in this catgory ' + @Category +'?' as [Question], @AnswerTitle as [CorrectAnswerTitle],  @AnswerImage as [CorrectAnswerImage]
	 select  top  3 [BookTitle] as [WrongAnswersTitle], [smallThumbnailUrl] as [WrongAnswersImage]  from books_category where [BookTitle] != '' and [CategoryNameB] != @Category and [smallThumbnailUrl] != '' order by  NEWID()

END
GO

EXEC spGenerateQuestion5;
go