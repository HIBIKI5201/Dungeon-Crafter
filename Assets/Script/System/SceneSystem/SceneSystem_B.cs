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
    /// ��������ɏ������s�������ꍇ�Ɏg�p���܂�
    /// </summary>
    protected virtual void Init_S() { }

    /// <summary>
    /// Update�ŌĂ΂�܂�
    /// �V�[���̃}�l�W�����g���s���Ă�������
    /// </summary>
    protected abstract void Think();
}
