USE [LocalTestDatabase]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Events] (
    [EventId]    INT             NOT NULL,
    [SportId]    INT             NOT NULL,
    [EventName]  NVARCHAR (50)   NOT NULL,
    [EventDate]  DATETIME2 (7)   NOT NULL,
    [Team1Price] DECIMAL (18, 2) NOT NULL,
    [DrawPrice]  DECIMAL (18, 2) NOT NULL,
    [Team2Price] DECIMAL (18, 2) NOT NULL
);


