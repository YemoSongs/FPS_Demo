using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ɽ�����Ʒ ����
/// </summary>
public abstract class Interactable : MonoBehaviour
{

    public bool useEvents;

    /// <summary>
    /// ����ҿ���ɽ�������ʱ չʾ����Ϣ
    /// </summary>
    public string promptMessage;

    public virtual string OnLook()
    {
        return promptMessage;
    }


    /// <summary>
    /// ��ҵ��õĽ�������
    /// </summary>
    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }
    /// <summary>
    /// ������Ʒ�Ľ����߼� ��������д
    /// </summary>
    protected virtual void Interact()
    {

    }


}
