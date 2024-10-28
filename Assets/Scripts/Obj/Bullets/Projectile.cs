using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int atk = 10;

    

    public float power = 10;

    //public bool isExposion = false;

    public float delayExposion = 3f;

   

    public int exposionAtk = 80;

    public float exposionRadius = 4f;

    public GameObject ExposionEff;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Rigidbody>().AddForce(transform.forward*power*100);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delayExposion)
        {
          
            Expode();

            Destroy(gameObject);
        }
    }


    void Expode()
    {
        GameObject eff = Instantiate(ExposionEff, transform.position, Quaternion.identity);

        Destroy(eff, 3f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, exposionRadius);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponentInParent<Enemy>() != null)
                {
                    collider.GetComponentInParent<Enemy>().Damage(exposionAtk);
                }

                if (collider.CompareTag("Player"))
                {
                    collider.GetComponentInParent<PlayerHealth>().TackDamage(exposionAtk);
                }


            }
        }
    }

}
