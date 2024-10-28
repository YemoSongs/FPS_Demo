using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/// <summary>
/// 输入管理器
/// </summary>
public class InputManager : MonoBehaviour
{

    


    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook playerLook;
    private WeaponController weaponController;

    public Animator animator;

    public RigBuilder rigBuilder;

    void Awake()
    {
       

        //LockCursor();

        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();
        weaponController = GetComponent<WeaponController>();
        onFoot.Jump.performed += ctx => motor.Jump();
      
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.started += ctx => motor.SprintStart();
        onFoot.Sprint.performed += ctx => motor.SprintEnd();

        onFoot.Throw.performed += ctx => weaponController.ThrowawayWeapon();

        onFoot.MouseUp.performed += ctx => weaponController.PreviousWeapon();

        onFoot.MouseDown.performed += ctx => weaponController.NextWeapon();

        //onFoot.Shoot.started += ctx => weaponController.ShootStart();
        //onFoot.Shoot.performed += ctx => weaponController.ShootEnd();


    }






    void Update()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        motor.ProcessMove(UIManager.Instance.GetPanel<PlayerPanel>().joystick.Direction);

        animator.SetFloat("Hspeed", onFoot.Movement.ReadValue<Vector2>().x);
        animator.SetFloat("Vspeed", onFoot.Movement.ReadValue<Vector2>().y);
        if (onFoot.Movement.ReadValue<Vector2>() != Vector2.zero)
            animator.SetBool("IsMove", true);
        else
            animator.SetBool("IsMove", false);
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }

    /// <summary>
    /// 锁定鼠标
    /// </summary>
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



}
