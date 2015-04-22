// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityUtilLib;
using System.Linq;

namespace DanmakU.Editor {

	
	public class DanmakuNoScriptEditor : EditorWindow {

		private static DanmakuNoScriptEditor instance;
		private bool test;
		private List<GameObject> targets;

		private float selectionWidth;
		private bool resizingSelection;
		private Rect selectionChangeRect;
		private Vector2 selectionScroll;

		public static bool IsOpen {
			get {
				return instance == null;
			}
		}


		[MenuItem("Window/Reference Editor")]
		public static void CreateWindow() {
			instance = EditorWindow.GetWindow<DanmakuNoScriptEditor> ("Reference Editor", true);
			instance.Show ();
			instance.targets = new List<GameObject> ();
			instance.selectionChangeRect.width = 200f;
		}

		void OnGUI() {
			ToolbarGUI ();
			BeginWindows ();
			EditorGUILayout.BeginHorizontal ();
			SelectionGUI ();
			GraphGUI ();
//			GUILayout.FlexibleSpace ();
			EditorGUILayout.EndHorizontal ();
			EndWindows ();
		}

		void Update() {
			UpdateValues ();
		}

		void OnSelectionChange()  {
			UpdateValues ();
		}

		void AddToSelection(GameObject target) {
			if(!targets.Contains(target) && target != null) {
				targets.Add(target);
			}
		}

		void UpdateValues() {
			foreach(GameObject target in targets) {
				ReferenceManager container = target.GetComponentInChildren<ReferenceManager>();
				if(container == null) {
					GameObject temp = new GameObject();
					temp.tag = "EditorOnly";
					temp.transform.parent = target.transform;
					container = temp.AddComponent<ReferenceManager>();
				}
				container.Initialize(target);
			}
			Repaint ();
		}
		
		void ToolbarGUI() {
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Add Selected", EditorStyles.toolbarButton)) {
				GameObject[] selectedGameObjects = Selection.gameObjects;
				foreach(GameObject gameObject in selectedGameObjects) {
					AddToSelection(gameObject);
				}
			}
			if (GUILayout.Button ("Clear Selection", EditorStyles.toolbarButton)) {
				targets.Clear();
			}
			GUILayout.EndHorizontal ();
		}

		void SelectionGUI() {
			EditorGUILayout.BeginVertical (GUILayout.Width(selectionWidth));
			{
				selectionScroll = EditorGUILayout.BeginScrollView (selectionScroll);
				{
					foreach(GameObject gameObj in targets) {
						EditorGUILayout.LabelField(gameObj.name);
					}
					GUILayout.FlexibleSpace();
				}
				EditorGUILayout.EndScrollView ();
			}
			EditorGUILayout.EndVertical ();
			Rect temp = GUILayoutUtility.GetLastRect ();

			Event currentEvent = Event.current;
			EventType currentEventType = currentEvent.type;

			if (currentEventType == EventType.DragExited) {
				DragAndDrop.PrepareStartDrag();
			}

			if (temp.Contains (currentEvent.mousePosition)) {
				
				switch (currentEventType) {
					case EventType.DragUpdated:
						Object[] objects = DragAndDrop.objectReferences;
						if(objects.Length > 0) {
							foreach(Object obj in objects) {
								if(obj is GameObject && !targets.Contains(obj as GameObject)) {
									DragAndDrop.visualMode = DragAndDropVisualMode.Link;
									break;
								}
							}
						}
						break;
					case EventType.DragPerform:
						foreach(GameObject gameObject in DragAndDrop.objectReferences) {
							AddToSelection(gameObject);
						}
						break;
					default:
						break;
				}
			}
			if (currentEventType == EventType.Repaint) {
				selectionWidth = temp.width;
			}
			selectionChangeRect.x = selectionWidth - 3f;
			selectionChangeRect.width = 6f;
			selectionChangeRect.y = temp.y;
			selectionChangeRect.height = temp.height;
//			Debug.Log (temp.ToString () + selectionChangeRect.ToString ());
			ResizeSelectionGUI ();
		}

		void SelectionWindowGUI(int windowID) {
		}

		void ResizeSelectionGUI() {
			GUI.DrawTexture(selectionChangeRect,EditorGUIUtility.whiteTexture);
			EditorGUIUtility.AddCursorRect(selectionChangeRect,MouseCursor.ResizeHorizontal);
			
			if( Event.current.type == EventType.mouseDown && selectionChangeRect.Contains(Event.current.mousePosition)){
				resizingSelection = true;
			}
			if(resizingSelection){
				selectionWidth = Event.current.mousePosition.x;
				selectionChangeRect.x = selectionWidth - selectionChangeRect.width / 2;
			}
			if(Event.current.type == EventType.MouseUp)
				resizingSelection = false;       
		}

		void GraphGUI() {
			EditorGUILayout.BeginVertical ((GUIStyle)"PreBackground");
			{
//				if(target != null) {
//					foreach(var node in nodes) {
//						EditorGUILayout.LabelField(node.name + " (" + node.GetType().Name + ")");
//					}
//				}
				GUILayout.FlexibleSpace ();
			}
			EditorGUILayout.EndVertical ();
		}

		void DragAndDropCheck() {
		}
	}

}
