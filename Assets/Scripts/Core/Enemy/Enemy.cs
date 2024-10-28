using CodeMonkey.HealthSystemCM;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ������
/// </summary>
public class Enemy : MonoBehaviour,IGetHealthSystem
{
    //��������ϵͳ
    private HealthSystem healthSystem;
         
    //�����״̬��
    private StateMachine stateMachine;
    //AIѰ·���
    private NavMeshAgent agent;

    public NavMeshAgent Agent { get { return agent; } }

    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private string currentState;//��ǰ״̬  �����ڱ༭ģʽ�˲�
    private GameObject player;//��ҽ�ɫ
    private Vector3 lastKnowPos;//����뿪��Ұ�����λ��

    //Ѳ��·��
    public Path path;

    public Vector3 LastKnowPos { get => lastKnowPos; set => lastKnowPos = value; }
    public GameObject Player => player;
    
    public float sightDistance = 20f;//������Ұ����
    public float fieldOfView = 75f;//������Ұ�Ƕ�
    public float eyeHeight;

    public GameObject bullet;
    public Transform gun;//�����ӵ���λ��
    public float fireRate = 1.5f;//����

    public Animator animator;

    private void Awake()
    {
        healthSystem = new HealthSystem(100);//��ʼ������Ѫ��
        //Ϊ���˽���ϵͳ��������¼�
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
    /// �õ������˵ķ���
    /// </summary>
    /// <param name="damage">������Ѫ</param>
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
    /// �ж�����Ƿ�����Ұ��Χ�ڵķ���
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
