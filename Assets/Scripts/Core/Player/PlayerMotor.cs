using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家运动相关
/// </summary>
public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;
    private bool lerpCrouch;
    private bool crouching;
    private float crouchTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        isGrounded = controller.isGrounded;
        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if(crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 1.8f, p);

            if(p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0;
            }
        }

    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection)*speed*Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2;
        controller.Move(playerVelocity*Time.deltaTime);

    }

    public void Jump()
    {
        if(isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
        if(crouching)
        {
            GetComponent<InputManager>().animator.SetLayerWeight(2, 1);
            GetComponent<InputManager>().animator.SetBool("isCrouch",true);
        }
        else
        {
            GetComponent<InputManager>().animator.SetLayerWeight(2, 0);
            GetComponent<InputManager>().animator.SetBool("isCrouch", false);
        }
    }

    public void SprintStart()
    {
        speed = 8;
    }

    public void SprintEnd()
    {
        speed = 5;
    }

}
