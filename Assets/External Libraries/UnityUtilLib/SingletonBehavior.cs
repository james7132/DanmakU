using UnityEngine;
using System;

namespace UnityUtilLib {
	/// <summary>
	/// Static game object.
	/// </summary>
	[Serializable]
	[RequireComponent(typeof(StaticGameObject))]
	public abstract class SingletonBehavior<T> : CachedObject where T : SingletonBehavior<T> {

		private static T instance;

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance {
			get { 
				if(instance == null) {
					instance = FindObjectOfType<T>();
				}
				return instance; 
			}
		}

		/// <summary>
		/// The destroy new instances.
		/// </summary>
		public bool destroyNewInstances;

		/// <summary>
		/// Awake this instance.
		/// </summary>
		public override void Awake () {
			base.Awake ();
			if(instance != null) {
				if(instance.destroyNewInstances) {
					Destroy (gameObject);
					return;
				} else {
					Destroy (instance.GameObject);
				}
			}
			
			instance = (T)this;
		}
	}
}