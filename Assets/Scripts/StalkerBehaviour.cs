using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[System.Serializable]
public class ToSaveData
{
	public float x = 0;
	public float y = 0;
	public float z = 0;

	public Vector3 position
	{
		get {
			return new Vector3 (x, y, z);
		}
		set {
			Vector3 v = value;
			x = v.x;
			y = v.y;
			z = v.z;
		}
	}

	public string dialog = null;

	public float euler_x = 0;
	public bool player = false;
	//
	public Inventory inventory = new Inventory();
	//
	public bool crouch = false;
	//
	public int money = 950;
	//
	public enum Group 
	{
		Standart,
		Army
	}
	public ToSaveData.Group group = ToSaveData.Group.Standart;
}

public enum MoveType
{
	NoAngle,
	Angle
}

public class StalkerBehaviour : MonoBehaviour {
	public UnityEngine.AI.NavMeshAgent agent;
	public Transform trans;
	public Animator anims;

	public static List<StalkerBehaviour> behaviours = new List<StalkerBehaviour> ();

	[Header("For IK")]
	public Transform leftLegVector;
	public Transform rightLegVector;
	public Transform leftFoot;
	public Transform rightFoot;
	[Header("For audio")]
	public AudioSource audioBeh;

	public MoveType type;

	//

	float attackDeltaTime = 0.25f;

	public float moveSpeed;

	private StalkerBehaviour attacking
	{
		get {
			return null;
		}
	}

	//

	public ToSaveData to_save_data = new ToSaveData();

	//

	private void Start () {

		StalkerBehaviour s = null;

		if (to_save_data.player) {
			s = gameObject.AddComponent<StalkerControl> ();
		} else {
			s = gameObject.AddComponent<StalkerAI> ();

		}

		if (s) {
			anims = GetComponent<Animator> ();
			s.type = type;
			s.to_save_data = to_save_data;
			s.moveSpeed = moveSpeed;
			s.leftLegVector = leftLegVector;
			s.rightLegVector = rightLegVector;
			s.leftFoot = anims.GetBoneTransform (HumanBodyBones.LeftFoot);
			s.rightFoot = anims.GetBoneTransform (HumanBodyBones.RightFoot);
		}

		Destroy (this);
	}

	public void PlaySound (AudioClip clip, float volume) {
		if (!audioBeh) {
			Transform head = anims.GetBoneTransform (HumanBodyBones.Head);
			AudioSource s = head.GetComponent<AudioSource> ();
			if (!s) {
				audioBeh = head.gameObject.AddComponent<AudioSource> ();
			} else {
				audioBeh = s;
			}
		}
		audioBeh.spatialBlend = 1;
		audioBeh.volume = volume;
		audioBeh.clip = clip;
		audioBeh.Play ();
	}


	public void OnDestroy () {
		behaviours.Remove (this);
	}

	public void StalkerStart () {
		agent = GetComponent<NavMeshAgent> ();
		trans = GetComponent<Transform> ();
		anims = GetComponent<Animator> ();

		behaviours.Add (this);

		if (!anims) {
			anims = GetComponentInChildren<Animator> ();
		}
		if (!agent) {
			agent = GetComponentInChildren<NavMeshAgent> ();
		}

		if (Game.gt == Game.GameType.LoadedGame) {
			agent.Warp(to_save_data.position);
			trans.eulerAngles = Vector3.up * to_save_data.euler_x;
		}
	}

	public Quaternion GetSafetyLookEuler (Vector3 forward) {
		forward = new Vector3 (forward.x, 0, forward.z).normalized;

		return Quaternion.LookRotation (forward);
	}

	public void SafetyLookAt (Vector3 forward) {
		Quaternion rot = GetSafetyLookEuler (forward);
		trans.rotation = Quaternion.Slerp (trans.rotation, rot, Time.deltaTime * 7);
	}

	public bool inMove
	{
		get {
			return agent.velocity.magnitude > 0.2f;
		}
	}

	public Vector3 lookWalkAngleDirection;

	public void StalkerSinhro () {
		Vector3 pos = trans.position;
		to_save_data.x = pos.x;
		to_save_data.y = pos.y;
		to_save_data.z = pos.z;
		to_save_data.euler_x = trans.eulerAngles.y;
		if (standing) {
			agent.speed = 0;
			agent.velocity = Vector3.zero;
		} else {
			float msp = moveSpeed;
			if (to_save_data.crouch) {
				msp = moveSpeed / 6;
			}
			agent.speed = msp * walkQ;
		}
		if (to_save_data.inventory.health < 1) {
			Die();
		}
		AnimationSinhro ();
	}

	public void StopWhileStanding () {
		return;
	}
	public bool standing
	{
		get {
			return IsInvoking ("StopWhileStanding");
		}
	}

	void AnimationSinhro () {
		bool crouch = to_save_data.crouch;
		float moveK = agent.velocity.magnitude / agent.speed * walkQ;
		anims.SetBool ("InStand", !crouch);
		anims.SetFloat ("MoveK", moveK);
	}
	public void StalkerFixedUpdate () {
		float off = 0f;
		if (to_save_data.crouch) {
			RaycastHit hit;
			Vector3[] vs = {simplePosition + trans.right * 0.25f,
				simplePosition - trans.right * 0.1f,
				simplePosition + trans.forward * 0.1f,
				simplePosition - trans.forward * 0.1f,
				simplePosition};
			for (int i = 0; i < vs.Length; i++) {
				if (Physics.Raycast (vs[i] + Vector3.up, Vector3.down, out hit, 1.5f)) {
					float f = (simplePosition - hit.point).y;
					if (f < off) {
						off = f;
					}
				}
			}
		} else {
			RaycastHit hit;
			if (Physics.Raycast(simplePosition + Vector3.up, Vector3.down, out hit, 1.5f)) {
				off = (simplePosition - hit.point).y;
			}
		}
		float time = Time.fixedDeltaTime * 8;
		agent.baseOffset = Mathf.Lerp(agent.baseOffset,-off * agentBaseK, time);
	}
	Vector3 l_leg
	{
		get {
			return l_leg_getter;
		}
		set {
			l_leg_getter = Vector3.Slerp (l_leg_getter, value, Time.deltaTime * 6);
		}
	}
	Vector3 l_leg_getter;
	Vector3 r_leg
	{
		get {
			return r_leg_getter;
		}
		set {
			r_leg_getter = Vector3.Slerp (r_leg_getter, value, Time.deltaTime * 6);
		}
	}
	Vector3 r_leg_getter;
	bool left_ik_step
	{
		get {
			float f_y = leftFoot.position.y;
			float t_y = (trans.position + trans.up * 0.15f).y;
			Vector3 delta = leftFoot.position - (trans.position - trans.forward * 0.1f);
			float angle = Vector3.Angle (delta, trans.forward);

			bool ik = f_y < t_y && angle < 50;

			if (ik != l_ik_getter) {
				if (ik) {

					Ray ray = new Ray (leftFoot.position + trans.up, Vector3.down);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 1.25f)) {
						l_step = hit.point;
					}
				}
			}

			if (ik) {
				Debug.DrawLine (trans.position, l_step, Color.red);
			} else {
				Debug.DrawLine (leftFoot.position, trans.position, Color.blue);
			}

			//Debug.Log ("Fy = " + f_y + "; ty  = " + t_y + "; angle = " + angle + "; ik = " + ik);

			l_ik_getter = ik;

			return ik;
		}
	}
	private bool l_ik_getter = false;
	private bool r_ik_getter = false;
	bool right_ik_step
	{
		get {
			float f_y = rightFoot.position.y;
			float t_y = (trans.position + trans.up * 0.15f).y;
			Vector3 delta = rightFoot.position - (trans.position - trans.forward * 0.1f);
			float angle = Vector3.Angle (delta, trans.forward);

			bool ik = f_y < t_y && angle < 50; 

			if (ik != r_ik_getter) {
				if (ik) {
					Ray ray = new Ray (rightFoot.position + trans.up, Vector3.down);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 1.25f)) {
						r_step = hit.point;
					}
				}
			}

			if (ik) {
				Debug.DrawLine (trans.position, r_step, Color.red);
			} else {
				Debug.DrawLine (rightFoot.position, trans.position, Color.blue);
			}

			//Debug.Log ("Fy = " + f_y + "; ty  = " + t_y + "; angle = " + angle + "; ik = " + ik);

			r_ik_getter = ik;

			return ik;
		}
	}
	Vector3 l_step;
	Vector3 r_step;

	float left_weight
	{
		get {
			return leftWeightGetter;
		}
		set {
			leftWeightGetter = Mathf.Lerp (leftWeightGetter, value, Time.deltaTime * 4);
		}
	}
	float leftWeightGetter = 0;

	float right_weight
	{
		get {
			return rightWeightGetter;
		}
		set {
			rightWeightGetter = Mathf.Lerp (rightWeightGetter, value, Time.deltaTime * 4);
		}
	}
	float rightWeightGetter = 0;

	public bool IsNearStalker (StalkerBehaviour stalker)
	{
		return (stalker.trans.position - trans.position).magnitude < 2;
	}

	public void OnAnimatorIK (int layerIndex) {
		float dir_l = (leftLegVector.position.y - leftFoot.position.y);
		float dir_r = (rightLegVector.position.y - rightFoot.position.y);

		float b_l = 0;
		float b_r = 0;

		if (agent.velocity.magnitude > 0.1f) {

		} else {
			if (!to_save_data.crouch) {

				float moveK = 0;

				RaycastHit hit;

				float speed = Time.deltaTime * 8;

				if (Physics.Raycast (leftLegVector.position - trans.right * 0.1f, Vector3.down, out hit, dir_l)) {
					l_leg = Vector3.Slerp (l_leg, hit.point + hit.normal * 0.15f, speed);
					b_l = 1 - moveK;
				}
				if (Physics.Raycast (rightLegVector.position + trans.right * 0.1f, Vector3.down, out hit, dir_r)) {
					r_leg = Vector3.Slerp (r_leg, hit.point + hit.normal * 0.15f, speed);
					b_r = 1 - moveK;
				}
			}
		}



		if (b_l < 1) {
			l_leg = leftFoot.position;
		}
		if (b_r < 1) {
			r_leg = rightFoot.position;
		}

		left_weight = b_l;
		right_weight = b_r;

		//Debug.Log (left_weight);

		//Debug.Log ("b_l = " + b_l);

		anims.SetIKPositionWeight (AvatarIKGoal.LeftFoot, left_weight);
		anims.SetIKPositionWeight (AvatarIKGoal.RightFoot, right_weight);

		anims.SetIKPosition (AvatarIKGoal.LeftFoot, l_leg);
		anims.SetIKPosition (AvatarIKGoal.RightFoot, r_leg);
	}
	public float walkQ = 1;
	public void MoveTo (Vector3 to) {
		MoveAtDirection (to - trans.position);
	}
	public bool MayAttack (StalkerBehaviour who, StalkerBehaviour at) {
		return true;
	}
	public int GetDamage () {
		return 10;
	}
	public void Die () {
		// animate death
		Destroy (agent);
		this.enabled = false;
	}
	public Transform FindCoverPoint () {
		return null;
	}
	public void MoveAtDirection (Vector3 direction) {
		direction.Normalize ();
		agent.SetDestination (simplePosition + direction);
	}
	private Vector3 simplePosition
	{
		get {
			return trans.position - Vector3.up * agent.baseOffset / agentBaseK;
		}
	}
	private float agentBaseK = 1f / 35f;
	public void ToOrOutCrouch () {
		to_save_data.crouch = !to_save_data.crouch;
		Invoke ("StopWhileStanding", 1.0f);
	}
	private bool haveToStop
	{
		get {
			return IsInvoking ("StopWhileStanding");
		}
	}
	public void Attack (StalkerBehaviour at) {
		if (!IsInvoking("AttackEnd")) {
			Invoke("AttackEnd", attackDeltaTime);
		}
	}
	void AttackEnd () {
		if (MayAttack (this, attacking)) {
			// attack
		}
	}
	public void ApplyDamage (float damage) {
		to_save_data.inventory.health -= damage;
		to_save_data.inventory.energy -= damage / 4;
	}
}