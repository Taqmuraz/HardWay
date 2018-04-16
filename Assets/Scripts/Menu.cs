using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[System.Serializable]
public class GroupClass
{
	public ToSaveData[] stalkers = new ToSaveData[0];
	public ToSaveData.Group group;
}

[System.Serializable]
public class Game
{

	public static Game buffer = new Game ();
	public static GameType gt = Game.GameType.NewGame;
	public enum GameType
	{
		NewGame,
		LoadedGame
	}

	public float time = 0;
	public GroupClass[] groups = new GroupClass[0];
	public ItemSavable[] items = new ItemSavable[0];

	public static void Save (Game current, string name) {
		string path = Application.persistentDataPath + "/saves";
		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}
		path = Application.persistentDataPath + "/saves" + "/" + name + ".sav";
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (path);
		bf.Serialize (file, current);
		file.Close ();
	}
	public static string[] names {
		get
		{
			string path = Application.persistentDataPath + "/saves";
			string[] name = new string[0];
			if (Directory.Exists(path)) {
				name = Directory.GetFiles(path);
			}
			return name;
		}
	}
	public static string ComparePathToName (string name, string folder) {
		int length = folder.Length;
		string next = "";
		for (int i = length; i < name.Length; i++) {
			next = next + name[i];
		}
		return next;
	}
	public static Game Load (string path) {
		Game current = new Game ();
		if (File.Exists(path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);
			current = (Game)bf.Deserialize (file);
			file.Close ();
		}
		return current;
	}
}

[System.Serializable]
public enum MenuState
{
	Main,
	New,
	Load,
	Sets,
	Runtime,
	Inventory,
	Dialog
}
[System.Serializable]
public class Settings
{
	public int grafic = 3;
	public int shadows_quality = 3;
	public int water_quality = 3;
	public float audio_volume = 0.5f;
	public float touch_sens = 0.5f;

	public static Settings current = new Settings ();

	public static void Apply () {
		Settings c = current;
		AudioListener.volume = c.audio_volume;
		QualitySettings.SetQualityLevel (c.grafic);
		//Light directional = GameObject.FindWithTag ("DLight").GetComponent<Light>();
	}

	public static void Save () {
		string path = Application.persistentDataPath + "/settings.set";
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (path);
		bf.Serialize (file, current);
		file.Close ();
	}
	public static void Load () {
		string path = Application.persistentDataPath + "/settings.set";
		if (File.Exists(path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);
			current = (Settings)bf.Deserialize (file);
			file.Close ();
		}
	}
}

public class Menu : MonoBehaviour {

	public MenuState menu_state
	{
		get {
			return stateGetter;
		}
		set {
			stateGetter = value;
			SetState ();
		}
	}
	private MenuState stateGetter;
	public GameObject[] states;

	public static Menu menu
	{
		get {
			if (!menuGetter) {
				menuGetter = FindObjectOfType<Menu> ();
			}
			return menuGetter;
		}
	}
	private static Menu menuGetter;

	public bool runtime = false;

	private void SetState () {
		for (int i = 0; i < states.Length; i++) {
			states[i].SetActive((MenuState)i == menu_state);
		}
		if (menu_state == MenuState.Load && !runtime) {
			CheckLoads();
		}
	}

	[Header ("ForSettings")]
	public Slider quality;
	public Slider shadow_quality;
	public Slider water_quality;
	public Slider audio_volume;
	public Slider touch_sens;

	[Header ("For save menu")]
	public InputField save_name;

	[Header ("For View")]
	public Scrollbar settings_scroll;
	public Scrollbar load_scroll;
	public Scrollbar new_scroll;

	public RectTransform settings_mask;
	public RectTransform load_mask;
	public RectTransform new_mask;

	[Header("Rundime dialog")]
	public Text actor;
	public RectTransform dialogButtonsParent;
	public ButtonScript dialogEnterButton;
	public GameObject dialogScrollSys;

	public ButtonScript pickItem;

	public Text memory;

	public float settings_pos = -40;
	public float load_pos = -40;
	public float new_pos = -40;

	private void ApplyGame (Game game) {
		GameObject stalker_prefab = (GameObject)Resources.Load ("Prefabs/Stalker");
		StalkerBehaviour[] stalkers = StalkerBehaviour.FindObjectsOfType<StalkerBehaviour> ();
		for (int i = 0; i < stalkers.Length; i++) {
			Destroy(stalkers[i].gameObject);
		}
		for (int g = 0; g < game.groups.Length; g++) {
			for (int s = 0; s < game.groups[g].stalkers.Length; s++) {
				Instantiate(stalker_prefab).GetComponent<StalkerBehaviour>().to_save_data = game.groups[g].stalkers[s];
			}
		}
		for (int i = 0; i < game.items.Length; i++) {
			GameObject pref = (GameObject)Resources.Load ("ItemsObjects/Item_" + game.items[i].id);
			Instantiate (pref, game.items[i].position, Quaternion.identity);
		}
	}
	public void SaveGame () {
		Game.Save (CreateFromGame(), save_name.text);
		save_name.text = "";
	}
	private Game CreateFromGame () {
		Game g = new Game ();
		g.groups = new GroupClass[2];

		for (int i = 0; i < g.groups.Length; i++) {
			g.groups[i] = new GroupClass();

			g.groups[i].group = (ToSaveData.Group)i;
			List<ToSaveData> tsd = new List<ToSaveData>();
			StalkerBehaviour[] stalkers = StalkerBehaviour.FindObjectsOfType<StalkerBehaviour> ();
			for (int s = 0; s < stalkers.Length; s++) {
				if (stalkers[s].to_save_data.group == g.groups[i].group) {
					tsd.Add(stalkers[s].to_save_data);
				}
			}
			g.groups[i].stalkers = tsd.ToArray();
		}

		ItemObject[] items = FindObjectsOfType<ItemObject> ();

		List<ItemSavable> savs = new List<ItemSavable> ();

		for (int i = 0; i < items.Length; i++) {
			savs.Add (items[i].sav);
		}

		g.items = savs.ToArray();

		return g;
	}

	private void Start () {
		GetFromSaved ();
		if (runtime) {
			if (Game.gt == Game.GameType.LoadedGame) {
				ApplyGame(Game.buffer);
			}
			dialogEnterButton.toDo = new ButtonScript.ToDo (delegate() {
				ToDialogWithNearest(true);
			});
		}
	}
	public string MemoryUsed () {

		int all = 0;

		Process[] prs = Process.GetProcesses ();

		for (int i = 0; i < prs.Length; i++) {
			all += (int)prs[i].WorkingSet64;
		}

		return "" + all;
	}
	private void CheckLoads () {
		GameObject button_prefab = (GameObject)Resources.Load ("Prefabs/LoadButton");
		string[] names = Game.names;

		Button[] bts = load_mask.GetComponentsInChildren<Button>();
		for (int i = 0; i < bts.Length; i++) {
			Destroy(bts[i].gameObject);
		}
		for (int i = 0; i < names.Length; i++) {
			RectTransform trans = Instantiate(button_prefab).GetComponent<RectTransform>();
			trans.SetParent(load_mask);
			trans.anchoredPosition = -Vector3.up * i * (trans.sizeDelta.y + 10);
			trans.GetComponentInChildren<Text>().text = Game.ComparePathToName(names[i], Application.persistentDataPath + "/saves/");
			trans.GetComponent<Button>().onClick.RemoveAllListeners();
			int index = (int)i;
			trans.GetComponent<Button>().onClick.AddListener(
				delegate {
				LoadSavedGame(index);
			});
		}
		load_mask.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,
		                                     (load_mask.sizeDelta.y + 10) * names.Length);
	}
	private void Update () {
		SinhroniseView ();
	}

	private void SinhroniseView () {
		settings_mask.anchoredPosition = Vector3.up * (settings_mask.sizeDelta.y * settings_scroll.value + settings_pos);
		load_mask.anchoredPosition = Vector3.up * (load_mask.sizeDelta.y * load_scroll.value + load_pos);
		new_mask.anchoredPosition = Vector3.up * (new_mask.sizeDelta.y * new_scroll.value + new_pos);
		if (runtime) {
			dialogEnterButton.SetActive (ToDialogWithNearest(false));
			if (currentDialogSystem != null && StalkerControl.player && StalkerControl.player.trans && currentDialogSystem.dialogCharacter) {
				StalkerBehaviour with = currentDialogSystem.dialogCharacter;
				StalkerControl.player.SafetyLookAt ((with.trans.position - StalkerControl.player.trans.position).normalized);
				with.SafetyLookAt ((StalkerControl.player.trans.position - with.trans.position).normalized);
			}
		}
		if (memory) {
			memory.text = "Используется память : " + MemoryUsed();
		}
		if (menu_state == MenuState.Dialog) {
			
		}
	}

	public void Apply () {
		Settings c = new Settings ();
		c.audio_volume = audio_volume.value;
		c.grafic = (int)quality.value;
		c.shadows_quality = (int)shadow_quality.value;
		c.water_quality = (int)water_quality.value;
		c.touch_sens = touch_sens.value;

		Settings.current = c;
		Settings.Apply ();

		Settings.Save ();
	}
	public void GetFromSaved () {
		Settings.Load ();
		Settings c = Settings.current;
		quality.value = c.grafic;
		shadow_quality.value = c.shadows_quality;
		water_quality.value = c.water_quality;
		audio_volume.value = c.audio_volume;
		touch_sens.value = c.touch_sens;
		Settings.Apply ();
	}

	public void ToNewGame () {
		menu_state = MenuState.New;
	}
	public void ToMain () {
		menu_state = MenuState.Main;
	}
	public void ToLoadGame () {
		menu_state = MenuState.Load;
	}
	public void ToInventory () {
		menu_state = MenuState.Inventory;
	}
	public void ToSettings () {
		menu_state = MenuState.Sets;
	}
	public void StartNewGame (int group_int) {
		Game.gt = Game.GameType.NewGame;
		SpaceScene.LoadLevel (2);
	}
	public void LoadSavedGame (int index) {
		Game.gt = Game.GameType.LoadedGame;
		Game.buffer = Game.Load (Game.names[index]);
		SpaceScene.LoadLevel (2);
	}
	public void LoadMenu () {
		SpaceScene.LoadLevel (0);
	}
	public void ToRuntime () {
		menu_state = MenuState.Runtime;
	}

	public DialogSystem currentDialogSystem;

	public bool ToDialogWithNearest (bool enterToDialog) {
		StalkerBehaviour nearest = null;
		if (StalkerControl.player) {
			IEnumerable<StalkerBehaviour> search = StalkerBehaviour.behaviours.Where ((StalkerBehaviour arg) => StalkerControl.player.IsNearStalker(arg) && arg != StalkerControl.player);
			search = search.OrderBy ((StalkerBehaviour arg) => (transform.position - arg.trans.position).magnitude);
			if (search.ToArray().Length > 0) {
				nearest = search.ToArray () [0];
				if (enterToDialog) {
					ToDialog (nearest);
				}
			}
		}
		return nearest != null;
	}

	public void ToDialog (StalkerBehaviour with) {
		if (with.to_save_data.dialog.Length > 0) {
			bool hasData = true;
			switch (with.to_save_data.dialog) {

			case "Osobist3":
				currentDialogSystem = DialogSystem.osobist3;
				break;

			default:
				hasData = false;
				break;
			}

			if (hasData) {
				currentDialogSystem.StartDialog ();
				currentDialogSystem.SetNodeCorner (currentDialogSystem.currentNode, dialogButtonsParent, actor, with, dialogScrollSys);
			}
		}
	}
	public void Quit () {
		Application.Quit ();
	}
}











