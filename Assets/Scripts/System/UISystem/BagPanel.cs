using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{

    

    public Button btnClose;

    public RectTransform content;

    public ClickBagItemInfo nowItemInfo;


    public RectTransform wep1Transform;
    public RectTransform wep2Transform;
    public RectTransform wep3Transform;
    public RectTransform graTransform;
    public RectTransform per1Transform;
    public RectTransform per2Transform;



    public Toggle togAll;
    public Toggle togWeapon;
    public Toggle togItem;
    public Toggle togPretective;
    public Toggle togOther;

    public GameObject player;

    /// <summary>
    /// 当前背包显示的物体
    /// </summary>
    public List<UIItem> items;

    public UIItem wep_1;
    public UIItem wep_2;
    public UIItem wep_3;

    public UIItem grenadeItem;

    public UIItem pertectItem_1;
    public UIItem pertectItem_2;


    public override void Init()
    {
        nowItemInfo.gameObject.SetActive(false);

        player = GameObject.FindWithTag("Player");

        player.GetComponent<InputManager>().enabled = false;

        UpdateRightWepItems();

        TogValueChanged(true);

        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<BagPanel>();
            player.GetComponent<InputManager>().enabled = true;

        });

        togAll.onValueChanged.AddListener(TogValueChanged);
        togWeapon.onValueChanged.AddListener(TogValueChanged);
        togItem.onValueChanged.AddListener(TogValueChanged);
        togPretective.onValueChanged.AddListener(TogValueChanged);
        togOther.onValueChanged.AddListener(TogValueChanged);
    }

    /// <summary>
    /// 背包页签发生变化时调用的方法，用来刷新背包显示
    /// </summary>
    /// <param name="v"></param>
    void TogValueChanged(bool v)
    {
        

        List<ItemInfo> itemsInfo = BagDataMgr.Instance.bagData.allItems;

        if (togAll.isOn)
        {
            UpdateBagItems(itemsInfo);
        }
        else if(togWeapon.isOn)
        {
            itemsInfo = BagDataMgr.Instance.bagData.weaponItems;
            UpdateBagItems(itemsInfo);
        }
        else if (togItem.isOn)
        {
            itemsInfo = BagDataMgr.Instance.bagData.itemItems;
            UpdateBagItems(itemsInfo);
        }
        else if (togPretective.isOn) 
        {
            itemsInfo = BagDataMgr.Instance.bagData.protectiveItems;
            UpdateBagItems(itemsInfo);
        }
        else if(togOther.isOn)
        {
            itemsInfo = BagDataMgr.Instance.bagData.otherItems;
            UpdateBagItems(itemsInfo);
        }

        UpdateRightWepItems();
    }

    /// <summary>
    /// 更新背包物品格子
    /// </summary>
    /// <param name="itemsInfo"></param>
    void UpdateBagItems(List<ItemInfo> itemsInfo)
    {

        //更新SV
        //先删除之前的格子 
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].gameObject != null)
                Destroy(items[i].gameObject);
        }

        items.Clear();

       


        //再更新现在的数据
        //动态创建UIItem预设体 并且把它存到items里面 方便下一次更新的时候删除
        for (int i = 0; i < itemsInfo.Count; i++)
        {
            //实例化预设体 并且得到身上的UIItem脚本
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/UIItem"));

            UIItem cell = obj.GetComponent<UIItem>();
            //设置父对象
            cell.transform.SetParent(content, false);
            //初始化数据
            cell.InitInfo(itemsInfo[i]);
            //把他存入items
            items.Add(cell);
        }



    }

    /// <summary>
    /// 更新右侧装备栏
    /// </summary>
    void UpdateRightWepItems()
    {
        if (wep_1 != null)
        {
            Destroy(wep_1.gameObject);
            wep_1 = null;
        }
           
        if (wep_2 != null)
        {
            Destroy(wep_2.gameObject);
            wep_2 = null;
        }
            
        if (wep_3 != null)
        {
            Destroy(wep_3.gameObject);
            wep_3 = null;
        }
            
        if(grenadeItem != null)
        {
            Destroy(grenadeItem.gameObject);
            grenadeItem = null;
        }



        ItemInfo _wep_1 = BagDataMgr.Instance.bagData.wep_1_Item;
        ItemInfo _wep_2 = BagDataMgr.Instance.bagData.wep_2_Item;
        ItemInfo _wep_3 = BagDataMgr.Instance.bagData.wep_3_Item;
        ItemInfo _gre = BagDataMgr.Instance.bagData.grenade_Item;


        if (_wep_1 != null)
        {
            //实例化预设体 并且得到身上的UIItem脚本
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/UIItem"));

            UIItem cell = obj.GetComponent<UIItem>();
            //设置父对象
            cell.transform.SetParent(wep1Transform, false);
            cell.transform.localPosition = Vector3.zero;
            //初始化数据
            cell.InitInfo(_wep_1);
            //把他存入items
            wep_1 = cell;
        }
        if(_wep_2 != null)
        {
            //实例化预设体 并且得到身上的UIItem脚本
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/UIItem"));

            UIItem cell = obj.GetComponent<UIItem>();
            //设置父对象
            cell.transform.SetParent(wep2Transform, false);
            cell.transform.localPosition = Vector3.zero;
            //初始化数据
            cell.InitInfo(_wep_2);
            //把他存入items
            wep_2 = cell;
        }
        if (_wep_3 != null)
        {
            //实例化预设体 并且得到身上的UIItem脚本
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/UIItem"));

            UIItem cell = obj.GetComponent<UIItem>();
            //设置父对象
            cell.transform.SetParent(wep3Transform, false);
            cell.transform.localPosition = Vector3.zero;
            //初始化数据
            cell.InitInfo(_wep_3);
            //把他存入items
            wep_3 = cell;
        }

        if (_gre != null)
        {
            //实例化预设体 并且得到身上的UIItem脚本
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("UI/UIItem"));

            UIItem cell = obj.GetComponent<UIItem>();
            //设置父对象
            cell.transform.SetParent(graTransform, false);
            cell.transform.localPosition = Vector3.zero;
            //初始化数据
            cell.InitInfo(_gre);
            //把他存入items
            grenadeItem = cell;

            UIManager.Instance.GetPanel<PlayerPanel>().btnGrenades.image.sprite = cell.itemImg.sprite;
        }

    }


}
