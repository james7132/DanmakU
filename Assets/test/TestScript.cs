using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanmakU.Fireables;

namespace DanmakU {

    public class TestScript : MonoBehaviour {

        [SerializeField]
        Circle circle;
        [SerializeField]
        Ring ring;
        [SerializeField]
        GameObject prefab;

        [SerializeField]
        DanmakuInitialState initialState;

        DanmakuPool danmaku;

        // Use this for initialization
        void Start () {
            var prefabFireable = new PrefabFireable(prefab, 10000);
            danmaku = prefabFireable.Pool;
            var fireable = ring.Of(circle).Of(prefabFireable);
            fireable.Fire(initialState);
        }

        void Update() {
            danmaku.UpdateDanmaku(Time.deltaTime);
            Debug.Log(danmaku.ActiveCount);
        }

    }

}
