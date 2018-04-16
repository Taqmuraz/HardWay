using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public void OnBeginDrag (PointerEventData data) {
		start_position = data.position;
		current_position = Vector2.zero;
		isDrag = true;
	}
	public void OnDrag (PointerEventData data) {
		current_position = data.position - start_position;
	}
	public void OnEndDrag (PointerEventData data) {
		isDrag = false;
		current_position = Vector2.zero;
	}
	public bool isDrag
	{
		get {
			return drag_getter;
		}
		set {
			drag_getter = value;
			if (joytype == JoyType.Dynamic) {
				point_obj.SetActive (drag_getter);
				back_obj.SetActive (drag_getter);
			}
		}
	}
	private bool drag_getter = false;
	public Vector2 start_position
	{
		get {
			if (joytype == JoyType.Dynamic) {
				return start_getter;
			} else {
				return back.position;
			}
		}
		set {
			start_getter = value;
			if (joytype == JoyType.Dynamic) {
				back.position = start_getter;
			}
		}
	}
	public Vector2 current_position
	{
		get {
			return current_getter;
		}
		set {
			current_getter = value;
			if (current_getter.magnitude > back.sizeDelta.y / 2) {
				current_getter = current_getter.normalized * back.sizeDelta.y / 2;
			}
			point.anchoredPosition = current_getter;
		}
	}
	private Vector2 start_getter = new Vector2 ();
	private Vector2 current_getter = new Vector2 ();
	public RectTransform point;
	public RectTransform back;
	public GameObject point_obj;
	public GameObject back_obj;

	public enum JoyType
	{
		Static,
		Dynamic
	}
	public JoyType joytype;
	public static Joystick joystick;
	private void Start () {
		if (joytype == JoyType.Dynamic) {
			joystick = this;
		}
	}
	private void Update () {
		Vector2 input = Vector2.zero;
		if (isDrag) {
			input = current_position / (back.sizeDelta.y / 2);
		}
		if (Application.isEditor) {
			Vector2 v = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			if (v.magnitude > 0) {
				input = v;
			}
		}
		InputData.joystick = input;
	}
}