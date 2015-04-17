// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Vexe.Runtime.Types;

namespace DanmakU {
	
	public abstract class DanmakuNode : IDanmakuObject, IEnumerable<DanmakuNode> {
		#region IDanmakuObject implementation
		[Hide]
		[DontSerialize]
		public DanmakuField Field {
			get;
			set;
		}
		#endregion
		
		[Hide]
		[Serialize]
		internal DanmakuNode[] parents;

		[Hide]
		[Serialize]
		internal DanmakuNode[] children;

		internal bool baked;

		public DanmakuNode[] Parents {
			get {
				return parents;
			}
		}

		public DanmakuNode[] Children {
			get {
				return children;
			}
		}

		[Hide]
		[Serialize]
		public bool Enabled {
			get;
			set;
		}

		[Hide]
		[DontSerialize]
		protected FireBuilder Target {
			get;
			set;
		}

		[Show]
		[Serialize]
		public string Identifier {
			get;
			set;
		}

		protected internal virtual bool Bakeable {
			get {
				return false;
			}
		}

		public DanmakuNode() {
			Identifier = Regex.Replace (GetType ().Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0").Trim ();
			Enabled = true;
		}

		protected abstract void Process ();

		protected internal virtual void Initialize() {
		}

		internal void Trigger(FireBuilder message) {
			if (!Enabled)
				return;
			if (message == null) {
				Target = message.Clone ();
				Process ();
			}
			if (children != null) {
				for (int i = 0; i < children.Length; i++) {
					if(!children[i].baked)
						children[i].Trigger (message);
				}
			}
		}

		#region IEnumerable implementation
		public IEnumerator<DanmakuNode> GetEnumerator () {
			foreach (var child in children)
				yield return child;
		}
		#endregion
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return children.GetEnumerator ();
		}
		#endregion
	}

}

