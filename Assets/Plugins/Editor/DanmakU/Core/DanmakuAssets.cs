// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using UnityEditor;

using DanmakU;
//using DanmakU.Phantasmagoria;
using System.IO;

/// <summary>
/// A static class full of editor shortcuts for faster development
/// </summary>
internal static class DanmakuAssets {

	private static string GetProjectWindowFolder() {
		UnityEngine.Object activeObject = Selection.activeObject;
		string path = "";
		if (activeObject == null)
			return "Assets"; // Nothing selected
		else
			path = AssetDatabase.GetAssetPath (activeObject);
		if (path.Length > 0) {
			if (Directory.Exists (path)) {
				return path; // path is a folder
			} else {
//				Debug.Log(path);
//				Debug.Log(Directory.GetParent(path).FullName);
				return Directory.GetParent(path).FullName;
			}
		}
		return ""; //path in Assets folder
	}

	/// <summary>
	/// Creates a blank DanmakuPrefab asset
	/// Found under Assets/Create/DanmakU/Danmaku Prefab
	/// </summary>
	[MenuItem("Assets/Create/DanmakU/Sprite Danmaku Prefab", false, 51)]
	public static void AddSpriteDanmakuPrefab() {
		GameObject temp = new GameObject ("Danmaku Prefab");
		temp.AddComponent<SpriteRenderer> ();
		temp.AddComponent<DanmakuPrefab> ();
		string pathName = GetProjectWindowFolder ();
		PrefabUtility.CreatePrefab (pathName + "/Danmaku Prefab.prefab", temp);
		Object.DestroyImmediate (temp);
	}

	[MenuItem("Assets/Create/DanmakU/Mesh Danmaku Prefab", false, 52)]
	public static void AddMeshDanmakuPrefab() {
		GameObject temp = new GameObject ("Danmaku Prefab");
		temp.AddComponent<MeshFilter> ();
		temp.AddComponent<MeshRenderer> ();
		temp.AddComponent<DanmakuPrefab> ();
		string pathName = GetProjectWindowFolder ();
		PrefabUtility.CreatePrefab (pathName + "/Danmaku Prefab.prefab", temp);
		Object.DestroyImmediate (temp);
	}

//	/// <summary>
//	/// Creates the base of a full Phantasmagoria game with a single click
//	/// Found under GameObject/Create/DanmakU/Phantasmagoria/Game Gamecontroller
//	/// </summary>
//	[MenuItem("GameObject/Create/DanmakU/Phantasmagoria/Game Controller")]
//	public static void CreatePhantasmagoriaGame() {
//		GameObject temp = new GameObject ("Game Controller");
//		temp.AddComponent<StaticGameObject> ();
//		PhantasmagoriaGameController pcg = temp.AddComponent<PhantasmagoriaGameController> ();
//		DanmakuField playerField1 = CreateDanmakuField ();
//		DanmakuField playerField2 = CreateDanmakuField ();
//		playerField1.gameObject.name = "Player 1 " + playerField1.gameObject.name;
//		playerField2.gameObject.name = "Player 2 " + playerField2.gameObject.name;
//		SerializedObject pcgS = new SerializedObject (pcg);
//		pcgS.FindProperty ("player1.field").objectReferenceValue = playerField1;
//		pcgS.FindProperty ("player2.field").objectReferenceValue = playerField2;
//		pcgS.ApplyModifiedProperties ();
//		playerField1.transform.position = new Vector3 (-100f, 0f, 0f);
//		playerField2.transform.position = new Vector3 (100f, 0f, 0f);
//		temp.AddComponent<TestSpawnPlayer> ();
//		temp.AddComponent<AudioListener> ();
//		temp.AddComponent<AudioSource>();
//		temp.AddComponent<AudioManager> ();
//	}

	/// <summary>
	/// Creates a PhantasmagoriaField with a single click
	/// Found under GameObject/Create/DanmakU/Phantasmagoria/Field
	/// </summary>
	/// <returns>The phantasmagoria field created.</returns>
	[MenuItem("GameObject/Create/DanmakU/Danmaku Field")]
	public static DanmakuField CreateDanmakuField() {
		GameObject temp = new GameObject ("Field");
		DanmakuField field = temp.AddComponent<DanmakuField> ();
		GameObject background = GameObject.CreatePrimitive (PrimitiveType.Quad);
		background.name = "Background";
		background.transform.parent = field.transform;
		background.transform.localPosition = new Vector3 (0f, 0f, 10f);
		Object.DestroyImmediate(background.GetComponent<MeshCollider> ());
		background.transform.localScale = new Vector3 (48.125f, 48.125f, 1f);
		GameObject cam = new GameObject ("Camera");
		cam.transform.parent = temp.transform;
		cam.transform.localScale = new Vector3 (0f, 0f, -10f); 	
		Camera camera = cam.AddComponent<Camera> ();
		SerializedObject camS = new SerializedObject (field);
		camS.FindProperty ("fieldCamera").objectReferenceValue = camera;
		camS.ApplyModifiedProperties ();
		camera.orthographic = true;
		camera.orthographicSize = 20;
		camera.farClipPlane = 25;
		return field;
	}

//	[MenuItem("Assets/Create/DanmakU/Custom Attack Pattern")]
//	public static void CreateAttackPattern() {
//		string text = "using UnityEngine;\nusing System.Collections;\nusing DanmakU;\n" +
//						"\npublic class NewAttackPattern : AttackPattern {\n" +
//						"\t//Used to determine when the attack pattern is finished and can terminate" +
//						"\n\tprotected override bool IsFinished {\n" +
//						"\t\tget {\n" +
//						"\t\t\treturn false;\n" +
//						"\t\t}\n" +
//						"\t}\n" +
//						"\n\t//The main loop of the attack pattern, executed every frame" +
//						"\tprotected override void MainLoop() {\n" +
//						"\n\n\t}\n" +
//						"}";
//		CreateScript (text, "NewAttackPattern");
//	}
//
//	private static void CreateScript(string scriptText, string name) {
//		File.WriteAllText (Application.dataPath + "/ " + name + ".cs", scriptText);
//		AssetDatabase.ImportAsset ("Assets/" + name + ".cs");
//	}
}
