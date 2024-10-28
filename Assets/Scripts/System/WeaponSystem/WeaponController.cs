using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.HID;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

/// <summary>
/// 武器控制器
/// </summary>
public class WeaponController : MonoBehaviour
{
   


    public Weapon currentWeapon;//当前武器

    
    

    public int maxWeaponNum = 3;
    public List<GameObject> weapons;               // The array that holds all the weapons that the player has
    public int startingWeaponIndex = 0;         // The weapon index that the player will start with
    [SerializeField]
    private int weaponIndex;                    // The current index of the active weapon

    



    [SerializeField]
    private Transform weaponPivot;//武器根节点

    //public Recoil recoil;//武器后坐力
   
    public LayerMask mask;//射击检测的层


    public Transform rightHand;
    public Transform leftHand;


    private Camera cam;//主摄像机

    private InputManager inputManager;

    public GrenadeThrower gradeThrower;

    private void Start()
    {
        BagDataMgr.Instance.playerWeaponController = this;

        cam = Camera.main;

        inputManager = GetComponent<InputManager>();

        inputManager.rigBuilder.enabled = false;
        // Make sure the starting active weapon is the one selected by the user in startingWeaponIndex
        if(weapons.Count > 0 )
        {
            inputManager.rigBuilder.enabled = true;
            weaponIndex = startingWeaponIndex;
            //GameObject obj = Instantiate(weapons[weaponIndex].gameObject);
            Weapon weapon = weapons[weaponIndex].GetComponent<Weapon>();
            weapon.transform.parent = weaponPivot.transform;
            ChangeLayer(weapon.transform, "Weapon/On");

            weapon.transform.localPosition = weapon.SetStartPosition;
            weapon.transform.localRotation = Quaternion.Euler(weapon.SetStartRotation);
            currentWeapon = weapon;
            weapon.WeaponController = this;

            //leftHand.SetParent(currentWeapon.transform);
            //rightHand.SetParent(currentWeapon.transform);

            leftHand.parent = currentWeapon.transform;
            rightHand.parent = currentWeapon.transform; 

        }
    }

    public void SetActiveWeapon(int index)
    {
        // Make sure this weapon exists before trying to switch to it
        if (index >= weapons.Count || index < 0)
        {
            //Debug.LogWarning("Tried to switch to a weapon that does not exist.  Make sure you have all the correct weapons in your weapons array.");
            return;
        }

        // Send a messsage so that users can do other actions whenever this happens
        SendMessageUpwards("OnEasyWeaponsSwitch", SendMessageOptions.DontRequireReceiver);

        // Make sure the weaponIndex references the correct weapon
        weaponIndex = index;



        // Start be deactivating all weapons
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }

        // Activate the one weapon that we want
        weapons[index].SetActive(true);
        currentWeapon = weapons[index].GetComponent<Weapon>();

        //leftHand.SetParent(currentWeapon.transform);
        //rightHand.SetParent(currentWeapon.transform);

        if(currentWeapon.isGun)
        {
            leftHand.parent  = (currentWeapon as Gun).weaponModel.transform;
            rightHand.parent = (currentWeapon as Gun).weaponModel.transform;
        }
        else
        {
            leftHand.parent = (currentWeapon as Melee).Model.transform;
            rightHand.parent = (currentWeapon as Melee).Model.transform;
        }

        leftHand.localPosition = currentWeapon.SetStartLeftHandPosition;
        leftHand.localRotation = Quaternion.Euler(-90,90,0);
        rightHand.localPosition = currentWeapon.SetStartRightHandPosition;
        rightHand.localRotation = Quaternion.Euler(90, -90, 0);
        inputManager.rigBuilder.enabled = true;

    }

    public void NextWeapon()
    {
        weaponIndex++;
        if (weaponIndex > weapons.Count - 1)
            weaponIndex = 0;
        SetActiveWeapon(weaponIndex);
    }

    public void PreviousWeapon()
    {
        weaponIndex--;
        if (weaponIndex < 0)
            weaponIndex = weapons.Count - 1;
        SetActiveWeapon(weaponIndex);
    }

   


    /// <summary>
    /// 拾取武器
    /// </summary>
    /// <param name="weapon"></param>Pick up weapons
    public void PickupWeapon(Weapon weapon)
    {

        if (weapons.Count < maxWeaponNum)
        {

            weapon.transform.parent = weaponPivot.transform;
            ChangeLayer(weapon.transform, "Weapon/On");

            weapon.transform.localPosition = weapon.SetStartPosition;
            weapon.transform.localRotation = Quaternion.Euler(weapon.SetStartRotation);
            currentWeapon = weapon;
            weapon.WeaponController = this;
            weapons.Add(weapon.gameObject);
            weaponIndex = weapons.Count - 1;

            SetActiveWeapon(weaponIndex);
            BagDataMgr.Instance.PutInWep123(weapon.GetComponent<ItemBase>().InitThisItem());

        }
        else
        {
            print("只能装备" + maxWeaponNum + "个武器");

            BagDataMgr.Instance.PutInBag(weapon.GetComponent<ItemBase>().InitThisItem());

            Destroy(weapon.gameObject);

        }
            
       
    }

    /// <summary>
    /// 丢弃武器
    /// </summary>
    public void ThrowawayWeapon()
    {
        if(currentWeapon != null)
        {
            currentWeapon.transform.position = gameObject.transform.position + gameObject.transform.forward;
            currentWeapon.transform.rotation = Quaternion.identity;
            currentWeapon.transform.parent = null;
            currentWeapon.WeaponController = null;
            ChangeLayer(currentWeapon.transform, "Weapon/Off");
            weapons.Remove(currentWeapon.gameObject);
            if(weapons.Count > 0)
            {
                currentWeapon = weapons[0].GetComponent<Weapon>();
                weaponIndex = 0;
                SetActiveWeapon(weaponIndex);
            }
            else
            {
                currentWeapon = null;

                inputManager.rigBuilder.enabled = false;
            }
        }
    }


    /// <summary>
    /// 丢弃装备栏里的武器
    /// </summary>
    /// <param name="id"></param>
    public void ThrowawayWep123(int id)
    {
        int index = 0;

        for(int i = 0; i < weapons.Count; i++)
        {
            if(weapons[i].GetComponent<ItemBase>().id == id)
            {
                index = i;
                break;
            }
        }

        if (!weapons[index].activeSelf)
            weapons[index].SetActive(true);

        weapons[index].transform.position = gameObject.transform.position + gameObject.transform.forward;
        weapons[index].transform.rotation = Quaternion.identity;
        weapons[index].transform.parent = null;
        weapons[index].GetComponent<Weapon>().WeaponController = null;
        ChangeLayer(weapons[index].transform, "Weapon/Off");
        weapons.Remove(weapons[index]);
        if (index == weaponIndex)
        {
            if (weapons.Count > 0)
            {
                currentWeapon = weapons[0].GetComponent<Weapon>();
                weaponIndex = 0;
                SetActiveWeapon(weaponIndex);
            }
            else
            {
                currentWeapon = null;

                inputManager.rigBuilder.enabled = false;
            }
        }
    }

    /// <summary>
    /// 把装备栏的武器 放到背包
    /// </summary>
    /// <param name="id"></param>
    public void PutWep123ToBag(int id)
    {
        int index = 0;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].GetComponent<ItemBase>().id == id)
            {
                index = i;
                break;
            }
        }

        rightHand.parent = null;
        leftHand.parent = null;

        Destroy(weapons[index].gameObject);

        weapons.Remove(weapons[index]);

        if (weapons.Count > 0)
        {
            currentWeapon = weapons[0].GetComponent<Weapon>();
            weaponIndex = 0;
            SetActiveWeapon(weaponIndex);
        }
        else
        {
            currentWeapon = null;

            inputManager.rigBuilder.enabled = false;
        }
    }

    /// <summary>
    /// 把背包的武器 放到装备栏
    /// </summary>
    /// <param name="info"></param>
    public void PutBagWepToWep123(ItemInfo info)
    {
        GameObject wep = Instantiate(Resources.Load<GameObject>(info.prefab));
        wep.transform.parent = weaponPivot.transform;
        ChangeLayer(wep.transform, "Weapon/On");
        Weapon weapon = wep.GetComponent<Weapon>();
        weapon.transform.localPosition = weapon.SetStartPosition;
        weapon.transform.localRotation = Quaternion.Euler(weapon.SetStartRotation);
        currentWeapon = weapon;
        weapon.WeaponController = this;
        weapons.Add(weapon.gameObject);
        weaponIndex = weapons.Count - 1;

        SetActiveWeapon(weaponIndex);
        BagDataMgr.Instance.PutInWep123(weapon.GetComponent<ItemBase>().InitThisItem());

    }



    void ChangeLayer(Transform trans, string targetLayer)
    {
        if (LayerMask.NameToLayer(targetLayer) == -1)
        {
            Debug.Log("Layer中不存在,请手动添加LayerName");

            return;
        }
        //遍历更改所有子物体layer
        trans.gameObject.layer = LayerMask.NameToLayer(targetLayer);
        foreach (Transform child in trans)
        {
            ChangeLayer(child, targetLayer);
           // Debug.Log(child.name + "子对象Layer更改成功！");
        }
    }

}


///// <summary>
///// 射击协程
///// </summary>
///// <returns></returns>
//private IEnumerator IGunAttack()
//{
//    while (isShooted)
//    {
//        //射线检测 来模拟射击

//        RaycastHit hitInfo;
//        if (Physics.Raycast(RandomShoot(), out hitInfo, currentWeapon.FireDistance, mask))
//        {

//            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
//            GameObject hitEff = Instantiate(Resources.Load<GameObject>("Prefabs/Eff/Eff_HitPoint_01"),
//                                    hitInfo.point, rot, hitInfo.collider.transform);

//            Enemy enemy = hitInfo.collider.GetComponentInParent<Enemy>();
//            if (enemy != null)
//            {
//                enemy.Damage(currentWeapon.Atk);

//            }


//        }

//        recoil.AddRecoil();

//        print("玩家协程攻击");
//        yield return new WaitForSeconds(currentWeapon.FireRate);//中断协程 停留攻击间隔时间
//    }

//}




///// <summary>
///// 射击的散布
///// </summary>
///// <returns></returns>
//private Ray RandomShoot()
//{
//    Ray ray = new Ray(cam.transform.position,cam.transform.forward);
//    Gun gun = currentWeapon as Gun;
//    Quaternion hQ = Quaternion.LookRotation(ray.direction);
//    hQ *= Quaternion.Euler(Random.Range(-gun.Dispersal, gun.Dispersal), Random.Range(-gun.Dispersal, gun.Dispersal), 0);
//    ray.direction = (hQ * Vector3.forward).normalized;

//    return ray;
//}


///// <summary>
///// 近战武器 范围检测
///// </summary>
//private void MeleeAttack()
//{
//    Collider[] colliders = Physics.OverlapSphere(Camera.main.transform.position + Camera.main.transform.forward*currentWeapon.FireDistance, currentWeapon.FireDistance, mask);

//    if(colliders.Length > 0)
//    {
//        foreach (Collider collider in colliders)
//        {
//            if (collider.GetComponentInParent<Enemy>() != null)
//            {
//                collider.GetComponentInParent<Enemy>().Damage(currentWeapon.Atk);
//            }
//        }
//    }

//    print("近战攻击");
//}


///// <summary>
///// 攻击
///// </summary>
//public void Shoot()
//{

//    if (currentWeapon != null && !isShooted)
//    {
//        if (currentWeapon.isGun)
//        {
//            GunAttack();
//        }
//        else
//        {
//            MeleeAttack();
//        }

//    }
//}


///// <summary>
///// 枪械射击方法 射线检测
///// </summary>
//private void GunAttack()
//{
//    isShooted = true;
//    //射线检测 来模拟射击

//    RaycastHit hitInfo;
//    if (Physics.Raycast(RandomShoot(), out hitInfo, currentWeapon.FireDistance, mask))
//    {
//        if (hitInfo.collider.GetComponentInParent<Enemy>() != null)
//        {
//            hitInfo.collider.GetComponentInParent<Enemy>().Damage(currentWeapon.Atk);
//        }

//    }
//    print("玩家非协程攻击");
//}

///// <summary>
///// 检测是否可以攻击
///// </summary>
//void CheckShoot()
//{
//    if ((currentWeapon as Gun).isIEnumerator)
//        return;

//    shootTimer += Time.deltaTime;
//    if (shootTimer > currentWeapon.FireRate)
//    {
//        shootTimer = 0;
//        isShooted = false;
//    }
//}


///// <summary>
///// 开始射击 
///// </summary>
//public void ShootStart()
//{

//    if (currentWeapon == null)
//        return;


//    if (currentWeapon.isGun)
//    {
//        (currentWeapon as Gun).ShootStart();//原程射击
//    }
//    else
//    {
//        (currentWeapon as Melee).Attack();//近战攻击
//    }
//}

///// <summary>
///// 射击停止
///// </summary>
//public void ShootEnd()
//{
//    if (currentWeapon == null)
//        return;
//    if(currentWeapon.isGun)
//        (currentWeapon as Gun).ShootEnd();//关闭射击协程

//    print("攻击结束");
//}


