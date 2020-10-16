using UnityEngine.Events;

namespace UnityTools
{
    public class EventVar<T>
    {
        public readonly UnityEvent<T> valueChangedEvent = new UnityEvent<T>();
        private T value;

        public EventVar(T initialValue) 
        {
            value = initialValue;
        }

        public void Set(T value)
        {
            this.value = value;
            valueChangedEvent.Invoke(value);
        }

        public T Get()
        {
            return value;
        }
    }
}