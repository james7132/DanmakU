using UnityEngine;
using System.Collections;
using UnityEditor;
using Danmaku2D;

[CustomEditor(typeof(ProjectileManager))]
public class ProjectilePoolEditor : Editor {

	void OnSceneGUI() {
		ProjectileManager pool = target as ProjectileManager;
		GUISkin skin = GUI.skin;
		Handles.BeginGUI ();
		GUILayout.BeginArea (new Rect(0,0,150,60), skin.box);
		GUILayout.BeginVertical ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label ("Projectile Pool");
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();
		GUILayout.Label ("Total Count: " + pool.TotalCount);
		GUILayout.Label ("Active Count: " + pool.ActiveCount);
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
		if(GUI.changed)
			EditorUtility.SetDirty(target);
		Handles.EndGUI ();
	}
}
