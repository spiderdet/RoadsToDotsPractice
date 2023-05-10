using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst.Intrinsics;//v128属于该命名空间

namespace DOTS.DOD.LESSON2
{
    [BurstCompile]
    public struct CubeRotateJobChunk : IJobChunk//不用partial因为dots不会再生成代码来补全，不像IJobEntity
    {
        [ReadOnly] public float elapsedTime;
        //IJobChunk需要额外传递句柄，就是IJobEntity中自动检索的component
        public ComponentTypeHandle<LocalTransform> TransformTypeHandle;//这个类型肯定是通过ComponentTypeHandle<T>来定义的，不可能直接是LocalTransformTypeHandle
        [ReadOnly] public ComponentTypeHandle<RotateSpeed> RotateSpeedTypeHandle; //记得能只读的就只读

        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask,
            in v128 chunkEnabledMask)
        {
            //通过GetNativeArray获取chunk内该类型的组件数组，是引用而非拷贝
            var chunkTransforms = chunk.GetNativeArray(ref TransformTypeHandle); //不加ref的写法已过时
            var chunkRotateSpeeds = chunk.GetNativeArray(ref RotateSpeedTypeHandle);
            //两种方式实现遍历，下面是用ChunkEntityEnumerator，注释的是for循环自己遍历
            var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
            while (enumerator.NextEntityIndex(out int i))
            {
                var speed = chunkRotateSpeeds[i];
                chunkTransforms[i] = chunkTransforms[i].RotateY(speed.rotateSpeed * elapsedTime);
            }
            
            // for (int i = 0; i < chunk.Count; i++)
            // {
            //     var speed = chunkRotateSpeeds[i];
            //     chunkTransforms[i] = chunkTransforms[i].RotateY(speed.rotateSpeed * elapsedTime);
            // }
        }
    }
    
    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(RotateCubeWithIJobChunkSystemGroup))]
    public partial struct CubeRotateSystemChunk : ISystem
    {
        //private EntityQueryBuilder queryBuilder;会报错！
        private EntityQuery _rotateCubes;
        private ComponentTypeHandle<LocalTransform> _transformTypeHandle;
        private ComponentTypeHandle<RotateSpeed> _rotateSpeedTypeHandle;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            //如果将queryBuilder定义为system内变量会报错!! 因为EntityQueryBuilder是 ref struct。https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/builtin-types/ref-struct
            var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<RotateSpeed, LocalTransform>();
            //尽量用state来getEntityQuery和GetComponentTypeHandle，不要用EntityManager
            _rotateCubes = state.GetEntityQuery(queryBuilder);
            _transformTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
            _rotateSpeedTypeHandle = state.GetComponentTypeHandle<RotateSpeed>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {    
            _transformTypeHandle.Update(ref state);//别忘了要update
            _rotateSpeedTypeHandle.Update(ref state);
            var job = new CubeRotateJobChunk
            {
                elapsedTime = SystemAPI.Time.DeltaTime,
                TransformTypeHandle = _transformTypeHandle,
                RotateSpeedTypeHandle = _rotateSpeedTypeHandle
            };
            state.Dependency = job.ScheduleParallel(_rotateCubes, state.Dependency);//IJobChunk要在参数里给EntityQuery！
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }
    }
}