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
			if (Menu.menu.menu_state != MenuState.Runtime) {
				joyGetter = Vector2.zero;
			}
			return joyGetter;
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
