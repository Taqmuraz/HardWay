using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerDownHandler {

	public RectTransform rect;

	public delegate void ToDo ();
	public ToDo toDo = delegate {
		return;
	};

	private void Start () {
		rect = GetComponent<RectTransform> ();
	}

	public void OnPointerDown (PointerEventData data) {
		toDo ();
	}
}
