using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Camera))]
public class CameraDelegateData : MonoBehaviour
{

	public static ButtonScript.ToDo onPreRender = delegate {
		return;
	};

	private void OnPreRender () {
		onPreRender ();
	}
}

