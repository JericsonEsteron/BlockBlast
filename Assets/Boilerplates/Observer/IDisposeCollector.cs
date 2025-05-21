using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    public interface IDisposeCollector
    {
        public void AddDisposable<IEvent>(Subject<IEvent> observer, GameObject gameObject);
    }

}
