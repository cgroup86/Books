CREATE PROCEDURE SP_GetAuthorsForBook
    @BookId INT
AS
BEGIN
    SELECT AuthorName
    FROM BookAuthors
    WHERE BookId = @BookId
END