using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //专门用于控制面板透明度的组件
    private CanvasGroup canvasGroup;
    //改变透明度的速度
    private float alphaSpeed = 10;

    //当前是自己是显示还是隐藏
    private bool isShow = false;

    //当隐藏完毕后 想要做的事情
    private UnityAction hiteCallBack = null;

    //当面板显示完毕后 想做的逻辑
    // private UnityAction showCallBack = null;

    protected virtual void Awake()
    {
        //得到面板上的CanvasGroup组件  如果没有找到就添加一个
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 注册控件事件的方法 让子类面板注册控件事件
    /// 写成抽象方法 让子类必须实现
    /// </summary>
    public abstract void Init();

    //显示自己的方法
    public virtual void ShowMe(bool isFade = true)
    {
        if (isFade)
            canvasGroup.alpha = 0;
        else
            canvasGroup.alpha = 1;
        isShow = true;

        //showCallBack = callBack;
    }



    //隐藏自己的方法
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;

        hiteCallBack = callBack;
    }


    protected virtual void Update()
    {
        //淡入
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
                //面板透明度为1时 再做的逻辑
                //showCallBack?.Invoke();
            }

        }
        //淡出
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //面板透明度为零时 再做的逻辑
                hiteCallBack?.Invoke();
            }

        }
    }
}
