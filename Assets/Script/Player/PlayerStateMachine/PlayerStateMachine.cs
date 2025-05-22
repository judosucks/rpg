using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    public PlayerState previousState { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        if (currentState != null)
        {
            Debug.Log("Exiting state: "+currentState.GetType().Name);

            // 添加保护，防止递归切换
            if (currentState == _newState)
            {
                Debug.LogWarning("Attempting to switch to the same state. Ignoring to prevent recursion.");
                return;
            }

            currentState.Exit(); // 调用当前状态的 Exit 方法
        }

        previousState = currentState; // Store the previous state before changing
        currentState = _newState; // 更新状态
        Debug.Log($"State transition: {previousState?.GetType().Name} -> {_newState.GetType().Name}");
        currentState.Enter(); // 调用新状态的 Enter 方法
    }
}
