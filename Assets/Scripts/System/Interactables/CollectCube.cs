using BuffSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CollectCube : Interactable
{
    public GameObject particle;

    public GameObject gun;

    private GameObject player;


    protected override void Interact()
    {
        GameObject newGun = Instantiate(gun,this.transform.position,this.transform.rotation);

        //player = GameObject.FindGameObjectWithTag("Player");

        //newGun.GetComponent<InteractionEvent>().OnInteract = player.GetComponent<WeaponController>().PickupWeapon(newGun.GetComponent<Weapon>());

        //BuffManager.Instance.AddBuff<Buff001>(GameObject.Find("Player"), promptMessage);

        Instantiate(particle,this.transform.position,Quaternion.identity);

        Destroy(this.gameObject);
    }

}
