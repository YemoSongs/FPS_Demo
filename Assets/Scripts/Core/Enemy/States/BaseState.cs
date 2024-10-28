using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各个状态的基类 
/// </summary>
public abstract class BaseState 
{
    /// <summary>
    /// 状态持有者
    /// </summary>
    public Enemy enemy;

    /// <summary>
    /// 状态机 管理该状态
    /// </summary>
    public StateMachine stateMachine;

    /// <summary>
    /// 进入该状态
    /// </summary>
    public abstract void Enter();
    /// <summary>
    /// 该状态中
    /// </summary>
    public abstract void Perform();
    /// <summary>
    /// 退出该状态
    /// </summary>
    public abstract void Exit();
}
