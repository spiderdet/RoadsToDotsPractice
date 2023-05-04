using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

namespace DOTS.DOD.LESSON1
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(Rotate9CubesSystemGroup))]
    [BurstCompile]
    public partial struct RotateBlueSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = Time.deltaTime;
            foreach (var (transform, rotateSpeedStruct) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>()
                .WithAll<BlueTag>())
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

    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(Rotate9CubesSystemGroup))]
    [BurstCompile]
    public partial struct RotateGreenSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //var data = SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>()
            //    .WithAll<GreenTag>();
            float deltaTime = Time.deltaTime;
            foreach (var (transform, rotateSpeedStruct) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>()
                        .WithAll<GreenTag>())
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


