using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : BasePanel
{

    [SerializeField]
    private TextMeshProUGUI proptText;

    public Image FHealthBar;
    public Image BHealthBar;

    public Image overlay;

    public FloatingJoystick joystick;

    public Button btnBag;
    public Button btnGrenades;

    /// <summary>
    /// 更新互动信息
    /// </summary>
    /// <param name="text"></param>
    public void UndateText(string text)
    {
        proptText.text = text;
    }


    public override void Init()
    {
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);

        btnBag.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<BagPanel>();
        });

    }



}
