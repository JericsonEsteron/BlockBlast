using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;

namespace EventMessage
{
    public class EventMessenger
    {
        public static readonly IMessenger Default = new Observer();
    }

}
