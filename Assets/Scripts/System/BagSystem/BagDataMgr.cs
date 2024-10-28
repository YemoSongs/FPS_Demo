using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// �������ݹ�����
/// </summary>
public class BagDataMgr 
{
    private static BagDataMgr instance = new BagDataMgr();
    public static BagDataMgr Instance = instance;


    public WeaponController playerWeaponController;


    /// <summary>
    /// ��ǰ��������
    /// </summary>
    public BagData bagData = new BagData();


    /// <summary>
    /// ����Ʒ���뱳��
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutInBag(ItemInfo itemInfo)
    {

        AddItemType(itemInfo);
    }

    /// <summary>
    /// ����Ʒ�ӱ����ó�
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutOutBag(ItemInfo itemInfo)
    {
        for (int i = 0; i < bagData.allItems.Count; i++)
        {
            if (bagData.allItems[i].id == itemInfo.id)
            {
                RemoveType(bagData.allItems[i]);
                bagData.allItems.RemoveAt(i);
            }
                
        }          
    }





    /// <summary>
    /// ������ʰȡ�� װ���� �����Ǳ��� 
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutInWep123(ItemInfo itemInfo)
    {
        if(bagData.wep_1_Item == null)
        {
            bagData.wep_1_Item = itemInfo;

        }
        else if(bagData.wep_2_Item == null)
        {
            bagData.wep_2_Item = itemInfo;
           
        }
        else if(bagData.wep_3_Item == null)
        {
            bagData.wep_3_Item = itemInfo;
           
        }

    }

    /// <summary>
    /// ��װ���������� ж�� �ŵ�����
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutWep123InBag(ItemInfo itemInfo)
    {

        

        if (bagData.wep_1_Item != null && bagData.wep_1_Item.id == itemInfo.id)
        {
            bagData.wep_1_Item = null;
            playerWeaponController.PutWep123ToBag(itemInfo.id);
            AddItemType(itemInfo);
        }
        else if (bagData.wep_2_Item != null && bagData.wep_2_Item.id == itemInfo.id)
        {
            bagData.wep_2_Item = null;
            playerWeaponController.PutWep123ToBag(itemInfo.id);
            AddItemType(itemInfo);
        }
        else if (bagData.wep_3_Item != null && bagData.wep_3_Item.id == itemInfo.id)
        {
            bagData.wep_3_Item = null;
            playerWeaponController.PutWep123ToBag(itemInfo.id);
            AddItemType(itemInfo);
        }
    }

    /// <summary>
    /// ������������� �ŵ� װ����
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutBagInWep123(ItemInfo itemInfo)
    {

        PutOutBag(itemInfo);

        playerWeaponController.PutBagWepToWep123(itemInfo);

    }


    /// <summary>
    /// �� װ���� ��װ������ 
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutOutWep123(ItemInfo itemInfo)
    {
        
        if (bagData.wep_1_Item!=null && bagData.wep_1_Item.id == itemInfo.id)
        {
            bagData.wep_1_Item = null;
            playerWeaponController.ThrowawayWep123(itemInfo.id);
        }
        else if (bagData.wep_2_Item != null && bagData.wep_2_Item.id == itemInfo.id)
        {
            bagData.wep_2_Item = null;
            playerWeaponController.ThrowawayWep123(itemInfo.id);
        }
        else if (bagData.wep_3_Item != null && bagData.wep_3_Item.id == itemInfo.id)
        {
            bagData.wep_3_Item = null;
            playerWeaponController.ThrowawayWep123(itemInfo.id);
        }

    }








    /// <summary>
    ///  �� ���� ʰȡ�� װ���� �����Ǳ��� 
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutInGre(ItemInfo itemInfo)
    {
        if (bagData.grenade_Item == null)
        {
            bagData.grenade_Item = itemInfo;

            playerWeaponController.gradeThrower.isReadyForThrow = true;
        }
       
    }

    /// <summary>
    /// ��װ���������� ж�� �ŵ�����
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutGreInBag(ItemInfo itemInfo)
    {
        if (bagData.grenade_Item != null)
        {
            bagData.grenade_Item = null;

            AddItemType(itemInfo);

            playerWeaponController.gradeThrower.isReadyForThrow = false;
        }
    }

    /// <summary>
    /// ������������� �ŵ� װ����
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutBagInGre(ItemInfo itemInfo)
    {
        PutOutBag(itemInfo);

        PutInGre(itemInfo);
    }



    /// <summary>
    /// �� װ���� �����׶��� 
    /// </summary>
    /// <param name="itemInfo"></param>
    public void PutOutGre(ItemInfo itemInfo)
    {
        if (bagData.grenade_Item != null )
        {
            bagData.grenade_Item = null;
            playerWeaponController.gradeThrower.isReadyForThrow = false;
            UIManager.Instance.GetPanel<PlayerPanel>().btnGrenades.image.sprite = null;
        }
    }



    /// <summary>
    /// ��Ӵ���Ʒ��  ����
    /// </summary>
    /// <param name="itemInfo"></param>
    void AddItemType(ItemInfo itemInfo)
    {

        switch (itemInfo.type)
        {
            case 0:

                int count0 = bagData.weaponItems.Count;

                if (count0 == 0)
                {
                    bagData.weaponItems.Add(itemInfo);
                    bagData.allItems.Add(itemInfo);
                    return;
                }
                

                for (int i = 0; i < count0; i++)
                {
                    if (bagData.weaponItems[i].id == itemInfo.id)
                    {
                        if((bagData.weaponItems[i].num+ itemInfo.num)<=itemInfo.numMax)
                                bagData.weaponItems[i].num += itemInfo.num;
                        else
                        {
                            bagData.weaponItems.Add(itemInfo);
                            bagData.allItems.Add(itemInfo);
                        }
                        
                        break;
                    }
                    else if (i == count0 - 1)
                    {
                        bagData.weaponItems.Add(itemInfo);
                        bagData.allItems.Add(itemInfo);
                        break;
                    }

                }
                break;
            case 1:

                int count1 = bagData.itemItems.Count;

                if (count1 == 0)
                {
                    bagData.itemItems.Add(itemInfo);
                    bagData.allItems.Add(itemInfo);
                    return;
                }
                for (int i = 0; i < count1; i++)
                {
                    if (bagData.itemItems[i].id == itemInfo.id)
                    {
                        if ((bagData.itemItems[i].num + itemInfo.num) <= itemInfo.numMax)
                            bagData.itemItems[i].num += itemInfo.num;
                        else
                        {
                            bagData.itemItems.Add(itemInfo);
                            bagData.allItems.Add(itemInfo);
                        }
                        break;
                    }
                    else if (i == count1 - 1)
                    {
                        bagData.itemItems.Add(itemInfo);
                        bagData.allItems.Add(itemInfo);
                        break;
                    }

                }
                break;
            case 2:

                int count2 = bagData.protectiveItems.Count;

                if (bagData.protectiveItems.Count == 0)
                {
                    bagData.protectiveItems.Add(itemInfo);
                    bagData.allItems.Add(itemInfo);
                    return;
                }
                

                for (int i = 0; i < count2; i++)
                {
                    if (bagData.protectiveItems[i].id == itemInfo.id)
                    {
                        if ((bagData.protectiveItems[i].num + itemInfo.num) <= itemInfo.numMax)
                            bagData.protectiveItems[i].num += itemInfo.num;
                        else
                        {
                            bagData.protectiveItems.Add(itemInfo);
                            bagData.allItems.Add(itemInfo);
                        }
                        break;
                    }
                    else if (i == count2 - 1)
                    {
                        bagData.protectiveItems.Add(itemInfo);
                        bagData.allItems.Add(itemInfo);
                        break;
                    }

                }
                break;
            case 3:

                int count3 = bagData.otherItems.Count;

                if (bagData.otherItems.Count == 0)
                {
                    bagData.otherItems.Add(itemInfo);
                    bagData.allItems.Add(itemInfo);
                    return;
                }
                

                for (int i = 0; i < count3; i++)
                {
                    if (bagData.otherItems[i].id == itemInfo.id)
                    {
                        if ((bagData.otherItems[i].num + itemInfo.num) <= itemInfo.numMax)
                            bagData.otherItems[i].num += itemInfo.num;
                        else
                        {
                            bagData.otherItems.Add(itemInfo);
                            bagData.allItems.Add(itemInfo);
                        }
                        break;
                    }
                    else if (i == count3 - 1)
                    {
                        bagData.otherItems.Add(itemInfo);
                        bagData.allItems.Add(itemInfo);
                        break;
                    }

                }
                break;
        }

    }

    /// <summary>
    /// �� ���� �Ƴ�����Ʒ
    /// </summary>
    /// <param name="itemInfo"></param>
    void RemoveType(ItemInfo itemInfo)
    {
        switch (itemInfo.type)
        {
            case 0:
                bagData.weaponItems.Remove(itemInfo);
                break;
            case 1:
                bagData.itemItems.Remove(itemInfo);
                break;
            case 2:
                bagData.protectiveItems.Remove(itemInfo);
                break;
            case 3:
                bagData.otherItems.Remove(itemInfo);
                break;
        }
    }


   
}
