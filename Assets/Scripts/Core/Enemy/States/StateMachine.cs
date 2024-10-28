
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ����״̬���� �������״̬
/// </summary>
public class StateMachine : MonoBehaviour
{
    /// <summary>
    /// ��ǰ�����״̬
    /// </summary>
    public BaseState activeState;

    

    /// <summary>
    /// ��ʼ��״̬��
    /// </summary>
    public void Initialise()
    {
       
        ChangeState(new PatrolState());
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activeState!=null)
        {
            activeState.Perform();
        }
    }

    /// <summary>
    /// �ı�״̬
    /// </summary>
    /// <param name="newState">��Ҫ�����״̬</param>
    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;


        if (activeState != null)
        {

            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }


    }



}
