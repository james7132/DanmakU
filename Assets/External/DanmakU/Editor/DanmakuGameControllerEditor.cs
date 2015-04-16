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
//		ProjectileManager pool = target as ProjectileManager;
//		GUISkin skin = GUI.skin;
//		Handles.BeginGUI ();
//		GUILayout.BeginArea (new Rect(0,0,150,60), skin.box);
//		GUILayout.BeginVertical ();
//		GUILayout.BeginHorizontal ();
//		GUILayout.FlexibleSpace ();
//		GUILayout.Label ("Danmaku Pool");
//		GUILayout.FlexibleSpace ();
//		GUILayout.EndHorizontal();
//		GUILayout.Label ("Total Count: " + pool.TotalCount);
//		GUILayout.Label ("Active Count: " + pool.ActiveCount);
//		GUILayout.EndVertical ();
//		GUILayout.EndArea ();
//		if(GUI.changed)
//			EditorUtility.SetDirty(target);
//		Handles.EndGUI ();
	}
}