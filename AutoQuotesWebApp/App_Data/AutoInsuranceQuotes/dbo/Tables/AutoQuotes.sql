CREATE TABLE [dbo].[AutoQuotes] (
    [AutoQuoteId]                   INT        IDENTITY (5000, 1) NOT NULL,
    [AutoQuoteDateTime]             DATETIME   NOT NULL,
    [InsureeId]                     INT        NOT NULL,
    [BaseRate]                      MONEY      NOT NULL,
    [AgeUnder18Rate]                MONEY      NOT NULL,
    [AgeBtwn19and25Rate]            MONEY      NOT NULL,
    [AgeOver25Rate]                 MONEY      NOT NULL,
    [AutoYearBefore2000Rate]        MONEY      NOT NULL,
    [AutoYearAfter2015Rate]         MONEY      NOT NULL,
    [IsPorscheRate]                 MONEY      NOT NULL,
    [IsCarreraRate]                 MONEY      NOT NULL,
    [SpeedingTicketsRate]           MONEY      NOT NULL,
    [SubtotalBeforeDuiCalc]         MONEY      NOT NULL,
    [DuiRate]                       MONEY      NOT NULL,
    [SubtotalAfterDuiCalc]          MONEY      NOT NULL,
    [CoverageTypeRate]              MONEY      NOT NULL,
    [SubtotalAfterCoverageTypeCalc] MONEY      NOT NULL,
    [MonthlyQuoteRate]              MONEY      NOT NULL,
    [YearlyQuoteRate]               MONEY      NOT NULL,
    CONSTRAINT [PK_AutoQuotes] PRIMARY KEY CLUSTERED ([AutoQuoteId] ASC),
    CONSTRAINT [FK_AutoQuotes_Insurees] FOREIGN KEY ([InsureeId]) REFERENCES [dbo].[Insurees] ([InsureeId])
);

