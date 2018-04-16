using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputData
{
	public static Vector2 joystick = new Vector2();
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
