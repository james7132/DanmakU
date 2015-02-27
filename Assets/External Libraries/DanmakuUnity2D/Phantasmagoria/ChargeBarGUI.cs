using UnityEngine;
using System.Collections;
using Danmaku2D.Phantasmagoria;

/// <summary>
/// Charge bar GU.
/// </summary>
public class ChargeBarGUI : MonoBehaviour {

	/// <summary>
	/// The field controller.
	/// </summary>
	[SerializeField]
	private PhantasmagoriaField field;
	private PhantasmagoriaPlayableCharacter player;

	/// <summary>
	/// The charge capacity.
	/// </summary>
	[SerializeField]
	private Transform chargeCapacity;

	/// <summary>
	/// The charge level.
	/// </summary>
	[SerializeField]
	private Transform chargeLevel;

	/// <summary>
	/// The indicator.
	/// </summary>
	[SerializeField]
	private GameObject indicator;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		player = (PhantasmagoriaPlayableCharacter)field.Player;
		int maxIndicatorLevel = player.MaxChargeLevel - 1;
		float inc = 0.5f / (float)player.MaxChargeLevel;
		Vector3 ls = indicator.transform.localScale;
		for(int i = -maxIndicatorLevel; i <= maxIndicatorLevel; i++) {
			if(i != 0) {
				GameObject newIndicator = (GameObject)Instantiate(indicator);
				newIndicator.transform.parent = transform;
				newIndicator.transform.localPosition = Vector3.right * inc * i + Vector3.forward * indicator.transform.localPosition.z;
				newIndicator.transform.localScale = new Vector3(ls.x / 2f, ls.y, ls.z);
			}
		}
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update() {
		Vector3 capacityScale = chargeCapacity.localScale;
		Vector3 levelScale = chargeCapacity.localScale;
		capacityScale.x = player.CurrentChargeCapacity / (float)player.MaxChargeLevel;
		levelScale.x = player.CurrentChargeLevel / (float)player.MaxChargeLevel;
		chargeCapacity.localScale = capacityScale;
		chargeLevel.localScale = levelScale;
	}
}
