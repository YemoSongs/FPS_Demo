using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;


/// <summary>
/// ��ҽ�����Ϊ
/// </summary>
public class PlayerInteract : MonoBehaviour
{

    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;

    

    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.Instance.GetPanel<PlayerPanel>().UndateText(string.Empty);

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                UIManager.Instance.GetPanel<PlayerPanel>().UndateText(interactable.promptMessage);

                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
            }
            
        }
        
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<Interactable>() != null)
    //    {
    //        Interactable interactable = other.GetComponent<Interactable>();
    //        UIManager.Instance.GetPanel<PlayerPanel>().UndateText(interactable.promptMessage);

    //        if (inputManager.onFoot.Interact.triggered)
    //        {
    //            interactable.BaseInteract();
    //        }
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    UIManager.Instance.GetPanel<PlayerPanel>().UndateText(string.Empty);
    //}
}
