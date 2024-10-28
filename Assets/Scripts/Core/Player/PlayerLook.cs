using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ҵ�һ�˳���Ұ
/// </summary>
public class PlayerLook : MonoBehaviour
{
   
    public Transform veiw;

    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    /// <summary>
    /// ���������Ұת�� 
    /// </summary>
    /// <param name="input">���ƫ��ֵ</param>
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        //�����������ת
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        veiw.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //veiw.DOLocalRotateQuaternion(Quaternion.Euler(xRotation, 0f, 0f), Time.deltaTime);
        //��ҵ�������ת
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime)* xSensitivity);
        //transform.DORotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity, Time.deltaTime);
    } 



}
