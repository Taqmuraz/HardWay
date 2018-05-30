using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Хранит аудио и текстовый ресурс реплики, 
/// </summary>

[System.Serializable]
public struct DialogReplic
{
	public AudioClip sourceClip { get; private set; }
	public string sourceText { get; private set; }

	public string actor;
	public string key;
	public string shortText;

	/// <summary>
	/// Словарь громкости голосов актеров. Актуален из-за разности в громкости записей разных актеров
	/// </summary>
	/// <value>The character voices.</value>

	public static Dictionary<string, float> characterVoices
	{
		get
		{
			Dictionary<string, float> chars = new Dictionary<string, float> ();

			chars.Add ("Osobist_osobist3", 0.3f);
			chars.Add ("Alt_osobist3", 1f);

			return chars;
		}
	}

	public static string LoadTextFromResources (string actor, string key) {
		return Resources.Load<TextAsset> ("Dialogs/Texts/" + actor + "/" + key).text;
	}
	public static AudioClip LoadClipFromResources (string actor, string key) {
		AudioClip clip = Resources.Load<AudioClip> ("Dialogs/Audio/" + actor + "/" + key);
		if (!clip) {
			Debug.Log ("Clip is null : " + "Dialogs/Audio/" + actor + "/" + key);
		}
		return clip;
	}
	public DialogReplic (string _actor, string _key, string shortVer) {
		actor = _actor;
		key = _key;
		shortText = shortVer;
		sourceClip = LoadClipFromResources (_actor, _key);
		sourceText = LoadTextFromResources (_actor, _key);
	}
	public DialogReplic (string _actor, string _key) {
		actor = _actor;
		key = _key;
		shortText = "";
		sourceClip = LoadClipFromResources (_actor, _key);
		sourceText = LoadTextFromResources (_actor, _key);
	}
}
/// <summary>
/// Хранит в себе информацию о реплике и переходе в следующий нод. Используется для хранения данных для отображения нода.
/// </summary>
[System.Serializable]
public struct DialogButton
{
	public DialogReplic replic;
	public int nextNodeIndex;

	public DialogButton (DialogReplic _replic, int _next) {
		replic = _replic;
		nextNodeIndex = _next;
	}
}
/// <summary>
/// Содержит реплику НПС, с которым говорит игрок и варианты ответа на нее
/// </summary>
[System.Serializable]
public struct DialogNode
{
	public DialogReplic actorReplic;
	public DialogButton[] playerReplics;

	public DialogNode (DialogReplic actor, params DialogButton[] buttons) {
		actorReplic = actor;
		playerReplics = buttons;
	}
}

/// <summary>
/// Сама диалоговая система
/// </summary>

[System.Serializable]
public class DialogSystem
{
	
	public DialogSystem (params DialogNode[] nds) {
		nodes = nds;
	}
	/// <summary>
	/// Создание диалога с особистом
	/// </summary>
	/// <value>The osobist3.</value>
	public static DialogSystem osobist3
	{
		get {
			DialogReplic whatHappened = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic01_osobist");
			DialogButton ograbili_a
			= new DialogButton (
				  new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic01_Alt", "Мы так не договаривались"),
				  2);
			DialogButton ograbili_b
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic02_Alt", "Меня ограбили"),
				1);
			
			DialogNode node_0 = new DialogNode (whatHappened, ograbili_a, ograbili_b);

			DialogReplic ograbili = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic03_osobist");

			DialogButton yes_ograbili
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic04_Alt", "[Рассказать]"),
				3);

			DialogNode node_1 = new DialogNode (ograbili, yes_ograbili);

			DialogReplic parry = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist2_replic02_osobist");

			DialogButton you_right
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic03_Alt", "[Согласиться]"),
				1);

			DialogNode node_2 = new DialogNode (parry, you_right);

			DialogReplic nolucky = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic04_osobist");

			DialogButton i_chasto
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic05_Alt", "И часто у вас тут такое бывает?"),
				4);

			DialogButton chto_delat
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic06_Alt", "Так что мне теперь делать?"),
				5);

			DialogNode node_3 = new DialogNode (nolucky, i_chasto, chto_delat);

			DialogReplic yes_was = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic05_osobist");

			DialogNode node_4 = new DialogNode (yes_was, chto_delat);

			DialogReplic nothing = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic06_osobist");

			DialogButton no_yet
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic07_Alt", "Почти"),
				6);

			DialogNode node_5 = new DialogNode (nothing, no_yet);

			DialogReplic do_some = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic07_osobist");

			DialogButton what
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic08_Alt", "Что же вам нужно?"),
				7);

			DialogNode node_6 = new DialogNode (do_some, what);

			DialogReplic go_to = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic08_osobist");

			DialogButton yes
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic01_Al_Yes", "Да"),
				8);

			DialogButton no
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic01_Alt_No", "Нет"),
				9);

			DialogNode node_7 = new DialogNode (go_to, yes, no);

			DialogReplic good = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic09_osobist");

			DialogButton und
			= new DialogButton (
				new DialogReplic ("Alt_osobist3", "PlotDialogue_osobist3_replic9_Alt", "Ясно"),
				-1);

			DialogNode node_8 = new DialogNode (good, und);

			DialogReplic but = new DialogReplic ("Osobist_osobist3", "PlotDialogue_osobist3_replic10_osobist");

			DialogNode node_9 = new DialogNode (but);

			return new DialogSystem (node_0, node_1, node_2, node_3, node_4, node_5, node_6, node_7, node_8, node_9);
		}
	}

	/// <summary>
	/// Ноды системы
	/// </summary>


	public DialogNode[] nodes;
	public int currentDialogIndex;

	/// <summary>
	/// Текущий нод
	/// </summary>
	/// <value>The current node.</value>

	public DialogNode currentNode
	{
		get {
			return nodes [currentDialogIndex];
		}
	}

	/// <summary>
	/// Поиск в словаре голоса актера
	/// </summary>
	/// <returns>The voice.</returns>
	/// <param name="actor">Actor.</param>

	public static float GetVoice (string actor) {
		float v = 1;
		if (DialogReplic.characterVoices.ContainsKey(actor)) {
			v = DialogReplic.characterVoices [actor];
		}
		return v;
	}

	/// <summary>
	/// Произношение реплики игроком
	/// </summary>

	public void SayReplic (DialogButton button, AliveBehaviour stalker, RectTransform bp, Text actor, GameObject bpo) {
		Stalker.player.PlaySound (button.replic.sourceClip, GetVoice(button.replic.actor));

		AudioClip c = button.replic.sourceClip;
		bpo.SetActive (false);
		TextSetter.SetTextByTime (actor, button.replic.sourceText, c.length, delegate {
			if (button.nextNodeIndex < 0) {
				CloseDialog ();
			} else {
				currentDialogIndex = button.nextNodeIndex;
				SetNodeCorner (currentNode, bp, actor, stalker, bpo);
			}
		});
	}

	/// <summary>
	/// Персонаж, с которым идет диалог
	/// </summary>

	public AliveBehaviour dialogCharacter;

	/// <summary>
	/// Закрытие диалога
	/// </summary>

	public void CloseDialog () {
		//Menu.menu.menu_state = MenuState.Runtime;
		//Menu.menu.currentDialogSystem = null;
	}

	/// <summary>
	/// Инициализация диалога. Почему для одной строки создан метод? Смотрим в будущее, он еще пригодится
	/// </summary>

	public void StartDialog () {
		//Menu.menu.menu_state = MenuState.Dialog;
	}

	/// <summary>
	/// Установка нода и произношение реплики НПС
	/// </summary>
	/// <param name="node">Node.</param>
	/// <param name="buttonsParent">Buttons parent.</param>
	/// <param name="actor">Actor.</param>
	/// <param name="stalker">Stalker.</param>
	/// <param name="bpo">Bpo.</param>

	public void SetNodeCorner (DialogNode node, RectTransform buttonsParent, Text actor, AliveBehaviour stalker, GameObject bpo) {
		AudioClip c = node.actorReplic.sourceClip;
		bpo.SetActive (false);
		dialogCharacter = stalker;
		TextSetter.SetTextByTime (actor, node.actorReplic.sourceText, c.length, delegate {
			PrepareNode(node, buttonsParent, actor, stalker, bpo);
		});
		stalker.PlaySound (c, GetVoice (node.actorReplic.actor));
	}

	/// <summary>
	/// Подготовкеа нода. Отображение и настройка кнопок
	/// </summary>
	/// <param name="node">Node.</param>
	/// <param name="buttonsParent">Buttons parent.</param>
	/// <param name="actor">Actor.</param>
	/// <param name="stalker">Stalker.</param>
	/// <param name="bpo">Bpo.</param>

	public void PrepareNode (DialogNode node, RectTransform buttonsParent, Text actor, AliveBehaviour stalker, GameObject bpo) {

		bpo.SetActive (true);

		Button[] bt = buttonsParent.GetComponentsInChildren<Button> ();

		foreach (var b in bt) {
			GameObject.Destroy (b.gameObject);
		}

		if (node.playerReplics.Length < 1) {
			CloseDialog ();
			return;
		}

		GameObject buttonPrefab = Resources.Load<GameObject> ("Prefabs/DialogSlot");

		int collumns = 2;

		if (node.playerReplics.Length < 2) {
			collumns = 1;
		}

		TextSetter.SetText (actor, node.actorReplic.sourceText);

		float[] sizes = new float[collumns];

		for (int i = 0; i < node.playerReplics.Length; i++) {
			int c = i % collumns;
			RectTransform trans = GameObject.Instantiate (buttonPrefab, buttonsParent).GetComponent<RectTransform> ();
			trans.anchoredPosition = new Vector2 ((buttonsParent.rect.width / collumns) * c, sizes [c]);
			trans.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, (buttonsParent.rect.width / collumns));
			Text t = trans.GetComponentInChildren<Text> ();
			TextSetter.SetText (t, node.playerReplics [i].replic.shortText);
			trans.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, t.rectTransform.rect.height + 30);
			sizes [c] += trans.rect.height;
			DialogButton cur = node.playerReplics [i];
			trans.GetComponent<Button> ().onClick.AddListener (delegate {

				SayReplic(cur, stalker, buttonsParent, actor, bpo);

			});
		}

		buttonsParent.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, sizes.OrderByDescending ((float arg) => arg).ToArray() [0]);
	}
}
