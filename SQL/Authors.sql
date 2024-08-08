CREATE TABLE Authors (
    AuthorName nVARCHAR(200) PRIMARY KEY,
    topWork nVARCHAR(200) NOT NULL,
    workCount INT NOT NULL,
    AuthorKey nVARCHAR(100) NOT NULL
);