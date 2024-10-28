using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowProperty : MonoBehaviour
{
    public Animate Animate;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI AD;


    private void Update()
    {
        HP.text = $"����ֵ��{Animate.HP}";
        AD.text = $"��������{Animate.AD}";
    }
}
