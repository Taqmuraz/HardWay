using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Нужен для сохранения предметов лута на локации. Хранит в себе позицию лута и ID хранящегося предмета
/// </summary>

[System.Serializable]
public class ItemSavable
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

	public int id = 0;
}
/// <summary>
/// MonoBehaviour компонент для хранения данных о выброшеннои предмете
/// </summary>
public class ItemObject : MonoBehaviour {
	public ItemSavable sav;
	public Transform trans;
	public GameObject button;

	public static List<ItemObject> itemsAll = new List<ItemObject> ();

	private bool IsNearStalker (StalkerBehaviour stalker)
	{
		return (stalker.trans.position - trans.position).magnitude < 2;
	}

	private void Start () {
		trans = transform;
		itemsAll.Add (this);
	}

	private void OnDestroy () {
		itemsAll.Remove (this);
	}

	private void Update () {
		sav.position = trans.position;
	}

	public static ItemObject[] GetNearestAtPoint (Vector3 point, float dist) {
		ItemObject[] objs = itemsAll.ToArray ();
		List<ItemObject> list = new List<ItemObject> ();
		for (int i = 0; i < objs.Length; i++) {
			float m = (objs [i].trans.position - point).magnitude;
			if (m < dist) {
				list.Add (objs[i]);
			}
		}
		return list.ToArray ();
	}
}
