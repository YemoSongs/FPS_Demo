using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffSystem;
public class Buff004 : BuffBase
{
    Animate m_Target;
    float m_DamagePerSeconds = 30;
    public override void Initialize(GameObject owner, string provider)
    {
        base.Initialize(owner, provider);
        m_Target = owner.GetComponent<Animate>();
        MaxDuration = 5f;
        TimeScale = 1f;
        MaxLevel = 5;
        BuffType = BuffType.Debuff;
        ConflictResolution = ConflictResolution.separate;
        Dispellable = true;
        Name = "流血";
        Description = "每层每秒受到30点伤害";
        IconPath = "Icon/Darius_PassiveBuff";
        Demotion = MaxLevel;
    }
    protected override void OnLevelChange(int change)
    {
            ResidualDuration = MaxDuration;
    }
    public override void FixedUpdate()
    {
        m_Target.HP -= m_DamagePerSeconds * CurrentLevel * BuffManager.FixedDeltaTime;
    }
}
