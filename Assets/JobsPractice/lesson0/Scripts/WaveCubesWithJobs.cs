using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections; //[ReadOnly]属于这个包
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Jobs; //TransformAccessArray属于这个包
using Unity.Burst;
using Unity.Mathematics;
using UnityEditor;
using Unity.Profiling;

namespace Jobs.DOD
{
    [BurstCompile]
    struct CubesWaveJob : IJobParallelForTransform
    {
        [ReadOnly] public float elapsedTime; //记得传递时间参数，并且加上[ReadOnly]优化
        public void Execute(int index, TransformAccess transform) //实现"接口"不需要override关键词！因为是实现不是重写
                                                                  //这里不是TransformAccessArray而是单个的TransformAccess
        {
            var distFromOrigin = math.distance(transform.position, new float3(0, 0, 0)); //用math.distance不要用Vector3.distance
            transform.localPosition += Vector3.up * math.sin(distFromOrigin*0.2f+elapsedTime*3f); //用Vector3.up乘 是最好的方法
        }
    }
    public class WaveCubesWithJobs : MonoBehaviour
    {
        public GameObject cubePrefab = null;
        [Range(10, 100)] public int xHalfCount = 50;
        [Range(10, 100)] public int zHalfCount = 50;
        private TransformAccessArray transformArray;
        static readonly ProfilerMarker profilerMarker = new ProfilerMarker("WaveCubesWithJobs update Transform"); 
        //如果unity中运行起来看不到profiler窗口，就像windows-analysis-profiler这样添加该窗口

        void Start()
        {
            int totalNum = xHalfCount * zHalfCount * 4;
            transformArray = new TransformAccessArray(totalNum);
            for(var z = -zHalfCount; z < zHalfCount; z++)
            {
                for (var x = -xHalfCount; x < xHalfCount; x++) 
                {
                    var tempObject = Instantiate<GameObject>(cubePrefab);
                    tempObject.transform.position = new Vector3(x * 1.1f, 0, z * 1.1f);
                    transformArray.Add(tempObject.transform);
                }
            }
        }

        void Update()
        {
            using (profilerMarker.Auto())
            {
                var cubeWaveJob = new CubesWaveJob { elapsedTime = Time.time };//在实例化时把需要的参数传进去，注意是大括号
                var jobHandle = cubeWaveJob.Schedule(transformArray);
                jobHandle.Complete();
            }
        }
         
        private void OnDestroy()
        {
            transformArray.Dispose();
        }
    }

}
