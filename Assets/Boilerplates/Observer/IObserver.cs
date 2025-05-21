using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;


namespace ObserverPattern
{
    public interface IObserver
    {
        public void Subscribe<IEvent>(Action<IEvent> observer, GameObject gameObject);
        public void UnSubscribe(Type type, object observer);
    }

}
