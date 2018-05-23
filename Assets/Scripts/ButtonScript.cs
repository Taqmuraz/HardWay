using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// упрощенный компонент кнопки, делегат toDo выполняется при нажатии
/// </summary>
public class ButtonScript : MonoBehaviour, IPointerDownHandler {

	public RectTransform rect;
	public GameObject obj;

	private bool active = true;

	public delegate void ToDo ();
	public ToDo toDo = delegate {
		return;
	};
	/// <summary>
	/// Активация/деактивация объекта
	/// </summary>
	/// <param name="act">If set to <c>true</c> act.</param>
	public void SetActive (bool act) {
		if (act != active) {
			active = act;
			obj.SetActive (act);
		}
	}
	/// <summary>
	/// Инициализация
	/// </summary>
	private void Start () {
		rect = GetComponent<RectTransform> ();
		obj = gameObject;
	}
	/// <summary>
	/// Реализация метода интерфейса IPointerDown
	/// </summary>
	/// <param name="data">Data.</param>
	public void OnPointerDown (PointerEventData data) {
		toDo ();
	}
}
