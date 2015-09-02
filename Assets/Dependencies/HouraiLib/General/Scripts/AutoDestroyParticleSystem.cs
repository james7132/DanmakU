using UnityEngine;

namespace Hourai {

    [RequireComponent(typeof (ParticleSystem))]
    public class AutoDestroyParticleSystem : MonoBehaviour {

        private ParticleSystem _particleSystem;

        private void Awake() {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update() {
            if (_particleSystem == null || !_particleSystem.IsAlive())
                Destroy(gameObject);
        }

    }

}