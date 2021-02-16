CREATE TABLE [dbo].[Insurees] (
    [InsureeId]       INT            IDENTITY (100, 1) NOT NULL,
    [FirstName]       NVARCHAR (50)  NOT NULL,
    [LastName]        NVARCHAR (50)  NOT NULL,
    [EmailAddress]    NVARCHAR (100) NOT NULL,
    [DateOfBirth]     DATETIME       NOT NULL,
    [AutoYear]        INT            NOT NULL,
    [AutoMake]        NVARCHAR (50)  NOT NULL,
    [AutoModel]       NVARCHAR (50)  NOT NULL,
    [SpeedingTickets] INT            NOT NULL,
    [DUI]             BIT            NOT NULL,
    [CoverageType]    BIT            NOT NULL,
    [AutoQuoteId]     INT            NOT NULL,
    CONSTRAINT [PK_Insurees] PRIMARY KEY CLUSTERED ([InsureeId] ASC),
    CONSTRAINT [FK_Insurees_AutoQuotes] FOREIGN KEY ([AutoQuoteId]) REFERENCES [dbo].[AutoQuotes] ([AutoQuoteId])
);

