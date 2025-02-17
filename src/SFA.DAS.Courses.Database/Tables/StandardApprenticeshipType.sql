CREATE TABLE [dbo].[StandardApprenticeshipType]
(
    [IfateReferenceNumber] VARCHAR(10) NOT NULL,
    [ApprenticeshipType] VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_StandardApprenticeshipType] PRIMARY KEY ([IfateReferenceNumber])
)
