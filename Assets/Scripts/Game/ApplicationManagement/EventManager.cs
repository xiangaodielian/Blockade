/*
 * EventManager Class
 * Handles all GameEvent Delegate Functions
 */

using System;
using UnityEngine;
using System.Collections.Generic;

namespace ApplicationManagement {
    public class GameEvent { }

    public class EventManager : MonoBehaviour {

        #region Variables

        private static EventManager eventManager;
        public static EventManager Instance {
            get {
                if(eventManager)
                    return eventManager;

                eventManager = FindObjectOfType<EventManager>();

                if(!eventManager)
                    Debug.LogError("No Active EventManager in Scene!");

                return eventManager;
            }
        }

        public delegate void EventDelegate<T>(T e) where T : GameEvent;
        private delegate void EventDelegate(GameEvent e);
        private readonly Dictionary<Type, EventDelegate> delegateDictionary = new Dictionary<Type, EventDelegate>();
        private readonly Dictionary<Delegate, EventDelegate> delegateLookup = new Dictionary<Delegate, EventDelegate>();

        #endregion

        #region Event Functions

        /// <summary>
        /// Add a delegate listener to the EventDelegate.
        /// </summary>
        public void AddListener<T>(EventDelegate<T> addDelegate) where T : GameEvent {
            if(delegateLookup.ContainsKey(addDelegate))
                return;

            EventDelegate internalDelegate = (e) => addDelegate((T) e);
            delegateLookup[addDelegate] = internalDelegate;

            EventDelegate tempDelegate;
            if(delegateDictionary.TryGetValue(typeof(T), out tempDelegate)) {
                tempDelegate += internalDelegate;
                delegateDictionary[typeof(T)] = tempDelegate;
            } else
                delegateDictionary[typeof(T)] = internalDelegate;
        }

        /// <summary>
        /// Remove a delegate listener from the EventDelegate
        /// </summary>
        public void RemoveListener<T>(EventDelegate<T> removeDelegate) where T : GameEvent {
            EventDelegate internalDelegate;
            if(delegateLookup.TryGetValue(removeDelegate, out internalDelegate)) {
                EventDelegate tempDelegate;
                if(delegateDictionary.TryGetValue(typeof(T), out tempDelegate)) {
                    tempDelegate -= internalDelegate;
                    if(tempDelegate == null)
                        delegateDictionary.Remove(typeof(T));
                    else
                        delegateDictionary[typeof(T)] = tempDelegate;
                }

                delegateLookup.Remove(removeDelegate);
            }
        }

        /// <summary>
        /// Invoke GameEvent e.
        /// </summary>
        public void Raise(GameEvent e) {
            EventDelegate invokeDelegate;
            if(delegateDictionary.TryGetValue(e.GetType(), out invokeDelegate))
                invokeDelegate.Invoke(e);
        }

        #endregion
    }
}