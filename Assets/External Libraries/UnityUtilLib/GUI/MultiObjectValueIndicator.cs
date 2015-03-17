using UnityEngine;
using System.Collections;

/// <summary>
/// A set of small utility GUI scripts that can be easily ported from one game to another
/// </summary>
namespace UnityUtilLib.GUI {

	/// <summary>
	/// Multi object value indicator.
	/// </summary>
	public abstract class MultiObjectValueIndicator : MonoBehaviour  {

		[SerializeField]
		private GameController gameController;
		protected GameController GameController {
			get {
				return gameController;
			}
		}

		[SerializeField]
		protected bool player;

		[SerializeField]
		protected GameObject baseIndicator;

		[SerializeField]
		protected Vector2 additionalOffset;

		protected GameObject[] indicators;

		protected abstract int GetMaxValue();

		protected abstract int GetValue ();

		void Start() {
			indicators = new GameObject[GetMaxValue()];
			indicators [0] = baseIndicator;
			Vector3 basePosition = baseIndicator.transform.position;
			for(int i = 1; i < indicators.Length; i++) {
				indicators[i] = (GameObject)Instantiate(baseIndicator);
				indicators[i].transform.parent = transform;
				indicators[i].transform.position = basePosition + i * new Vector3(additionalOffset.x, additionalOffset.y);
			}
		}

		void Update() {
			Vector3 basePosition = baseIndicator.transform.position;
			for(int i = 0; i < indicators.Length; i++) {
				indicators[i].SetActive(GetValue() > i);
				indicators[i].transform.position = basePosition + i * new Vector3(additionalOffset.x, additionalOffset.y);
			}
		}
	}
}
