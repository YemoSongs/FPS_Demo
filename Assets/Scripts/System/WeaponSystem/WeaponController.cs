using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.HID;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

/// <summary>
/// ����������
/// </summary>
public class WeaponController : MonoBehaviour
{
   


    public Weapon currentWeapon;//��ǰ����

    
    

    public int maxWeaponNum = 3;
    public List<GameObject> weapons;               // The array that holds all the weapons that the player has
    public int startingWeaponIndex = 0;         // The weapon index that the player will start with
    [SerializeField]
    private int weaponIndex;                    // The current index of the active weapon

    



    [SerializeField]
    private Transform weaponPivot;//�������ڵ�

    //public Recoil recoil;//����������
   
    public LayerMask mask;//������Ĳ�


    public Transform rightHand;
    public Transform leftHand;


    private Camera cam;//�������

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
    /// ʰȡ����
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
            print("ֻ��װ��" + maxWeaponNum + "������");

            BagDataMgr.Instance.PutInBag(weapon.GetComponent<ItemBase>().InitThisItem());

            Destroy(weapon.gameObject);

        }
            
       
    }

    /// <summary>
    /// ��������
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
    /// ����װ�����������
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
    /// ��װ���������� �ŵ�����
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
    /// �ѱ��������� �ŵ�װ����
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
            Debug.Log("Layer�в�����,���ֶ����LayerName");

            return;
        }
        //������������������layer
        trans.gameObject.layer = LayerMask.NameToLayer(targetLayer);
        foreach (Transform child in trans)
        {
            ChangeLayer(child, targetLayer);
           // Debug.Log(child.name + "�Ӷ���Layer���ĳɹ���");
        }
    }

}


///// <summary>
///// ���Э��
///// </summary>
///// <returns></returns>
//private IEnumerator IGunAttack()
//{
//    while (isShooted)
//    {
//        //���߼�� ��ģ�����

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

//        print("���Э�̹���");
//        yield return new WaitForSeconds(currentWeapon.FireRate);//�ж�Э�� ͣ���������ʱ��
//    }

//}




///// <summary>
///// �����ɢ��
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
///// ��ս���� ��Χ���
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

//    print("��ս����");
//}


///// <summary>
///// ����
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
///// ǹе������� ���߼��
///// </summary>
//private void GunAttack()
//{
//    isShooted = true;
//    //���߼�� ��ģ�����

//    RaycastHit hitInfo;
//    if (Physics.Raycast(RandomShoot(), out hitInfo, currentWeapon.FireDistance, mask))
//    {
//        if (hitInfo.collider.GetComponentInParent<Enemy>() != null)
//        {
//            hitInfo.collider.GetComponentInParent<Enemy>().Damage(currentWeapon.Atk);
//        }

//    }
//    print("��ҷ�Э�̹���");
//}

///// <summary>
///// ����Ƿ���Թ���
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
///// ��ʼ��� 
///// </summary>
//public void ShootStart()
//{

//    if (currentWeapon == null)
//        return;


//    if (currentWeapon.isGun)
//    {
//        (currentWeapon as Gun).ShootStart();//ԭ�����
//    }
//    else
//    {
//        (currentWeapon as Melee).Attack();//��ս����
//    }
//}

///// <summary>
///// ���ֹͣ
///// </summary>
//public void ShootEnd()
//{
//    if (currentWeapon == null)
//        return;
//    if(currentWeapon.isGun)
//        (currentWeapon as Gun).ShootEnd();//�ر����Э��

//    print("��������");
//}


