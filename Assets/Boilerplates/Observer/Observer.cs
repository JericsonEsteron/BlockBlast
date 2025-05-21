using System;
using System.Collections.Generic;
using EventMessage;
using UnityEngine;

namespace ObserverPattern
{
    public class Observer : IMessenger, IDisposeCollector
    {
        private readonly Dictionary<Type, List<object>> observerDict = new();
        private readonly Dictionary<Type, List<object>> observersToRemove = new();
        private readonly object _lock = new();

        private int publishDepth = 0;

        public void Publish<IEvent>(IEvent eventMessage)
        {
            List<object> observers;

            lock (_lock)
            {
                if (!observerDict.TryGetValue(typeof(IEvent), out observers))
                    return;

                publishDepth++;
            }

            // Invoke outside lock to avoid deadlocks if user code re-enters
            foreach (Subject<IEvent> subject in observers)
            {
                subject.InvokeAction(eventMessage);
            }

            lock (_lock)
            {
                publishDepth--;

                if (publishDepth == 0)
                    RemoveObservers();
            }
        }

        public void Subscribe<IEvent>(Action<IEvent> observer, GameObject gameObject)
        {
            var newObserver = new Subject<IEvent>(observer);
            var type = typeof(IEvent);

            lock (_lock)
            {
                if (!observerDict.TryGetValue(type, out var observers))
                {
                    observers = new List<object>();
                    observerDict.Add(type, observers);
                }

                observers.Add(newObserver);
            }

            AddDisposable(newObserver, gameObject);
        }

        public void UnSubscribe(Type type, object observer)
        {
            lock (_lock)
            {
                if (publishDepth == 0)
                {
                    if (observerDict.TryGetValue(type, out var observers))
                    {
                        observers.Remove(observer);
                    }
                }
                else
                {
                    if (!observersToRemove.TryGetValue(type, out var toRemove))
                    {
                        toRemove = new List<object>();
                        observersToRemove[type] = toRemove;
                    }
                    toRemove.Add(observer);
                }
            }
        }

        private void RemoveObservers()
        {
            foreach (var pair in observersToRemove)
            {
                if (observerDict.TryGetValue(pair.Key, out var observers))
                {
                    foreach (var observer in pair.Value)
                    {
                        observers.Remove(observer);
                    }
                }
            }

            observersToRemove.Clear();
        }

        public void AddDisposable<IEvent>(Subject<IEvent> observer, GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<IDisposable>(out var disposable))
            {
                disposable = gameObject.AddComponent<DisposalHandler>();
            }

            disposable.AddSubjectInDisposable(observer, this);
        }

        public void GetDictionary()
        {
            lock (_lock)
            {
                foreach (var key in observerDict.Keys)
                {
                    Debug.Log($"{key} : {observerDict[key].Count}");
                }
            }
        }
    }

    public class Subject<T>
    {
        Action<T> OnInvoke;

        public Subject(Action<T> OnInvoke)
        {
            this.OnInvoke = OnInvoke;
        }

        public void InvokeAction(T eventMessage)
        {
            OnInvoke?.Invoke(eventMessage);
        }
    }

}
