using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityUtilLib;
using System.Linq;

namespace Danmaku2D.Editor {

	
	public class DanmakuNoScriptEditor : EditorWindow {

		private class NodeWrapper {
			public IDanmakuNode node;
			public Rect displayRect;

			private string displayName;
			public string DisplayName {
				get {
					return displayName;
				}
			}

			public Color NodeColor {
				get {
					return node.NodeColor;
				}
			}

			public NodeWrapper(IDanmakuNode node, Vector2 location) {
				this.node = node;
				displayName = Regex.Replace(node.NodeName, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
				displayRect = new Rect(location.x, location.y, 100, 100);
			}

			public static bool operator ==(NodeWrapper n1, NodeWrapper n2) {
				return n1.node == n2.node;
			}

			public static bool operator !=(NodeWrapper n1, NodeWrapper n2) {
				return !(n1 == n2);
			}

			public override bool Equals (object obj) {
				if (obj is NodeWrapper) {
					return this == (obj as NodeWrapper);
				}
				if (obj is IDanmakuNode) {
					return node == (obj as IDanmakuNode);
				}
				return false;
			}

			public override int GetHashCode () {
				return node.GetHashCode ();
			}
		}

		private static DanmakuNoScriptEditor instance;
		private bool test;
		private GameObject target;
		private HashSet<Object> nodes;

		private bool lockObject;

		public static bool IsOpen {
			get {
				return instance == null;
			}
		}

		[MenuItem("Danmaku/Show Editor")]
		public static void CreateWindow() {
			DanmakuNoScriptEditor window = EditorWindow.GetWindow<DanmakuNoScriptEditor> ("Danmaku", true);
			window.Show ();
			if (window.nodes == null)
				window.nodes = new HashSet<Object> ();
		}

		void OnGUI() {
			ToolbarGUI ();
			BeginWindows ();
			GraphGUI ();
			EndWindows ();
		}

		void Update() {
			UpdateValues ();
		}

		void OnSelectionChange()  {
			UpdateValues ();
		}

		void UpdateValues() {
			if (!lockObject) {
				GameObject oldTarget = target;
				target = Selection.activeGameObject;
				if (target != null) {
					Component[] targetNodes = Util.GetComponentsInChildren<Component> (target);
					foreach(var node in targetNodes) {
						//nodes.Add(new NodeWrapper(node, Vector2.zero));
						nodes.Add(node);
					}
					nodes.RemoveWhere(w => !targetNodes.Contains(w));
				}
				Repaint ();
			}
		}
		
		void ToolbarGUI() {
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			lockObject = GUILayout.Toggle (lockObject, "Lock Selection", EditorStyles.toolbarButton);
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
		}

		void GraphGUI() {
			EditorGUILayout.BeginVertical ((GUIStyle)"PreBackground");
			{
				if(target != null) {
					foreach(var node in nodes) {
						EditorGUILayout.LabelField(node.name + " (" + node.GetType().Name + ")");
					}
				}
				GUILayout.FlexibleSpace ();
			}
			EditorGUILayout.EndVertical ();
		}
	}

}
