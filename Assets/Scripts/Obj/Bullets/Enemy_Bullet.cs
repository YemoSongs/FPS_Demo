using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ×Óµ¯
/// </summary>
public class Enemy_Bullet : MonoBehaviour
{


    private void Start()
    {
        Destroy(gameObject, 3f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            hitTransform.GetComponent<PlayerHealth>().TackDamage(Random.Range(8,18));
            Destroy(gameObject);
        }
       
    }



}
