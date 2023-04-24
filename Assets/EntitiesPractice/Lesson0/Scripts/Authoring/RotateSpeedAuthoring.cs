using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;


namespace DOTS.DOD.LESSON0 
{
    struct RotateSpeed : IComponentData
    {
        public float rotateSpeed;
        //可以封装不止一个数据，
    }
    public class RotateSpeedAuthoring : MonoBehaviour
    {
        [Range(0, 360)] public float rotateSpeed = 200;

        public class Baker : Baker<RotateSpeedAuthoring>//必须和class名字相同，相当于针对这个class的baker
        {
            public override void Bake(RotateSpeedAuthoring authoring) //参数还要传一次 
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic); //GetEntity在Baker.cs中的抽象类IBaker中定义
                //entity通过GetEntity新建，但是Component直接new就可以！不是通过GetComponent！
                var component = new RotateSpeed { rotateSpeed = math.radians(authoring.rotateSpeed) };
                AddComponent<RotateSpeed>(entity,component);//也可以添加多个Component
            }
        }
    }
}

