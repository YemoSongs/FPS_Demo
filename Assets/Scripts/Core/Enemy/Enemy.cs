using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 怪物类
/// </summary>
public class Enemy : MonoBehaviour,IGetHealthSystem
{
    //敌人生命系统
    private HealthSystem healthSystem;
         
    //怪物的状态机
    private StateMachine stateMachine;
    //AI寻路组件
    private NavMeshAgent agent;

    public NavMeshAgent Agent { get { return agent; } }

    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private string currentState;//当前状态  用于在编辑模式核查
    private GameObject player;//玩家角色
    private Vector3 lastKnowPos;//玩家离开视野的最后位置

    //巡逻路径
    public Path path;

    public Vector3 LastKnowPos { get => lastKnowPos; set => lastKnowPos = value; }
    public GameObject Player => player;
    
    public float sightDistance = 20f;//敌人视野距离
    public float fieldOfView = 75f;//敌人视野角度
    public float eyeHeight;

    public GameObject bullet;
    public Transform gun;//发射子弹的位置
    public float fireRate = 1.5f;//射速

    public Animator animator;

    private void Awake()
    {
        healthSystem = new HealthSystem(100);//初始化敌人血量
        //为敌人健康系统添加死亡事件
        healthSystem.OnDead += (o, s) =>
        {
            Destroy(this.gameObject);
        };
    }


   
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// 让敌人受伤的方法
    /// </summary>
    /// <param name="damage">减多少血</param>
    public void Damage(float damage)
    {
        healthSystem.Damage(damage);
    }

    
    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    /// <summary>
    /// 判断玩家是否在视野范围内的方法
    /// </summary>
    /// <returns></returns>
    public bool CanSeePlayer()
    {
        if(player != null)
        {
            if(Vector3.Distance(transform.position, player.transform.position) < sightDistance && Vector3.Distance(transform.position, player.transform.position) > 1)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up*eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection,transform.forward);
                if(angleToPlayer >= -fieldOfView&&angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + Vector3.up*eyeHeight,targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray,out hitInfo,sightDistance,mask))
                    {
                        if(hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin,ray.direction*sightDistance);   
                            return true;
                        }
                    }

                }
            }
        }

        return false;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }


    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
