using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventEmitter<T> : ScriptableObject
{
    public TEvent eventEmitter = new TEvent();

    [Serializable]
    public class TEvent : UnityEvent<T> { }
}