using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	public class StaticGameObject : MonoBehaviour {

		[SerializeField]
		private bool keepBetweenScenes = true;

		void Awake() {
			if(keepBetweenScenes) {
				DontDestroyOnLoad (this);
			}
		}
	}
}
