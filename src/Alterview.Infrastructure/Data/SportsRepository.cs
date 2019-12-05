using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alterview.Infrastructure.Data
{
    public class SportsRepository : RepositoryBase, ISportsRepository
    {
        public SportsRepository(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<SportInfo>> GetSportsWithEventsCount()
        {
            using var db = Connection;
            //string dirtyReadQuery = $"SELECT Sports.SportId, Sports.Name, COUNT(Events.EventId) AS EventsCount FROM Sports LEFT JOIN Events WITH(NOLOCK) ON Sports.SportId = Events.SportId GROUP BY Sports.SportId, Sports.Name";
            string query = $"SELECT Sports.SportId, Sports.Name, COUNT(Events.EventId) AS EventsCount FROM Sports LEFT JOIN Events ON Sports.SportId = Events.SportId GROUP BY Sports.SportId, Sports.Name";

            IEnumerable<SportInfo> result = null;
            try
            {
                result = await db.QueryAsync<SportInfo>(query);
            }
            catch { }

            return result;
        }

        public override int InitializeSchema()
        {
            using var db = Connection;
            string query = "CREATE TABLE [dbo].[Sports] (" +
            " [SportId] INT IDENTITY(1, 1) NOT NULL," +
            " [Name] NVARCHAR(50) NOT NULL," +
            " CONSTRAINT[PK_Sports] PRIMARY KEY CLUSTERED([SportId] ASC) );";

            var result = db.Execute(query);

            return result;
        }
    }
}
