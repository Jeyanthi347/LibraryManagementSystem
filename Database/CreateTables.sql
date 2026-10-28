
IF DB_ID('LibraryDB') IS NULL
BEGIN
    CREATE DATABASE LibraryDB;
    PRINT 'LibraryDB database created successfully.';
END
ELSE
BEGIN
    PRINT 'LibraryDB database already exists.';
END;



USE LibraryDB;
GO

-- 1. Authors table
CREATE TABLE Authors
(
    AuthorId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL UNIQUE,
    Country NVARCHAR(60) NULL
);
GO

-- 2. Categories table
CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(60) NOT NULL UNIQUE,
    Description NVARCHAR(250) NULL
);
GO

-- 3. Books table
CREATE TABLE Books
(
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Isbn NVARCHAR(20) NOT NULL UNIQUE,
    AuthorId INT NOT NULL,
    CategoryId INT NOT NULL,
    PublicationYear INT NULL
        CHECK (PublicationYear BETWEEN 1450 AND 2100),
    TotalCopies INT NOT NULL DEFAULT 1
        CHECK (TotalCopies >= 1),
    AvailableCopies INT NOT NULL DEFAULT 1
        CHECK (AvailableCopies >= 0),
    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Books_Authors
        FOREIGN KEY (AuthorId)
        REFERENCES Authors(AuthorId),

    CONSTRAINT FK_Books_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId),

    CONSTRAINT CK_Books_AvailableCopies
        CHECK (AvailableCopies <= TotalCopies)
);
GO

-- 4. Members table
CREATE TABLE Members
(
    MemberId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Phone NVARCHAR(20) NULL,
    MembershipStartDate DATE NOT NULL
        DEFAULT (CAST(SYSDATETIME() AS DATE)),
    MembershipExpiryDate DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT CK_Members_MembershipDates
        CHECK (MembershipExpiryDate > MembershipStartDate)
);
GO

-- 5. Borrows table
CREATE TABLE Borrows
(
    BorrowId INT IDENTITY(1,1) PRIMARY KEY,
    BookId INT NOT NULL,
    MemberId INT NOT NULL,
    BorrowDate DATETIME2(0) NOT NULL,
    DueDate DATETIME2(0) NOT NULL,
    ReturnDate DATETIME2(0) NULL,

    CONSTRAINT FK_Borrows_Books
        FOREIGN KEY (BookId)
        REFERENCES Books(BookId),

    CONSTRAINT FK_Borrows_Members
        FOREIGN KEY (MemberId)
        REFERENCES Members(MemberId),

    CONSTRAINT CK_Borrows_BorrowDate_NotFuture
        CHECK (BorrowDate <= SYSDATETIME()),

    CONSTRAINT CK_Borrows_DueDate_After_BorrowDate
        CHECK (DueDate > BorrowDate),

    CONSTRAINT CK_Borrows_ReturnDate_NotBeforeBorrow
        CHECK (ReturnDate IS NULL OR ReturnDate >= BorrowDate),

    CONSTRAINT CK_Borrows_ReturnDate_NotFuture
        CHECK (ReturnDate IS NULL OR ReturnDate <= SYSDATETIME())
);
GO

-- A member cannot have duplicate active borrows
-- for the same book.
CREATE UNIQUE INDEX UX_Borrows_ActiveMemberBook
ON Borrows (MemberId, BookId)
WHERE ReturnDate IS NULL;
GO

-- Indexes for faster searches
CREATE INDEX IX_Borrows_MemberId
ON Borrows (MemberId);
GO

CREATE INDEX IX_Borrows_BookId
ON Borrows (BookId);
GO

-- 6. Fines table
CREATE TABLE Fines
(
    FineId INT IDENTITY(1,1) PRIMARY KEY,
    BorrowId INT NOT NULL,
    OverdueDays INT NOT NULL
        CHECK (OverdueDays > 0),
    DailyRate DECIMAL(10,2) NOT NULL
        CHECK (DailyRate > 0),
    FineAmount DECIMAL(10,2) NOT NULL,
    AmountPaid DECIMAL(10,2) NOT NULL DEFAULT 0,
    Status NVARCHAR(10) NOT NULL DEFAULT N'Unpaid',
    CreatedAt DATETIME2(0) NOT NULL DEFAULT SYSDATETIME(),
    LastPaymentAt DATETIME2(0) NULL,

    CONSTRAINT FK_Fines_Borrows
        FOREIGN KEY (BorrowId)
        REFERENCES Borrows(BorrowId),

    CONSTRAINT UQ_Fines_BorrowId
        UNIQUE (BorrowId),

    CONSTRAINT CK_Fines_FineAmount
        CHECK (FineAmount = OverdueDays * DailyRate),

    CONSTRAINT CK_Fines_AmountPaid
        CHECK (AmountPaid >= 0 AND AmountPaid <= FineAmount),

    CONSTRAINT CK_Fines_Status
        CHECK
        (
            (Status = N'Unpaid'
                AND AmountPaid = 0
                AND LastPaymentAt IS NULL)

            OR

            (Status = N'Partial'
                AND AmountPaid > 0
                AND AmountPaid < FineAmount
                AND LastPaymentAt IS NOT NULL)

            OR

            (Status = N'Paid'
                AND AmountPaid = FineAmount
                AND LastPaymentAt IS NOT NULL)
        )
);
GO

PRINT 'All six LibraryDB tables created successfully.';
GO
