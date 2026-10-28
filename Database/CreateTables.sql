
IF DB_ID('LibraryDB') IS NULL
BEGIN
    CREATE DATABASE LibraryDB;
    PRINT 'LibraryDB database created successfully.';
END
ELSE
BEGIN
    PRINT 'LibraryDB database already exists.';
END;
