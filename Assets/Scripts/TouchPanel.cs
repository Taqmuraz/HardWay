using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IDragHandler
{
	public void OnDrag (PointerEventData data) {
		InputData.camera = data.delta;
	}
}