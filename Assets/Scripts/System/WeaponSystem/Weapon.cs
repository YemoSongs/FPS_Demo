using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 武器基类 包含武器的基本属性
/// </summary>
public abstract class Weapon : MonoBehaviour
{

  

    public abstract bool isGun
    {
        get;
    }


    public abstract float Atk
    {
        get;
        set;
    }


    public abstract bool isAttacking
    {
        get;
        set;
    }

    public abstract float FireRate
    {
        get;
    }

    public abstract float FireDistance
    {
        get;
        set;
    }

    public abstract Vector3 SetStartPosition
    {
        get;
    }


    public abstract Vector3 SetStartRotation
    {
        get;
    }

    public abstract Vector3 SetStartLeftHandPosition
    {
        get;
    }


    public abstract Vector3 SetStartRightHandPosition
    {
        get;
    }




    public virtual WeaponController WeaponController { get; set; }

}
