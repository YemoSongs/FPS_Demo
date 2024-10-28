using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ñ°ÕÒÍæ¼Ò×´Ì¬
/// </summary>
public class SearchState : BaseState
{

    private float searchTimer;
    private float moveTimer;

    public override void Enter()
    {
        enemy.animator.SetBool("isWalk", true);
        enemy.Agent.SetDestination(enemy.LastKnowPos);
    }

    public override void Exit()
    {
        enemy.animator.SetBool("isWalk", false);
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
            stateMachine.ChangeState(new AttackState());

        if(enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(2, 4))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 6));
                moveTimer = 0;
            }


            if (searchTimer > 8)
            {
                stateMachine.ChangeState(new PatrolState());    
            }
        }
    }
}
