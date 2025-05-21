using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObserverPattern
{
    [DisallowMultipleComponent]
    public class DisposalHandler : MonoBehaviour, IDisposable
    {
        private Observer observerInstance;
        private Dictionary<Type, List<object>> disposableDict = new Dictionary<Type, List<object>>();
        
        public void AddSubjectInDisposable<IEvent>(Subject<IEvent> observer, Observer observerInstance)
        {
            List<object> observers;
            this.observerInstance = observerInstance;
            if(disposableDict.TryGetValue(typeof(IEvent), out observers))
            {
                observers.Add(observer);
                disposableDict[typeof(IEvent)] = observers;
            }
            else
            {
                observers = new List<object>
                {
                    observer
                };
                disposableDict.Add(typeof(IEvent), observers);
            }
        }

        private void OnDestroy() 
        {
            foreach (Type key in disposableDict.Keys)
            {
                foreach (var observer in disposableDict[key])
                {
                    observerInstance.UnSubscribe(key, observer);
                }
            }
        }
    }

}
