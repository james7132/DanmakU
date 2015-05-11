// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections.Generic;

namespace DanmakU {

	/// <summary>
	/// A high performance
	/// </summary>
	public sealed class DanmakuGroup : HashSet<Danmaku> {

		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public new void Add (Danmaku item) {
			bool added = base.Add(item);
			if (added) {
				item.groups.Add (this);
			}
		}

		public new void Clear () {
			foreach(Danmaku danmaku in this) {
				danmaku.RemoveFromGroup(this);
			}
			base.Clear ();
		}

		public new bool Remove (Danmaku item) {
			bool success = false;
			success = base.Remove(item);
			if (success) {
				item.groups.Remove (this);
			}
			return success;
		}
	
	}
}

