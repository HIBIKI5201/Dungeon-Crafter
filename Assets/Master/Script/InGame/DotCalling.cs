using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace DCFrameWork
{
    public class DotCalling : MonoBehaviour
    {
        DotJob _job=new();
        MeshRenderer[] _mesh;
        void Initialize()
        {
            var objects = GameObject.FindGameObjectsWithTag("DisappearOnBack");
            _mesh = Array.ConvertAll(objects, x=>x.GetComponent<MeshRenderer>());
            float3[] foward = Array.ConvertAll(_mesh,x=>new float3( x.transform.forward.x,x.transform.forward.y,x.transform.forward.z));
            var nArray = new NativeArray<float3>(foward,Allocator.TempJob);
            for(var i =0; i < _mesh.Length; i++)
            {
                nArray[i] = new float3(foward[i].x, foward[i].y, foward[i].z);
            }
            _job.objfoward = nArray;
        }

        void Update()
        {
            var a = _job.Schedule(_mesh.Length, 0);
            a.Complete();
            for (var i = 0; i < _mesh.Length; i++)
            {
                if (_job.result[i] < 0)
                {
                    _mesh[i].enabled = false;
                }
                else _mesh[i].enabled = true;
            }
        }
    }
    [BurstCompile]
    struct DotJob : IJobParallelFor
    {
        public float3 cameraPos;
        public NativeArray<float3> objfoward;
        public NativeArray<float> result;
        public void Execute(int index)
        {
            result[index] = math.dot(cameraPos, objfoward[index]);
        }
    }
}
