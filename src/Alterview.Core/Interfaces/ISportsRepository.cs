using Alterview.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alterview.Core.Interfaces
{
    public interface ISportsRepository
    {
        Task<IEnumerable<SportInfo>> GetSportsWithEventsCount();
    }
}
