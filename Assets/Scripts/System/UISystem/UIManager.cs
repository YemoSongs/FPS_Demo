using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    //用于存储显示着的面板  每显示一个面板就会存入这个字典
    //隐藏面板时 直接获取字典中的对应面板 进行隐藏 
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //场景上的canvas对象  用于设置成面板的父对象
    private Transform canvasTrans;

    private UIManager()
    {
        //得到场景上的canvas对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;

        //通过过场景不移除 来实现整个游戏只有一个Canvas对象
        GameObject.DontDestroyOnLoad(canvas);
    }


    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <returns></returns>
    public T ShowPanel<T>(bool isFade = true) where T : BasePanel
    {
        // 我们只需要保证泛型T的类型 和 面板预设体的名字 相同   定一个这样的规则就可以方便我们使用
        string panelName = typeof(T).Name;

        //判断 字典中 是否已经显示了这个面板
        //如果字典中有这个面板
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        //如果字典中没有这个面板 
        //显示面板  根据面板的名字 动态创建预设体 设置父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        //把这个对象放到场景中的canvas对象下面
        panelObj.transform.SetParent(canvasTrans, false);

        //执行面板上 的显示逻辑 并保存到字典中 方便之后的获取和隐藏
        T panel = panelObj.GetComponent<T>();
        panelDic.Add(panelName, panel);
        //显示面板
        panel.ShowMe(isFade);


        return panel;
    }



    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <param name="isFade">是否淡出</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //根据泛型得名字
        string panelName = typeof(T).Name;
        //判断字典中是否存在
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //淡出完再删
                panelDic[panelName].HideMe(() =>
                {
                    // 删除面板对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    // 从字典中移除
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                // 删除面板对象
                GameObject.Destroy(panelDic[panelName].gameObject);
                // 从字典中移除
                panelDic.Remove(panelName);
            }

        }

    }


    /// <summary>
    /// 得到面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <returns></returns>
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;

        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[(panelName)] as T;
        }
        // 如果没有对应面板就返回空
        return null;
    }
}
