using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可交互物品 基类
/// </summary>
public abstract class Interactable : MonoBehaviour
{

    public bool useEvents;

    /// <summary>
    /// 当玩家看向可交互物体时 展示的信息
    /// </summary>
    public string promptMessage;

    public virtual string OnLook()
    {
        return promptMessage;
    }


    /// <summary>
    /// 玩家调用的交互方法
    /// </summary>
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }
    /// <summary>
    /// 交互物品的交互逻辑 让子类重写
    /// </summary>
    protected virtual void Interact()
    {

    }


}
