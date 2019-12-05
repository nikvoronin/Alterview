USE [LocalTestDatabase]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpsertEvent]
	@EventId    INT,
    @SportId    INT,
    @EventName  NVARCHAR (50),
    @EventDate  DATETIME2 (7),
    @Team1Price DECIMAL (18, 2),
    @DrawPrice  DECIMAL (18, 2),
    @Team2Price DECIMAL (18, 2)
AS 
 
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
    BEGIN TRANSACTION;

	UPDATE [dbo].[Events] WITH (UPDLOCK, SERIALIZABLE)
	SET SportId = @SportId,
		EventName = @EventName,
		EventDate = @EventDate,
		Team1Price = @Team1Price,
		DrawPrice = @DrawPrice,
		Team2Price = @Team2Price
	WHERE EventId = @EventId;
 
	IF(@@ROWCOUNT = 0)
	BEGIN
		INSERT [dbo].[Events]
				( EventId,  SportId,  EventName,  EventDate,  Team1Price,  DrawPrice,  Team2Price)
		VALUES  (@EventId, @SportId, @EventName, @EventDate, @Team1Price, @DrawPrice, @Team2Price)
	END
 
	COMMIT

RETURN 0
