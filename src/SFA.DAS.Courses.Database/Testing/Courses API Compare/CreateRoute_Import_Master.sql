CREATE TABLE ##Route_Import_Master
(
    Id INT PRIMARY KEY,
    Name VARCHAR(500) NOT NULL,
    Active BIT NOT NULL DEFAULT 1
);

SET NOCOUNT ON;

SELECT 
    'INSERT INTO ##Route_Import_Master (Id, Name, Active) VALUES (' +
    CAST(Id AS VARCHAR) + ', ' +
    '''' + REPLACE(Name, '''', '''''') + ''', ' +
    CAST(Active AS VARCHAR) +
    ');'
FROM dbo.Route_Import;
