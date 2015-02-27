using System;
using UnityEngine;

namespace UnityUtilLib {
	/// <summary>
	/// Cached object.
	/// </summary>
	public class CachedObject : MonoBehaviour {
		private GameObject gameObj;
		private Transform trans;

		/// <summary>
		/// Gets the transform.
		/// </summary>
		/// <value>The transform.</value>
		public Transform Transform {
			get {
				if(trans == null) {
					trans = transform;
				}
				return trans;
			}
		}

		/// <summary>
		/// Gets the game object.
		/// </summary>
		/// <value>The game object.</value>
		public GameObject GameObject {
			get {
				if(gameObj == null) {
					gameObj = gameObject;
				}
				return gameObj;
			}
		}

		/// <summary>
		/// Awake this instance.
		/// </summary>
		public virtual void Awake() {
			trans = transform;
			gameObj = gameObject;
		}
	}
}

