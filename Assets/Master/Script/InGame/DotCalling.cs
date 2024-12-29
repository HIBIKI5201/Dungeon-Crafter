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
        [SerializeField, Range(-1, 1)] float _cAngle = 0;
        DotJob _job = new();
        GameObject[] obj;
        NativeArray<float> _result;
        public void Initialize()
        {
            obj = GameObject.FindGameObjectsWithTag("DisappearOnBack");
            _job.objfoward = new NativeArray<float2>(Array.ConvertAll(obj,
                x => new float2(x.transform.forward.x, x.transform.forward.z))
                , Allocator.TempJob);
            _job.objPos = new NativeArray<float2>(Array.ConvertAll(obj,
                x => new float2(x.transform.position.x, x.transform.position.z))
                , Allocator.TempJob);
        }

            void Update()
        {
            _job.cameraPos = new float2(Camera.main.transform.position.x, Camera.main.transform.position.z);
            var result = new NativeArray<float>(_job.objfoward.Length, Allocator.TempJob);
            _job.result = result;
            var dot = _job.Schedule(obj.Length, 0);
            dot.Complete();
            for (var i = 0; i < obj.Length; i++)
            {
                if (result[i] < _cAngle)
                {
                    obj[i].SetActive(true);
                }
                else obj[i].SetActive(false);
            }
            _job.result.Dispose();
        }
    }
    [BurstCompile]
    struct DotJob : IJobParallelFor
    {
        public float2 cameraPos;
        public NativeArray<float2> objfoward;
        public NativeArray<float2> objPos;
        public NativeArray<float> result;
        public void Execute(int index)
        {
            result[index] = math.dot(math.normalize(objfoward[index]), math.normalize(objPos[index] - cameraPos));
        }
    }
}
