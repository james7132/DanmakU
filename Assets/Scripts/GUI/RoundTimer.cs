using UnityEngine;
using System.Collections;
using UnityUtilLib;
using Danmaku2D.Phantasmagoria;

[RequireComponent(typeof(GUIText))]
public class RoundTimer : MonoBehaviour {

	[SerializeField]
	private PhantasmagoriaGameController gameController;

	[SerializeField]
	private Color flashColor;

	[SerializeField]
	private CountdownDelay flashInterval;

	[SerializeField]
	private float flashThreshold;

	private Color normalColor;
	private bool flashState;
	private GUIText label;

	void Start() {
		label = guiText;
		normalColor = label.color;
		flashState = false;
	}

	void Update() {
		int timeSec = Mathf.FloorToInt (gameController.RemainingRoundTime);
		int seconds = timeSec % 60;
		int minutes = timeSec / 60;
		label.text = minutes.ToString ("D2") + ":" + seconds.ToString ("D2");;
		if (timeSec < flashThreshold) {
			if(flashInterval.Tick(Time.deltaTime)) {
				label.color = (flashState) ? flashColor : normalColor;
				flashState = !flashState;
			}
		} else {
			label.color = normalColor;
			flashState = false;
			flashInterval.ForceReady();
		}
	}
}