using UnityEngine;
using UnityUtilLib;
using System.Collections;

/// <summary>
/// Controlled agent.
/// </summary>
public class PhantasmagoriaControlledAgent : AbstractPlayerAgent  {
	private string horizontalMoveAxis;
	private string verticalMoveAxis;
	private string fireButton;
	private string focusButton;
	private string chargeButton;

	public PhantasmagoriaControlledAgent(int playerNumber) {
		string strPlay = "Player ";
		horizontalMoveAxis = "Horizontal Movement " + strPlay + playerNumber;
		verticalMoveAxis = "Vertical Movement " + strPlay + playerNumber;
		focusButton = "Focus " + strPlay + playerNumber;
		fireButton = "Fire " + strPlay + playerNumber;
		chargeButton = "Charge " + strPlay + playerNumber;
	}

	/// <summary>
	/// Update the specified dt.
	/// </summary>
	/// <param name="dt">Dt.</param>
	public override void Update (float dt) {
		PhantasmagoriaPlayableCharacter player = (PhantasmagoriaPlayableCharacter)Player;
		Vector2 movementVector = Vector2.zero;
		movementVector.x = Util.Sign(Input.GetAxis (horizontalMoveAxis));
		movementVector.y = Util.Sign(Input.GetAxis (verticalMoveAxis));
		//Debug.Log (horizontalMoveAxis + " : " + Input.GetAxis (horizontalMoveAxis));
		//Debug.Log ("movement vector: " + movementVector.ToString ());
		bool focus = Input.GetButton (focusButton);
		bool fire = Input.GetButton (fireButton);
		bool charge = Input.GetButton (chargeButton);

		player.IsFiring = fire;
		player.IsCharging = charge;

		Player.Move (movementVector.x, movementVector.y, focus, dt);
	}
}
