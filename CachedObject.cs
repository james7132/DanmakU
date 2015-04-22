// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;

/// <summary>
/// A utilty library of random useful and portable scripts for Unity
/// </summary>
namespace UnityUtilLib {

	/// <summary>
	/// A abstract class for "cached" object. It caches commonly both the behaviour's GameObject and Transform to increase efficency
	/// </summary>
	public abstract class CachedObject : MonoBehaviour {

//		[SerializeField]
//		private new string name;
//
//		public virtual string Name {
//			get {
//				return name;
//			}
//			set {
//				name = value;
//			}
//		}		

		private GameObject gameObj;
		private Transform trans;

		/// <summary>
		/// Gets the cached GameObject's transform.
		/// Replaces <a href="http://docs.unity3d.com/ScriptReference/Component-transform.html">Component.transform</a> for easier use.
		/// </summary>
		/// <value>The transform.</value>
		public new Transform transform {
			get {
				if(trans == null)
					trans = base.transform;
				return trans;
			}
		}

		/// <summary>
		/// Gets the cached GameObject.
		/// Replaces <a href="http://docs.unity3d.com/ScriptReference/Component-transform.html">Component.gameObject</a> for easier use.
		/// </summary>
		/// <value>The game object.</value>
		public new GameObject gameObject {
			get {
				if(gameObj == null)
					gameObj = base.gameObject;
				return gameObj;
			}
		}

		/// <summary>
		/// Called upon Component instantiation.
		/// See <a href="http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html">MonoBehavior.Awake()</a>
		/// </summary>
		public virtual void Awake() {
			trans = base.transform;
			gameObj = base.gameObject;
		}

		public Task StartTask(IEnumerator task) {
			return UtilExtensionMethods.StartTask (this, task);
		}

		public Task StartTask(IEnumerable task) {
			return UtilExtensionMethods.StartTask (this, task);
		}
	}
}

