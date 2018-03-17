using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class Inventory
{
	public InventorySlot[] slots;

	public float health = 100;
	public float energy = 0;
	public float radiation = 0;
	public float bloodLost = 0;
	public bool haveToRefresh {
		get {
			return htrGetter;
		}
		set {
			htrGetter = value;
			if (htrGetter) {
				Refresh ();
			}
		}
	}
	private bool htrGetter = true;

	public int armorIndex = -1;
	public int artefactIndex = -1;
	public int helmIndex = -1;
	public int weaponIndex = -1;
	public int pistolIndex = -1;
	public int meleeIndex = -1;
	public int bagIndex = -1;
	public int miniBagIndex = -1;
	public int maskIndex = -1;
	public int legsIndex = -1;
	public int footsIndex = -1;

	public int unicIndex = 0;

	public void Refresh () {
		for (int i = 0; i < slots.Length; i++) {
			slots [i].arrayIndex = i;
		}
	}

	public InventorySlot weapon_0
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = weaponIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot weapon_pistol
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = pistolIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot armor
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = armorIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot helm
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = helmIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot legs
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = legsIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot foots
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = footsIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot mask
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = maskIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot bag
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = bagIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot melee
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = meleeIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot miniBag
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = miniBagIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot artefact
	{
		get {
			InventorySlot slot = new InventorySlot(-1, 0, 0);
			int ind = artefactIndex;
			InventorySlot n = GetByUnicIndex (ind);
			if (n.equipped) {
				slot = n;
			}
			return slot;
		}
	}
	public InventorySlot GetByUnicIndex (int index) {
		InventorySlot slot = new InventorySlot (-1, 0, 0);
		for (int i = 0; i < slots.Length; i++) {
			if (slots[i].index == index) {
				slot = slots [i];
				break;
			}
		}
		return slot;
	}

	public void EquipSlot (int index) {
		switch (slots[index].item.itemType) {
		case ItemType.Armor:
			EquipArmor (index);
			break;
		case ItemType.Artefact:
			EquipArtefact (index);
			break;
		case ItemType.Bag:
			EquipBag (index);
			break;
		case ItemType.MeleeWeapon:
			EquipMeleeWeapon (index);
			break;
		case ItemType.MiniBag:
			EquipMiniBag (index);
			break;
		case ItemType.Weapon:
			EquipWeapon (index);
			break;
		case ItemType.Weapon_Pistol:
			EquipWeapon (index);
			break;
		}
	}
	public void EquipArmor (int index) {
		ArmorType arType = ((ArmorItem)slots [index].item).parameter;
		switch (arType) {
		case ArmorType.Helm:
			if (helm.itemID > -1) {
				UnequipSlot (helm.index);
			}
			helmIndex = slots[index].index;
			break;
		case ArmorType.Foots:
			if (foots.itemID > -1) {
				UnequipSlot (foots.index);
			}
			footsIndex = slots[index].index;
			break;
		case ArmorType.Legs:
			if (legs.itemID > -1) {
				UnequipSlot (legs.index);
			}
			legsIndex = slots[index].index;
			break;
		case ArmorType.Mask:
			if (mask.itemID > -1) {
				UnequipSlot (mask.index);
			}
			maskIndex = slots[index].index;
			break;
		case ArmorType.Tors:
			if (armor.itemID > -1) {
				UnequipSlot (armor.index);
			}
			armorIndex = slots[index].index;
			break;
		}
		slots [index].equipped = true;
	}
	public void AddClearItem (int id) {
		AddSlot (new InventorySlot(id, 1, 0));
	}
	public void EquipBag (int index) {
		if (bag.itemID > -1) {
			UnequipSlot (bag.index);
		}
		bagIndex = slots[index].index;
		slots [index].equipped = true;
	}
	public void EquipMiniBag (int index) {
		if (miniBag.itemID > -1) {
			UnequipSlot (miniBag.index);
		}
		miniBagIndex = slots[index].index;
		slots [index].equipped = true;
	}
	public void EquipWeapon (int index) {
		if (slots [index].item.itemType == ItemType.Weapon) {
			if (weapon_0.itemID > -1) {
				UnequipSlot (weapon_0.index);
			}
			weaponIndex = slots[index].index;
			slots [index].equipped = true;
		} else {
			if (weapon_pistol.itemID > -1) {
				UnequipSlot (weapon_pistol.index);
			}
			pistolIndex = slots[index].index;
			slots [index].equipped = true;
		}
	}
	public void EquipMeleeWeapon (int index) {
		if (melee.itemID > -1) {
			UnequipSlot (melee.index);
		}
		meleeIndex = slots[index].index;
		slots [index].equipped = true;
	}
	public void EquipArtefact (int index) {
		if (artefact.itemID > -1) {
			UnequipSlot (artefact.index);
		}
		artefactIndex = slots[index].index;
		slots [index].equipped = true;
	}
	public void Heal (float points) {
		health += points;
		health = Mathf.Clamp (health, 0, 100);
	}
	public void GetBloodLost (float points) {
		bloodLost += points;
		bloodLost = Mathf.Clamp (bloodLost, 0, 100);
	}
	public void GetRadio (float points) {
		radiation += points;
		radiation = Mathf.Clamp (radiation, 0, 100);
	}
	public void UnequipSlot (int index) {
		InventorySlot s = GetByUnicIndex (index);
		if (s.itemID > -1) {
			slots [s.arrayIndex].equipped = false;
		}
	}
	public void UseSlot (int index) {
		UsableParameter param = ((UseOnCharItem)slots [index].item).parameter;
		Heal (param.healing);
		GetRadio (param.radiation);
		GetBloodLost (param.bloodLost);
	}
	public void AddSlot (InventorySlot slot) {
		unicIndex++;
		slot.index = unicIndex;
		List<InventorySlot> l = new List<InventorySlot> ();
		l.AddRange (slots);
		l.Add (slot);
		slots = l.ToArray ();
		haveToRefresh = true;
	}
	public void Drop (int index, Vector3 position, Quaternion rotation) {
		GameObject obj = (GameObject)Resources.Load ("ItemsObjects/Item_" + slots[index].itemID);
		if (obj) {
			StalkerBehaviour.Instantiate (obj, position, rotation);
		}
		RemoveSlot (index);
	}
	public void RemoveSlot (int index) {
		List<InventorySlot> l = new List<InventorySlot> ();
		l.AddRange (slots);
		l.RemoveAt (index);
		slots = l.ToArray ();
		haveToRefresh = true;
	}
	public void SlotEvent (int index) {
		Item it = slots [index].item;
		if (it.itemType != ItemType.Patrones && it.itemType != ItemType.UsableOnChar) {
			EquipSlot (index);
		} else {
			UseSlot (index);
		}

		haveToRefresh = true;
	}
}
[System.Serializable]
public class InventorySlot
{
	public int itemID = -1;
	public float condition = 1;
	public bool equipped = false;
	public int eqIndex = 0;
	public int arrayIndex = 0;

	public int index = 0;

	public Item item
	{
		get {
			if (itemID > -1) {
				return Item.ItemsDatabase [itemID];
			} else {
				return new Item ();
			}
		}
	}
	public Texture2D texture
	{
		get {
			return Item.GetTexture(itemID);
		}
	}

	public InventorySlot (int id, float c, int eqI) {
		this.itemID = id;
		this.condition = c;
		this.equipped = false;
		this.eqIndex = eqI;
	}
}
[System.Serializable]
public enum ItemType
{
	Weapon,
	Weapon_Pistol,
	MeleeWeapon,
	Artefact,
	UsableOnChar,
	Patrones,
	Grenade,
	None,
	Bag,
	Armor,
	MiniBag
}
[System.Serializable]
public enum ArmorType
{
	Helm,
	Mask,
	Tors,
	Legs,
	Foots
}
[System.Serializable]
public class WeaponParameter
{
	public int patrontage = 1;
	public float damage = 1;
	public float conditionOnStrike = 0.001f;
	public WeaponParameter (int p, float d, float c) {
		this.patrontage = p;
		this.damage = d;
		this.conditionOnStrike = c;
	}
}
[System.Serializable]
public class WeaponItem : Item
{
	public WeaponParameter parameter;

	public WeaponItem (WeaponParameter param, bool isPistol, int cst) {
		this.parameter = param;
		this.cost = cst;
		if (isPistol) {
			this.itemType = ItemType.Weapon_Pistol;
		} else {
			this.itemType = ItemType.Weapon;
		}
	}
}
[System.Serializable]
public class ArmorItem : Item
{
	public ArmorType parameter;

	public ArmorItem (ArmorType param, int cst) {
		this.parameter = param;
		this.cost = cst;
		this.itemType = ItemType.Armor;
	}
}
[System.Serializable]
public class GrenadeItem : Item
{
	public GrenadeParameter parameter;

	public GrenadeItem (GrenadeParameter param, int cst) {
		this.parameter = param;
		this.cost = cst;
		this.itemType = ItemType.Grenade;
	}
}
[System.Serializable]
public class GrenadeParameter
{
	public float damage = 0;
	public float distance = 1f;

	public GrenadeParameter (float d, float dist) {
		this.damage = d;
		this.distance = dist;
	}
}
[System.Serializable]
public class MeleeWeaponParameter
{
	public float damage = 0;
	public float speed = 1;
	public float distance = 1f;

	public MeleeWeaponParameter (float d, float sp, float dist) {
		this.speed = sp;
		this.damage = d;
		this.distance = dist;
	}
}
[System.Serializable]
public class MeleeWeaponItem : Item
{
	public MeleeWeaponParameter parameter;

	public MeleeWeaponItem (MeleeWeaponParameter param, int cst) {
		this.parameter = param;
		this.cost = cst;
		this.itemType = ItemType.MeleeWeapon;
	}
}
[System.Serializable]
public class UsableParameter
{
	public int healing = 0;
	public int armoring = 0;
	public int radiation = -1;
	public int bloodLost = -1;

	public UsableParameter (int heal, int armor, int radio, int bloodL) {
		this.armoring = armor;
		this.radiation = radio;
		this.healing = heal;
		this.bloodLost = bloodL;
	}
}
[System.Serializable]
public class PatronesParameter
{
	public float damage = 0;
	public int maxCount = 10;

	public PatronesParameter (float d, int max) {
		this.damage = d;
		this.maxCount = max;
	}
}
[System.Serializable]
public class PatronesItem : Item
{
	public PatronesParameter parameter;

	public PatronesItem (PatronesParameter param, int cst) {
		this.parameter = param;
		this.cost = cst;
		this.itemType = ItemType.Patrones;
	}
}
[System.Serializable]
public class UseOnCharItem : Item
{
	public UsableParameter parameter;

	public UseOnCharItem (UsableParameter param, int cst) {
		this.parameter = param;
		this.cost = cst;
		this.itemType = ItemType.UsableOnChar;
	}
}
[System.Serializable]
public class ArtefactItem : Item
{
	public UsableParameter parameter;

	public ArtefactItem (UsableParameter param, int cst) {
		this.parameter = param;
		this.cost = cst;
		this.itemType = ItemType.Artefact;
	}
}
[System.Serializable]
public class BagItem : Item
{
	public int slots = 12;
	public float maxWeight = 50f;

	public BagItem (int s, float max, int cst) {
		this.slots = s;
		this.maxWeight = max;
		this.cost = cst;
		this.itemType = ItemType.Bag;
	}
}
[System.Serializable]
public class MiniBagItem : Item
{
	public int slots = 12;
	public float maxWeight = 50f;

	public MiniBagItem (int s, float max, int cst) {
		this.slots = s;
		this.maxWeight = max;
		this.cost = cst;
		this.itemType = ItemType.MiniBag;
	}
}
[System.Serializable]
public class ItemStateControl
{
	public RawImage image;
	public RectTransform conditionData;
}
public class StalkerInventory : MonoBehaviour {
	public RectTransform slotsParent;
	public ItemStateControl armor;
	public ItemStateControl mask;
	public ItemStateControl helm;
	public ItemStateControl foots;
	public ItemStateControl legs;
	public ItemStateControl bag;
	public ItemStateControl miniBag;
	public ItemStateControl weapon_0;
	public ItemStateControl weapon_pistol;
	public ItemStateControl weapon_melee;
	public Inventory inv
	{
		get {
			return beh.to_save_data.inventory;
		}
	}
	public StalkerBehaviour beh;
	public RectTransform showSlotMenu;
	int lastLength = 0;

	private void Update () {
		if (beh) {
			if (lastLength != inv.slots.Length || inv.haveToRefresh) {
				ReloadInventory ();
				inv.haveToRefresh = false;
			}
			if (!addedStart && Game.gt == Game.GameType.NewGame) {
				inv.AddClearItem (0);
				inv.AddClearItem (1);
				inv.AddClearItem (2);
				inv.AddClearItem (3);
				Debug.Log ("Added");
				addedStart = true;
			}
		} else {
			beh = StalkerControl.player;
		}
	}
	private bool addedStart = false;
	private void Start () {
		beh = StalkerControl.player;
	}
	public void UnequipArmor () {
		inv.UnequipSlot (inv.armorIndex);
	}
	public void UnequipHelm () {
		inv.UnequipSlot (inv.helmIndex);
	}
	public void UnequipMask () {
		inv.UnequipSlot (inv.maskIndex);
	}
	public void UnequipFoots () {
		inv.UnequipSlot (inv.footsIndex);
	}
	public void UnequipLegs () {
		inv.UnequipSlot (inv.legsIndex);
	}
	public void UnequipBag () {
		inv.UnequipSlot (inv.bagIndex);
	}
	public void UnequipMiniBag () {
		inv.UnequipSlot (inv.miniBagIndex);
	}
	public void UnequipWeapon () {
		inv.UnequipSlot (inv.weaponIndex);
	}
	public void UnequipPistol () {
		inv.UnequipSlot (inv.pistolIndex);
	}
	public void UnequipMelee () {
		inv.UnequipSlot (inv.meleeIndex);
	}

	public void ShowSlotMenu (int index, float btSizeDelta, int showCount) {
		selectedSlot = index;
		showSlotMenu.anchoredPosition = (-Vector2.up * (showCount / 6) + Vector2.right * (showCount % 6)) * btSizeDelta;
		showSlotMenu.gameObject.SetActive (true);
	}
	public void HideSlotMenu () {
		selectedSlot = -1;
		showSlotMenu.gameObject.SetActive (false);
	}
	public void Drop () {
		inv.Drop (selectedSlot, beh.trans.position + Vector3.up + beh.trans.forward, beh.trans.rotation);
		HideSlotMenu ();
	}
	public int selectedSlot = -1;
	public void SlotEvent () {
		inv.SlotEvent (selectedSlot);
		HideSlotMenu ();
	}
	private void ReloadInventory () {

		armor.conditionData.localScale = new Vector3 (1, inv.armor.condition, 1);
		mask.conditionData.localScale = new Vector3 (1, inv.armor.condition, 1);
		helm.conditionData.localScale = new Vector3 (1, inv.helm.condition, 1);
		foots.conditionData.localScale = new Vector3 (1, inv.foots.condition, 1);
		legs.conditionData.localScale = new Vector3 (1, inv.legs.condition, 1);
		weapon_pistol.conditionData.localScale = new Vector3 (1, inv.weapon_pistol.condition, 1);
		weapon_0.conditionData.localScale = new Vector3 (1, inv.weapon_0.condition, 1);
		weapon_melee.conditionData.localScale = new Vector3 (1, inv.melee.condition, 1);
		bag.conditionData.localScale = new Vector3 (1, inv.bag.condition, 1);
		miniBag.conditionData.localScale = new Vector3 (1, inv.miniBag.condition, 1);


		armor.image.texture = Item.GetTexture (inv.armor.itemID);
		mask.image.texture = Item.GetTexture (inv.mask.itemID);
		helm.image.texture = Item.GetTexture (inv.helm.itemID);
		foots.image.texture = Item.GetTexture (inv.foots.itemID);
		legs.image.texture = Item.GetTexture (inv.legs.itemID);
		weapon_pistol.image.texture = Item.GetTexture (inv.weapon_pistol.itemID);
		weapon_0.image.texture = Item.GetTexture (inv.weapon_0.itemID);
		weapon_melee.image.texture = Item.GetTexture (inv.melee.itemID);
		bag.image.texture = Item.GetTexture (inv.bag.itemID);
		miniBag.image.texture = Item.GetTexture (inv.miniBag.itemID);

		ButtonScript[] bts = slotsParent.GetComponentsInChildren<ButtonScript> ();

		for (int i = 0; i < bts.Length; i++) {
			Destroy (bts[i].gameObject);
		}

		GameObject slot = (GameObject)Resources.Load ("Prefabs/InventorySlot");
		int eq = 0;
		for (int i = 0; i < inv.slots.Length; i++) {
			if (!inv.slots[i].equipped) {
				GameObject sl = Instantiate (slot, slotsParent);
				RectTransform r = sl.GetComponent<RectTransform> ();
				r.anchoredPosition = (-Vector2.up * (eq / 6) + Vector2.right * (eq % 6)) * r.sizeDelta.y;
				r.GetComponent<RawImage> ().texture = inv.slots [i].texture;
				float sd = r.sizeDelta.y;
				int index = i;
				int showI = eq;
				ButtonScript b = r.GetComponent<ButtonScript> ();
				b.toDo = (delegate {

					ShowSlotMenu(index, sd, showI);

				});
				eq++;
			}
		}
	}
}


[System.Serializable]
public class Item
{
	public ItemType itemType = ItemType.None;
	public int cost = 99;

	public Item (ItemType type, int sellCost) {
		this.itemType = type;
		this.cost = sellCost;
	}
	public Item () {
		this.cost = 99;
		this.itemType = ItemType.None;
	}

	public static Texture2D GetTexture (int id) {
		Texture2D tex = (Texture2D)Resources.Load ("ItemsImages/Item_" + id);
		return tex;
	}

	public static Item[] ItemsDatabase
	{
		get {
			List<Item> items = new List<Item> ();

			MiniBagItem smallBag = new MiniBagItem (12, 25, 2500);
			items.Add (smallBag);

			BagItem mediumBag = new BagItem (24, 50, 5000);
			items.Add (mediumBag);

			WeaponItem pm = new WeaponItem (new WeaponParameter(8, 10, 0.001f), true, 1250);
			items.Add (pm);

			WeaponItem toz = new WeaponItem (new WeaponParameter(2, 50f, 0.01f), false, 2000);
			items.Add (toz);


			return items.ToArray ();
		}
	}
}











