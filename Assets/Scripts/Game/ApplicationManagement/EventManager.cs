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
        private Dictionary<Delegate, EventDelegate> delegateLookup = new Dictionary<Delegate, EventDelegate>();

        #endregion

        #region Event Functions

        public void AddListener<T>(EventDelegate<T> addDelegate) where T : GameEvent {
            if(delegateLookup.ContainsKey(addDelegate))
                return;

            EventDelegate internalDelegate = (e) => addDelegate((T) e);
            delegateLookup[addDelegate] = internalDelegate;

            EventDelegate tempDelegate;
            if(delegateDictionary.TryGetValue(typeof(T), out tempDelegate))
                delegateDictionary[typeof(T)] = tempDelegate += internalDelegate;
            else
                delegateDictionary[typeof(T)] = internalDelegate;
        }

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

        public void Raise(GameEvent e) {
            EventDelegate invokeDelegate;
            if(delegateDictionary.TryGetValue(e.GetType(), out invokeDelegate))
                invokeDelegate.Invoke(e);
        }

//        /// <summary>
//        ///     Add a zero-argument UnityAction as a delegate listener.
//        /// </summary>
//        /// <param name="listener">Listener method to add.</param>
//        public static void StartListening(EventNames eventName, UnityAction listener) {
//            UnityEvent thisEvent;
//
//            if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
//                thisEvent.AddListener(listener);
//            else {
//                thisEvent = new UnityEvent();
//                thisEvent.AddListener(listener);
//                Instance.eventDictionary.Add(eventName, thisEvent);
//            }
//        }
//
//        /// <summary>
//        ///     Add a delegate listener for the SceneManager.sceneLoaded UnityEvent.
//        /// </summary>
//        /// <param name="listener">Listener method to add.</param>
//        public static void StartListening(UnityAction<Scene, LoadSceneMode> listener) {
//            SceneManager.sceneLoaded += listener;
//        }
//
//        /// <summary>
//        ///     Removea zero-argument UnityAction as a delegate listener.
//        /// </summary>
//        public static void StopListening(EventNames eventName, UnityAction listener) {
//            if(eventManager == null)
//                return;
//
//            UnityEvent thisEvent;
//
//            if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
//                thisEvent.RemoveListener(listener);
//        }
//
//        /// <summary>
//        ///     Remove a delegate listener for the SceneManager.sceneLoaded UnityEvent.
//        /// </summary>
//        public static void StopListening(UnityAction<Scene, LoadSceneMode> listener) {
//            SceneManager.sceneLoaded -= listener;
//        }
//
//        /// <summary>
//        ///     Invoke all active listeners for the UnityEvent.
//        /// </summary>
//        public static void TriggerEvent(EventNames eventName) {
//            UnityEvent thisEvent;
//
//            if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
//                thisEvent.Invoke();
//        }

        #endregion
    }
}