using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class ButtonPressedEvent : IEvent
    {
        public string message;
        public ButtonPressedEvent(string message)
        {
            this.message = message;
        }
    }

}
