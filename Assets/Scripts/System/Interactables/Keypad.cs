using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField]
    private GameObject Door;

    private bool isOpen;

    protected override void Interact()
    {
        //Debug.Log("Open Door");

        isOpen = !isOpen;

        //Door.GetComponent<Animator>().SetBool("IsOpen", isOpen);
        //Door.SetActive(!isOpen);
        if (isOpen)
        {
            //Door.transform.localRotation = Quaternion.AngleAxis(90, Vector3.up);
            Door.transform.DORotateQuaternion(Quaternion.AngleAxis(90, Vector3.up), 0.5f);
            promptMessage = "关门";
        }
        else
        {
            //Door.transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
            Door.transform.DORotateQuaternion(Quaternion.AngleAxis(0, Vector3.up), 0.5f);
            promptMessage = "开门";
        }
           
    }

}
