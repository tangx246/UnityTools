using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Registrar<T> where T : Behaviour
{
    private readonly static HashSet<T> items = new();

    private readonly static UnityEvent<T> addEvent = new();
    private readonly static UnityEvent<T> removeEvent = new();

    public static void Register(T item)
    {
        items.Add(item);
        addEvent.Invoke(item);

        var destroyHook = item.gameObject.AddComponent<OnDestroyHook>();
        destroyHook.hook = () =>
        {
            items.Remove(item);
            removeEvent.Invoke(item);
        };
    }

    public static void AddRegisterListener(Behaviour listener, UnityAction<T> action)
    {
        AddListener(listener, action, addEvent);
    }

    private static void AddListener(Behaviour listener, UnityAction<T> action, UnityEvent<T> unityEvent)
    {
        unityEvent.AddListener(action);
        var destroyHook = listener.gameObject.AddComponent<OnDestroyHook>();
        destroyHook.hook = () => addEvent.RemoveListener(action);
    }

    public static void AddUnregisterListener(Behaviour listener, UnityAction<T> action)
    {
        AddListener(listener, action, removeEvent);
    }

    public static IEnumerable<T> GetAll()
    {
        return items;
    }
}

public class OnDestroyHook : MonoBehaviour
{
    public Action hook;

    private void OnDestroy()
    {
        hook();
    }
}