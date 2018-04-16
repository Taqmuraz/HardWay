using UnityEngine;
using System.Collections;

public class StalkerControl : StalkerBehaviour {

	private Transform cam_trans;
	private Transform cam_parent_trans;
	private Camera camera_main;
	private Vector3 camera_euler;

	public static StalkerBehaviour player;

	private void Start () {
		StalkerStart ();
		camera_main = Camera.main;
		cam_trans = camera_main.transform;
		cam_parent_trans = cam_trans.parent;
		camera_euler = Vector3.right * to_save_data.euler_x;
		TouchController.crouch = to_save_data.crouch;
		player = this;
		CameraDelegateData.onPreRender = delegate {
			CameraMotor();
		};
	}
	private void FixedUpdate () {
		StalkerFixedUpdate ();
	}
	private void Update () {
		StalkerSinhro ();
		MovingMotor ();
	}
	public void MovingMotor () {
		if (to_save_data.crouch != TouchController.crouch) {
			ToOrOutCrouch ();
		}
		lookWalkAngleDirection = cam_trans.forward;
		Vector3 m = (Vector3)InputData.joystick;
		m = new Vector3 (m.x, 0, m.y);

		walkQ = m.magnitude;

		if (type == MoveType.NoAngle) {
			camera_euler += Vector3.right * m.x * Time.deltaTime * 70;
		}
		this.MoveAtDirection (GetMainCameraAxes(m));
	}
	public Vector3 GetMainCameraAxes (Vector3 origin) {
		Vector3 f = cam_trans.forward;
		Vector3 r = cam_trans.right;
		f = new Vector3 (f.x, 0, f.z).normalized;
		r = new Vector3 (r.x, 0, r.z).normalized;
		return f * origin.z + r * origin.x;
	}
	public void CameraMotor () {
		Ray ray = new Ray (trans.position + Vector3.up * 1.6f + cam_trans.right * 0.5f,
		                   -cam_trans.forward);
		Vector3 c = cam_parent_trans.position;
		RaycastHit hit;
		float dist = 2;
		if (to_save_data.crouch) {
			dist = 1;
		}
		if (Physics.Raycast(ray, out hit, dist)) {
			if (hit.distance < dist) {
				dist = hit.distance;
			}
		}
		//Vector3 n = ray.GetPoint (dist);
		Vector3 n = trans.position + Vector3.up * 1.6f + cam_trans.right * 0.5f;
		cam_parent_trans.position = Vector3.Slerp (c, n, Time.deltaTime * 4);
		Vector3 v = (Vector3)InputData.camera;
		cam_trans.localPosition = -Vector3.forward * (dist - 0.1f);
		camera_euler += v * Settings.current.touch_sens / 10;
		camera_euler.y = Mathf.Clamp (camera_euler.y, -85, 85);
		Vector3 e = new Vector3(-camera_euler.y, camera_euler.x, 0);
		cam_parent_trans.rotation = Quaternion.Slerp (cam_trans.rotation,
		                                       Quaternion.Euler(e),
			Time.fixedDeltaTime * 6);
	}
}
