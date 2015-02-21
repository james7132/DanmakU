using UnityEngine;
using UnityUtilLib;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Player field controller.
/// </summary>
public class PhantasmagoriaField : AbstractDanmakuField {

	private AbstractDanmakuField targetField;
	/// <summary>
	/// Sets the target field.
	/// </summary>
	/// <value>The target field.</value>
	public override AbstractDanmakuField TargetField {
		get {
			return targetField;
		}
	}

	private int playerNumber = 1;
	/// <summary>
	/// Gets or sets the player number.
	/// </summary>
	/// <value>The player number.</value>
	public int PlayerNumber {
		get {
			return playerNumber; 
		}
		set { 
			playerNumber = value; 
		}
	}

	public void SetTargetField(AbstractDanmakuField field) {
		targetField = field;
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void RoundReset() {
		Player.Reset (5);
	}
}
