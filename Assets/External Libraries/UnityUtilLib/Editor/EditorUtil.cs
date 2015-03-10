using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace UnityUtilLib.Editor {

	/// <summary>
	/// A static class of an assortment of utility functions useful when making Editor extensions
	/// </summary>
	public static class EditorUtil {

		/// <summary>
		/// Returns the EditorWindow containing the Game View.
		/// Note: this method uses Reflection to access internal variables in the Unity Editor. This is prone to change with future releases of Unity.
		/// Currently tested and working with Unity 5.0.
		/// </summary>
		/// <returns>The Game View EditorWindow.</returns>
		public static EditorWindow GetMainGameView() {
			Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
			MethodInfo GetMainGameView = T.GetMethod("GetMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			object Res = GetMainGameView.Invoke(null,null);
			return (UnityEditor.EditorWindow)Res;
		}

		/// <summary>
		/// Gets the Editor Game View aspect ratio.
		/// Will return Vector2.zero if the Game View is set to Free Aspect.
		/// Note: this method uses Reflection to access internal variables in the Unity Editor. This is prone to change with future releases of Unity.
		/// Currently tested and working with Unity 5.0.
		/// </summary>
		/// <returns>The Game View's aspect ratio.</returns>
		public static Vector2 GetGameViewAspectRatio() {
			EditorWindow gameView = GetMainGameView();
			PropertyInfo prop = gameView.GetType().GetProperty("currentGameViewSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			object gvsize = prop.GetValue(gameView, new object[0]{});
			Type gvSizeType = gvsize.GetType();

			int ScreenHeight = (int)gvSizeType.GetProperty("height", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0]{});
			int ScreenWidth = (int)gvSizeType.GetProperty("width", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0]{});
			return new Vector2 ((float)ScreenWidth, (float)ScreenHeight);
		}
		
		/// <summary>
		/// Finds the concrete types that derive from a given type.
		/// This searches all assemblies in the current domain. Not just the current assembly.
		/// </summary>
		/// <returns>The concrete types that derive from the given type</returns>
		/// <typeparam name="T">the name of the type to search from</typeparam>
		public static Type[] FindConcreteTypes<T>() {
			return FindConcreteTypes (typeof(T));
		}

		/// <summary>
		/// Finds the concrete types that derive from a given type.
		/// This searches all assemblies in the current domain. Not just the current assembly.
		/// </summary>
		/// <returns>The concrete types that derive from the given type</returns>
		/// <param name="parentType">the name of the type to search from</param>
		public static Type[] FindConcreteTypes(string typeName) {
			return FindConcreteTypes (Type.GetType (typeName));
		}

		/// <summary>
		/// Finds the concrete types that derive from a given type.
		/// This searches all assemblies in the current domain. Not just the current assembly.
		/// </summary>
		/// <returns>The concrete types that derive from the given type</returns>
		/// <param name="parentType">the type to search from</param>
		public static Type[] FindConcreteTypes(Type parentType) {
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies ();
			List<Type> types = new List<Type> ();
			for (int i = 0; i < assemblies.Length; i++) {
				Type[] assemblyTypes = null;
				try {
					assemblyTypes = assemblies[i].GetTypes();
				} catch (ReflectionTypeLoadException loadException) {
					assemblyTypes = loadException.Types;
				}
				for(int j = 0; j < assemblyTypes.Length; j++) {
					Type typeCheck = assemblyTypes[j];
					if(typeCheck != null && 
					   !typeCheck.Equals(parentType) &&
					   typeCheck.IsAssignableFrom(parentType) &&
					   !typeCheck.IsAbstract &&
					   !typeCheck.IsInterface){
						types.Add(assemblyTypes[j]);
					}
				}
			}
			return types.ToArray ();
		}

		/// <summary>
		/// Create a blank ScriptableObject of type T
		/// </summary>
		/// <typeparam name="T">The type of the ScriptableObject to create</typeparam>
		public static void CreateScriptableAsset<T>() where T : ScriptableObject {
			T asset = ScriptableObject.CreateInstance<T> ();
			CreateAsset (asset);
		}

		/// <summary>
		/// Create a blank ScriptableObject of the type specified by the provided type name.
		/// </summary>
		/// <param name="typeName">The name of the type of the ScriptableObject to create</param>
		public static void CreateScriptableAsset(string typeName) {
			UnityEngine.Object asset = ScriptableObject.CreateInstance(typeName);
			CreateAsset (asset);
		}
		
		/// <summary>
		/// Create a blank ScriptableObject of the type specified by the provided type name.
		/// </summary>
		/// <param name="type">The type of the ScriptableObject to create</param>
		public static void CreateScriptableAsset(Type type) {
			UnityEngine.Object asset = ScriptableObject.CreateInstance (type);
			CreateAsset (asset);
		}

		/// <summary>
		/// Creates an *.asset Asset from a given object at the selected project folder
		/// </summary>
		/// <param name="asset">the object to make an asset out of</param>
		public static void CreateAsset(UnityEngine.Object asset) {
//			string path = AssetDatabase.GetAssetPath (Selection.activeObject);
//			if (path == "") {
//				path = "Assets";
//			} else if (Path.GetExtension (path) != "") {
//				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
//			}
//			string uniquePath = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + asset.GetType ().ToString () + ".asset");
//			AssetDatabase.CreateAsset (asset, uniquePath);
//			AssetDatabase.SaveAssets ();
//			AssetDatabase.Refresh ();
//			EditorUtility.FocusProjectWindow ();
//			Selection.activeObject = asset;
			ProjectWindowUtil.CreateAsset (asset, "New " + asset.GetType ().Name + ".asset");
		}
	}
}