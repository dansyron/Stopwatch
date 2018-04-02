using System.Collections.Generic;
using UnityEngine;
using System;

//Event delegate
public delegate void CustomEvent();

/// <summary>
/// This enumeration contains all the possible types of events that will ever be triggered
/// </summary>
enum EventType
{
    PlayerDeath,
    LevelWin,
    GainTime,
    //as you need more, add them here.
};

/// <summary>
/// Event manager incharge of managing game events.
/// </summary>
class EventManager
{
    #region Fields

    static EventManager instance;

    //initialize delegates
    Dictionary<EventType, CustomEvent> events = new Dictionary<EventType, CustomEvent>();

    #endregion

    #region Constructor

    /// <summary>
    /// Private constructor
    /// </summary>
    private EventManager()
    {
        //Any event you intialize should be done by EventManager.Instance.RegisterForEvent();
        //If you need to add a new event type then add it in the enumeration at the top of this file.
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the singleton instance of the event manager
    /// </summary>
    public static EventManager Instance
    {
        get { return instance ?? (instance = new EventManager()); }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Registers a method to be triggered whenever an event is fired
    /// </summary>
    /// <param name="eventType">The type of event you want to register the method to</param>
    /// <param name="method">The method that you want called whenever the event is fired</param>
    public void RegisterToEvent(EventType eventType, CustomEvent method)
    {
        //if the dictionary already contains the event type
        if (events.ContainsKey(eventType))
        {
            events[eventType] -= method; //Prevents adding the same method twice. 
            events[eventType] += method;
        }
        //if the event type has yet to have something registered to it
        else
        {
            events.Add(eventType, method);
        }
    }

    /// <summary>
    /// Removes a method so that it does not get called whenever the event is fired
    /// </summary>
    /// <param name="eventType">The type of event that you want to remove it from</param>
    /// <param name="method">The method you want to remove from the event</param>
    public void UnregisterFromEvent(EventType eventType, CustomEvent method)
    {
        //if the dictionary already contains the event type
        if (events.ContainsKey(eventType))
        {
            events[eventType] -= method;
            if (events[eventType] == null)
            {
                events.Remove(eventType);
            }
        }
        //if the event type has yet to have something registered to it
        else
        {
            Debug.Log("No event of this type has been registered");
        }
    }

    /// <summary>
    /// Fires an event
    /// </summary>
    /// <param name="eventType">The type of event we want to fire</param>
    public void FireEvent(EventType eventType)
    {
        if (events.ContainsKey(eventType))
        {
            try
            {
                events[eventType]();
            }
            catch(MissingReferenceException)
            {
                Debug.LogError("You forgot to unregister an object to an event when it got deleted. Double click me and read the comment below...");

                // Somewhere, somehow, an object registered to an event and then the object was deleted without unregistering itself.
                // Unfortunately I don't think it is possible to automatically detect this and remove the method in the script attached to the object you deleted from the event.
                // Because of this unfortunate circumstance, you will need to manually unregister everything you registered on the object as it is deleted.
                // The best practice to do so is as follows:
                //
                // if your script registers to an event, at any given time ever, create a "void OnDestroy()" method. If this script is a monobehaviour this method will automatically be called when it is destroyed.
                // you have to unsubscribe in this "OnDestroy" method to every event this object can possibly subscribe to
                //
                // EX:
                //void Start()
                //{
                //    EventManager.Instance.RegisterToEvent(EventType.PlayerDeath, CalledWhenPlayerDies);
                //    EventManager.Instance.RegisterToEvent(EventType.LevelWin, CalledWhenLevelIsWon);
                //}

                //void OnDestroy()
                //{
                //    EventManager.Instance.UnregisterFromEvent(EventType.PlayerDeath, CalledWhenPlayerDies);
                //    EventManager.Instance.UnregisterFromEvent(EventType.LevelWin, CalledWhenLevelIsWon);
                //}
            }
        }
        else
        {
            Debug.Log("Attempting to fire an event that has nothing registered to it. Did you forget to call EventManager.Instance.RegisterToEvent?");
        }
    }

    /// <summary>
    /// Performs a clean sweep on the event manager so that no events have been registered
    /// </summary>
    public void ClearEvents()
    {
        events.Clear();
    }

    #endregion
}

