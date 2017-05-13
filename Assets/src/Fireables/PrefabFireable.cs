using UnityEngine;

namespace DanmakU {

    public class PrefabFireable : IFireable {

        public GameObject Prefab { get; set; }

        public PrefabFireable(GameObject prefab) {
            Prefab = prefab;
        }

        public void Fire(DanmakuInitialState state) {
            var instance = Object.Instantiate(Prefab) as GameObject;
            instance.transform.position = state.Position;
            instance.transform.Rotate(new Vector3(0, 0f, state.Rotation * Mathf.Rad2Deg));
        }

    }

}