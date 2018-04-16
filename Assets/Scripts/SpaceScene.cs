using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceScene : MonoBehaviour {
	
	public static int level = 0;
	public Text loadProcentText;
	public Image loadProcentImage;
	
	public static void LoadLevel (int levelname) {
		level = levelname;
		LoadLevelNow (1);
	}

	public static AsyncOperation operation;

	public static void LoadLevelNow (int l) {
		AsyncOperation op = SceneManager.LoadSceneAsync (l);
		op.priority = 15;
		operation = op;
	}

	private void Update () {
		float proc = operation.progress;
		loadProcentImage.fillAmount = proc;
		loadProcentText.text = "Загрузка..." + ((int)(proc * 100.0f)) + "%";
	}
	
	void Start () {
		Load ();
	}
	void Load () {
		LoadLevelNow (level);
	}
}