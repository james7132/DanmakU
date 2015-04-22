// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityEditor;
using Vexe.Editor.GUIs;

namespace DanmakU.Editor {
	
	public class DanmakuEditor : EditorWindow {

		private static DanmakuEditor instance;
		private bool test;
		private bool selectionLock;
		private GameObject targetGameObject;
		private Transform targetTransform;
		private DanmakuEmitter target;
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
					target = temp.GetComponentInChildren<DanmakuEmitter> ();
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
			Rect graphArea = EditorGUILayout.BeginVertical ((GUIStyle)"PreBackground");
			{
				if(target != null) {
					scroll = EditorGUILayout.BeginScrollView (scroll);
					{
						GUILayout.FlexibleSpace ();
					}
					EditorGUILayout.EndScrollView ();
				} else {
					if(!selectionLock && targetGameObject == null && Selection.activeGameObject != null) {
						targetGameObject = Selection.activeGameObject;
					}
					if(targetGameObject != null) {
						EditorGUILayout.BeginHorizontal();
						{
							//GUILayout.FlexibleSpace();
							EditorGUILayout.BeginVertical();
							{
								GUILayout.FlexibleSpace();
								if(GUILayout.Button("Add Danmaku Controller to Selected Object")) {
									target = Selection.activeGameObject.AddComponent<DanmakuEmitter>();
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
			DragAndDropCheck(graphArea);
		}

		void DragAndDropCheck(Rect graphArea) {
			Event currentEvent = Event.current;
			EventType currentEventType = currentEvent.type;
			
			if (currentEventType == EventType.DragExited) {
				DragAndDrop.PrepareStartDrag();
			}
			
			if (graphArea.Contains (currentEvent.mousePosition)) {
				
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
						//TODO -fill out
						break;
					default:
						break;
				}
			}
		}
	}

}
