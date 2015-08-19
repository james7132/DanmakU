using UnityEngine;
using UnityEditor;

namespace Hourai.Editor {

    public class HouraiEditor : MonoBehaviour {

        [MenuItem("Assets/Create/BGM Group")]
        static void CreateStageData() {
            AssetUtil.CreateAssetInProjectWindow<BGMGroup>();
        }
    }

}
