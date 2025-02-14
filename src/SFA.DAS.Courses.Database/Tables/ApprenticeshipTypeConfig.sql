CREATE TABLE [dbo].[ApprenticeshipTypeConfig]
(
    [IfateReferenceNumber] VARCHAR(10) NOT NULL,
    [ApprenticeshipType] VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_ApprenticeshipTypeConfig] PRIMARY KEY ([IfateReferenceNumber])
)
