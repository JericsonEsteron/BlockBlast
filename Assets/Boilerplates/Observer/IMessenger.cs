using System.Collections;
using System.Collections.Generic;
using ObserverPattern;

namespace EventMessage
{
    public interface IMessenger : IObservable, IObserver
    {
        public void GetDictionary();
    }

}
