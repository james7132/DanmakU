using UnityEngine;
using Vexe.Runtime.Types;

namespace Hourai {

    public abstract class Singleton<T> : BetterBehaviour where T : Singleton<T> {

        private static T _instance;

        [Serialize, Show, Default(false)]
        private bool _dontDestroyOnLoad;

        public static T Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null) {
                        Debug.LogError("Something is trying to access the " + typeof(T) +
                                       " Singleton instance, but none exists.");
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake() {
            if (_instance == null) {
                _instance = this as T;
                if (_dontDestroyOnLoad)
                    DontDestroyOnLoad(this);
            } else {
                if (_instance == this)
                    return;
                Debug.Log("Destroying " + gameObject + " because " + _instance + " already exists.");
                Destroy(gameObject);
            }
        }
    }
}