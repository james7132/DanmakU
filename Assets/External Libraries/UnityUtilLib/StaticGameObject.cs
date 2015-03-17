using UnityEngine;

/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// A static game object, one that can stay between scenes.
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class StaticGameObject : MonoBehaviour {

		[SerializeField]
		private bool keepBetweenScenes = true;
		
		/// <summary>
		/// Called upon Component instantiation <br>
		/// See <a href="http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html">Unity Script Reference: MonoBehavior.Awake()</see>
		/// </summary>
		void Awake() {
			if(keepBetweenScenes) {
				DontDestroyOnLoad (gameObject);
			}
		}
	}
}
