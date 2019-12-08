using System;
using System.Collections.Generic;
using System.Text;

namespace Alterview.Core.Interfaces
{
    public interface IExternalDataSource<T>
    {
        event EventHandler<T> MessageReceive;
        void Acknowledge(ulong id, AckStatus status);
    }

    public enum AckStatus { Ack, Reject }
}
