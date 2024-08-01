Create table [Users] (
	[id] int primary key identity(0,1),
	[name] nvarchar(100) not null,
	[email] nvarchar(200) not null unique,
	[password] nvarchar(200) not null,
	[isAdmin] bit not null default(0),
	[isActive] bit not null default(1) 
)


