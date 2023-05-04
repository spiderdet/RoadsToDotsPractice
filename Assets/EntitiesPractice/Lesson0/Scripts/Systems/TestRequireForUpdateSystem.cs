using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;


namespace DOTS.DOD.LESSON0
{
    [UpdateInGroup(typeof(TestSystemGroup))]
    [BurstCompile]
    public partial struct TestSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            //state.RequireForUpdate<LocalTransform>();
        }

        
        [BurstCompile]
        //[RequireMatchingQueriesForUpdate]
        public void OnUpdate(ref SystemState state) {
            //Debug.Log("In TestSystem-OnUpdate");
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
}


