using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventManager : MonoBehaviour
{
    public delegate void OnEvent(params object[] parameterContainer);
    
    static Dictionary<EventID, OnEvent> eventList;

    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventManager>();
                if (instance == null)
                {
                    instance = new GameObject("new EventManager Object").AddComponent<EventManager>().GetComponent<EventManager>();
                    eventList = new Dictionary<EventID, OnEvent>();
                }
            }
            return instance;
        }
    }

    public void Subscribe(EventID evID, OnEvent listener)
    {
        if (!eventList.ContainsKey(evID)) eventList.Add(evID, null);
        eventList[evID] += listener;
    }

    public void Unsubscribe(EventID evID, OnEvent listener)
    {
        if (eventList.ContainsKey(evID)) eventList[evID] -= listener;
    }

    public void DispatchEvent(EventID evID)
    {
        DispatchEvent(evID, null);
    }

    public void DispatchEvent(EventID evID, params object[] paramContainer)
    {
        if (eventList.ContainsKey(evID) && eventList[evID] != null) eventList[evID](paramContainer);
    }
}

public enum EventID
{
    SCENE_CHANGED,
}
