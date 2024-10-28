using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public bool isTrigger = false;


    [Header("Explosion Prefab")]
    [SerializeField] private GameObject exposionEffectPrafab;
    [SerializeField] private Vector3 exposionParticleOffset = new Vector3(0,1,0);

    [Header("Explosion Settings")]
    [SerializeField] private float exposionATK = 100f;
    [SerializeField] private float exposionDelay = 3f;
    [SerializeField] private float exposionForce = 700f;
    [SerializeField] private float exposionRadius = 5f;

    [Header("Explosion Audio")]
    [SerializeField] private AudioClip exposionSound;
    [SerializeField] private AudioClip impactSound;

    [SerializeField] private GameObject audioPrefab;




    private float countdown;
    


    private void Start()
    {
        
        countdown = exposionDelay;

    }

    private void Update()
    {
        if (isTrigger)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                Expode();
                
            }
        }
    }

    void Expode()
    {
        GameObject exposionEffect = Instantiate(exposionEffectPrafab,transform.position+exposionParticleOffset, Quaternion.identity);

        Destroy(exposionEffect,4f);

        GameObject audio = Instantiate(audioPrefab,transform.position, Quaternion.identity);
        audio.GetComponent<AudioSource>().PlayOneShot(exposionSound);
        Destroy(audio, exposionSound.length);

        GrenadeAtk();

        NearbyForceApply();

        Destroy(gameObject);
    }


    void GrenadeAtk()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position , exposionRadius);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponentInParent<Enemy>() != null)
                {
                    collider.GetComponentInParent<Enemy>().Damage(exposionATK);
                }

                if (collider.CompareTag("Player"))
                {
                    collider.GetComponentInParent<PlayerHealth>().TackDamage(exposionATK);
                }


            }
        }
    }


    void NearbyForceApply()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, exposionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(exposionForce, transform.position, exposionRadius);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<AudioSource>().PlayOneShot(impactSound);
    }


    /// <summary>
    /// Ê°È¡ÊÖÀ×
    /// </summary>
    public void PickUpGrenade()
    {

        if (BagDataMgr.Instance.bagData.grenade_Item == null)
        {
            BagDataMgr.Instance.PutInGre(GetComponent<ItemBase>().InitThisItem());

            UIManager.Instance.GetPanel<PlayerPanel>().btnGrenades.image.sprite = Resources.Load<Sprite>(GetComponent<ItemBase>().icon); ;

            Destroy(gameObject);
            return;
        }
            
        BagDataMgr.Instance.PutInBag(GetComponent<ItemBase>().InitThisItem());

        Destroy(gameObject);
    }



}
