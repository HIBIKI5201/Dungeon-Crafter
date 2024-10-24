using DCFrameWork.InputBuffer;
using DCFrameWork.MainSystem;
using UnityEngine;


public abstract class SceneSystem_B<Input, UIManager> : MonoBehaviour where Input : InputBuffer_B where UIManager : UIManager_B
{
    [HideInInspector]
    public GameBaseSystem System;
    protected Input _input;
    protected UIManager _UIManager;

    private void Start()
    {
        Init(null);
    }

    public void Init(GameBaseSystem system)
    {
        System = system;
        _UIManager = transform.GetComponentInChildren<UIManager>();
        if (_UIManager is null)
            Debug.LogWarning("UIManager��������܂���ł���");
        _input = GetComponentInChildren<Input>();
        if (_input is null)
            Debug.LogWarning("InputBuffer��������܂���ł���");

        Init_S();
    }

    protected virtual void Init_S() { }

    private void Update()
    {
        Think(_input.GetContext());
    }

    /// <summary>
    /// Update�ŌĂ΂�܂�
    /// �V�[���̃}�l�W�����g���s���Ă�������
    /// </summary>
    protected abstract void Think(InputContext input);
}
