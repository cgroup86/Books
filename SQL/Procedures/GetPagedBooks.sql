CREATE PROCEDURE SP_GetPagedBooks
    @IsEbook BIT,
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ensure PageNumber and PageSize are positive
    IF @PageNumber < 1 OR @PageSize < 1
    BEGIN
        RAISERROR('PageNumber and PageSize must be greater than 0.', 16, 1);
        RETURN;
    END

    -- Calculate the total number of records
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM Books B
    WHERE B.IsEbook = @IsEbook;

    -- Perform the pagination query
    SELECT *
    FROM (
        SELECT B.*, 
            ROW_NUMBER() OVER (ORDER BY B.BookId) AS RowNumber
        FROM Books B
        WHERE B.IsEbook = @IsEbook
    ) AS Result
    WHERE RowNumber BETWEEN ((@PageNumber - 1) * @PageSize) + 1 AND @PageNumber * @PageSize;

    -- Return the total number of records
    SELECT @TotalRecords AS TotalRecords;
END