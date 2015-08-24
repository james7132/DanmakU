using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Hourai {

    public abstract class Game : Singleton<Game> {

        private bool _paused;
        private float _oldTimeScale;

        public bool Paused {
            get { return _paused; }
            set {
                if (_paused == value)
                    return;
                if (value) {
                    _oldTimeScale = Time.timeScale;
                    Time.timeScale = 0f;
                } else {
                    Time.timeScale = _oldTimeScale;
                }
                _paused = value;
            }
        }

        // All the things that require an actual instance
        #region Global Callbacks 

        public static event Action OnUpdate;
        public static event Action OnLateUpdate;
        public static event Action OnFixedUpdate;
        public static event Action<int> OnLoad;
        public static event Action OnApplicationFocused;
        public static event Action OnApplicationUnfocused;
        public static event Action OnApplicationExit;

        private void Update() {
            OnUpdate.SafeInvoke();
        }

        private void LateUpdate() {
            OnLateUpdate.SafeInvoke();
        }

        private void FixedUpdate() {
            OnFixedUpdate.SafeInvoke();
        }

        private void OnApplicationFocus(bool focus) {
            if (focus)
                OnApplicationFocused.SafeInvoke();
            else
                OnApplicationUnfocused.SafeInvoke();
        }

        private void OnApplicationQuit() {
            OnApplicationExit.SafeInvoke();
        }

        private void OnLevelWasLoaded(int level) {
            OnLoad.SafeInvoke(level);
        }

        #endregion
 
        #region Global Coroutines

        public static Coroutine StaticCoroutine(IEnumerator coroutine) {
            if(Instance == null)
                throw new InvalidOperationException("Cannot start a static coroutine without a Game instance.");
            if(coroutine == null)
                throw new ArgumentNullException("coroutine");
            return Instance.StartCoroutine(coroutine);
        }

        public static Coroutine StaticCoroutine(IEnumerable coroutine) {
            if(Instance == null)
                throw new InvalidOperationException("Cannot start a static coroutine without a Game instance.");
            if (coroutine == null)
                throw new ArgumentNullException("coroutine");
            return StaticCoroutine(coroutine.GetEnumerator());
        }

        #endregion
    }

    public abstract class ConfigurableGame<T> : Game where T : GameConfig {

        [Serialize, Show, Inline]
        private T _config;

        public static T Config {
            get { return Instance == null ? null : ((ConfigurableGame<T>)Instance)._config; }
        }

        protected override void Awake() {
            base.Awake();
            if (_config != null)
                return;
            T[] configs = Resources.FindObjectsOfTypeAll<T>();
            if (configs.Length > 0)
                _config = configs[0];
            else {
                Debug.LogError(
                               "Game singledton does not have an assigned Config and no configs are found in resources");
            }
        }

        public static bool IsPlayer(Component obj) {
            return obj.CompareTag(Config.PlayerTag);
        }
        
        public static Transform[] GetRespawnPoints() {
            return GameObject.FindGameObjectsWithTag(Config.RespawnTag).Select(go => go.transform).ToArray();
        }
        
        public static GameObject FindPlayer() {
            return GameObject.FindGameObjectWithTag(Config.PlayerTag);
        }

        public static GameObject[] FindPlayers() {
            return GameObject.FindGameObjectsWithTag(Config.PlayerTag);
        }

        public static GameObject FindRespawn() {
            return GameObject.FindGameObjectWithTag(Config.RespawnTag);
        }

        public static GameObject[] FindRespawns() {
            return GameObject.FindGameObjectsWithTag(Config.RespawnTag);
        }

        public static GameObject FindGUI() {
            return GameObject.FindGameObjectWithTag(Config.GUITag);
        }
    }

}
