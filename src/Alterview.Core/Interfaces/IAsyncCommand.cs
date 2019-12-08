using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alterview.Core.Interfaces
{
    public interface IAsyncCommand<T>
    {
        Task<int> ExecuteAsync(T x);
    }
}
