using UnityEngine;
using System.Collections;

/// <summary>
/// Компонент, присутствующий на камере. Нужен для хранения делегата toDo и выполнения его в OnPreRender
/// </summary>
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

