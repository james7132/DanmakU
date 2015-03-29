using UnityEngine;
using UnityEditor;
using UnityUtilLib;
using Danmaku2D;
using Danmaku2D.Phantasmagoria;
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
	/// Creates a blank ProjectilePrefab asset
	/// Found under Assets/Create/Danmaku 2D/Projectile Prefab
	/// </summary>
	[MenuItem("Assets/Create/Danmaku 2D/Projectile Prefab", false, 51)]
	public static void AddProjectilePrefab() {
		GameObject temp = new GameObject ("Projectile Prefab");
		temp.AddComponent<ProjectilePrefab> ();
		string pathName = GetProjectWindowFolder ();
		GameObject prefab = PrefabUtility.CreatePrefab (pathName + "/Projectile Prefab.prefab", temp);
		Object.DestroyImmediate (temp);
		SerializedObject so = new SerializedObject (prefab.GetComponent<ProjectilePrefab> ());
		so.FindProperty ("circleCollider").objectReferenceValue = prefab.GetComponent<CircleCollider2D> ();
		so.FindProperty ("spriteRenderer").objectReferenceValue = prefab.GetComponent<SpriteRenderer> ();
		so.ApplyModifiedProperties ();
	}

	/// <summary>
	/// Creates the base of a full Phantasmagoria game with a single click
	/// Found under GameObject/Create/Danmaku 2D/Phantasmagoria/Game Gamecontroller
	/// </summary>
	[MenuItem("GameObject/Create/Danmaku 2D/Phantasmagoria/Game Controller")]
	public static void CreatePhantasmagoriaGame() {
		GameObject temp = new GameObject ("Game Controller");
		temp.AddComponent<StaticGameObject> ();
		PhantasmagoriaGameController pcg = temp.AddComponent<PhantasmagoriaGameController> ();
		DanmakuField playerField1 = CreateDanmakuField ();
		DanmakuField playerField2 = CreateDanmakuField ();
		playerField1.gameObject.name = "Player 1 " + playerField1.gameObject.name;
		playerField2.gameObject.name = "Player 2 " + playerField2.gameObject.name;
		SerializedObject pcgS = new SerializedObject (pcg);
		pcgS.FindProperty ("player1.field").objectReferenceValue = playerField1;
		pcgS.FindProperty ("player2.field").objectReferenceValue = playerField2;
		pcgS.ApplyModifiedProperties ();
		playerField1.transform.position = new Vector3 (-100f, 0f, 0f);
		playerField2.transform.position = new Vector3 (100f, 0f, 0f);
		temp.AddComponent<TestSpawnPlayer> ();
		temp.AddComponent<AudioListener> ();
		temp.AddComponent<AudioSource>();
		temp.AddComponent<MusicManager> ();
	}

	/// <summary>
	/// Creates a PhantasmagoriaField with a single click
	/// Found under GameObject/Create/Danmaku 2D/Phantasmagoria/Field
	/// </summary>
	/// <returns>The phantasmagoria field created.</returns>
	[MenuItem("GameObject/Create/Danmaku 2D/Danmaku Field")]
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

//	[MenuItem("Assets/Create/Danmaku 2D/Custom Attack Pattern")]
//	public static void CreateAttackPattern() {
//		string text = "using UnityEngine;\nusing System.Collections;\nusing Danmaku2D;\n" +
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
