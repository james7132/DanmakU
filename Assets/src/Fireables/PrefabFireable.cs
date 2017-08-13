using UnityEngine;

namespace DanmakU {

    public class PrefabFireable : IFireable {

        public GameObject Prefab { get; set; }
        internal DanmakuPool Pool { get; private set; }

        public PrefabFireable(GameObject prefab, int count = 1000) {
            Prefab = prefab;
            Pool = new DanmakuPool(count, () => {
                var instance = Object.Instantiate<GameObject>(Prefab) as GameObject;
                instance.hideFlags = HideFlags.HideInHierarchy;
                return new GameObjectDanamku(instance);
            });
        }

        public void Fire(DanmakuState state) {
            var danmaku = Pool.Get();
            danmaku.ApplyState(state);
        }

    }

}