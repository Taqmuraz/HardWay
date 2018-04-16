using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerDownHandler {

	public RectTransform rect;
	public GameObject obj;

	private bool active = true;

	public delegate void ToDo ();
	public ToDo toDo = delegate {
		return;
	};

	public void SetActive (bool act) {
		if (act != active) {
			active = act;
			obj.SetActive (act);
		}
	}

	private void Start () {
		rect = GetComponent<RectTransform> ();
		obj = gameObject;
	}

	public void OnPointerDown (PointerEventData data) {
		toDo ();
	}
}
