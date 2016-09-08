using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	#region Variables

	public enum EventNames{
		levelReset,
		levelFinish,
		dissolveFinish,
		launchBall
	};

	private Dictionary<EventNames, UnityEvent> eventDictionary;
	private static EventManager eventManager;

	public static EventManager instance{
		get{
			if(!eventManager){
				eventManager = FindObjectOfType<EventManager>();

				if(!eventManager)
					Debug.LogError("No Active EventManager in Scene!");
				else{
					eventManager.Init();
				}
			}

			return eventManager;
		}
	}

	#endregion
	#region Utility

	void Init(){
		if(eventDictionary == null)
			eventDictionary = new Dictionary<EventNames, UnityEvent>();
	}

	#endregion
	#region Event Functions

	public static void StartListening(EventNames eventName, UnityAction listener){
		UnityEvent thisEvent = null;

		if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
			thisEvent.AddListener(listener);
		else{
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(EventNames eventName, UnityAction listener){
		if(eventManager == null)
			return;

		UnityEvent thisEvent = null;

		if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
			thisEvent.RemoveListener(listener);
	}

	public static void TriggerEvent(EventNames eventName){
		UnityEvent thisEvent = null;

		if(instance.eventDictionary.TryGetValue(eventName, out thisEvent))
			thisEvent.Invoke();
	}

	#endregion
}
