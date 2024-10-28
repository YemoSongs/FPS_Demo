using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    [SerializeField] private Path path;

    [SerializeField] private float spawnTime = 20;
    private float timer = 0;
    

    void Start()
    {
        Spawner();
    }

    public void Spawner()
    {
        if (enemy != null)
        {
            GameObject e = Instantiate(enemy,this.transform.position,Quaternion.identity); 
            e.GetComponent<Enemy>().path = this.path;
           
        }

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnTime)
        {
            Spawner();
            timer = 0;
        }
    }



}
