using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Danmaku2D;
using UnityUtilLib;

namespace Danmaku2D.Editor {

	[CustomEditor(typeof(DanmakuField))]
	internal class DanmakuFieldEditor : UnityEditor.Editor {

		private DanmakuField field;

		private SerializedProperty clipBoundary;
		private SerializedProperty playerSpawnLocation;
		private SerializedProperty gamePlaneDistance;
		private SerializedProperty camera3D;
		private SerializedProperty camera2D;
		private SerializedProperty cameraTransform2D;

		private SerializedProperty camera2Drotation;

		private SerializedProperty size;
		private SerializedProperty viewportRect;
		//private SerializedProperty cullingMask;
		private SerializedProperty depth;
		private SerializedProperty renderingPath;
		private SerializedProperty renderTexture;
		private SerializedProperty occlusionCulling;
		private SerializedProperty hdr;
		
		private SerializedProperty c_size;
		private SerializedProperty c_viewportRect;
		private SerializedProperty c_cullingMask;
		private SerializedProperty c_depth;
		private SerializedProperty c_renderingPath;
		private SerializedProperty c_renderTexture;
		private SerializedProperty c_occlusionCulling;
		private SerializedProperty c_hdr;

		private SerializedProperty fixedCameraArea;
		private SerializedProperty anchorPoint;
		private SerializedProperty nativeAspect;
		private SerializedProperty nativeScreenBounds;

		private SerializedObject cameraSO;

		private GUIContent camera3DLabel;
		private GUIContent sizeLabel;
		private GUIContent cullingMaskLabel;
		private GUIContent viewportLabel;
		private GUIContent depthLabel;
		private GUIContent renderingPathLabel;
		private GUIContent renderTextureLabel;
		private GUIContent occlusionCullingLabel;
		private GUIContent hdrLabel;

		private GUIContent rotationLabel;

		private Camera camera2Dobj;

		public void OnEnable() {
			field = target as DanmakuField;

			clipBoundary = serializedObject.FindProperty ("ClipBoundary");
			playerSpawnLocation = serializedObject.FindProperty ("playerSpawnLocation");
			gamePlaneDistance = serializedObject.FindProperty ("GamePlaneDistance");
			camera3D = serializedObject.FindProperty ("camera3D");
			camera2D = serializedObject.FindProperty ("camera2D");
			cameraTransform2D = serializedObject.FindProperty ("cameraTransform2D");

			camera2Drotation = serializedObject.FindProperty ("camera2DRotation");

			viewportRect = serializedObject.FindProperty ("normalArea");

			fixedCameraArea = serializedObject.FindProperty("fixedCameraArea");
			anchorPoint = serializedObject.FindProperty("cameraScreenAnchorPoint");
			nativeAspect = serializedObject.FindProperty("nativeScreenAspectRatio");
			nativeScreenBounds = serializedObject.FindProperty("nativeScreenBounds");

			size = serializedObject.FindProperty ("size");
			viewportRect = serializedObject.FindProperty ("viewportRect");
			//cullingMask = serializedObject.FindProperty ("cullingMask");
			depth = serializedObject.FindProperty ("depth");
			renderingPath = serializedObject.FindProperty("renderingPath");
			renderTexture = serializedObject.FindProperty ("renderTexture");
			occlusionCulling = serializedObject.FindProperty("occlusionCulling");
			hdr = serializedObject.FindProperty("hdr");

			camera3DLabel = new GUIContent ("3D Camera");
			sizeLabel = new GUIContent ("size");
			cullingMaskLabel = new GUIContent ("Culling Mask");
			viewportLabel = new GUIContent ("Viewport Rect");
			depthLabel = new GUIContent("Depth");
			renderingPathLabel = new GUIContent ("Rendering Path");
			renderTextureLabel = new GUIContent ("Target Texture");
			occlusionCullingLabel = new GUIContent ("Occlusion Culling");
			hdrLabel = new GUIContent ("HDR");

			rotationLabel = new GUIContent ("Rotation");

			SetupCamera ();
		}

		private void SetupCamera() {
			camera2Dobj = camera2D.objectReferenceValue as Camera;
			if (camera2Dobj == null) {
				cameraSO = null;
				cameraTransform2D.objectReferenceValue = null;
			} else {
				cameraSO = new SerializedObject (camera2Dobj);
				cameraTransform2D.objectReferenceValue = camera2Dobj.transform;
				c_size = cameraSO.FindProperty ("orthographic size");
				c_cullingMask = cameraSO.FindProperty ("m_CullingMask");
				c_viewportRect = cameraSO.FindProperty ("m_NormalizedViewPortRect");
				c_depth = cameraSO.FindProperty ("m_Depth");
				c_renderingPath= cameraSO.FindProperty ("m_RenderingPath");
				c_renderTexture = cameraSO.FindProperty ("m_TargetTexture");
				c_occlusionCulling = cameraSO.FindProperty ("m_OcclusionCulling");
				c_hdr = cameraSO.FindProperty ("m_HDR");
			}
		}

		public override bool RequiresConstantRepaint () {
			return true;
		}

		public override void OnInspectorGUI () {
			serializedObject.Update ();
			EditorGUILayout.PropertyField (clipBoundary);
			EditorGUILayout.PropertyField (playerSpawnLocation);
			EditorGUILayout.PropertyField (gamePlaneDistance);
			EditorGUILayout.PropertyField (camera3D, camera3DLabel);
			
//			if(camera2Dobj != null) {
//				camera2Dobj.orthographicSize = size.floatValue;
//				camera2Dobj.cullingMask = cullingMask.intValue;
//				camera2Dobj.rect = viewportRect.rectValue;
//				camera2Dobj.depth = depth.floatValue;
//				camera2Dobj.renderingPath = (RenderingPath)renderingPath.enumValueIndex;
//				camera2Dobj.targetTexture = (RenderTexture)renderTexture.objectReferenceValue;
//				camera2Dobj.useOcclusionCulling = occlusionCulling.boolValue;
//				camera2Dobj.hdr = hdr.boolValue;
//			}

			SetupCamera ();

			if (cameraSO != null) {
				cameraSO.Update();
				EditorGUILayout.PropertyField (c_size, sizeLabel);
				EditorGUILayout.LabelField ("2D Camera");
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(camera2Drotation, rotationLabel);
				EditorGUILayout.PropertyField (c_cullingMask, cullingMaskLabel);
				EditorGUILayout.PropertyField (c_viewportRect, viewportLabel);
				EditorGUILayout.PropertyField (c_depth, depthLabel);
				EditorGUILayout.PropertyField (c_renderingPath, renderingPathLabel);
				EditorGUILayout.PropertyField (c_renderTexture, renderTextureLabel);
				EditorGUILayout.PropertyField (c_occlusionCulling, occlusionCullingLabel);
				EditorGUILayout.PropertyField (c_hdr, hdrLabel);

				size.floatValue = c_size.floatValue;
//				cullingMask.intValue = c_cullingMask.intValue;
//				print(cullingMask.intValue);
//				print(c_cullingMask.intValue);
				viewportRect.rectValue = c_viewportRect.rectValue;
				depth.floatValue = c_depth.floatValue;
				renderingPath.enumValueIndex = c_renderingPath.enumValueIndex;
				renderTexture.objectReferenceValue = c_renderTexture.objectReferenceValue;
				occlusionCulling.boolValue = c_occlusionCulling.boolValue;
				hdr.boolValue = c_hdr.boolValue;

				EditorGUILayout.PropertyField(fixedCameraArea);
				if(fixedCameraArea.boolValue) {
					EditorGUILayout.PropertyField(anchorPoint);
					EditorGUILayout.PropertyField(nativeAspect);
					EditorGUILayout.PropertyField(nativeScreenBounds);
					if(GUILayout.Button("Reinitialize")) {
						Vector2 screenSize = EditorUtil.GetGameViewAspectRatio();
						if(screenSize == Vector2.zero) {
							Debug.LogWarning("Warning: Game View in Free Aspect. Native Aspect Ratio is improperly initialized. Try again with a fixed aspect ratio.");
						}
						nativeAspect.floatValue = screenSize.x / screenSize.y;
						nativeScreenBounds.rectValue = camera2Dobj.rect;
					}
				}
				EditorGUI.indentLevel--;
			}
			if (UnityEngine.GUI.changed) {
				cameraSO.ApplyModifiedProperties();
				serializedObject.ApplyModifiedProperties ();
				camera2Dobj.enabled = true;
				field.enabled = true;
			}
		}
	}
}