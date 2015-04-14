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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityUtilLib;
using System.Linq;

namespace Danmaku2D.Editor {
	
	public class DanmakuEditor : EditorWindow {

		private static DanmakuEditor instance;
		private bool test;
		private bool selectionLock;
		private GameObject targetGameObject;
		private Transform targetTransform;
		private DanmakuNoScriptContainer target;

		private Vector2 scroll;

		public static bool IsOpen {
			get {
				return instance == null;
			}
		}

		public static void Open() {
			if (instance == null)
				CreateWindow ();
			else
				instance.Show ();
		}

		[MenuItem("Window/Danmaku Editor")]
		public static void CreateWindow() {
			instance = EditorWindow.GetWindow<DanmakuEditor> ("Danmaku", true);
			instance.Show ();
		}

		void OnGUI() {
			ToolbarGUI ();
			BeginWindows ();
			EditorGUILayout.BeginHorizontal ();
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

		void UpdateValues() {
			if (!selectionLock) {
				GameObject temp = Selection.activeGameObject;
				if (temp != null) {
					target = temp.GetComponentInChildren<DanmakuNoScriptContainer> ();
					if(target != null) {
						targetGameObject = temp;
						targetTransform = temp.transform;
					} else {
						targetGameObject = null;
						targetTransform = null;
					}
				} else {
					target = null;
					targetGameObject = null;
					targetTransform = null;
				}
			}
			Repaint ();
		}
		
		void ToolbarGUI() {
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			if (target != null) {
				EditorGUILayout.LabelField("Selected: " + target.name);
			}
			GUILayout.FlexibleSpace ();
			selectionLock = GUILayout.Toggle (selectionLock, "Lock", EditorStyles.toolbarButton);
			GUILayout.EndHorizontal ();
		}

		void SelectionGUI() {
//			EditorGUILayout.BeginVertical (GUILayout.Width(selectionWidth));
//			{
//				selectionScroll = EditorGUILayout.BeginScrollView (selectionScroll);
//				{
////					foreach(GameObject gameObj in targets) {
////						EditorGUILayout.LabelField(gameObj.name);
////					}
////					GUILayout.FlexibleSpace();
//				}
//				EditorGUILayout.EndScrollView ();
//			}
//			EditorGUILayout.EndVertical ();
//			Rect temp = GUILayoutUtility.GetLastRect ();
//
//			Event currentEvent = Event.current;
//			EventType currentEventType = currentEvent.type;
//
//			if (currentEventType == EventType.DragExited) {
//				DragAndDrop.PrepareStartDrag();
//			}
//
//			if (temp.Contains (currentEvent.mousePosition)) {
//				
////				switch (currentEventType) {
////					case EventType.DragUpdated:
////						Object[] objects = DragAndDrop.objectReferences;
////						if(objects.Length > 0) {
////							foreach(Object obj in objects) {
////								if(obj is GameObject && !targets.Contains(obj as GameObject)) {
////									DragAndDrop.visualMode = DragAndDropVisualMode.Link;
////									break;
////								}
////							}
////						}
////						break;
////					case EventType.DragPerform:
////						foreach(GameObject gameObject in DragAndDrop.objectReferences) {
////							AddToSelection(gameObject);
////						}
////						break;
////					default:
////						break;
////				}
//			}
		}

		void GraphGUI() {
			Rect temp = EditorGUILayout.BeginVertical ((GUIStyle)"PreBackground");
			{
				if(target != null) {
					scroll = EditorGUILayout.BeginScrollView (scroll);
					{
						GUILayout.FlexibleSpace ();
					}
					EditorGUILayout.EndScrollView ();
				} else {
					if(Selection.activeGameObject != null) {
						EditorGUILayout.BeginHorizontal();
						{
							//GUILayout.FlexibleSpace();
							EditorGUILayout.BeginVertical();
							{
								GUILayout.FlexibleSpace();
								if(GUILayout.Button("Add Danmaku Controller to Selected Object")) {
									target = Selection.activeGameObject.AddComponent<DanmakuNoScriptContainer>();
									targetGameObject = Selection.activeGameObject;
									targetTransform = Selection.activeGameObject.transform;
								}
								GUILayout.FlexibleSpace();
							}
							EditorGUILayout.EndVertical();
							//GUILayout.FlexibleSpace();
						}
						EditorGUILayout.EndHorizontal();
					}
				}
			}
			EditorGUILayout.EndVertical ();
			
			Event currentEvent = Event.current;
			EventType currentEventType = currentEvent.type;

			if (currentEventType == EventType.DragExited) {
				DragAndDrop.PrepareStartDrag();
			}

			if (temp.Contains (currentEvent.mousePosition)) {
				
				switch (currentEventType) {
					case EventType.DragUpdated:
						Object[] objects = DragAndDrop.objectReferences;
						if (targetTransform != null && objects.Length > 0) {
							foreach (Object obj in objects) {
								GameObject referenceGameObject = obj as GameObject;
								Transform referenceTransform = obj as Transform;
								if(referenceGameObject != null) {
									referenceTransform = referenceGameObject.transform;
								}
								if(referenceTransform != null && referenceTransform.IsChildOf(targetTransform)) {
									
								}
							}
						}
						break;
					case EventType.DragPerform:
						foreach (GameObject gameObject in DragAndDrop.objectReferences) {
							
						}
						break;
					default:
						break;
				}
			}
		}

		void DragAndDropCheck() {
		}
	}

}
