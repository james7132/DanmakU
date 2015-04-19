// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Vexe.Runtime.Types;

namespace DanmakU {
	
	public class Path {
		
		private List<DanmakuNode> nodes;

		private FireNode source;
		private EmitterNode emitter;
		
		public bool IsValid {
			get {
				return source != null && emitter != null;
			}
		}

		private Path(Path other) {
			nodes = new List<DanmakuNode>(other.nodes);
			source = other.source;
			emitter = other.emitter;
		}

		public Path(TriggerNode trigger) {
			nodes = new List<DanmakuNode>();
			nodes.Add(trigger);
			source = null;
			emitter = null;
		}

		public Path Clone() {
			return new Path(this);
		}

		public void Add(DanmakuNode node) {
			nodes.Add(node);
			if (node is FireNode) {
				source = node as FireNode;
			}
			if (node is EmitterNode) {
				emitter = node as EmitterNode;
			}
		}

		public void Fire() {
			FireBuilder target = new FireBuilder();
			for(int i = 0; i < nodes.Count; i++) {
				DanmakuNode node = nodes[i];
				if(node.Enabled) {
					nodes[i].Process(target);
				}
			}
		}

	}
	
	public abstract class DanmakuNode : IDanmakuObject, IEnumerable<DanmakuNode> {
		#region IDanmakuObject implementation
		[Hide]
		[DontSerialize]
		public DanmakuField Field {
			get;
			set;
		}
		#endregion

		public void GeneratePaths(Path currentPath, List<Path> validPaths) {
			currentPath.Add(this);
			if (children.Length != 0) {
				for (int i = 0; i < children.Length; i++) {
					children [i].GeneratePaths(currentPath.Clone(), validPaths);
				}
			} else {
				if(currentPath.IsValid) {
					validPaths.Add(currentPath);
				}
			}
		}

		[Hide, Serialize]
		internal DanmakuNode[] parents;

		[Hide, Serialize]
		internal DanmakuNode[] children;

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
	
		[Show, Serialize]
		public bool Enabled {
			get;
			set;
		}

		[Show, Serialize]
		public string Identifier {
			get;
			set;
		}

		[Hide, Serialize]
		internal Rect EditorRect;

		public DanmakuNode() {
			Identifier = Regex.Replace (GetType ().Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0").Trim ();
			Enabled = true;
		}

		protected internal virtual void Initialize() {
		}

		public abstract void Process (FireBuilder target);

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

