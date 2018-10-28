using DanmakU;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

public class VortexModifier : MonoBehaviour, IDanmakuModifier
{
    public float SpinningAngularVelocity; // rad per second

    public JobHandle UpdateDannmaku(DanmakuPool pool, JobHandle dependency)
    {
        return new RotateJob
        {
            SpinningAngularVelocity = SpinningAngularVelocity,
            InitialStates = pool.InitialStates,
            Speeds = pool.Speeds,
            Positions = pool.Positions,
            Rotations = pool.Rotations
        }.ScheduleBatch(pool.ActiveCount, DanmakuPool.kBatchSize, dependency);
    }

    internal struct RotateJob : IJobBatchedFor
    {
        public float SpinningAngularVelocity;

        [ReadOnly] public NativeArray<DanmakuState> InitialStates;
        [ReadOnly] public NativeArray<Vector2> Positions;
        public NativeArray<float> Speeds;
        public NativeArray<float> Rotations;


        public unsafe void Execute(int start, int end)
        {
            var initStatePtr = (DanmakuState*)(InitialStates.GetUnsafeReadOnlyPtr()) + start;
            var posPtr = (Vector2*)(Positions.GetUnsafeReadOnlyPtr()) + start;
            var speedPtr = (float*)(Speeds.GetUnsafePtr()) + start;
            var rotPtr = (float*)(Rotations.GetUnsafePtr()) + start;

            var pEnd = speedPtr + (end - start);

            while (speedPtr < pEnd)
            {
                var radialSpeed = (*initStatePtr).Speed;

                var r = (*posPtr - (*initStatePtr).Position);
                if (r.magnitude == 0)
                {
                    return;
                }

                var radialVector = r.normalized * radialSpeed;

                var peripheralSpeed = SpinningAngularVelocity * r.magnitude;
                var peripheralVector = Vector2.Perpendicular(radialVector).normalized * peripheralSpeed;

                var totalSpeedVector = radialVector + peripheralVector;

                var speed = totalSpeedVector.magnitude;

                initStatePtr++;
                posPtr++;
                *speedPtr++ = speed;
                *rotPtr++ = Vector2.SignedAngle(Vector2.right, totalSpeedVector) * Mathf.Deg2Rad;
            }
        }
    }
}
