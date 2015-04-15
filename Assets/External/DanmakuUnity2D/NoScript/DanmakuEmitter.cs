// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;
using Vexe.Runtime.Types;

namespace Danmaku2D {

	[AddComponentMenu("Danmaku 2D/Danmaku Emitter")]
	public sealed class DanmakuEmitter : BetterBehaviour, IDanmakuObject, IEnumerable<DanmakuNode> {

		[Serialize]
		private DanmakuNode[] nodes;

		[Serialize]
		private TriggerNode[] triggers;

		void Awake() {
			for (int i = 0; i < nodes.Length; i++) {
				nodes [i].Initialize ();
			}
		}

		public T GetNode<T>() where T : DanmakuNode {
			for (int i = 0; i < nodes.Length; i++)
				if (nodes [i] is T)
					return nodes [i] as T;
			return null;
		}

		public T[] GetNodes<T>() where T : DanmakuNode {
			var store = new List<T> ();
			for (int i = 0; i < nodes.Length; i++)
				if (nodes [i] is T)
					store.Add (nodes [i] as T);
			return store.ToArray ();
		}

		#if UNITY_EDITOR

		public void AttemptBake() {
			RemoveCycles ();

		}
		
		public void RemoveCycles() {
			var open = new Stack<DanmakuNode> ();
			var closed = new HashSet<DanmakuNode> ();
			for (int i = 0; i < triggers.Length; i++) {
				RemoveCyclesHelper(triggers[i], open, closed);
			}
		}

		private void RemoveCyclesHelper(DanmakuNode current, 
		                                Stack<DanmakuNode> open, 
		                                HashSet<DanmakuNode> closed) {
			if (current == null)
				return;
			open.Push (current);
			for (int i = 0; i < current.children.Length; i++) {
				DanmakuNode child = current.children[i];
				if(closed.Contains(child))
					continue;
				if(open.Contains(child)) {
					var childParents = new List<DanmakuNode> (child.parents);
					var parentChildren = new List<DanmakuNode> (current.children);
					childParents.Remove (current);
					parentChildren.Remove (child);
					child.parents = childParents.ToArray ();
					current.children = parentChildren.ToArray ();
				} else {
					RemoveCyclesHelper(child, open, closed);
				}
			}
			open.Pop ();
			closed.Add (current);
		}

		#endif

		#region IEnumerable implementation
		public IEnumerator<DanmakuNode> GetEnumerator () {
			if (nodes == null)
				yield break;
			for (int i = 0; i < nodes.Length; i++)
				yield return nodes [i];
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return nodes.GetEnumerator ();
		}
		#endregion

		#region IDanmakuObject implementation
		private DanmakuField field;
		public DanmakuField Field {
			get {
				return field;
			}
			set {
				field = value;
				if(nodes != null) {
					for(int i = 0; i < nodes.Length; i++) {
					}
				}
			}
		}
		#endregion
	}
}