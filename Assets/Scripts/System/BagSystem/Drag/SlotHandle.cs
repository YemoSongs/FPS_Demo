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
            // 如果有子物体在返回，没有就为空
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
                //判断这个右侧装备栏物品是什么类型 武器 手雷 护具
                //分别执行 从装备栏卸下 放入背包的方法
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
                //把手雷区的手雷丢弃
                GameObject obj = Instantiate(Resources.Load<GameObject>(DragHandle.itemBeginDragged.GetComponent<UIItem>().info.prefab),
                GameObject.FindWithTag("Player").transform.position + Vector3.forward, Quaternion.identity);
                obj.GetComponent<ItemBase>().InitNum(DragHandle.itemBeginDragged.GetComponent<UIItem>().info.num);
                BagDataMgr.Instance.PutOutGre(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);
            }
            else if ((DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Per12))
            {
                //把护具区的护具丢弃
            }
           
            DragHandle.itemBeginDragged.gameObject.SetActive(false);
            return;
        }

        // 如果上面没有物体，就能放置
        if (!item && DragHandle.itemBeginDragged.GetComponent<UIItem>().itemType==type)
        {
            DragHandle.itemBeginDragged.transform.SetParent(transform);
            DragHandle.itemBeginDragged.transform.localPosition = Vector3.zero;
            
            //根据当前拖动的UIItem的类型 来判断 执行那种物品的 从背包放到装备栏的方法
            switch (type)
            {
                case ItemType.Weapon:
                    //武器
                    if (DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Wep123)
                        break;
                    BagDataMgr.Instance.PutBagInWep123(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);

                    break;


                case ItemType.Item:
                    //手雷
                    if (DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Gre)
                        break;
                    BagDataMgr.Instance.PutBagInGre(DragHandle.itemBeginDragged.GetComponent<UIItem>().info);
                   
                    break;


                case ItemType.Protective:
                    //护具
                    if (DragHandle.startParent.GetComponentInParent<SlotHandle>().bagType == BagType.Per12)
                        break;
                    

                    break;


                case ItemType.Other:
                   
                    break;
            }


        }


    }


}