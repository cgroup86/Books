CREATE PROCEDURE SP_GetUserAndTopScores
    @UserId INT
AS
BEGIN
    -- Select the score and name of the specific user
    SELECT 
        u.Id AS UserId,
        u.Name AS UserName,
        COALESCE(qs.Score, 0) AS Score
    FROM Users u
    LEFT JOIN QuizScore qs ON u.Id = qs.UserId
    WHERE u.Id = @UserId;

    -- Select the top 5 scores and names
    SELECT TOP 3
        u.Name AS UserName,
        qs.Score
    FROM QuizScore qs
    INNER JOIN Users u ON qs.UserId = u.Id
    ORDER BY qs.Score DESC;
END;
