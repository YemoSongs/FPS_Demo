using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // ��ק������
    public static GameObject itemBeginDragged;

    public Image img;

    // ��ʼλ��
    Vector3 startPos;
    // ��¼��ק����ĸ�����
    public static Transform startParent;

    // ��ʼ��ק
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeginDragged = gameObject;
        startPos = transform.position;
        startParent = transform.parent;
        // ��ק��ʱ�����赲���ߣ���Ȼһ���ڿ����з��õ�ʱ�������䲻��������
        img.raycastTarget = false;
        //transform.parent = UIManager.Instance.GetPanel<BagPanel>().transform;
        transform.SetParent(UIManager.Instance.GetPanel<BagPanel>().transform, false);
    }

    // ��ק��
    public void OnDrag(PointerEventData eventData)
    {
        // 
        transform.position = Input.mousePosition;
    }

    // ������ק
    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeginDragged = null;
        // ������ק�������ܽ�������
        img.raycastTarget = true;
        // ���û�������µĸ����壬�ͻص�ԭ���ĵط���
        if (transform.parent == UIManager.Instance.GetPanel<BagPanel>().transform)
        {
            transform.SetParent(startParent, false);
            transform.position = startPos;
        }

    }


}