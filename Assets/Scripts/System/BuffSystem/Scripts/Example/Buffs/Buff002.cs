using BuffSystem;
using UnityEngine;

public class Buff002 : BuffBase
{
    private float m_ADUp = 10f;
    private Animate m_Animate;
    public override void Initialize(GameObject owner, string provider)
    {
        base.Initialize(owner, provider);
        MaxDuration = 5f;
        MaxLevel = 10;
        BuffType = BuffType.Buff;
        ConflictResolution = ConflictResolution.combine;
        Dispellable = false;
        Name = "借来的短剑";
        Description = "每层增加10点攻击力";
        IconPath = "Icon/1036";
        Demotion = MaxLevel;
        m_Animate = Owner.GetComponent<Animate>();
    }
    protected override void OnLevelChange(int change)
    {
        m_Animate.AD += m_ADUp * change;
        ResidualDuration = MaxDuration;
    }
}
