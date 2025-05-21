using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    public interface IDisposable
    {
        public void AddSubjectInDisposable<IEvent>(Subject<IEvent> observer, Observer observerInstance);
    }

}
