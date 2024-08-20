CREATE PROCEDURE SP_UpsertQuizScore
    @UserId INT,
    @Score INT
AS
BEGIN
    -- Check if a record with the same UserId already exists
    IF EXISTS (SELECT 1 FROM QuizScore WHERE UserId = @UserId)
    BEGIN
        -- Update the existing record with the new score
        UPDATE QuizScore
        SET Score = @Score
        WHERE UserId = @UserId;
    END
    ELSE
    BEGIN
        -- Insert a new record if no existing record is found
        INSERT INTO QuizScore (UserId, Score)
        VALUES (@UserId, @Score);
    END
END;
