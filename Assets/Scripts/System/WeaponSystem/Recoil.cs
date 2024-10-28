using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    [SerializeField]
    private Vector2 recoil;
    public Vector2 addSpeed;
    public Vector2 subSpeed = new Vector2(2, 3f);
    public Vector2 maxRecoil = new Vector2(3, 5f);

    public PlayerLook playerLook;
    

    private void Update()
    {
        if (addSpeed == Vector2.zero)
            return;
        

        
        recoil.x = Mathf.MoveTowards(recoil.x, 0, subSpeed.x * Time.deltaTime);
        recoil.y = Mathf.MoveTowards(recoil.y, 0, subSpeed.y * Time.deltaTime);
        //playerLook.ProcessLook(-recoil);       
    }

    public void AddRecoil()
    {
        if (addSpeed == Vector2.zero)
            return;
        recoil.x = Mathf.Clamp(recoil.x + Random.Range(-1, 1) * addSpeed.x, -maxRecoil.x, maxRecoil.x);
        recoil.y = Mathf.Clamp(recoil.y + addSpeed.y , 0 , maxRecoil.y);

        playerLook.ProcessLook(recoil);
    }

    
}
