using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {
	
	public static bool crouch = false;

	public void ToOrOutCrouch () {
		crouch = !crouch;
	}
}