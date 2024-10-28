using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffSystem;
using static UnityEngine.GraphicsBuffer;

public class Buff003 : BuffBase
{
    Animate m_Target;
    public override void Initialize(GameObject owner, string provider)
    {
        base.Initialize(owner, provider);
        TimeScale = 0f;
        MaxLevel = int.MaxValue;
        BuffType = BuffType.Buff;
        ConflictResolution = ConflictResolution.separate;
        Dispellable = false;
        Name = "盛宴";
        Description = "增加生命值";
        IconPath = "Icon/Feast";
        Demotion = 0;
        m_Target = owner.GetComponent<Animate>();
    }
    protected override void OnLevelChange(int change)
    {
        m_Target.HP += change;
    }
}
