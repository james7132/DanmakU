using System;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace DanmakU {
  
internal interface IJobBatchedFor {
  void Execute(int start, int end);
}

internal static class BatchedForJobStruct<T> where T : struct, IJobBatchedFor {

  delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref   JobRanges ranges, int jobIndex);

  public static IntPtr Initialize() {
    return JobsUtility.CreateJobReflectionData(typeof(T), JobType.ParallelFor, (ExecuteJobFunction)Execute);
  }

  public static unsafe void Execute(ref T data, IntPtr additionalData, IntPtr bufferRangePatchData, 
                                    ref JobRanges ranges, int jobIndex) {
    int start, end;
    while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out start, out end)) {
      #if ENABLE_UNITY_COLLECTIONS_CHECKS
      JobsUtility.PatchBufferMinMaxRanges(bufferRangePatchData, UnsafeUtility.AddressOf(ref data), start, end - start);
      #endif
      data.Execute(start, end);
    }
  }

}

internal unsafe static class IJobBatchedForExtensions {

  public static JobHandle ScheduleBatch<T>(this T jobData, 
                                           int arrayLength, 
                                           int minIndicesPerJobCount, 
                                           JobHandle dependsOn = new JobHandle()) where T : struct, IJobBatchedFor
  {
      var scheduleParams = 
        new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), 
          BatchedForJobStruct<T>.Initialize(), dependsOn, ScheduleMode.Batched);
      return JobsUtility.ScheduleParallelFor(ref scheduleParams, arrayLength, minIndicesPerJobCount);
  }
}

}