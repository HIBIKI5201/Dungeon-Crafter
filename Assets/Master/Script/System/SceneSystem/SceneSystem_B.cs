using DCFrameWork.InputBuffer;
using DCFrameWork.MainSystem;
using UnityEngine;


public abstract class SceneSystem_B : MonoBehaviour
{
    public GameBaseSystem System;
    protected InputBuffer_B _input;
    protected UIManager_B _UIManager;

    public void Init(GameBaseSystem system)
    {
        System = system;
        _UIManager = FindAnyObjectByType<UIManager_B>();
        if (_UIManager is null)
            Debug.LogWarning("UIManagerが見つかりませんでした");
        _input = FindAnyObjectByType<InputBuffer_B>();
        if (_input is null)
            Debug.LogWarning("InputBufferが見つかりませんでした");
    }

    private void Update()
    {
        Think(_input.GetContext());
    }

    /// <summary>
    /// Updateで呼ばれます
    /// シーンのマネジメントを行ってください
    /// </summary>
    protected abstract void Think(InputContext input);
}
