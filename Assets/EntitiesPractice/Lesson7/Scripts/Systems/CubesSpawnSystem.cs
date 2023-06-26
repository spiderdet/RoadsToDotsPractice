using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace DOTS.DOD.LESSON7 //这就是子命名空间，因此可以使用父命名空间DOTS.DOD 中的类
{
    [BurstCompile]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof())]
    public partial struct CubesSpawnSystem : ISystem
    {
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {    
        state.RequireForUpdate<CubeSpawner>();
        var builder = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<GoInGameRequest>()
            .WithAll<ReceiveRpcCommandRequest>();
        state.RequireForUpdate(state.GetEntityQuery(builder));
        TFromEntity = state.GetComponentLookup<T>(true);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {    
        var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
        TFromEntity.Update(ref state);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
    }
}
