create table BookCategoryB (
	BookId int references Books(BookId),
	CategoryNameB nvarchar(150) references CategoryB(CategoryNameB),
	primary key (BookId, CategoryNameB)
);