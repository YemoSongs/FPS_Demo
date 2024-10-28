using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BagType
{
    Bag,
    Wep123,
    Gre,
    Per12
}



public class SlotHandle : MonoBehaviour, IDropHandler
{

    [SerializeField]private bool isIntoBag;
    [SerializeField]private bool isOutBag;

    public Transform content;

    public ItemType type;

    public BagType bagType;

    public GameObject item
    {
        get
        {
            // ������������ڷ��أ�û�о�Ϊ��
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    
    public void OnDrop(PointerEventData eventData)
    {
        if(isIntoBag)
        {
            if (DragHandle.itemBeginDragged != null)
            {
                DragHandle.itemBeginDragged.transform.SetParent(content);

                UIManager.Instance.GetPanel<BagPanel>().items.Add(DragHandle.itemBeginDragged.GetComponent<UIItem>());
                //�ж�����Ҳ�װ������Ʒ��ʲô���� ���� ���� ����
                //�ֱ�ִ�� ��װ����ж�� ���뱳���ķ���
                switch (DragHandle.itemBeginDragged.GetComponent<UIItem>().info.type)
                {
                    case 0:
                        BagDataMgr.Instance.PutWep123InBag(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);

                        break;
                    case 1:
                        BagDataMgr.Instance.PutGreInBag(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);
                        break;
                    case 2:

                        break;
                    case 3:
                        break;
                }
                


            }
                
            return;
        }
        if(isOutBag)
        {
            if ((DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Bag))
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>(DragHandle.itemBeginDragged.GetComponent<UIItem>().info.prefab),
                GameObject.FindWithTag("Player").transform.position + Vector3.forward, Quaternion.identity);
                obj.GetComponent<ItemBase>().InitNum(DragHandle.itemBeginDragged.GetComponent<UIItem>().info.num);
                BagDataMgr.Instance.PutOutBag(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);
            }
            else if (DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Wep123)
            {
                BagDataMgr.Instance.PutOutWep123(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);
            }
            else if(DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Gre)
            {
                //�������������׶���
                GameObject obj = Instantiate(Resources.Load<GameObject>(DragHandle.itemBeginDragged.GetComponent<UIItem>().info.prefab),
                GameObject.FindWithTag("Player").transform.position + Vector3.forward, Quaternion.identity);
                obj.GetComponent<ItemBase>().InitNum(DragHandle.itemBeginDragged.GetComponent<UIItem>().info.num);
                BagDataMgr.Instance.PutOutGre(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);
            }
            else if ((DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Per12))
            {
                //�ѻ������Ļ��߶���
            }
           
            DragHandle.itemBeginDragged.gameObject.SetActive(false);
            return;
        }

        // �������û�����壬���ܷ���
        if (!item && DragHandle.itemBeginDragged.GetComponent<UIItem>().itemType==type)
        {
            DragHandle.itemBeginDragged.transform.SetParent(transform);
            DragHandle.itemBeginDragged.transform.localPosition = Vector3.zero;
            
            //���ݵ�ǰ�϶���UIItem������ ���ж� ִ��������Ʒ�� �ӱ����ŵ�װ�����ķ���
            switch (type)
            {
                case ItemType.Weapon:
                    //����
                    if (DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Wep123)
                        break;
                    BagDataMgr.Instance.PutBagInWep123(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);

                    break;


                case ItemType.Item:
                    //����
                    if (DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Gre)
                        break;
                    BagDataMgr.Instance.PutBagInGre(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);
                   
                    break;


                case ItemType.Protective:
                    //����
                    if (DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Per12)
                        break;
                    

                    break;


                case ItemType.Other:
                   
                    break;
            }


        }


    }


}