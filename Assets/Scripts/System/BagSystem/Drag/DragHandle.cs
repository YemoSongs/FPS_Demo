using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // 拖拽的物体
    public static GameObject itemBeginDragged;

    public Image img;

    // 初始位置
    Vector3 startPos;
    // 记录拖拽物体的父物体
    public static Transform startParent;

    // 开始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeginDragged = gameObject;
        startPos = transform.position;
        startParent = transform.parent;
        // 拖拽的时候不能阻挡射线，不然一会在卡槽中放置的时候，射线射不到卡槽上
        img.raycastTarget = false;
        //transform.parent = UIManager.Instance.GetPanel<BagPanel>().transform;
        transform.SetParent(UIManager.Instance.GetPanel<BagPanel>().transform, false);
    }

    // 拖拽中
    public void OnDrag(PointerEventData eventData)
    {
        // 
        transform.position = Input.mousePosition;
    }

    // 结束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeginDragged = null;
        // 结束拖拽后，让它能接受射线
        img.raycastTarget = true;
        // 如果没有设置新的父物体，就回到原来的地方。
        if (transform.parent == UIManager.Instance.GetPanel<BagPanel>().transform)
        {
            transform.SetParent(startParent, false);
            transform.position = startPos;
        }

    }


}