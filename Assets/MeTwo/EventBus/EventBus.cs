using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<IEventListenerBase>> _subscribers;

    static EventBus()
    {
        _subscribers = new Dictionary<Type, List<IEventListenerBase>>();
    }
    
    public static void AddListener<TEvent>(IEventListener<TEvent> listener) where TEvent : struct
    {
        var type = typeof(TEvent);
        if (!_subscribers.ContainsKey(type))
        {
            _subscribers.Add(type, new List<IEventListenerBase>());
        }

        if (!IsAlreadySubscribed(type, listener))
        {
            _subscribers[type].Add(listener);
        }
    }

    public static void RemoveListener<TEvent>(IEventListener<TEvent> listener) where TEvent : struct
    {
        var type = typeof(TEvent);
        if (!_subscribers.TryGetValue(type, out var listenersList))return;
        for (int i = listenersList.Count - 1; i >= 0; i--)
        {
            if (listenersList[i] == listener)
            {
                listenersList.RemoveAt(i);
                if (listenersList.Count == 0)
                {
                    _subscribers.Remove(type);
                }
                return;
            }
        }
        
    }

    public static void TriggerEvent<TEvent>(TEvent e) where TEvent : struct
    {
        var type = typeof(TEvent);
        if (!_subscribers.TryGetValue(type, out var listenersList ))return;
        foreach (var t in listenersList)
        {
            (t as IEventListener<TEvent>)!.OnEvent(e);
        }
        
    }

    private static bool IsAlreadySubscribed(Type type, IEventListenerBase listener)
    {
        if (!_subscribers.TryGetValue(type, out var listenersList)) return false;
        foreach (var clistener in listenersList)
        {
            if (clistener == listener)
            {
                return true;
            } 
        }
        return false;
    }
}