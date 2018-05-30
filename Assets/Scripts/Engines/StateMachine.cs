using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.AI;

/// <summary>
/// Машина состояний
/// </summary>

namespace FSM
{
	/// <summary>
	/// Делегат с параметром живого существа
	/// </summary>
	public delegate void AliveAction (AliveBehaviour alive);
	/// <summary>
	/// Делегат, возвращающий bool с входным параметром живого существа
	/// </summary>
	public delegate bool AliveBool (AliveBehaviour alive);

	/// <summary>
	/// Содержит условие перехода из одного состояния в другое
	/// </summary>

	public struct StateTransition
	{
		/// <summary>
		/// само условие
		/// </summary>
		/// <value>The predicate.</value>
		public AliveBool predicate { get; private set; }
		/// <summary>
		/// имя состояния, в которое ведет переход
		/// </summary>
		/// <value>The name of the next state.</value>
		public string nextStateName { get; private set; }

		/// <summary>
		/// Конструктор перехода в состояние
		/// </summary>
		/// <param name="_predicate">Predicate.</param>
		/// <param name="_nextStateName">Next state name.</param>

		public StateTransition (AliveBool _predicate, string _nextStateName) {
			predicate = _predicate;
			nextStateName = _nextStateName;
		}
	}

	/// <summary>
	/// Состояние
	/// </summary>

	public class State
	{
		/// <summary>
		/// имя состояния
		/// </summary>
		/// <value>The name of the state.</value>
		public string stateName { get; private set; }
		/// <summary>
		/// делегат, выполняемый на входе в состояние
		/// </summary>
		/// <value>The on enter.</value>
		public AliveAction onEnter { get; private set; }
		/// <summary>
		/// делегат, выполняемый постоянно, пока состояние активно
		/// </summary>
		/// <value>The update.</value>
		public AliveAction update { get; private set; }
		/// <summary>
		/// делегат, выполняемый на выходе из состояния
		/// </summary>
		/// <value>The on exit.</value>
		public AliveAction onExit { get; private set; }
		/// <summary>
		/// возможные переходы в другие состояния (если их нет, машина состояний не сможет выйти из этого состояния никогда)
		/// </summary>
		/// <value>The transitions.</value>
		public StateTransition[] transitions { get; private set; }
		/// <summary>
		/// Носитель машины состояний
		/// </summary>
		/// <value>The alive.</value>
		public AliveBehaviour alive { get; private set; }

		public State (AliveBehaviour _alive, string _stateName, AliveAction _onEnter, AliveAction _update, AliveAction _onExit, params StateTransition[] _transitions) {
			stateName = _stateName;
			update = _update;
			onEnter = _onEnter;
			onExit = _onExit;
			transitions = _transitions;
			alive = _alive;
		}
		/// <summary>
		/// следующее состояние, к которому возможен переход из текущего
		/// </summary>
		public State Next () {
			StateTransition next = transitions.FirstOrDefault ((StateTransition st) => st.predicate(alive));
			State nextState = string.IsNullOrEmpty (next.nextStateName) ? this : alive.fsm.GetByName (next.nextStateName);
			return nextState;
		}
	}
	/// <summary>
	/// машина состояний
	/// </summary>
	public class StateMachine
	{
		/// <summary>
		/// все возможные состояния
		/// </summary>
		/// <value>The states.</value>
		public State[] states { get; private set; }
		/// <summary>
		/// текущее состояние
		/// </summary>
		/// <value>The state of the current.</value>
		public State currentState { get; private set; }
		/// <summary>
		/// носитель машины состояний
		/// </summary>
		/// <value>The alive.</value>
		public AliveBehaviour alive { get; private set; }

		public StateMachine (AliveBehaviour _alive, State _initState, params State[] _states) {
			alive = _alive;
			currentState = _initState;
			states = _states;
		}
		/// <summary>
		/// возвращает машину состояний для сталкера
		/// </summary>
		/// <returns>The machine.</returns>
		/// <param name="сам сталкер, для которого создается машина состояний">Stalker.</param>
		public static StateMachine StalkerMachine (Stalker stalker) {
			State motion = new State ((AliveBehaviour)stalker, "Motion", (AliveBehaviour a) => {
				return;
			}, (AliveBehaviour a) => a.MotionUpdate (), (AliveBehaviour a) => {
				return;
			});
			return new StateMachine (stalker, motion, motion);
		}
		/// <summary>
		/// смена состояния
		/// </summary>
		/// <param name="to">состояние, к которому нужно перейти.</param>
		private void ChangeState (State to) {
			currentState.onExit (alive);
			currentState = to;
			currentState.onEnter (alive);
		}

		/// <summary>
		/// выполнение update делегатов в текущем состоянии
		/// </summary>

		public void FSMUpdate () {
			State n = currentState.Next ();
			if (currentState == n) {
				currentState.update (alive);
			} else {
				ChangeState (n);
			}
		}

		/// <summary>
		/// возвращает состояние с необходимым именем. возвращает null, если состояние с нужным именем не найдено
		/// </summary>
		/// <returns>The by name.</returns>
		/// <param name="name">имя искомого состояния.</param>

		public State GetByName (string name) {
			return states.FirstOrDefault ((State s) => s.stateName == name);
		}
	}
}