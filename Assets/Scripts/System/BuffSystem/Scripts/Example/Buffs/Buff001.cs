using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffSystem;
using System;
using static UnityEngine.GraphicsBuffer;

public class Buff001 : BuffBase
{
    private float m_HealingPerSecond = 1f;
    private PlayerHealth m_Target;
    public override void Initialize(GameObject owner, string provider)
    {
        base.Initialize(owner, provider);
        m_Target = owner.GetComponent<PlayerHealth>();
        MaxDuration = 15;
        TimeScale = 1f;
        MaxLevel = 5;
        BuffType = BuffType.Buff;
        ConflictResolution = ConflictResolution.combine;
        Dispellable = false;
        Name = "恢复体力";
        Description = $"每秒恢复{m_HealingPerSecond}点生命值";
        Demotion = 1;
        IconPath = "Icon/2003";
    }
    public override void FixedUpdate()
    {
        //m_Target.HP += m_HealingPerSecond * BuffManager.FixedDeltaTime;

        m_Target.RestoreHealth(m_HealingPerSecond * BuffManager.FixedDeltaTime);
    }
}

