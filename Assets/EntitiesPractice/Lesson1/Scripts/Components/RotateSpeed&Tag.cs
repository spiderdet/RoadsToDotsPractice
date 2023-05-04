using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DOTS.DOD.LESSON1
{
    struct RotateSpeed : IComponentData
    {
        public float rotateSpeed;
    }
    struct GreenTag : IComponentData { }
    struct BlueTag : IComponentData { }

}
