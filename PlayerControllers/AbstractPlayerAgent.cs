using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Player agent.
/// </summary>
[Serializable]
public abstract class AbstractPlayerAgent {

	private AbstractPlayableCharacter player;
	/// <summary>
	/// Gets the player avatar.
	/// </summary>
	/// <value>The player avatar.</value>
	public AbstractPlayableCharacter Player {
		get {
			return player;
		}
		set {
			player = value;
			field = player.Field;
		}
	}
	
	private AbstractDanmakuField field;
	/// <summary>
	/// Gets the field controller.
	/// </summary>
	/// <value>The field controller.</value>
	public AbstractDanmakuField Field {
		get {
			return field;
		}
		set {
			field = value;
			player = field.Player;
		}
	}

	/// <summary>
	/// Update the specified dt.
	/// </summary>
	/// <param name="dt">Dt.</param>
	public abstract void Update(float dt);
}
