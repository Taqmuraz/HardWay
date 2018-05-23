using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Слот для зранения данных о материалах на сцене. Экспериментальный прототип, будет обновляться
/// </summary>

[System.Serializable]
public struct RenderSlot
{
	public Material material;
	public TexturesData main;
	public TexturesData transparent;

	public RenderSlot (Material mat) {
		material = mat;
		main = new TexturesData (material.mainTexture, material.GetTexture("_BumpMap"), material.color);
		transparent = new TexturesData (null, null, new Color (0.6f, 0.8f, 1, 0.5f));
	}

	public void SetMode (RenderMode mode, Shader[] shaders) {
		TexturesData data = (TexturesData)GetType ().GetField (mode.ToString()).GetValue (this);
		material.mainTexture = data.main;
		material.color = data.color;
		material.SetTexture ("_BumpMap", data.normals);
		material.shader = shaders [(int)mode];
	}
}
[System.Serializable]
public struct TexturesData
{
	public Texture main;
	public Texture normals;
	public Color color;

	public TexturesData (Texture _main, Texture _normals, Color clr) {
		main = _main;
		normals = _normals;
		color = clr;
	}
}

[System.Serializable]
public enum RenderMode
{
	main,
	transparent
}
/// <summary>
/// Нужен для осуществления перехода из одного режима отображения игры в другой
/// </summary>
public class RenderHolder : MonoBehaviour
{
	public List<RenderSlot> rendSlots = new List<RenderSlot>();

	public Shader[] shaders;


	public void SetRenderMode (RenderMode mode) {
		foreach (var item in rendSlots) {
			item.SetMode (mode, shaders);
		}
	}
	public void SetRenderMode (int mode) {
		SetRenderMode ((RenderMode)mode);
	}

	private void Start () {
		AddAll ();
	}
	public void AddAll () {
		IEnumerable<Renderer> rends = FindObjectsOfType<Renderer> ().Where ((Renderer arg) => (arg.gameObject.layer == LayerMask.NameToLayer("TR")));
		foreach (var r in rends) {
			Material[] mats = r.materials;
			foreach (var m in mats) {
				RenderSlot slot = new RenderSlot (m);
				rendSlots.Add (slot);
			}
		}
	}
}
