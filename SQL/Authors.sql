CREATE TABLE Authors (
    [name] nVARCHAR(200) PRIMARY KEY,
    topWork nVARCHAR(200) NOT NULL,
    workCount INT NOT NULL,
    [key] nVARCHAR(100) NOT NULL
);