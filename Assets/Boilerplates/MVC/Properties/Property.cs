using System;

namespace MVC
{
    public class Property<P> 
    {
        private P _value;

        public P Value
        {
            get => _value;
            set
            {
                if(_value != null && _value.Equals(value))
                    return;

                OnValueChangedWithPreviousNewValue?.Invoke(_value, value);

                _value = value;

                OnValueChanged?.Invoke();
                OnValueChangedWithNewValue?.Invoke(_value);

            }
        }

        public Action OnValueChanged;
        public Action<P> OnValueChangedWithNewValue;

        /// <summary>
        /// First P is previous Value, Second is the new Value
        /// </summary>
        public Action<P, P> OnValueChangedWithPreviousNewValue;
    }

}
