using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityTools
{
    public class Resource<T> : MonoBehaviour where T : IComparable
    {
        public T maxValue;

        [SerializeField]
        private T _value;
        public T value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value.CompareTo(maxValue) > 0)
                {
                    _value = maxValue;
                }
                else
                {
                    _value = value;
                }

                resourceUpdated.Invoke(_value);
            }
        }
        public ResourceUpdated resourceUpdated = new ResourceUpdated();

        [Serializable]
        public class ResourceUpdated : UnityEvent<T> { }

        public virtual void Start()
        {
            resourceUpdated.Invoke(value);
        }
    }
}