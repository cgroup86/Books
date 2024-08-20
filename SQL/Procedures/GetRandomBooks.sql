Use igroup186_test2 
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Abofull>
-- Create date: <19/8/2024>
-- Description:	<get 5 random books>
-- =============================================
CREATE PROCEDURE SP_GetRandomBooks

AS
BEGIN

	SET NOCOUNT ON;

	select top 5 BookId, BookTitle, Price, smallThumbnailUrl,previewLink, isEbook, isAvailable
	from [Books] where [BookTitle] != '' and isEbook = 1 and isActive = 1 order by  NEWID()
END
GO
