using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BuffSystem;

public class ShowBuffInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_BuffName;
    [SerializeField]
    private TextMeshProUGUI m_Description;
    [SerializeField]
    private TextMeshProUGUI m_Provider;
    #region Public方法
    public void ShowInfo(BuffBase buff)
    {
        m_BuffName.text = buff.Name;
        m_Description.text = buff.Description;
        m_Provider.text = "来自：" + buff.Provider;
    }
    #endregion

    #region Private方法

    #endregion

    #region Unity方法
    #endregion
}
