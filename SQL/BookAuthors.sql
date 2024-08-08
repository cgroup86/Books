Create table BookAuthors (
	BookId int references Books(BookId) not null,
	AuthorName nvarchar(200) references Authors(AuthorName) not null,
	primary key(BookId, AuthorName)
);