CREATE TABLE PersonalLibrary (
    UserId INT,
    BookId INT,
    [Status] BIT NOT NULL,       -- "ToRead" = false or "HaveRead" = true
    IsPurchased BIT NOT NULL,  -- true = "purchased"
    PRIMARY KEY (UserId, BookId),
    FOREIGN KEY (UserId) REFERENCES Users(id),
    FOREIGN KEY (BookId) REFERENCES Books(BookId)
);
