using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alterview.Infrastructure.Data
{
    public class SportsRepository : RepositoryBase, ISportsRepository
    {
        public SportsRepository(string connectionString) : base(connectionString)
        {
        }

        public Task<IEnumerable<SportInfo>> GetSportsWithEventsCount()
        {
            return Task.FromResult<IEnumerable<SportInfo>>(null);
        }

        public override int InitializeSchema()
        {
            return 0;
        }
    }
}
