CREATE TABLE Tasks (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(250),
    Description NVARCHAR(MAX),
    AssignedTo NVARCHAR(150),
    DueDate DATE,
    Status NVARCHAR(50),
    IsActive BIT DEFAULT 1,
    CreatedOn DATETIME DEFAULT GETDATE()
)
