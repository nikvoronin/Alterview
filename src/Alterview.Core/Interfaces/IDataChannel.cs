using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alterview.Core.Interfaces
{
    public interface IDataChannel<T>
    {
        int Id { get; }
        bool PushData(T data);
        void Abort();
        Thread Worker { get; }
    }
}
