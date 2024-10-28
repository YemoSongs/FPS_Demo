using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����״̬�Ļ��� 
/// </summary>
public abstract class BaseState 
{
    /// <summary>
    /// ״̬������
    /// </summary>
    public Enemy enemy;

    /// <summary>
    /// ״̬�� �����״̬
    /// </summary>
    public StateMachine stateMachine;

    /// <summary>
    /// �����״̬
    /// </summary>
    public abstract void Enter();
    /// <summary>
    /// ��״̬��
    /// </summary>
    public abstract void Perform();
    /// <summary>
    /// �˳���״̬
    /// </summary>
    public abstract void Exit();
}
