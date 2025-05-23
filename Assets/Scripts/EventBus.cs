using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus 
{
    private static Dictionary<string, Action<object>> listeners = new();
    public static void Subscribe(string eventName, Action<object> listener)
    {
        if(listeners.TryGetValue(eventName,out var exisiting)){
            listeners[eventName] = listener+exisiting;
        }
        else
            listeners[eventName] = listener;
    }
    public static void Unsubscribe(string eventName, Action<object> listener) {
        if (listeners.TryGetValue(eventName, out var exisiting)) { 
            exisiting-=listener;
            if (exisiting == null)
                listeners.Remove(eventName);
            else
                listeners[eventName] = exisiting;
        }
    }
    public static void Publish(string eventName, object param = null) { 
        if(listeners.TryGetValue(eventName,out var callback))
        {
            callback?.Invoke(param);
        }
    }
    public static void Clear()
    {
        listeners.Clear();
    }
}
