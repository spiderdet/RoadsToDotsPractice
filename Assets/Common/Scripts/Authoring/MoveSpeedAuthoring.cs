using Unity.Entities;
using UnityEngine;

namespace DOTS.DOD
{
    public struct MoveSpeed: IComponentData
    {
        public float moveSpeed;
    }

    public partial class MoveSpeedAuthoring : MonoBehaviour
    {
        [Range(1, 10)]public float moveSpeed = 5;

        public class Baker : Baker<MoveSpeedAuthoring>
        {
            public override void Bake(MoveSpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var moveSpeedComp = new MoveSpeed { moveSpeed = authoring.moveSpeed };
                AddComponent(entity, moveSpeedComp);
            }
        }
    }
}