using UnityEngine;
using System.Collections;

/// <summary>
/// Компонент искуственного интеллекта
/// </summary>

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
