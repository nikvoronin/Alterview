using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alterview.Infrastructure.Data
{
    public class EventsRepository : RepositoryBase, IEventsRepository
    {
        public EventsRepository(string connectionString) : base(connectionString)
        {
        }

        public Task<SportEvent> GetEventById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SportEvent>> GetEventsBySportAndDate(int sportId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public override int InitializeSchema()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateEvent(SportEvent ev)
        {
            throw new NotImplementedException();
        }
    }
}
