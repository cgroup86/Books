CREATE TABLE QuizScore (
    UserId INT NOT NULL,
    Score INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
	PRIMARY KEY (UserId, Score)
);
