using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{

    [Header("Grenade Prefab")]
    [SerializeField] public GameObject grenadePrefab;

    [Header("Grenade Settings")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse1;
    [SerializeField] private Transform throwPosition;
    [SerializeField] private Vector3 throwDirection = new Vector3(0,1,0);

    [Header("Grenade Force")]
    [SerializeField] private float throwForce = 10f;

    [Header("Trajectory Settings")]
    [SerializeField] private LineRenderer trajectoryLine;

    private Camera cam;

    public bool isReadyForThrow = false;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        trajectoryLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isReadyForThrow)
            return;

        if(Input.GetKeyDown(throwKey))
        {
            StartThrowing();
        }
        if(Input.GetKey(throwKey))
        {
            Vector3 grenadeVelocity = (cam.transform.forward + throwDirection).normalized*throwForce;
            ShowTrajectory(throwPosition.position + throwPosition.forward, grenadeVelocity);
        }
        if(Input.GetKeyUp(throwKey))
        {
            ReleaseThrow();
        }
    }

    void StartThrowing()
    {
        trajectoryLine.enabled = true;

       
    }

    void ReleaseThrow()
    {
        trajectoryLine.enabled = false;
        ThrowGrenade(throwForce);
        if(BagDataMgr.Instance.bagData.grenade_Item.num > 0)
        {
            BagDataMgr.Instance.bagData.grenade_Item.num--;

            if(BagDataMgr.Instance.bagData.grenade_Item.num <= 0)
            {
                BagDataMgr.Instance.PutOutGre(BagDataMgr.Instance.bagData.grenade_Item);

            }
        }
    }

    void ThrowGrenade(float force)
    {
        Vector3 spawnPosition = throwPosition.position + cam.transform.forward;

        GameObject grenade = Instantiate(grenadePrefab, spawnPosition, cam.transform.rotation);

        grenade.GetComponent<Grenade>().isTrigger = true;

        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        Vector3 finalThrowDirection = (cam.transform.forward + throwDirection).normalized;
        rb.AddForce(finalThrowDirection*force,ForceMode.VelocityChange);

    }

    void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[50];  
        trajectoryLine.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin +speed * time + 0.5f * Physics.gravity * time * time;
        }

        trajectoryLine.SetPositions(points);
    }




}
