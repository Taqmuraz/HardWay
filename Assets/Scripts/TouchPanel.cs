using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Испотзуется для получения информации о свайпах игрока по зоне для управления камерой
/// </summary>

public class TouchPanel : MonoBehaviour, IDragHandler
{
	public void OnDrag (PointerEventData data) {
		InputData.camera = data.delta;
	}
}