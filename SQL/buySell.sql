CREATE TABLE buySell (
    sellerId INT,
    bookId INT,
    buyerId INT,
    PRIMARY KEY (sellerId, bookId, buyerId),
    FOREIGN KEY (sellerId) REFERENCES Users(id), 
    FOREIGN KEY (buyerId) REFERENCES Users(id) , 
	FOREIGN KEY (bookId) REFERENCES Books(BookId)
);
