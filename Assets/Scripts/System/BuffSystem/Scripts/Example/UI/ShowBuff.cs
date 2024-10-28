using BuffSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShowBuff : MonoBehaviour
{
    [SerializeField]
    private GameObject m_BuffItemTemplate;
    [SerializeField]
    private GameObject m_Pool;
    [SerializeField]
    private GameObject m_Buffs;
    [SerializeField]
    private Animate m_Hero;

    private ObjectPool<UI_BuffItem> m_BuffItemPool;
    #region Public方法

    #endregion

    #region Private方法
    private UI_BuffItem Pool_CreateFunc()
    {

        return Instantiate(m_BuffItemTemplate, this.transform).GetComponent<UI_BuffItem>();
    }
    private void Pool_ActionOnGet(UI_BuffItem UI_BuffItem)
    {
        UI_BuffItem.gameObject.SetActive(true);
        UI_BuffItem.transform.SetParent(m_Buffs.transform);
    }
    private void Pool_ActionOnRelease(UI_BuffItem UI_BuffItem)
    {
        UI_BuffItem.gameObject.SetActive(false);
        UI_BuffItem.transform.SetParent(m_Pool.transform);
    }

    private void Pool_ActionOnDestroy(UI_BuffItem UI_BuffItem)
    {
        Destroy(UI_BuffItem.gameObject);
    }
    private void BuffListener(BuffBase newBuff)
    {
        ShowBuffCore(newBuff);
    }

    private void ShowBuffCore(BuffBase buff)
    {
        m_BuffItemPool.Get().Initialize(buff, m_BuffItemPool);
    }
    #endregion

    #region Unity方法
    private void Awake()
    {
        m_BuffItemPool = new ObjectPool<UI_BuffItem>(
                    Pool_CreateFunc,
                    Pool_ActionOnGet,
                    Pool_ActionOnRelease,
                    Pool_ActionOnDestroy,
                    true,
                    100,
                    10000
                    );

        foreach (BuffBase item in BuffManager.Instance.StartObserving(m_Hero.gameObject, BuffListener))
        {
            ShowBuffCore(item);
        }
    }
    #endregion
}
