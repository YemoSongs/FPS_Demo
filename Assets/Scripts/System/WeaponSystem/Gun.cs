using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine.UI;



public enum WeaponType
{
    Projectile,
    Raycast,
}
public enum Auto
{
    Full,
    Semi
}

public enum BulletHoleSystem
{
    Tag,
    Material,
    Physic_Material
}


[System.Serializable]
public class SmartBulletHoleGroup
{
    public string tag;
    public Material material;
    public PhysicMaterial physicMaterial;
    public BulletHolePool bulletHole;

    public SmartBulletHoleGroup()
    {
        tag = "Everything";
        material = null;
        physicMaterial = null;
        bulletHole = null;
    }
    public SmartBulletHoleGroup(string t, Material m, PhysicMaterial pm, BulletHolePool bh)
    {
        tag = t;
        material = m;
        physicMaterial = pm;
        bulletHole = bh;
    }
}

/// <summary>
/// ǹе����
/// </summary>
public class Gun : Weapon
{
    


    // Weapon Type
    public WeaponType type = WeaponType.Raycast;

    // Auto
    public Auto auto = Auto.Full;

    // General
    public bool playerWeapon = true;                    // Whether or not this is a player's weapon as opposed to an AI's weapon
    public GameObject weaponModel;                      // The actual mesh for this weapon
    public Transform raycastStartSpot;                  // The spot that the raycasting weapon system should use as an origin for rays
    public float delayBeforeFire = 0.0f;				// An optional delay that causes the weapon to fire a specified amount of time after it normally would (0 for no delay)



    // Projectile
    public GameObject projectile;                       // The projectile to be launched (if the type is projectile)
    public Transform projectileSpawnSpot;               // The spot where the projectile should be instantiated

    // Ammo
    public bool infiniteAmmo = false;                   // Whether or not this weapon should have unlimited ammo
    public int ammoCapacity = 12;                       // The number of rounds this weapon can fire before it has to reload
    public int shotPerRound = 1;                        // The number of "bullets" that will be fired on each round.  Usually this will be 1, but set to a higher number for things like shotguns with spread
    private int currentAmmo;                            // How much ammo the weapon currently has
    public float reloadTime = 2.0f;                     // How much time it takes to reload the weapon
    public bool showCurrentAmmo = true;                 // Whether or not the current ammo should be displayed in the GUI
    public bool reloadAutomatically = true;             // Whether or not the weapon should reload automatically when out of ammo

    // Accuracy
    public float accuracy = 80.0f;                      // How accurate this weapon is on a scale of 0 to 100
    private float currentAccuracy;                      // Holds the current accuracy.  Used for varying accuracy based on speed, etc.
    public float accuracyDropPerShot = 1.0f;            // How much the accuracy will decrease on each shot
    public float accuracyRecoverRate = 0.1f;            // How quickly the accuracy recovers after each shot (value between 0 and 1)

    // Burst
    public int burstRate = 3;                           // The number of shots fired per each burst
    public float burstPause = 0.0f;                     // The pause time between bursts
    private int burstCounter = 0;                       // Counter to keep track of how many shots have been fired per burst
    private float burstTimer = 0.0f;                    // Timer to keep track of how long the weapon has paused between bursts

    // Power
    public float power = 80.0f;                         // The amount of power this weapon has (how much damage it can cause) (if the type is raycast or beam)
    public float forceMultiplier = 10.0f;               // Multiplier used to change the amount of force applied to rigid bodies that are shot
    public float beamPower = 1.0f;						// Used to determine damage caused by beam weapons.  This will be much lower because this amount is applied to the target every frame while firing



    // Recoil
    public bool recoil = true;                          // Whether or not this weapon should have recoil
    public float recoilKickBackMin = 0.1f;              // The minimum distance the weapon will kick backward when fired
    public float recoilKickBackMax = 0.3f;              // The maximum distance the weapon will kick backward when fired
    public float recoilRotationMin = 0.1f;              // The minimum rotation the weapon will kick when fired
    public float recoilRotationMax = 0.25f;             // The maximum rotation the weapon will kick when fired
    public float recoilRecoveryRate = 0.01f;            // The rate at which the weapon recovers from the recoil displacement

    // Effects
    public bool spitShells = false;                     // Whether or not this weapon should spit shells out of the side
    public GameObject shell;                            // A shell prop to spit out the side of the weapon
    public float shellSpitForce = 1.0f;                 // The force with which shells will be spit out of the weapon
    public float shellForceRandom = 0.5f;               // The variant by which the spit force can change + or - for each shot
    public float shellSpitTorqueX = 0.0f;               // The torque with which the shells will rotate on the x axis
    public float shellSpitTorqueY = 0.0f;               // The torque with which the shells will rotate on the y axis
    public float shellTorqueRandom = 1.0f;              // The variant by which the spit torque can change + or - for each shot
    public Transform shellSpitPosition;                 // The spot where the weapon should spit shells from
    public bool makeMuzzleEffects = true;				// Whether or not the weapon should make muzzle effects
    public GameObject[] muzzleEffects =
        new GameObject[] { null };                      // Effects to appear at the muzzle of the gun (muzzle flash, smoke, etc.)
    public Transform muzzleEffectsPosition;             // The spot where the muzzle effects should appear from
    public bool makeHitEffects = true;                  // Whether or not the weapon should make hit effects
    public GameObject[] hitEffects =
        new GameObject[] { null };                      // Effects to be displayed where the "bullet" hit

    // Bullet Holes
    public bool makeBulletHoles = true;                 // Whether or not bullet holes should be made
    public BulletHoleSystem bhSystem = BulletHoleSystem.Tag;    // What condition the dynamic bullet holes should be based off
    public List<string> bulletHolePoolNames = new
        List<string>();                                 // A list of strings holding the names of bullet hole pools in the scene
    public List<string> defaultBulletHolePoolNames =
        new List<string>();                             // A list of strings holding the names of default bullet hole pools in the scene
    public List<SmartBulletHoleGroup> bulletHoleGroups =
        new List<SmartBulletHoleGroup>();				// A list of bullet hole groups.  Each one holds a tag for GameObjects that might be hit, as well as a corresponding bullet hole
    public List<BulletHolePool> defaultBulletHoles =
        new List<BulletHolePool>();                     // A list of default bullet holes to be instantiated when none of the custom parameters are met
    public List<SmartBulletHoleGroup> bulletHoleExceptions =
        new List<SmartBulletHoleGroup>();               // A list of SmartBulletHoleGroup objects that defines conditions for when no bullet hole will be instantiated.
                                                        // In other words, the bullet holes in the defaultBulletHoles list will be instantiated on any surface except for
                                                        // the ones specified in this list.


    // Crosshairs
    public bool showCrosshair = true;                   // Whether or not the crosshair should be displayed
    public Texture2D crosshairTexture;                  // The texture used to draw the crosshair
    public int crosshairLength = 10;                    // The length of each crosshair line
    public int crosshairWidth = 4;                      // The width of each crosshair line
    public float startingCrosshairSize = 10.0f;         // The gap of space (in pixels) between the crosshair lines (for weapon inaccuracy)
    private float currentCrosshairSize;                 // The gap of space between crosshair lines that is updated based on weapon accuracy in realtime


    // Audio
    public AudioClip fireSound;                         // Sound to play when the weapon is fired
    public AudioClip reloadSound;                       // Sound to play when the weapon is reloading
    public AudioClip dryFireSound;                      // Sound to play when the user tries to fire but is out of ammo

    // Other
    private bool canFire = true;                        // Whether or not the weapon can currently fire (used for semi-auto weapons)

    /// <summary>
    /// ����Ƶ�ʣ����٣�
    /// </summary>
    public override float FireRate => rateOfFire;
    // Rate of Fire
    public float rateOfFire = 10;                       // The number of rounds this weapon fires per second
    private float actualROF;                            // The frequency between shots based on the rateOfFire
    private float fireTimer;							// Timer used to fire at a set frequency


    [SerializeField]
    public float atk = 10f;
   
    public override float Atk
    {
        get { return atk; }
        set
        {
            atk = value;
        }
    }
  

    


    [SerializeField]
    public Vector3 setStartPosition = Vector3.zero;
    [SerializeField]
    public Vector3 setStartRotation = Vector3.zero;

    

    /// <summary>
    /// ����װ������ʱ��λ��
    /// </summary>
    public override Vector3 SetStartPosition => setStartPosition;
    /// <summary>
    /// ����װ������ʱ����ת
    /// </summary>
    public override Vector3 SetStartRotation => setStartRotation;

    [SerializeField]
    public Vector3 setStartLeftHandPosition = Vector3.zero;

    /// <summary>
    /// ����װ������ʱ����IK��λ��
    /// </summary>
    public override Vector3 SetStartLeftHandPosition => setStartLeftHandPosition;


    [SerializeField]
    public Vector3 setStartRightHandPosition = Vector3.zero;

    /// <summary>
    /// ����װ������ʱ����IK��λ��
    /// </summary>
    public override Vector3 SetStartRightHandPosition => setStartRightHandPosition;



    [SerializeField]
    public float fireDistance = 20f;
    /// <summary>
    /// �������
    /// </summary>
    public override float FireDistance
    {
        get { return fireDistance; }
        set { fireDistance = value; }
    }


    //[SerializeField]
    //private Vector2 gunRecoil = new Vector2(0.2f,0.2f);
    ///// <summary>
    ///// ǹе������
    ///// </summary>
    //public Vector2 GunRecoil => gunRecoil;

    //[SerializeField]
    //private float dispersal = 0.5f;
    ///// <summary>
    ///// ˮƽɢ��
    ///// </summary>
    //public float Dispersal => dispersal;

    public override bool isGun => true;

    public InputManager inputManager;
   

   
    public override bool isAttacking
    {
        get { return canFire; }
        set { canFire = value; }
    }



    private void Start()
    {
        inputManager = GameObject.Find("Player").GetComponent<InputManager>();

        // Calculate the actual ROF to be used in the weapon systems.  The rateOfFire variable is
        // designed to make it easier on the user - it represents the number of rounds to be fired
        // per second.  Here, an actual ROF decimal value is calculated that can be used with timers.  ����������ϵͳ��ʹ�õ�ʵ��ROF��rateOfFire������Ϊ�˷����û�ʹ�ö���Ƶġ�������ʾÿ�뷢����ӵ��������������һ��ʵ�ʵ�ROFʮ����ֵ���������ʱ��һ��ʹ�á�
        if (rateOfFire != 0)
            actualROF = 1.0f / rateOfFire;
        else
            actualROF = 0.01f;

        // Initialize the current crosshair size variable to the starting value specified by the user   ����ǰ׼�Ǵ�С������ʼ��Ϊ�û�ָ������ʼֵ
        currentCrosshairSize = startingCrosshairSize;

        // Make sure the fire timer starts at 0
        fireTimer = 0.0f;

        // Start the weapon off with a full magazine   ������ϻ��������
        currentAmmo = ammoCapacity;

        // Give this weapon an audio source component if it doesn't already have one
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent(typeof(AudioSource));
        }

        // Make sure raycastStartSpot isn't null
        if (raycastStartSpot == null)
            raycastStartSpot = gameObject.transform;
        // Make sure muzzleEffectsPosition isn't null
        if (muzzleEffectsPosition == null)
            muzzleEffectsPosition = gameObject.transform;

        // Make sure projectileSpawnSpot isn't null
        if (projectileSpawnSpot == null)
            projectileSpawnSpot = gameObject.transform;

        // Make sure weaponModel isn't null
        if (weaponModel == null)
            weaponModel = gameObject;

        // Make sure crosshairTexture isn't null
        if (crosshairTexture == null)
            crosshairTexture = new Texture2D(0, 0);

        // Initialize the bullet hole pools list   ��ʼ�����׳��б�
        for (int i = 0; i < bulletHolePoolNames.Count; i++)
        {
            GameObject g = GameObject.Find(bulletHolePoolNames[i]);

            if (g != null && g.GetComponent<BulletHolePool>() != null)
                bulletHoleGroups[i].bulletHole = g.GetComponent<BulletHolePool>();
            else
                Debug.LogWarning("Bullet Hole Pool does not exist or does not have a BulletHolePool component.  Please assign GameObjects in the inspector that have the BulletHolePool component.");
        }

        // Initialize the default bullet hole pools list   ��ʼ��Ĭ�ϵĵ��׳��б�
        for (int i = 0; i < defaultBulletHolePoolNames.Count; i++)
        {
            GameObject g = GameObject.Find(defaultBulletHolePoolNames[i]);

            if (g.GetComponent<BulletHolePool>() != null)
                defaultBulletHoles[i] = g.GetComponent<BulletHolePool>();
            else
                Debug.LogWarning("Default Bullet Hole Pool does not have a BulletHolePool component.  Please assign GameObjects in the inspector that have the BulletHolePool component.");
        }
    }


    private void Update()
    {
        if (WeaponController == null)
            return;

        // /����������ĵ�ǰ����
        currentAccuracy = Mathf.Lerp(currentAccuracy, accuracy, accuracyRecoverRate * Time.deltaTime);

        // Calculate the current crosshair size.  This is what causes the crosshairs to grow and shrink dynamically while shooting ���㵱ǰ��׼�Ǵ�С������ǵ���׼�������ʱ��̬������������ԭ��
        currentCrosshairSize = startingCrosshairSize + (accuracy - currentAccuracy) * 0.8f;

        // Update the fireTimer
        fireTimer += Time.deltaTime;

        if (playerWeapon )
        {
            CheckForUserInput();
        }

        // Reload if the weapon is out of ammo
        if (reloadAutomatically && currentAmmo <= 0)
            Reload();

        // Recoil Recovery
        if (playerWeapon && recoil )
        {
            weaponModel.transform.position = Vector3.Lerp(weaponModel.transform.position, transform.position, recoilRecoveryRate * Time.deltaTime);
            weaponModel.transform.rotation = Quaternion.Lerp(weaponModel.transform.rotation, transform.rotation, recoilRecoveryRate * Time.deltaTime);
        }

        

    }


    // Checks for user input to use the weapons - only if this weapon is player-controlled
    void CheckForUserInput()
    {

        // Fire if this is a raycast type weapon and the user presses the fire button
        if (type == WeaponType.Raycast)
        {
            if (fireTimer >= actualROF && burstCounter < burstRate && canFire)
            {
                if (inputManager.onFoot.Shoot.ReadValue<float>()>0)
                {
                    Fire();
                }
                
            }
        }
        // Launch a projectile if this is a projectile type weapon and the user presses the fire button
        if (type == WeaponType.Projectile)
        {
            if (fireTimer >= actualROF && burstCounter < burstRate && canFire)
            {
                if (inputManager.onFoot.Shoot.ReadValue<float>() > 0)
                {
                    Launch();
                }
                
            }

        }
        // Reset the Burst
        if (burstCounter >= burstRate)
        {
            burstTimer += Time.deltaTime;
            if (burstTimer >= burstPause)
            {
                burstCounter = 0;
                burstTimer = 0.0f;
            }
        }
        

        // Reload if the "Reload" button is pressed
        if (Input.GetButtonDown("Reload"))
            Reload();

        // If the weapon is semi-auto and the user lets up on the button, set canFire to true
        if (inputManager.onFoot.Shoot.ReadValue<float>() == 0)
            canFire = true;
    }





    void OnGUI()
    {

        // Crosshairs
        if (type == WeaponType.Projectile )
        {
            currentAccuracy = accuracy;
        }
        if (showCrosshair&&WeaponController!=null)
        {
            // Hold the location of the center of the screen in a variable
            Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);

            // Draw the crosshairs based on the weapon's inaccuracy
            // Left
            Rect leftRect = new Rect(center.x - crosshairLength - currentCrosshairSize, center.y - (crosshairWidth / 2), crosshairLength, crosshairWidth);
            GUI.DrawTexture(leftRect, crosshairTexture, ScaleMode.StretchToFill);
            // Right
            Rect rightRect = new Rect(center.x + currentCrosshairSize, center.y - (crosshairWidth / 2), crosshairLength, crosshairWidth);
            GUI.DrawTexture(rightRect, crosshairTexture, ScaleMode.StretchToFill);
            // Top
            Rect topRect = new Rect(center.x - (crosshairWidth / 2), center.y - crosshairLength - currentCrosshairSize, crosshairWidth, crosshairLength);
            GUI.DrawTexture(topRect, crosshairTexture, ScaleMode.StretchToFill);
            // Bottom
            Rect bottomRect = new Rect(center.x - (crosshairWidth / 2), center.y + currentCrosshairSize, crosshairWidth, crosshairLength);
            GUI.DrawTexture(bottomRect, crosshairTexture, ScaleMode.StretchToFill);
        }

        // Ammo Display
        if (showCurrentAmmo && WeaponController != null)
        {
            
            if (type == WeaponType.Raycast || type == WeaponType.Projectile)
                GUI.Label(new Rect(10, Screen.height - 30, 200, 40), "Ammo: " + currentAmmo);
           
        }
    }


    // Reload the weapon
    void Reload()
    {
        currentAmmo = ammoCapacity;
        fireTimer = -reloadTime;
        GetComponent<AudioSource>().PlayOneShot(reloadSound);

        // Send a messsage so that users can do other actions whenever this happens
        SendMessageUpwards("OnEasyWeaponsReload", SendMessageOptions.DontRequireReceiver);
    }


    IEnumerator DelayFire()
    {
        // Reset the fire timer to 0 (for ROF)
        fireTimer = 0.0f;

        // Increment the burst counter
        burstCounter++;

        // If this is a semi-automatic weapon, set canFire to false (this means the weapon can't fire again until the player lets up on the fire button)
        if (auto == Auto.Semi)
            canFire = false;

        // Send a messsage so that users can do other actions whenever this happens
        SendMessageUpwards("OnEasyWeaponsFire", SendMessageOptions.DontRequireReceiver);

        yield return new WaitForSeconds(delayBeforeFire);
        Fire();
    }
    IEnumerator DelayLaunch()
    {
        // Reset the fire timer to 0 (for ROF)
        fireTimer = 0.0f;

        // Increment the burst counter
        burstCounter++;

        // If this is a semi-automatic weapon, set canFire to false (this means the weapon can't fire again until the player lets up on the fire button)
        if (auto == Auto.Semi)
            canFire = false;

        // Send a messsage so that users can do other actions whenever this happens
        SendMessageUpwards("OnEasyWeaponsLaunch", SendMessageOptions.DontRequireReceiver);

        yield return new WaitForSeconds(delayBeforeFire);
        Launch();
    }


    // Raycasting system
    void Fire()
    {
        // Reset the fireTimer to 0 (for ROF)
        fireTimer = 0.0f;

        // Increment the burst counter
        burstCounter++;

        // If this is a semi-automatic weapon, set canFire to false (this means the weapon can't fire again until the player lets up on the fire button)
        if (auto == Auto.Semi)
            canFire = false;
       
        // First make sure there is ammo
        if (currentAmmo <= 0)
        {
            DryFire();
            return;
        }

        // Subtract 1 from the current ammo
        if (!infiniteAmmo)
            currentAmmo--;


        // Fire once for each shotPerRound value
        for (int i = 0; i < shotPerRound; i++)
        {
            // Calculate accuracy for this shot
            float accuracyVary = (100 - currentAccuracy) / 1000;
            Vector3 direction = raycastStartSpot.forward;
            direction.x += UnityEngine.Random.Range(-accuracyVary, accuracyVary);
            direction.y += UnityEngine.Random.Range(-accuracyVary, accuracyVary);
            direction.z += UnityEngine.Random.Range(-accuracyVary, accuracyVary);
            currentAccuracy -= accuracyDropPerShot;
            if (currentAccuracy <= 0.0f)
                currentAccuracy = 0.0f;

            // The ray that will be used for this shot
            Ray ray = new Ray(raycastStartSpot.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, fireDistance))
            {

                // Damage
                //hit.collider.gameObject.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);
                
                if(hit.collider.gameObject.tag == "Enemy")
                    hit.collider.GetComponentInParent<Enemy>().Damage(Atk);
                if(hit.collider.gameObject.tag == "Player")
                {
                    hit.collider.GetComponent<PlayerHealth>().TackDamage(Atk);
                }


                // Bullet Holes

                    // Make sure the hit GameObject is not defined as an exception for bullet holes
                bool exception = false;
                if (bhSystem == BulletHoleSystem.Tag)
                {
                    foreach (SmartBulletHoleGroup bhg in bulletHoleExceptions)
                    {
                        if (hit.collider.gameObject.tag == bhg.tag)
                        {
                            exception = true;
                            break;
                        }
                    }
                }
                else if (bhSystem == BulletHoleSystem.Material)
                {
                    foreach (SmartBulletHoleGroup bhg in bulletHoleExceptions)
                    {
                        MeshRenderer mesh = FindMeshRenderer(hit.collider.gameObject);
                        if (mesh != null)
                        {
                            if (mesh.sharedMaterial == bhg.material)
                            {
                                exception = true;
                                break;
                            }
                        }
                    }
                }
                else if (bhSystem == BulletHoleSystem.Physic_Material)
                {
                    foreach (SmartBulletHoleGroup bhg in bulletHoleExceptions)
                    {
                        if (hit.collider.sharedMaterial == bhg.physicMaterial)
                        {
                            exception = true;
                            break;
                        }
                    }
                }

                // Select the bullet hole pools if there is no exception
                if (makeBulletHoles && !exception)
                {
                    // A list of the bullet hole prefabs to choose from
                    List<SmartBulletHoleGroup> holes = new List<SmartBulletHoleGroup>();

                    // Display the bullet hole groups based on tags
                    if (bhSystem == BulletHoleSystem.Tag)
                    {
                        foreach (SmartBulletHoleGroup bhg in bulletHoleGroups)
                        {
                            if (hit.collider.gameObject.tag == bhg.tag)
                            {
                                holes.Add(bhg);
                            }
                        }
                    }

                    // Display the bullet hole groups based on materials
                    else if (bhSystem == BulletHoleSystem.Material)
                    {
                        // Get the mesh that was hit, if any
                        MeshRenderer mesh = FindMeshRenderer(hit.collider.gameObject);

                        foreach (SmartBulletHoleGroup bhg in bulletHoleGroups)
                        {
                            if (mesh != null)
                            {
                                if (mesh.sharedMaterial == bhg.material)
                                {
                                    holes.Add(bhg);
                                }
                            }
                        }
                    }

                    // Display the bullet hole groups based on physic materials
                    else if (bhSystem == BulletHoleSystem.Physic_Material)
                    {
                        foreach (SmartBulletHoleGroup bhg in bulletHoleGroups)
                        {
                            if (hit.collider.sharedMaterial == bhg.physicMaterial)
                            {
                                holes.Add(bhg);
                            }
                        }
                    }


                    SmartBulletHoleGroup sbhg = null;

                    // If no bullet holes were specified for this parameter, use the default bullet holes
                    if (holes.Count == 0)   // If no usable (for this hit GameObject) bullet holes were found...
                    {
                        List<SmartBulletHoleGroup> defaultsToUse = new List<SmartBulletHoleGroup>();
                        foreach (BulletHolePool h in defaultBulletHoles)
                        {
                            defaultsToUse.Add(new SmartBulletHoleGroup("Default", null, null, h));
                        }

                        // Choose a bullet hole at random from the list
                        sbhg = defaultsToUse[Random.Range(0, defaultsToUse.Count)];
                    }

                    // Make the actual bullet hole GameObject
                    else
                    {
                        // Choose a bullet hole at random from the list
                        sbhg = holes[Random.Range(0, holes.Count)];
                    }

                    // Place the bullet hole in the scene
                    if (sbhg.bulletHole != null)
                        sbhg.bulletHole.PlaceBulletHole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                }

                // Hit Effects
                if (makeHitEffects)
                {
                    foreach (GameObject hitEffect in hitEffects)
                    {
                        if (hitEffect != null)
                            Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    }
                }

                // Add force to the object that was hit
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(ray.direction * power * forceMultiplier);
                }
            }
        }

        // Recoil
        if (recoil)
            Recoil();

        // Muzzle flash effects
        if (makeMuzzleEffects)
        {
            GameObject muzfx = muzzleEffects[Random.Range(0, muzzleEffects.Length)];
            if (muzfx != null)
                Instantiate(muzfx, muzzleEffectsPosition.position, muzzleEffectsPosition.rotation);
        }

        // Instantiate shell props
        if (spitShells)
        {
            GameObject shellGO = Instantiate(shell, shellSpitPosition.position, shellSpitPosition.rotation) as GameObject;
            shellGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(shellSpitForce + Random.Range(0, shellForceRandom), 0, 0), ForceMode.Impulse);
            shellGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(shellSpitTorqueX + Random.Range(-shellTorqueRandom, shellTorqueRandom), shellSpitTorqueY + Random.Range(-shellTorqueRandom, shellTorqueRandom), 0), ForceMode.Impulse);
        }

        // Play the gunshot sound
        GetComponent<AudioSource>().PlayOneShot(fireSound);
    }

    // Projectile system
    public void Launch()
    {
        // Reset the fire timer to 0 (for ROF)
        fireTimer = 0.0f;

        // Increment the burst counter
        burstCounter++;

        // If this is a semi-automatic weapon, set canFire to false (this means the weapon can't fire again until the player lets up on the fire button)
        if (auto == Auto.Semi)
            canFire = false;

        // First make sure there is ammo
        if (currentAmmo <= 0)
        {
            DryFire();
            return;
        }

        // Subtract 1 from the current ammo
        if (!infiniteAmmo)
            currentAmmo--;

        // Fire once for each shotPerRound value
        for (int i = 0; i < shotPerRound; i++)
        {
            // Instantiate the projectile
            if (projectile != null)
            {
                GameObject proj = Instantiate(projectile, projectileSpawnSpot.position, projectileSpawnSpot.rotation) as GameObject;
                //proj.GetComponent<Rigidbody>().AddForce(projectileSpawnSpot.forward * power*100);
                
            }
            else
            {
                Debug.Log("Projectile to be instantiated is null.  Make sure to set the Projectile field in the inspector.");
            }
        }

        // Recoil
        if (recoil)
            Recoil();

        // Muzzle flash effects
        if (makeMuzzleEffects)
        {
            GameObject muzfx = muzzleEffects[Random.Range(0, muzzleEffects.Length)];
            if (muzfx != null)
                Instantiate(muzfx, muzzleEffectsPosition.position, muzzleEffectsPosition.rotation);
        }

        // Instantiate shell props
        if (spitShells)
        {
            GameObject shellGO = Instantiate(shell, shellSpitPosition.position, shellSpitPosition.rotation) as GameObject;
            shellGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(shellSpitForce + Random.Range(0, shellForceRandom), 0, 0), ForceMode.Impulse);
            shellGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(shellSpitTorqueX + Random.Range(-shellTorqueRandom, shellTorqueRandom), shellSpitTorqueY + Random.Range(-shellTorqueRandom, shellTorqueRandom), 0), ForceMode.Impulse);
        }

        // Play the gunshot sound
        GetComponent<AudioSource>().PlayOneShot(fireSound);
    }


    // When the weapon tries to fire without any ammo
    void DryFire()
    {
        GetComponent<AudioSource>().PlayOneShot(dryFireSound);
    }


    // Recoil FX.  This is the "kick" that you see when the weapon moves back while firing
    void Recoil()
    {
        // No recoil for AIs
        if (!playerWeapon)
            return;

        // Make sure the user didn't leave the weapon model field blank
        if (weaponModel == null)
        {
            Debug.Log("Weapon Model is null.  Make sure to set the Weapon Model field in the inspector.");
            return;
        }

        // Calculate random values for the recoil position and rotation
        float kickBack = Random.Range(recoilKickBackMin, recoilKickBackMax);
        float kickRot = Random.Range(recoilRotationMin, recoilRotationMax);

        // Apply the random values to the weapon's postion and rotation
        weaponModel.transform.Translate(new Vector3(0, 0, -kickBack), Space.Self);
        weaponModel.transform.Rotate(new Vector3(-kickRot, 0, 0), Space.Self);
    }


    // Find a mesh renderer in a specified gameobject, it's children, or its parents
    MeshRenderer FindMeshRenderer(GameObject go)
    {
        MeshRenderer hitMesh;

        // Use the MeshRenderer directly from this GameObject if it has one
        if (go.GetComponent<Renderer>() != null)
        {
            hitMesh = go.GetComponent<MeshRenderer>();
        }

        // Try to find a child or parent GameObject that has a MeshRenderer
        else
        {
            // Look for a renderer in the child GameObjects
            hitMesh = go.GetComponentInChildren<MeshRenderer>();

            // If a renderer is still not found, try the parent GameObjects
            if (hitMesh == null)
            {
                GameObject curGO = go;
                while (hitMesh == null && curGO.transform != curGO.transform.root)
                {
                    curGO = curGO.transform.parent.gameObject;
                    hitMesh = curGO.GetComponent<MeshRenderer>();
                }
            }
        }

        return hitMesh;
    }




    public void ShootStart()
    {
        isAttacking = true;
        if (fireTimer >= actualROF && burstCounter < burstRate && canFire)
        {
            if (type == WeaponType.Raycast)
            {
                StartCoroutine(DelayFire());
            }
            else if (type == WeaponType.Projectile)
            {
                StartCoroutine(DelayLaunch());
            }
        }
       
    }

    public void ShootEnd()
    {
        isAttacking = false;

        canFire = true;
        //if (type == WeaponType.Raycast)
        //{
        //    StopCoroutine(DelayFire());
        //}
        //else if (type == WeaponType.Projectile)
        //{
        //    StopCoroutine(DelayLaunch());
        //}
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
//        if (Physics.Raycast(RandomShoot(), out hitInfo, FireDistance, WeaponController.mask))
//        {

//            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
//            GameObject hitEff = Instantiate(Resources.Load<GameObject>("Prefabs/Eff/Eff_HitPoint_01"),
//                                    hitInfo.point, rot, hitInfo.collider.transform);

//            Enemy enemy = hitInfo.collider.GetComponentInParent<Enemy>();
//            if (enemy != null)
//            {
//                enemy.Damage(Atk);

//            }


//        }

//        //WeaponController.recoil.AddRecoil();

//        print("���Э�̹���");
//        yield return new WaitForSeconds(FireRate);//�ж�Э�� ͣ���������ʱ��
//    }

//}




///// <summary>
///// �����ɢ��
///// </summary>
///// <returns></returns>
//private Ray RandomShoot()
//{

//    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

//    Quaternion hQ = Quaternion.LookRotation(ray.direction);
//    hQ *= Quaternion.Euler(Random.Range(-Dispersal, Dispersal), Random.Range(-Dispersal, Dispersal), 0);
//    ray.direction = (hQ * Vector3.forward).normalized;

//    return ray;
//}

//public void ShootStart()
//{
//    isAttacking = true;
//    StartCoroutine(IGunAttack());
//}

//public void ShootEnd()
//{
//    isAttacking = false;
//    StopCoroutine(IGunAttack());
//}