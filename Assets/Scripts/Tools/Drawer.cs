using UnityEngine;
using System.Collections;

public class Drawer : MonoBehaviour
{
	[SerializeField]
	private Transform root;
	[SerializeField]
	private Color color;

	private void Update () {
		Draw (root, root);
	}

	private void Draw (Transform r, Transform ch) {

		Debug.DrawLine (r.position, ch.position, color);

		if (ch.childCount < 1) {
			return;
		}

		for (int i = 0; i < ch.childCount; i++) {
			Draw (ch, ch.GetChild(i));
		}
	}
}

