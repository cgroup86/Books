CREATE TABLE Survey (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Question NVARCHAR(255) NOT NULL,
    AnswerA NVARCHAR(255) NOT NULL,
    AnswerB NVARCHAR(255) NOT NULL,
    AnswerC NVARCHAR(255) NOT NULL,
    AnswerD NVARCHAR(255) NOT NULL
);