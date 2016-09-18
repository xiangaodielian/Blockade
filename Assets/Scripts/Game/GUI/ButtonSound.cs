using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerDownHandler {

	public void OnPointerDown(PointerEventData eventData){
		if(GetComponent<Button>().interactable)
			GUIManager.Instance.PlayButtonSound();
	}
}
