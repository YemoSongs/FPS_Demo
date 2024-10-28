using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家第一人称视野
/// </summary>
public class PlayerLook : MonoBehaviour
{
   
    public Transform veiw;

    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    /// <summary>
    /// 控制玩家视野转动 
    /// </summary>
    /// <param name="input">鼠标偏移值</param>
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //摄像机上下旋转
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        veiw.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //veiw.DOLocalRotateQuaternion(Quaternion.Euler(xRotation, 0f, 0f), Time.deltaTime);
        //玩家的左右旋转
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime)* xSensitivity);
        //transform.DORotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity, Time.deltaTime);
    } 



}
