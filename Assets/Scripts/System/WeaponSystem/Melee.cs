using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Melee : Weapon
{

    [SerializeField]private GameObject model;

    public GameObject Model 
    { 
        get { return model; }
    }

    private bool isShooted;
    public override bool isAttacking
    {
        get { return isShooted; }
        set { isShooted = value; }
    }

    [SerializeField]
    public float atk = 15f;

    public override float Atk
    {
        get { return atk; }
        set
        {
            atk = value;
        }
    }






    [SerializeField]
    private float fireRate = 0.8f;

    /// <summary>
    /// 攻击频率（射速）
    /// </summary>
    public override float FireRate => fireRate;




    [SerializeField]
    private Vector3 setStartPosition = Vector3.zero;
    [SerializeField]
    private Vector3 setStartRotation = Vector3.zero;

    /// <summary>
    /// 设置装备武器时的位置
    /// </summary>
    public override Vector3 SetStartPosition => setStartPosition;
    /// <summary>
    /// 设置装备武器时的旋转
    /// </summary>
    public override Vector3 SetStartRotation => setStartRotation;

    [SerializeField]
    public Vector3 setStartLeftHandPosition = Vector3.zero;

    /// <summary>
    /// 设置装备武器时左手IK的位置
    /// </summary>
    public override Vector3 SetStartLeftHandPosition => setStartLeftHandPosition;


    [SerializeField]
    public Vector3 setStartRightHandPosition = Vector3.zero;

    /// <summary>
    /// 设置装备武器时右手IK的位置
    /// </summary>
    public override Vector3 SetStartRightHandPosition => setStartRightHandPosition;



    [SerializeField]
    private float fireDistance = 20f;
    /// <summary>
    ///  攻击距离
    /// </summary>
    public override float FireDistance
    {
        get { return fireDistance; }
        set
        {
            fireDistance = value;
        }
    }


    public override bool isGun => false;

    

    private float shootTimer;

    public InputManager inputManager;

    private Animator animator;

    public AudioClip atkSound;

    private void Start()
    {
        animator = GetComponent<Animator>();

        inputManager = GameObject.Find("Player").GetComponent<InputManager>();


        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent(typeof(AudioSource));
        }
    }


    /// <summary>
    /// 近战武器 范围检测
    /// </summary>
    private void MeleeAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(Camera.main.transform.position + Camera.main.transform.forward * FireDistance, FireDistance, WeaponController.mask);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponentInParent<Enemy>() != null)
                {
                    collider.GetComponentInParent<Enemy>().Damage(Atk);
                }
            }
        }
        GetComponent<AudioSource>().PlayOneShot(atkSound);
        print("近战攻击");
    }


    //public void Attack()
    //{
    //    if (!isAttacking)
    //    {
           
    //        MeleeAttack();
    //        isAttacking = true;
    //        if (animator != null)
    //        {
    //            animator.SetBool("IsAtk", isAttacking);
    //        }
    //    }
    //}

    /// <summary>
    /// 检测是否可以攻击
    /// </summary>
    void CheckShoot()
    {
        

        shootTimer += Time.deltaTime;
        if (shootTimer > FireRate)
        {
            shootTimer = 0;
            isShooted = false;
            animator.SetBool("IsAtk", isAttacking);
        }
    }

    private void Update()
    {
        if (WeaponController == null)
            return;


        if (isAttacking)
        {
            CheckShoot();
        }

        
        if(inputManager.onFoot.Shoot.ReadValue<float>() > 0.8f && !isAttacking)
        {
            isAttacking = true;

            animator.SetBool("IsAtk", isAttacking);

            MeleeAttack();
           
        }


    }


}
