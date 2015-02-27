using UnityEngine; 
using UnityEditor;

class MeshPostprocessor : AssetPostprocessor {

	void OnPreprocessModel () { 
		(assetImporter as ModelImporter).globalScale = 1f; 
	}
} 