using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¹¥»÷×´Ì¬
/// </summary>
public class AttackState : BaseState
{



    private float losePlayerTimer;
    private float shootTimer;


    public override void Enter()
    {
        enemy.Agent.isStopped = true;
    }

    public override void Exit()
    {
        enemy.Agent.isStopped = false;
    }

    public override void Perform()
    {
        if(enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
          
            shootTimer += Time.deltaTime;


            //enemy.transform.DOLookAt(enemy.Player.transform.position, 2f);


            if (shootTimer > enemy.fireRate)
            {
                shootTimer = 0;
                //enemy.transform.LookAt(enemy.Player.transform);
                enemy.transform.DOLookAt(enemy.Player.transform.position, 1f,AxisConstraint.Y).OnComplete(Shoot);
                
            }

            enemy.LastKnowPos = enemy.Player.transform.position;    
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer > 5)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }


    private void Shoot()
    {
        Debug.Log("Enemy Shoot");

        Transform gunbarrel = enemy.gun;

        GameObject bullet = GameObject.Instantiate(enemy.bullet, gunbarrel.position, enemy.transform.rotation);

        Vector3 shootDirection  = (enemy.Player.transform.position - gunbarrel.position).normalized;

        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-1.5f,1.5f),Vector3.up) *shootDirection*40;

        shootTimer = 0;
    }


}
