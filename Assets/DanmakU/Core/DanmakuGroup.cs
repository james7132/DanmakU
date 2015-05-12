// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections.Generic;

namespace DanmakU {

	/// <summary>
	/// A high performance
	/// </summary>
	public class DanmakuGroup : ICollection<Danmaku> {

		protected ICollection<Danmaku> Group;

		public DanmakuGroup() {
			Group = new HashSet<Danmaku> ();
		}

		public DanmakuGroup(IEnumerable<Danmaku> collection) {
			Group = new HashSet<Danmaku> ();
			foreach (var danmaku in collection) {
				Add (danmaku);
			}
		}
	
		#region ICollection implementation

		public void Add (Danmaku item) {
			if (!Group.Contains(item)) {
				Group.Add(item);
				item.groups.Add (this);
			}
		}

		public void Clear () {
			foreach(Danmaku danmaku in this) {
				danmaku.RemoveFromGroup(this);
			}
			Group.Clear ();
		}

		public bool Contains (Danmaku item) {
			return Group.Contains (item);
		}

		public void CopyTo (Danmaku[] array, int arrayIndex) {
			Group.CopyTo (array, arrayIndex);
		}

		public bool Remove (Danmaku item) {
			bool success = Group.Remove(item);
			if (success) {
				item.groups.Remove (this);
			}
			return success;
		}

		public int Count {
			get {
				return Group.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return Group.IsReadOnly;
			}
		}
		#endregion

		#region IEnumerable implementation
		public IEnumerator<Danmaku> GetEnumerator () {
			return Group.GetEnumerator ();
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return Group.GetEnumerator ();
		}
		#endregion
	}
}

