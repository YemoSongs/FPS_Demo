using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickBagItemInfo : MonoBehaviour
{
    public Image imgHead;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtNum;
    public TextMeshProUGUI txtInfo;


    public void UpdateInfo(UIItem uIItem)
    {
        imgHead.sprite = uIItem.itemImg.sprite;
        txtName.text = uIItem.info.itemName;
        txtNum.text = uIItem.info.num.ToString();
    }
}
