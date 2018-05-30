using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM;
using AM;
using System.Linq;

/// <summary>
/// Тип поведения живого существа
/// </summary>

public enum AliveType
{
	NPC,
	Player
}

/// <summary>
/// Класс любого живого существа
/// </summary>

public class AliveBehaviour : MonoBehaviour {
	[SerializeField]
	private AliveType _aliveType_get;
	public AliveType aliveType
	{
		get {
			return _aliveType_get;
		}
		private set {
			_aliveType_get = value;
		}
	}
	public Transform trans { get; private set; }
	public Rigidbody body { get; private set; }
	public NavMeshAgent agent { get; private set; }
	public Collider coll  { get; private set; }
	public Animator anims { get; private set; }
	public StateMachine fsm { get; private set; }
	public AudioSource audioSource { get; private set; }
	public AnimsMachine animMachine { get; private set; }

	/// <summary>
	/// находится ли существо на земле?
	/// </summary>
	/// <value><c>true</c> if on ground; otherwise, <c>false</c>.</value>

	public bool onGround { get; private set; }

	/// <summary>
	/// направление движения существа в локальном пространстве
	/// </summary>
	/// <value>The local velocity.</value>

	public Vector3 localVelocity
	{
		get {
			return body && trans ? trans.InverseTransformDirection (velocity) : Vector3.zero;
		}
		set {
			velocity = trans && body ? trans.TransformDirection (value) : Vector3.zero;
		}
	}

	/// <summary>
	/// Направление движения существа в глобальном пространстве
	/// </summary>
	/// <value>The velocity.</value>

	public Vector3 velocity
	{
		get {
			Vector3 b = body.velocity;
			return new Vector3 (b.x, 0, b.z);
		}
		set {
			Debug.Log ("Motion setted : " + value);
			body.velocity = new Vector3 (value.x, body.velocity.y, value.z);
		}
	}

	public virtual void Start () {
		Init ();
	}

	/// <summary>
	/// инициализация существа
	/// </summary>

	public virtual void Init () {
		trans = transform;
		audioSource = GetComponent<AudioSource> ();
		body = GetComponent<Rigidbody> ();
		agent = GetComponent<NavMeshAgent> ();
		coll = GetComponent<Collider> ();
		anims = GetComponent<Animator> ();

		animMachine = AnimsMachine.StalkerAnims (this);

		switch (aliveType) {
		case AliveType.Player:
			fsm = StateMachine.StalkerMachine ((Stalker)this);
			break;
		}
	}

	/// <summary>
	/// функция перемещения существа. пуста здесь, но используется в наследующих классах
	/// </summary>

	public virtual void MotionUpdate () {
		return;
	}

	/// <summary>
	/// Проигрывания звука
	/// </summary>
	/// <param name="сам клип">Clip.</param>
	/// <param name="громкость проигрывания">Volume.</param>

	public virtual void PlaySound (AudioClip clip, float volume) {
		audioSource.PlayOneShot (clip, volume);
	}

	private void Update () {
		fsm.FSMUpdate ();
		animMachine.AnimsUpdate ();
	}

	private void FixedUpdate () {
		CheckoutOnGround ();
	}

	/// <summary>
	/// проверка, хаодится ли существо на земле
	/// </summary>

	private void CheckoutOnGround () {
		onGround = Physics.OverlapSphere (trans.position, 0.25f).FirstOrDefault ((Collider c) => !c.isTrigger && c != coll);
	}
}
