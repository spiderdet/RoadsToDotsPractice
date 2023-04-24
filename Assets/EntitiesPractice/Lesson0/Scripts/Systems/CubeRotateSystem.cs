using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

namespace DOTS.DOD.LESSON0 
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(CubeRotateSystemGroup))]
    [BurstCompile]
    public partial struct CubeRotateSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = Time.deltaTime;
            foreach (var (transform, rotateSpeedStruct) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(deltaTime * rotateSpeedStruct.ValueRO.rotateSpeed);
                //注意是query出来的是component，只有经过.ValueRO.xxdata才获取到组件内部的某个数据
            }
    }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}


