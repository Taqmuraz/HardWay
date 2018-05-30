using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;


/// <summary>
/// сталкер
/// </summary>

public class Stalker : AliveBehaviour {

	/// <summary>
	/// сонстанта скорости движения
	/// </summary>

	public const float moveSpeed = 3f;


	/// <summary>
	/// игрок текущей игры
	/// </summary>
	/// <value>The player.</value>

	public static Stalker player { get; private set; }

	public override void Start () {
		Init ();
	}

	/// <summary>
	/// иницаализация
	/// </summary>

	public override void Init ()
	{
		base.Init ();
		player = aliveType == AliveType.Player ? this : player;
	}

	/// <summary>
	/// функция перемещения сталкера
	/// </summary>

	public override void MotionUpdate ()
	{
		base.MotionUpdate ();
		switch (aliveType) {
		case AliveType.Player:
			PlayerMotionUpdate ();
			break;
		case AliveType.NPC:
			NPCMotionUpdate ();
			break;
		}
	}

	/// <summary>
	/// движение по направлению
	/// </summary>
	/// <param name="direction">Direction.</param>

	private void MoveAt (Vector3 direction) {
		direction = direction.magnitude > 1 ? direction.normalized : direction;
		velocity = direction * moveSpeed;
	}

	/// <summary>
	/// контроль движения игрока
	/// </summary>

	public void PlayerMotionUpdate () {
		Vector3 j = InputData.joystick;
		j = new Vector3 (j.x, 0, j.y);
		MoveAt (j);
	}

	/// <summary>
	/// контроль движения NPC
	/// </summary>

	public void NPCMotionUpdate () {
		
	}
}
