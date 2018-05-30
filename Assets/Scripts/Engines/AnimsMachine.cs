using UnityEngine;
using System.Collections;
using FSM;

/// <summary>
/// машина анимации
/// </summary>

namespace AM
{
	/// <summary>
	/// возвращает число с плавающей запятой, входной параметр живое существо
	/// </summary>
	public delegate float AliveFloat (AliveBehaviour alive);

	/// <summary>
	/// интерфейс для полиморфизма параметров
	/// </summary>

	public interface IAnimParameter
	{
		string Name ();
	}
	/// <summary>
	/// параметр числа с плавающей запятой
	/// </summary>
	public class AnimFloatParam : IAnimParameter
	{
		private string name;
		/// <summary>
		/// имя параметра
		/// </summary>
		public string Name () {
			return name;
		}
		/// <summary>
		/// сам делегат параметра
		/// </summary>
		/// <value>The float parameter.</value>
		public AliveFloat floatParam { get; private set; }

		public AnimFloatParam (string _name, AliveFloat _float) {
			name = _name;
			floatParam = _float;
		}
	}
	/// <summary>
	/// bool параметр
	/// </summary>
	public class AnimBoolParam : IAnimParameter
	{
		private string name;
		/// <summary>
		/// имя параметра
		/// </summary>
		public string Name () {
			return name;
		}
		/// <summary>
		/// делегат параметра
		/// </summary>
		/// <value>The bool parameter.</value>
		public AliveBool boolParam { get; private set; }

		public AnimBoolParam (string _name, AliveBool _bool) {
			name = _name;
			boolParam = _bool;
		}
	}
	/// <summary>
	/// машина анимации
	/// </summary>
	public class AnimsMachine
	{
		/// <summary>
		/// все параметры
		/// </summary>
		/// <value>The parameters.</value>
		public IAnimParameter[] parameters { get; private set; }
		/// <summary>
		/// носитель машины анимации
		/// </summary>
		/// <value>The alive.</value>
		public AliveBehaviour alive { get; private set; }

		public AnimsMachine (AliveBehaviour _alive, params IAnimParameter[] _parameters) {
			alive = _alive;
			parameters = _parameters;
		}
		/// <summary>
		/// установка всех параметров аниматора в соответствии с параметрами IAnimParameter
		/// </summary>
		public void AnimsUpdate () {
			foreach (var p in parameters) {
				if (p is AnimBoolParam) {
					alive.anims.SetBool (p.Name(), ((AnimBoolParam)p).boolParam (alive));
				}
				if (p is AnimFloatParam) {
					alive.anims.SetFloat (p.Name(), ((AnimFloatParam)p).floatParam (alive));
				}
			}
		}
		/// <summary>
		/// возвращает машину состояний, созданную для сталкера
		/// </summary>
		/// <returns>The anims.</returns>
		/// <param name="stalker">сам сталкер, на которого ориентируется машина анимации.</param>
		public static AnimsMachine StalkerAnims (AliveBehaviour stalker) {
			return new AnimsMachine (stalker,
				new AnimFloatParam("WalkLR", (AliveBehaviour a) => a.localVelocity.x),
				new AnimFloatParam("WalkFB", (AliveBehaviour a) => a.localVelocity.y));
		}
	}
}