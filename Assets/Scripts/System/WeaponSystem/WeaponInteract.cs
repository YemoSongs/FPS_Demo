using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteract : Interactable
{



    protected override void Interact()
    {

       GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>().PickupWeapon(this.GetComponent<Weapon>());

    }
}
