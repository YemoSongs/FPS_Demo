using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巡逻状态
/// </summary>
public class PatrolState : BaseState
{
    //当前路径点索引
    public int waypointIndex;
    //到达路径点后 等待的时间
    public float waitTime = 3f;



    private float waitTimer;

    public override void Enter()
    {
       
    }

    public override void Exit()
    {
        enemy.animator.SetBool("isWalk", false);
    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    /// <summary>
    /// 巡逻逻辑
    /// </summary>
    public void PatrolCycle()
    {
        if(enemy.Agent.remainingDistance < 0.2f)
        {
            enemy.animator.SetBool("isWalk", false);
            waitTimer += Time.deltaTime;
            if(waitTimer > waitTime)
            {
                enemy.animator.SetBool("isWalk", true);
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }


       
    }

}
