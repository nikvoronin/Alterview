using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;

namespace Alterview.Infrastructure.Commands
{
    public class UpdateEventCommand : IAsyncCommand<SportEvent>
    {
        IEventsRepository _eventsRepo;

        public UpdateEventCommand(IEventsRepository eventsRepo)
        {
            _eventsRepo = eventsRepo;
        }

        public async Task<int> ExecuteAsync(SportEvent ev)
        {
            return await _eventsRepo.UpdateEvent(ev);
        }
    }
}
