using BuffSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class UI_BuffItem : MonoBehaviour
{
    [SerializeField]
    private GameObject m_VerticalLine;
    [SerializeField]
    private Transform m_RollLine;
    [SerializeField]
    private Image m_Mask_M;
    [SerializeField]
    private TextMeshProUGUI m_Level;
    [SerializeField]
    private ShowBuffInfo m_ShowBuffInfo;
    [SerializeField]
    private Image m_Frame;
    [SerializeField]
    private Image m_Icon;


    private ObjectPool<UI_BuffItem> m_RecyclePool;


    private bool m_Initialized = false;
    private bool m_NeedNumber = false;
    private bool m_NeedLine = false;


    private BuffBase m_TargetBuff;


    #region Public方法
    public void OnPointerEnter()
    {
        m_ShowBuffInfo.gameObject.SetActive(true);
        m_ShowBuffInfo.ShowInfo(m_TargetBuff);
    }
    public void OnPointerExit()
    {
        m_ShowBuffInfo.gameObject.SetActive(false);
    }
    public void Initialize(BuffBase buff, ObjectPool<UI_BuffItem> recyclePool)
    {
        m_Icon.sprite = Resources.Load<Sprite>(buff.IconPath);
        m_TargetBuff = buff;
        m_RecyclePool = recyclePool;
        if (m_TargetBuff.MaxLevel > 1)
        {
            m_NeedNumber = true;
            m_Level.gameObject.SetActive(true);
        }
        else
        {
            m_NeedNumber = false;
            m_Level.gameObject.SetActive(false);
        }


        if (m_TargetBuff.TimeScale > 0)
        {
            m_NeedLine = true;
            m_RollLine.gameObject.SetActive(true);
            m_VerticalLine.SetActive(true);
            m_Mask_M.gameObject.SetActive(true);
        }
        else
        {
            m_NeedLine = false;
            m_RollLine.gameObject.SetActive(false);
            m_VerticalLine.SetActive(false);
            m_Mask_M.gameObject.SetActive(false);
        }

        switch (buff.BuffType)
        {
            case BuffType.Buff:
                m_Frame.color = ColorLibrary.UI_Buff;
                break;
            case BuffType.Debuff:
                m_Frame.color = ColorLibrary.UI_Debuff;
                break;
            case BuffType.None:
                m_Frame.color = ColorLibrary.UI_NoneBuff;
                break;
            default:
                break;
        }
        m_Initialized = true;
    }
    #endregion

    #region Private方法

    #endregion

    #region Unity方法
    private void Update()
    {
        if (m_Initialized)
        {
            //需要显示计时工具才显示
            if (m_NeedLine)
            {
                m_Mask_M.fillAmount = 1 - (m_TargetBuff.ResidualDuration / m_TargetBuff.MaxDuration);
                m_RollLine.eulerAngles = new Vector3(0, 0, m_Mask_M.fillAmount * -360);
            }
            //需要显示等级才显示
            if (m_NeedNumber)
            {
                m_Level.text = m_TargetBuff.CurrentLevel.ToString();
            }
            //如果当前等级等于零说明他已经被废弃了，所以就可以回收了
            if (m_TargetBuff.CurrentLevel == 0 )
            {
                m_RecyclePool.Release(this);
            }
        }
    }
    #endregion
}
