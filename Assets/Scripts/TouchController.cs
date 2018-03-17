using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {
	public static Vector2 getMoveVector
	{
		get {
			Vector2 v = move_input;
			if (Application.isEditor) {
				v = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				if (!Input.GetKey(KeyCode.LeftShift)) {
					v /= 4;
				}
			}
			if (v.magnitude > 1) {
				v = v.normalized;
			} else {
				float m = v.magnitude;
				float d = ((float)((int)(m * 4))) / 4;
				v = v.normalized * d;
			}
			return v;
		}
	}
	public bool cameraDrag = false;
	public bool moveDrag = false;
	public static bool crouch = false;
	public int finger_move_ID;
	public int finger_camera_ID;
	public static Vector2 move_input = new Vector2();
	public static Vector2 camera_input = new Vector2();
	public Vector2 startPosition_move = new Vector2();
	public RectTransform joyRoot;
	public RectTransform joyBone;
	[Header ("Testing")]
	public Text inputText;

	void Update () {
		if (finger_camera_ID < 0) {
			EndDrag(false);
		}
		if (finger_move_ID < 0) {
			EndDrag(true);
		}
		if (cameraDrag) {
			Drag(false);
		}
		if (moveDrag) {
			joyBone.anchoredPosition = startPosition_move + move_input * Screen.height / 6;
			joyRoot.anchoredPosition = startPosition_move;
			joyBone.gameObject.SetActive(true);
			joyRoot.gameObject.SetActive(true);
			Drag (true);
		} else {
			joyBone.gameObject.SetActive(false);
			joyRoot.gameObject.SetActive(false);
		}
		if (inputText) {
			inputText.text = "move : " + move_input + '\n' + "camera : " + camera_input;
		}
	}
	public Touch GetTouchFromID (int id, bool move) {
		Touch touch = new Touch();
		bool has = false;
		for (int i = 0; i < Input.touchCount; i++) {
			Touch cur = Input.GetTouch(i);
			if (cur.fingerId == id) {
				touch = cur;
				has = true;
				break;
			}
		}
		if (!has) {
			if (move) {
				finger_move_ID = -1;
			} else {
				finger_camera_ID = -1;
			}
		}
		return touch;
	}
	public void StartDrag (bool move) {
		//Debug.Log ("Started drag!");
		if (move) {
			moveDrag = true;
		} else {
			cameraDrag = true;
		}
		CheckDrag (move);
	}
	void CheckDrag (bool forMove) {
		float maxX = Screen.width / 2;
		float minX = 0;
		bool has = false;
		if (!forMove) {
			maxX = Screen.width;
			minX = Screen.width / 2;
		}
		for (int i = 0; i < Input.touchCount; i++) {
			Touch currentTouch = Input.GetTouch(i);
			Vector2 pos = currentTouch.position;
			if (pos.x < maxX && pos.x > minX) {
				if (forMove) {
					finger_move_ID = currentTouch.fingerId;
					startPosition_move = pos;
				} else {
					finger_camera_ID = currentTouch.fingerId;
				}
				has = true;
				break;
			}
		}
		if (!has) {
			EndDrag(forMove);
		}
	}

	void Drag (bool forMove) {
		Vector2 inp = Vector2.zero;
		float screenJoy = Screen.height / 6;
		if (forMove) {
			Touch touch = GetTouchFromID (finger_move_ID, true);
			if (finger_move_ID > -1) {
				inp = ((touch.position - startPosition_move) / screenJoy);
				if (inp.magnitude > 1) {
					inp = inp.normalized;
				}
			}
			move_input = inp;
		} else {
			Touch touch = GetTouchFromID (finger_camera_ID, false);
			if (finger_camera_ID > -1) {
				inp = (touch.deltaPosition);
			}
			camera_input = inp;
		}
	}
	public void EndDrag (bool move) {
		//Debug.Log ("Ended drag!");
		if (move) {
			finger_move_ID = -1;
			move_input = Vector2.zero;
			moveDrag = false;
		} else {
			finger_camera_ID = -1;
			camera_input = Vector2.zero;
			cameraDrag = false;
		}
	}
	public void ToOrOutCrouch () {
		crouch = !crouch;
	}
}