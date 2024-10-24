using DCFrameWork.InputBuffer;
using DCFrameWork.MainSystem;
using UnityEngine;


public abstract class SceneSystem_B : MonoBehaviour
{
    [HideInInspector]
    public GameBaseSystem System;
    protected InputBuffer_B _input;
    protected UIManager_B _UIManager;

    private void Start()
    {
        Init(null);
    }

    public void Init(GameBaseSystem system)
    {
        System = system;
        _UIManager = transform.GetComponentInChildren<UIManager_B>();
        if (_UIManager is null)
            Debug.LogWarning("UIManager��������܂���ł���");
        _input = GetComponentInChildren<InputBuffer_B>();
        if (_input is null)
            Debug.LogWarning("InputBuffer��������܂���ł���");

        Init_S();
    }

    protected virtual void Init_S() { }

    private void Update()
    {
        Think(_input?.GetContext() ?? new());
    }

    /// <summary>
    /// Update�ŌĂ΂�܂�
    /// �V�[���̃}�l�W�����g���s���Ă�������
    /// </summary>
    protected abstract void Think(InputContext input);
}
