using Alterview.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alterview.Core.Interfaces
{
    public interface IEventsRepository
    {
        Task<SportEvent> GetEventById(int id);
        Task<IEnumerable<SportEvent>> GetEventsBySportAndDate(int sportId, DateTime date);
        Task<int> UpdateEvent(SportEvent ev);
    }
}
