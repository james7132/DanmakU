// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;
using Vexe.Runtime.Types;
namespace DanmakU {

	[System.Serializable]
	public class BranchingModifier : DanmakuModifier, ICollection<DanmakuModifier> {

		[Serialize]
		private List<DanmakuModifier> subModifiers;

		/// <summary>
		/// Initializes a new instance of the <see cref="DanmakU.BranchingModifier"/> class.
		/// </summary>
		public BranchingModifier() {
			subModifiers = new List<DanmakuModifier>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DanmakU.BranchingModifier"/> class from a prexisting
		/// collection of modifiers.
		/// </summary>
		/// <param name="subModifiers">the source collection to pull from.</param>
		public BranchingModifier(IEnumerable<DanmakuModifier> subModifiers) {
			this.subModifiers = new List<DanmakuModifier>(subModifiers);
		}

		#region ICollection implementation

		/// <summary>
		/// Adds the DanmakuModifier as a submodifier. If it is null, nothing happens.
		/// </summary>
		/// <param name="item">the submodifier to add</param>
		public void Add (DanmakuModifier item) {
			if(item != null)
				subModifiers.Add(item);
		}

		/// <summary>
		/// Clears all submodifiers.
		/// </summary>
		public void Clear () {
			subModifiers.Clear();
		}

		/// <summary>
		/// Checks if this instance has the specified DanmakuModifier as a submodifier.
		/// </summary>
		/// <param name="item">the modifier to check.</param>
		public bool Contains (DanmakuModifier item) {
			return subModifiers.Contains(item);
		}

		//TODO Document
		public void CopyTo (DanmakuModifier[] array, int arrayIndex) {
			subModifiers.CopyTo(array, arrayIndex);
		}

		public bool Remove (DanmakuModifier item) {
			return subModifiers.Remove(item);
		}

		public int Count {
			get {
				return subModifiers.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return subModifiers.GetEnumerator();
		}

		#endregion

		#region implemented abstract members of DanmakuModifier

		public override void OnFire (Vector2 position, DFloat rotation) {
			DanmakuModifier original = SubModifier;
			int count = Count;
			if(!(count > 0 && original == null))
				FireSingle (position, rotation);
			for(var i = 0; i < count; i++) {
				DanmakuModifier newSubmodifier = subModifiers[i];
				if(newSubmodifier == null)
					continue;
				SubModifier = newSubmodifier;
				FireSingle(position, rotation);
			}
			SubModifier = original;
		}

		#endregion




	}

}