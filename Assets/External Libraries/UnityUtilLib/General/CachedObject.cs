using System;
using UnityEngine;

namespace UnityUtilLib {
	/// <summary>
	/// A abstract class for "cached" object. It caches commonly both the behaviour's GameObject and Transform to increase efficency
	/// </summary>
	public abstract class CachedObject : MonoBehaviour {
		private GameObject gameObj;
		private Transform trans;

		/// <summary>
		/// Gets the cached GameObject's transform.
		/// </summary>
		/// <value>The transform.</value>
		public Transform Transform {
			get {
				if(trans == null)
					trans = transform;
				return trans;
			}
		}

		/// <summary>
		/// Gets the cached GameObject
		/// </summary>
		/// <value>The game object.</value>
		public GameObject GameObject {
			get {
				if(gameObj == null)
					gameObj = gameObject;
				return gameObj;
			}
		}

		/// <summary>
		/// Called upon Component instantiation <br>
		/// See <a href="http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html">Unity Script Reference: MonoBehavior.Awake()</see>
		/// </summary>
		public virtual void Awake() {
			trans = transform;
			gameObj = gameObject;
		}
	}
}

