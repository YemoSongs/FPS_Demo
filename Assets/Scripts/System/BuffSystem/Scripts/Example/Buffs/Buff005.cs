using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffSystem;
public class Buff005 : BuffBase
{
    Animate m_Target;
    float m_DamagePerSeconds = 10;
    public override void Initialize(GameObject owner, string provider)
    {
        base.Initialize(owner, provider);
        m_Target = owner.GetComponent<Animate>();
        MaxDuration = 1f;
        TimeScale = 1f;
        MaxLevel = int.MaxValue;
        BuffType = BuffType.Debuff;
        ConflictResolution = ConflictResolution.combine;
        Dispellable = true;
        Name = "被点燃";
        Description = "每秒受到10点伤害";
        IconPath = "Icon/Darius_PassiveBuff";
        Demotion = 1;
    }

    public override void FixedUpdate()
    {
        m_Target.HP -= m_DamagePerSeconds * BuffManager.FixedDeltaTime;
    }
}
