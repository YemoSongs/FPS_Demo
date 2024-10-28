using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    //���ڴ洢��ʾ�ŵ����  ÿ��ʾһ�����ͻ��������ֵ�
    //�������ʱ ֱ�ӻ�ȡ�ֵ��еĶ�Ӧ��� �������� 
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //�����ϵ�canvas����  �������ó����ĸ�����
    private Transform canvasTrans;

    private UIManager()
    {
        //�õ������ϵ�canvas����
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;

        //ͨ�����������Ƴ� ��ʵ��������Ϸֻ��һ��Canvas����
        GameObject.DontDestroyOnLoad(canvas);
    }


    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <returns></returns>
    public T ShowPanel<T>(bool isFade = true) where T : BasePanel
    {
        // ����ֻ��Ҫ��֤����T������ �� ���Ԥ��������� ��ͬ   ��һ�������Ĺ���Ϳ��Է�������ʹ��
        string panelName = typeof(T).Name;

        //�ж� �ֵ��� �Ƿ��Ѿ���ʾ��������
        //����ֵ�����������
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        //����ֵ���û�������� 
        //��ʾ���  ������������ ��̬����Ԥ���� ���ø�����
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        //���������ŵ������е�canvas��������
        panelObj.transform.SetParent(canvasTrans, false);

        //ִ������� ����ʾ�߼� �����浽�ֵ��� ����֮��Ļ�ȡ������
        T panel = panelObj.GetComponent<T>();
        panelDic.Add(panelName, panel);
        //��ʾ���
        panel.ShowMe(isFade);


        return panel;
    }



    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <param name="isFade">�Ƿ񵭳�</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //���ݷ��͵�����
        string panelName = typeof(T).Name;
        //�ж��ֵ����Ƿ����
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //��������ɾ
                panelDic[panelName].HideMe(() =>
                {
                    // ɾ��������
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    // ���ֵ����Ƴ�
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                // ɾ��������
                GameObject.Destroy(panelDic[panelName].gameObject);
                // ���ֵ����Ƴ�
                panelDic.Remove(panelName);
            }

        }

    }


    /// <summary>
    /// �õ����
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <returns></returns>
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;

        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[(panelName)] as T;
        }
        // ���û�ж�Ӧ���ͷ��ؿ�
        return null;
    }
}
