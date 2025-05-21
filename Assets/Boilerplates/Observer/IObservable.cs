using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    public interface IObservable
    {
        public void Publish<IEvent>(IEvent eventMessage);
    }

}
