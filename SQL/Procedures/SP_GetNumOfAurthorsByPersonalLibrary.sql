
-- =============================================
-- Author:		<Cgroup86>
-- Create date: <20/08/2024>
-- Description:	<get the number of author in all praivate librares>
-- =============================================
alter PROCEDURE SP_GetNumOfAurthorsByPersonalLibrary
	@authorName NVARCHAR(200)
AS
BEGIN

	SELECT COUNT(*)
	from BookAuthors ba 
	inner join PersonalLibrary pl  on pl.BookId = ba.BookId
	where ba.AuthorName = @authorName
END
GO