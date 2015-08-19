using System;
using System.Linq;
using Hourai.SmashBrew;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Hourai {

    public abstract class Game : Singleton<Game> {

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
    }

}
