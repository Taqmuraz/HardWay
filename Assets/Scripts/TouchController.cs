using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Методы управления игроком. Требуют переноса в класс Joystick
/// </summary>

public class TouchController : MonoBehaviour {
	
	public static bool crouch = false;

	public void ToOrOutCrouch () {
		crouch = !crouch;
	}
}