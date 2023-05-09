using DOTS.DOD.LESSON0;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace DOTS.DOD.LESSON2
{
    [BurstCompile]
    public partial struct CubeRotateJobEntity : IJobEntity//如果RotateSpeed定义时没有加public，默认为internal，此时在该jobentity上加public就会
                                                          //报错Inconsistent accessibility XXX is less accessible than XXX，即内部数据被公开暴露
    {
        [ReadOnly] public float elapsedTime; 
        void Execute(ref LocalTransform transform, RotateSpeed rotateSpeed) //是否需要ref和in？要！
        {
            transform = transform.RotateY(rotateSpeed.rotateSpeed * elapsedTime);
        }
    }
    
    [UpdateInGroup(typeof(RotateCubeWithIJobEntitySystemGroup))]
    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    public partial struct CubeRotateSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new CubeRotateJobEntity{elapsedTime = SystemAPI.Time.DeltaTime};//state.Time.DeltaTime已过时
            job.ScheduleParallel();
            // state.Dependency = job.ScheduleParallel(state.Dependency);
            //job不需要dispose？
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
    }
}