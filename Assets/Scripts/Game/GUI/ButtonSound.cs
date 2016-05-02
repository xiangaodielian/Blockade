/*----------------------------/
  ButtonSound Class - Blockade
  Manages audio functions for
  Button highlight and click
  Writen by Joe Arthur
  Latest Revision - 2 May, 2016
/-----------------------------*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerDownHandler {

	public void OnPointerDown(PointerEventData eventData){
		UIManager.instance.PlayButtonSound();
	}
}
