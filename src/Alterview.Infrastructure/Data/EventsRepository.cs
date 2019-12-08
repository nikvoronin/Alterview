using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Alterview.Infrastructure.Commands;
using Dapper;

namespace Alterview.Infrastructure.Data
{
    public class EventsRepository : RepositoryBase, IEventsRepository
    {
        public EventsRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<SportEvent>> GetEventsBySportAndDate(int sportId, DateTime date)
        {
            using var db = Connection;
            string query = "SELECT * FROM Events WHERE SportId = @SportId AND EventDate >= @DateStart AND EventDate < @DateEnd";
            IEnumerable<SportEvent> result = null;

            try
            {
                DateTime dateStart = date.Date;
                DateTime dateNext = dateStart.AddDays(1);
                result = await db.QueryAsync<SportEvent>(
                    query,
                    new
                    {
                        SportId = sportId,
                        DateStart = dateStart,
                        DateEnd = dateNext
                    });
            }
            catch { }

            return result;
        }

        public async Task<int> UpdateEvent(SportEvent ev)
        {
            return await UpsertEvent(ev);
        }

        public async Task<int> InsertEvent(SportEvent ev)
        {
            return await UpsertEvent(ev);
        }

        public async Task<int> UpsertEvent(SportEvent ev)
        {
            if (ev == null)
                throw new ArgumentNullException("ev");

            using var db = Connection;

            int rows = 0;
            try
            {
                rows = await db.ExecuteAsync("UpsertEvent",
                    new
                    {
                        EventId = ev.EventId,
                        SportId = ev.SportId,
                        EventName = ev.EventName,
                        EventDate = ev.EventDate,
                        Team1Price = ev.Team1Price,
                        DrawPrice = ev.DrawPrice,
                        Team2Price = ev.Team2Price
                    },
                    commandType: CommandType.StoredProcedure);
            }
            catch { }

            return rows;
        }

        public async Task<SportEvent> GetEventById(int id)
        {
            using var db = Connection;
            string query = "SELECT * FROM Events WHERE EventId = @EventId";

            SportEvent result = null;
            try
            {
                result = await db.QueryFirstOrDefaultAsync<SportEvent>(query, new { EventId = id });
            }
            catch { }

            return result;
        }

        public override int InitializeSchema()
        {
            using var db = Connection;

            string query =
                "CREATE TABLE [dbo].[Events] (" +
                " [EventId]    INT NOT NULL," +
                " [SportId] INT NOT NULL," +
                " [EventName] NVARCHAR(50)   NOT NULL," +
                " [EventDate] DATETIME2(7)   NOT NULL," +
                " [Team1Price] DECIMAL(18, 2) NOT NULL," +
                " [DrawPrice]  DECIMAL(18, 2) NOT NULL," +
                " [Team2Price] DECIMAL(18, 2) NOT NULL," +
                " CONSTRAINT[PK_Events] PRIMARY KEY CLUSTERED([EventId] ASC));";

            var result = 0;

            try
            {
                result = db.Execute(query);
            }
            catch { }

            return result;
        }

        public IAsyncCommand<SportEvent> CreateUpdateCommand()
        {
            return new UpdateEventCommand(this);
        }
    }
}
