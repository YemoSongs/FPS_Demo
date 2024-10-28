using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 道具数据结构类
/// </summary>
public class ItemInfo
{
    public int id;
    public int num;
    public int numMax = 1;
    /// <summary>
    /// 道具的类型 0武器 1道具 2护具 3其他
    /// </summary>
    public int type;
    public string itemName;
    public string icon;
    public string description;
    public string prefab;
}



/// <summary>
/// 道具类型
/// </summary>
public enum ItemType
{
    /// <summary>
    /// 武器类
    /// </summary>
    Weapon,
    /// <summary>
    /// 道具
    /// </summary>
    Item,
    /// <summary>
    /// 护具
    /// </summary>
    Protective,
    /// <summary>
    /// 其他
    /// </summary>
    Other
}



/// <summary>
/// UI界面的道具物体
/// </summary>
public class UIItem : MonoBehaviour
{

    public ItemInfo info;

    public Image itemImg;

    public TextMeshProUGUI itemNum;

    public ItemType itemType;

    /// <summary>
    /// 初始化道具UI信息
    /// </summary>
    /// <param name="itemInfo"></param>
    public void InitInfo(ItemInfo itemInfo)
    {
        itemType = (ItemType)itemInfo.type;

        itemImg.sprite = Resources.Load<Sprite>(itemInfo.icon);

        itemNum.text = itemInfo.num.ToString();

        info = itemInfo;
    }

    /// <summary>
    /// 点击该UI物体 就会更新信息
    /// </summary>
    public void ClickThisItem()
    {
        UIManager.Instance.GetPanel<BagPanel>().nowItemInfo.gameObject.SetActive(true);
        UIManager.Instance.GetPanel<BagPanel>().nowItemInfo.UpdateInfo(this);

    }




}
