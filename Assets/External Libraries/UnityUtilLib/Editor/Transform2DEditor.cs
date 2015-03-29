using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace UnityUtilLib.Editor {

	[CustomEditor(typeof(Transform2D))]
	internal sealed class Transform2DEditor : UnityEditor.Editor {

		private static GUIContent[] modes = new GUIContent[] {new GUIContent("2D"), new GUIContent("3D")};

		private Transform2D transform2D;
		private SerializedObject transformSO;
		private SerializedProperty localPosition;
		private SerializedProperty localRotation;
		private SerializedProperty localScale;
		private SerializedProperty editMode;

		private GUIContent positionLabel;
		private GUIContent rotationLabel;
		private GUIContent scaleLabel;
		private GUIContent modeLabel;

		public void OnEnable() {
			transform2D = target as Transform2D;
			transformSO = new SerializedObject ((target as Component).transform);
			localPosition = transformSO.FindProperty ("m_LocalPosition");
			localScale = transformSO.FindProperty ("m_LocalScale");
			localRotation = transformSO.FindProperty ("m_LocalRotation");
			positionLabel = new GUIContent ("Position");
			rotationLabel = new GUIContent ("Rotation");
			scaleLabel = new GUIContent ("Scale");
			modeLabel = new GUIContent ("Edit Mode");
			editMode = serializedObject.FindProperty ("editMode");
		}

		public override bool RequiresConstantRepaint () {
			return Application.isPlaying;
		}

		public override void OnInspectorGUI () {
			while(ComponentUtility.MoveComponentUp(transform2D)) {
				//Move the Transform2D component to the top of the Inspector
			}
			EditorGUIUtility.LookLikeControls ();
			Vector3 position = localPosition.vector3Value;
			Vector3 rotation = localRotation.quaternionValue.eulerAngles;
			Vector3 scale = localScale.vector3Value;
			int mode = editMode.intValue;
			mode = EditorGUILayout.Popup(modeLabel, mode, modes);
			editMode.intValue = mode;
			position = EditorGUILayout.Vector3Field (positionLabel, position);
			if (mode == 0) {
				float newRotation = EditorGUILayout.FloatField (rotationLabel, rotation.z);
				rotation = new Vector3 (0f, 0f, newRotation);
			} else {
				rotation = EditorGUILayout.Vector3Field (rotationLabel, rotation);
			}
			scale = EditorGUILayout.Vector3Field (scaleLabel, scale);
			localPosition.vector3Value = position;
			localRotation.quaternionValue = Quaternion.Euler (rotation);
			localScale.vector3Value = scale;
			if (UnityEngine.GUI.changed) {
				transformSO.ApplyModifiedProperties();
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}