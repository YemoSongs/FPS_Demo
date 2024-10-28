using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �������ݽṹ��
/// </summary>
public class ItemInfo
{
    public int id;
    public int num;
    public int numMax = 1;
    /// <summary>
    /// ���ߵ����� 0���� 1���� 2���� 3����
    /// </summary>
    public int type;
    public string itemName;
    public string icon;
    public string description;
    public string prefab;
}



/// <summary>
/// ��������
/// </summary>
public enum ItemType
{
    /// <summary>
    /// ������
    /// </summary>
    Weapon,
    /// <summary>
    /// ����
    /// </summary>
    Item,
    /// <summary>
    /// ����
    /// </summary>
    Protective,
    /// <summary>
    /// ����
    /// </summary>
    Other
}



/// <summary>
/// UI����ĵ�������
/// </summary>
public class UIItem : MonoBehaviour
{

    public ItemInfo info;

    public Image itemImg;

    public TextMeshProUGUI itemNum;

    public ItemType itemType;

    /// <summary>
    /// ��ʼ������UI��Ϣ
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
    /// �����UI���� �ͻ������Ϣ
    /// </summary>
    public void ClickThisItem()
    {
        UIManager.Instance.GetPanel<BagPanel>().nowItemInfo.gameObject.SetActive(true);
        UIManager.Instance.GetPanel<BagPanel>().nowItemInfo.UpdateInfo(this);

    }




}
