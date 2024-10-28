using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public int id;
    public int num = 1;
    public int numMax = 1;
    public int type;
    public string itemName;
    public string icon;
    public string description;
    public string prefab;

    public ItemInfo itemInfo = new ItemInfo();

    private void Awake()
    {

        InitThisItem();
       
    }

    public ItemInfo InitThisItem()
    {
        itemInfo.id = this.id;
        itemInfo.num = this.num;
        itemInfo.numMax = this.numMax;
        itemInfo.type = this.type;
        itemInfo.itemName = this.itemName;
        itemInfo.icon = this.icon;
        itemInfo.description = this.description;
        itemInfo.prefab = this.prefab;
        return itemInfo;
    }


    public void InitNum(int num)
    {
        this.num = num;
    }


}
