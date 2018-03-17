using UnityEngine;
using System.Collections;

public class StalkerAI : StalkerBehaviour {
	public void Start () {
		StalkerStart ();
	}
	public void Update () {
		StalkerSinhro ();
		AI ();
	}
	public void AI () {
	}
	private void FixedUpdate () {
		StalkerFixedUpdate ();
	}
}
