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
using System.Text.RegularExpressions;
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

		/// <summary>
		/// Gets a node of the specified type.
		/// 
		/// Optionally a you can pass in a string representation of a regex to filter the results based on the nodes' identifier.
		/// </summary>
		/// <returns>A matching node.</returns>
		/// <param name="match">An optional regex to add to filter the results. If left as null, the results will not be filtered.</param>
		/// <typeparam name="T">The type of DanmakuNode to look for.</typeparam>
		public T GetNode<T>(string match = null) where T : DanmakuNode {
			if (match == null) {
				for (int i = 0; i < nodes.Length; i++)
					if (nodes [i] != null && nodes [i] is T)
						return nodes [i] as T;
			} else {
				Regex matchRegex = new Regex(match);
				for (int i = 0; i < nodes.Length; i++)
					if(nodes != null && matchRegex.IsMatch(nodes[i].Identifier) && nodes[i] is T)
						return nodes[i] as T;
			}
			return null;
		}

		/// <summary>
		/// Gets all nodes of the specified type.
		/// 
		/// Optionally a you can pass in a string representation of a regex to filter the results based on the nodes' identifier.
		/// </summary>
		/// <returns>A matching node.</returns>
		/// <param name="match">An optional regex to add to filter the results. If left as null, the results will not be filtered.</param>
		/// <typeparam name="T">The type of DanmakuNode to look for.</typeparam>
		public T[] GetNodes<T>(string match = null) where T : DanmakuNode {
			var store = new List<T> ();
			if (match == null) {
				for (int i = 0; i < nodes.Length; i++)
					if (nodes [i] != null && nodes [i] is T)
						store.Add(nodes[i] as T);
			} else {
				Regex matchRegex = new Regex(match);
				for (int i = 0; i < nodes.Length; i++)
					if(nodes != null && matchRegex.IsMatch(nodes[i].Identifier) && nodes[i] is T)
						store.Add(nodes[i] as T);
			}
			return store.ToArray ();
		}

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