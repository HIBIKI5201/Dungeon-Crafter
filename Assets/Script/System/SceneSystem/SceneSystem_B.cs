using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SceneSystem_B : MonoBehaviour
{
    public MainSystem System;

    public void Init(MainSystem system)
    {
        System = system;
        Init_S();
    }

    private void Update()
    {
        Think();
    }

    /// <summary>
    /// 初期化後に処理を行いたい場合に使用します
    /// </summary>
    protected virtual void Init_S() { }

    /// <summary>
    /// Updateで呼ばれます
    /// シーンのマネジメントを行ってください
    /// </summary>
    protected abstract void Think();
}
