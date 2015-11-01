namespace WcfHelper
{
    using System;

    public interface IClientInvokerCommunication
    {
        event EventHandler Closed;
        event EventHandler Closing;
        event EventHandler Faulted;
        event EventHandler Opened;
        event EventHandler Opening;
    }
}