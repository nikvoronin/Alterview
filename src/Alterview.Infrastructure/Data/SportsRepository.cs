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
            throw new NotImplementedException();
        }

        public override int InitializeSchema()
        {
            throw new NotImplementedException();
        }
    }
}
