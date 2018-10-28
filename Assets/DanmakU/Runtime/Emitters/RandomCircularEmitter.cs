using DanmakU;
using DanmakU.Fireables;
using UnityEngine;

namespace _Scripts.Enemies.Emitters
{

    public class RandomCircularEmitter : DanmakuBehaviour
    {
        public int DirectionsCount; // how many Line's
        public Range ShotCount; // how many shots fired per Line
        public float ShotPause; // how much time between each shot
        public float LinePauseDuration; // how much time between each set of shots
        public Line Line;
        public float FrameRate;
        [Space(20)]

        [Header("Bullet")]
        public DanmakuPrefab VortexBulletPrefab;
        public float Speed;

        private DanmakuSet set;
        private IFireable fireable;
        private float[] timers;
        private int[] bulletsLeftCount;

        void Start()
        {
            if (VortexBulletPrefab == null)
            {
                Debug.LogWarning($"Emitter doesn't have a valid DanmakuPrefab", this);
                return;
            }
            set = CreateSet(VortexBulletPrefab);
            set.AddModifiers(GetComponents<IDanmakuModifier>());

            timers = new float[DirectionsCount];
            for (int i = 0; i < timers.Length; i++)
            {
                timers[i] = 0;
            }

            bulletsLeftCount = new int[DirectionsCount];
            for (int i = 0; i < bulletsLeftCount.Length; i++)
            {
                bulletsLeftCount[i] = 0;
            }

            fireable = Line.Of(set);
        }

        void Update()
        {
            if (fireable == null) return;

            var deltaTime = Time.deltaTime;
            if (FrameRate > 0) deltaTime = 1f / FrameRate;

            for (int i = 0; i < timers.Length; i++)
            {
                timers[i] -= deltaTime;

                if (timers[i] > 0)
                {
                    continue;
                }

                // if has ammunition, fire
                if (bulletsLeftCount[i] > 0)
                {
                    DanmakuConfig config = new DanmakuConfig
                    {
                        Position = transform.position,
                        Rotation = transform.rotation.eulerAngles.z + i * (Mathf.PI * 2 / DirectionsCount),
                        Speed = Speed,
                        AngularSpeed = 0,
                        Color = (i % 2 == 0) ? Color.magenta : Color.cyan
                    };
                    fireable.Fire(config);

                    bulletsLeftCount[i]--;

                    // shots left?
                    if (bulletsLeftCount[i] > 0)
                    {
                        timers[i] = ShotPause;
                    }
                    else
                    { // wait for reload
                        timers[i] = LinePauseDuration;
                    }
                    continue;
                }

                // if was waiting to reload bullets, reload
                bulletsLeftCount[i] = Mathf.FloorToInt(ShotCount.GetValue());
            }
        }
    }
}