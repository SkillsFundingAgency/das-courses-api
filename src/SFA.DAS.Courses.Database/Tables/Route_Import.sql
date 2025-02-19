CREATE TABLE [dbo].[Route_Import]
(
    [Id] INT PRIMARY KEY,
    [Name] VARCHAR(500) NOT NULL,
    [Active] BIT NOT NULL DEFAULT 1
)
GO
