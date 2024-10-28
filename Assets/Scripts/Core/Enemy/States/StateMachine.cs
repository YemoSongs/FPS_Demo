
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  有限状态机类 管理各个状态
/// </summary>
public class StateMachine : MonoBehaviour
{
    /// <summary>
    /// 当前激活的状态
    /// </summary>
    public BaseState activeState;

    

    /// <summary>
    /// 初始化状态机
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
    /// 改变状态
    /// </summary>
    /// <param name="newState">将要进入的状态</param>
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
