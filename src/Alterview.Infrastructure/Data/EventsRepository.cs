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
            return Task.FromResult<SportEvent>(null);
        }

        public Task<IEnumerable<SportEvent>> GetEventsBySportAndDate(int sportId, DateTime date)
        {
            return Task.FromResult<IEnumerable<SportEvent>>(null);
        }

        public override int InitializeSchema()
        {
            return 0;
        }

        public Task<int> UpdateEvent(SportEvent ev)
        {
            return Task.FromResult(0);
        }
    }
}
