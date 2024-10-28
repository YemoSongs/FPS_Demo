using BuffSystem;
using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Animate Hero;

    private void Awake()
    {

    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BuffManager.Instance.AddBuff<Buff001>(Hero.gameObject, "天外飞兔", 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BuffManager.Instance.AddBuff<Buff002>(Hero.gameObject, "天外飞兔", 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BuffManager.Instance.AddBuff<Buff003>(Hero.gameObject, "天外飞兔", 80);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BuffManager.Instance.AddBuff<Buff004>(Hero.gameObject, "天外飞兔", 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            int number = 1;
            if(BuffManager.Instance.FindBuff<Buff005>(Hero.gameObject).Count ==0  )
            {
                number = 2;
            }
            BuffManager.Instance.AddBuff<Buff005>(Hero.gameObject, "天外飞兔", number);
        }
    }
}