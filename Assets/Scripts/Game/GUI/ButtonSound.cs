using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ApplicationManagement;

public class ButtonSound : MonoBehaviour, IPointerDownHandler {

	public void OnPointerDown(PointerEventData eventData){
		if(GetComponent<Button>().interactable)
			GUIManager.Instance.PlayButtonSound();
	}
}
