using System;
using System.Collections.Generic;
using UnityEngine;

// Source: http://bernardopacheco.net/using-an-event-manager-to-decouple-your-game-in-unity

public class EventManager : MonoBehaviour
{
    private Dictionary<string, Action<Dictionary<string, object>>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(instance);
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();
        }
    }

    public static void StartListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (instance.eventDictionary.TryGetValue(eventName, 
            out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent += listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }

        Debug.Log("Now listening to " + eventName);
    }

    public static void StopListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (eventManager == null) return;
        if (instance.eventDictionary.TryGetValue(eventName, 
            out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent -= listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, Dictionary<string, object> message)
    {
        if (instance.eventDictionary.TryGetValue(eventName, 
            out Action<Dictionary<string, object>> thisEvent))
        {
            thisEvent?.Invoke(message);
        }
    }
}