using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	/// <summary>
	/// A abstract class for pausable CachedObjects
	/// </summary>
	public abstract class PausableGameObject : CachedObject, IPausable {
		
		/// <summary>
		/// Whether this object <see cref="UnityUtilLib.IPausable"/> is paused.
		/// </summary>
		/// <value><c>true</c> if paused; otherwise, <c>false</c>.</value>
		public bool Paused {
			get;
			set;
		}

		/// <summary>
		/// Update function called by the Unity Engine every frame
		/// <see href="http://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html">Unity Script Reference: MonoBehavior.Update()</see>
		/// </summary>
		public void Update() {
			AlwaysUpdate ();
			if(!Paused)
				NormalUpdate();
		}

		/// <summary>
		/// An overridable function that is called at every Update call
		/// </summary>
		public virtual void AlwaysUpdate() {
		}

		/// <summary>
		/// An overridable function that is called only when the instance is not paused
		/// <see cref="PausableGameObject.Paused"/>
		/// </summary>
		public virtual void NormalUpdate() {
		}
	}
}