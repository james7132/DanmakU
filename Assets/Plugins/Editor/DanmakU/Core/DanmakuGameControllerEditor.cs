// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityEditor;
using DanmakU;

/// <summary>
/// Custom <a href="http://docs.unity3d.com/ScriptReference/Editor.html">Editor</a> for ProjectileManager
/// </summary>
[CustomEditor(typeof(DanmakuGameController))]
internal class DanmakuGameControllerEditor : UnityEditor.Editor {

	/// <summary>
	/// Creates custom GUI useful for statistics/debug on the Scene View
	/// Shows how many active Projectiles and how many
	/// </summary>
	public void OnSceneGUI() {
		GUISkin skin = GUI.skin;
		Handles.BeginGUI ();
		GUILayout.BeginArea (new Rect(0,0,150,60), skin.box);
		GUILayout.BeginVertical ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label ("Danmaku Pool");
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();
		GUILayout.Label ("Total Count: " + Danmaku.TotalCount);
		GUILayout.Label ("Active Count: " + Danmaku.ActiveCount);
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
		if(GUI.changed)
			EditorUtility.SetDirty(target);
		Handles.EndGUI ();
	}
}