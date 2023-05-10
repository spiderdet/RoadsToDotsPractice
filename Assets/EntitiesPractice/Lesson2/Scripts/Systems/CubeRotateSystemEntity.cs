using System.ComponentModel;
using JetBrains.Annotations;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace DOTS.DOD.LESSON2
{
    [BurstCompile]
    public partial struct CubeRotateJobEntity : IJobEntity
    {
        [Unity.Collections.ReadOnly] public float deltaTime;

        void Execute(ref LocalTransform transform, in RotateSpeed speed)
        {
            transform = transform.RotateY(speed.rotateSpeed * deltaTime);
        }
    } 
    
    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(RotateCubeWithIJobEntitySystemGroup))]
    public partial struct CubeRotateSystemEntity : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {    

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new CubeRotateJobEntity { deltaTime = SystemAPI.Time.DeltaTime };
            state.Dependency = job.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        
        }
    }
}
