using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //ר�����ڿ������͸���ȵ����
    private CanvasGroup canvasGroup;
    //�ı�͸���ȵ��ٶ�
    private float alphaSpeed = 10;

    //��ǰ���Լ�����ʾ��������
    private bool isShow = false;

    //��������Ϻ� ��Ҫ��������
    private UnityAction hiteCallBack = null;

    //�������ʾ��Ϻ� �������߼�
    // private UnityAction showCallBack = null;

    protected virtual void Awake()
    {
        //�õ�����ϵ�CanvasGroup���  ���û���ҵ������һ��
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// ע��ؼ��¼��ķ��� ���������ע��ؼ��¼�
    /// д�ɳ��󷽷� ���������ʵ��
    /// </summary>
    public abstract void Init();

    //��ʾ�Լ��ķ���
    public virtual void ShowMe(bool isFade = true)
    {
        if (isFade)
            canvasGroup.alpha = 0;
        else
            canvasGroup.alpha = 1;
        isShow = true;

        //showCallBack = callBack;
    }



    //�����Լ��ķ���
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;

        hiteCallBack = callBack;
    }


    protected virtual void Update()
    {
        //����
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
                //���͸����Ϊ1ʱ �������߼�
                //showCallBack?.Invoke();
            }

        }
        //����
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //���͸����Ϊ��ʱ �������߼�
                hiteCallBack?.Invoke();
            }

        }
    }
}
