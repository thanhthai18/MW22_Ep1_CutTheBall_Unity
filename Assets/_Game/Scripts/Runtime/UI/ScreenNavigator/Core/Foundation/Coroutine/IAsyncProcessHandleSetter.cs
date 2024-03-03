using System;

namespace UnityScreenNavigator.Runtime.Foundation.Coroutine
{
    internal interface IAsyncProcessHandleSetter
    {
        void Complete(object result);

        void Error(Exception ex);
    }
}