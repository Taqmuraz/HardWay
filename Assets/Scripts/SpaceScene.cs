using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceScene : MonoBehaviour {
	
	public static int level = 0;
	public Text loadProcentText;
	public Image loadProcentImage;
	public float timeToLoad = 5;

	private float time_started = 0;
	
	public static void LoadLevel (int levelname) {
		level = levelname;
		LoadLevelNow (1);
	}

	public static void LoadLevelNow (int l) {
		AsyncOperation op = SceneManager.LoadSceneAsync (l);
		op.priority = 15;
	}

	private void Update () {
		float proc = Mathf.Clamp ((Time.time - time_started) / timeToLoad, 0.0f, 1.0f);
		loadProcentImage.fillAmount = proc;
		loadProcentText.text = "Загрузка..." + ((int)(proc * 100.0f)) + "%";
	}
	
	void Start () {
		Invoke ("Load", timeToLoad);
		time_started = Time.time;
	}
	void Load () {
		LoadLevelNow (level);
	}
}