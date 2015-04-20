using UnityEngine;
using DanmakU;

[RequireComponent(typeof(AttackPattern))]
public class TestAttackPattern : MonoBehaviour {

	void Start() {
		var ap = GetComponent<AttackPattern>();
		ap.Field = DanmakuField.FindClosest(this);
		ap.Fire();
	}

}
