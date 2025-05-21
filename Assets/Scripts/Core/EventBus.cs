using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    // Singleton pattern
    public static EventBus Instance { get; private set; }
    
    // Dictionary to store event subscribers
    private Dictionary<Type, List<Delegate>> eventSubscribers = new Dictionary<Type, List<Delegate>>();

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // Subscribe to event of type T
    public void Subscribe<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);

        if (!eventSubscribers.ContainsKey(eventType))
        {
            eventSubscribers[eventType] = new List<Delegate>();
        }
        
        eventSubscribers[eventType].Add(eventHandler);
    }
    
    // Unsubscribe from event of type T
    public void Unsubscribe<T>(Action<T> eventHandler)
    {
        Type eventType = typeof(T);
        
        if (eventSubscribers.ContainsKey(eventType))
        {
            eventSubscribers[eventType].Remove(eventHandler);
        }
    }
    
    // Publish event of type T
    public void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);
        
        if (!eventSubscribers.ContainsKey(eventType))
        {
            return;
        }
        
        foreach (var subscriber in eventSubscribers[eventType].ToArray())
        {
            var handler = subscriber as Action<T>;
            handler?.Invoke(eventData);
        }
    }
}
