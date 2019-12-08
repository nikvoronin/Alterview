using System;
using System.Collections.Generic;
using System.Text;

namespace Alterview.Core.Interfaces
{
    public interface IDataChannel<T>
    {
        int Id { get; }
        bool PushData(T data);
    }
}
