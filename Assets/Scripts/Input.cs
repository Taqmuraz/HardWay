using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс собирает в себе информацию об управлении игрока
/// </summary>

public class InputData
{
	public static Vector2 joystick
	{
		get {
			Vector2 win = new Vector2(Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
			return win.magnitude > joyGetter.magnitude ? win : joyGetter;
		}
		set {
			joyGetter = value;
		}
	}
	private static Vector2 joyGetter;
	public static Vector2 camera
	{
		get {
			Vector2 c = camGetter;
			camGetter = Vector2.zero;
			return c;
		}
		set {
			camGetter = value;
		}
	}
	private static Vector2 camGetter = new Vector2();
}
