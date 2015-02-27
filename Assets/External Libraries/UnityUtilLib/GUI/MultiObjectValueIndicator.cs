using UnityEngine;
using System.Collections;

namespace UnityUtilLib.GUI {

	/// <summary>
	/// Multi object value indicator.
	/// </summary>
	public abstract class MultiObjectValueIndicator : MonoBehaviour  {

		/// <summary>
		/// The game controller.
		/// </summary>
		[SerializeField]
		private AbstractGameController gameController;
		protected AbstractGameController GameController {
			get {
				return gameController;
			}
		}

		/// <summary>
		/// The player.
		/// </summary>
		[SerializeField]
		protected bool player;

		/// <summary>
		/// The base indicator.
		/// </summary>
		[SerializeField]
		protected GameObject baseIndicator;

		/// <summary>
		/// The additional offset.
		/// </summary>
		[SerializeField]
		protected Vector2 additionalOffset;

		/// <summary>
		/// The indicators.
		/// </summary>
		protected GameObject[] indicators;

		/// <summary>
		/// Gets the max value.
		/// </summary>
		/// <returns>The max value.</returns>
		protected abstract int GetMaxValue();

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		protected abstract int GetValue ();

		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start()
		{
			indicators = new GameObject[GetMaxValue()];
			indicators [0] = baseIndicator;
			Vector3 basePosition = baseIndicator.transform.position;
			for(int i = 1; i < indicators.Length; i++)
			{
				indicators[i] = (GameObject)Instantiate(baseIndicator);
				indicators[i].transform.parent = transform;
				indicators[i].transform.position = basePosition + i * new Vector3(additionalOffset.x, additionalOffset.y);
			}
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update()
		{		
			Vector3 basePosition = baseIndicator.transform.position;
			for(int i = 0; i < indicators.Length; i++)
			{
				indicators[i].SetActive(GetValue() > i);
				indicators[i].transform.position = basePosition + i * new Vector3(additionalOffset.x, additionalOffset.y);
			}
		}
	}
}
