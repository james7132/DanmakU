using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    public class TestScript : MonoBehaviour {

        [SerializeField]
        Circle circle;
        [SerializeField]
        Ring ring;
        [SerializeField]
        GameObject prefab;

        // Use this for initialization
        void Start () {
            var fireable = ring.Of(circle).Of(new PrefabFireable(prefab));
            fireable.Fire(new DanmakuInitialState());
        }

    }

}
