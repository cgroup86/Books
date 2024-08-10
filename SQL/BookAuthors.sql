Create table BookAuthors (
	BookId int references Books(BookId),
	AuthorName nvarchar(200) references Authors(AuthorName),
	primary key(BookId, AuthorName)
);