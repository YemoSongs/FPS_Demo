using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Íæ¼Ò½¡¿µÏµÍ³
/// </summary>
public class PlayerHealth : MonoBehaviour
{

    [SerializeField]
    private float health;
    private float lerpTimer;
    [Header("Health Bar")]
    public float maxhealth = 100f;
    public float chipSpeed = 2f;

    public Image FHealthBar;
    public Image BHealthBar;


    private float fillF;
    private float fillB;
    private float hFraction;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;

    private float durationTimer;

    void Start()    
    {
        FHealthBar = UIManager.Instance.GetPanel<PlayerPanel>().FHealthBar;
        BHealthBar = UIManager.Instance.GetPanel<PlayerPanel>().BHealthBar;
        overlay = UIManager.Instance.GetPanel<PlayerPanel>().overlay;
        
        health = maxhealth;
               
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health,0,maxhealth);
        UpdataHealthUI();

        if(overlay.color.a > 0)
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime* fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b,tempAlpha);
            }
        }
    }

    public void UpdataHealthUI()
    {
        fillF = FHealthBar.fillAmount;
        fillB = BHealthBar.fillAmount;
        hFraction = health / maxhealth;

        if(fillB > hFraction)
        {
            FHealthBar.fillAmount = hFraction;
            BHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            BHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if(fillF < hFraction)
        {
            BHealthBar.fillAmount = hFraction;
            BHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            FHealthBar.fillAmount = Mathf.Lerp(fillF, BHealthBar.fillAmount, percentComplete);
        }
    }

    /// <summary>
    /// ÈÃÍæ¼Ò¼õÑª
    /// </summary>
    /// <param name="damage"></param>
    public void TackDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0;

        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0.6f);
    }

    /// <summary>
    /// ÈÃÍæ¼Ò¼ÓÑª
    /// </summary>
    /// <param name="healAmount"></param>
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0;
    }


}
