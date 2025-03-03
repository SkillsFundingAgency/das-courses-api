CREATE TABLE [dbo].[StandardApprenticeshipType]
(
    [IfateReferenceNumber] VARCHAR(10) NOT NULL,
    [ApprenticeshipType] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_StandardApprenticeshipType] PRIMARY KEY ([IfateReferenceNumber])
)
