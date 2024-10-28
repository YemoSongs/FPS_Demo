using System;
using UnityEngine;
namespace BuffSystem
{
    public class BuffBase
    {
        private GameObject m_Owner;
        private string m_Provider = "";
        private float m_MaxDuration = 3;
        private float m_TimeScale = 1;
        private int m_MaxLevel = 1;
        private BuffType m_BuffType = BuffType.None;
        private ConflictResolution m_ConflictResolution = ConflictResolution.cover;
        private bool m_Dispellable = true;
        private string m_Name = "默认名称";
        private string m_Description = "这个Buff没有介绍";
        private int m_Demotion = 1;
        private string m_IconPath = "";


        private int m_CurrentLevel = 0;
        private float m_ResidualDuration = 3;

        private bool m_Initialized = false;

        /// <summary>
        /// 此buff的持有者
        /// </summary>
        public GameObject Owner
        {
            get { return m_Owner; }
            protected set { m_Owner = value; }
        }
        /// <summary>
        /// 此Buff的提供者
        /// </summary>
        public string Provider
        {
            get { return m_Provider; }
            protected set { m_Provider = value; }
        }
        /// <summary>
        /// Buff的初始持续时间
        /// </summary>
        public float MaxDuration
        {
            get { return m_MaxDuration; }
            protected set { m_MaxDuration = Math.Clamp(value, 0, float.MaxValue); }
        }
        /// <summary>
        /// buff的时间流失速度，最小为0，最大为10。
        /// </summary>
        public float TimeScale
        {
            get { return m_TimeScale; }
            set { m_TimeScale = Math.Clamp(value, 0, 10); }
        }
        /// <summary>
        /// buff的最大堆叠层数，最小为1，最大为2147483647
        /// </summary>
        public int MaxLevel
        {
            get { return m_MaxLevel; }
            protected set { m_MaxLevel = Math.Clamp(value, 1, int.MaxValue); }
        }
        /// <summary>
        /// Buff的类型，分为正面、负面、中立三种
        /// </summary>
        public BuffType BuffType
        {
            get { return m_BuffType; }
            protected set { m_BuffType = value; }
        }
        /// <summary>
        /// 当两个不同单位向同一个单位施加同一个buff时的冲突处理
        /// 分为三种：
        /// combine,合并为一个buff，叠层（提高等级）
        ///  separate,独立存在
        ///   cover, 覆盖，后者覆盖前者
        /// </summary>
        public ConflictResolution ConflictResolution
        {
            get { return m_ConflictResolution; }
            protected set { m_ConflictResolution = value; }
        }
        /// <summary>
        /// 可否被驱散
        /// </summary>
        public bool Dispellable
        {
            get { return m_Dispellable; }
            protected set { m_Dispellable = value; }
        }
        /// <summary>
        /// Buff对外显示的名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            protected set { m_Name = value; }
        }
        /// <summary>
        /// Buff的介绍文本
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            protected set { m_Description = value; }
        }
        /// <summary>
        /// 图标资源的路径
        /// </summary>
        public string IconPath
        {
            get { return m_IconPath; }
            protected set { m_IconPath = value; }
        }

        /// <summary>
        /// 每次Buff持续时间结束时降低的等级，一般降低1级或者降低为0级。
        /// </summary>
        public int Demotion
        {
            get { return m_Demotion; }
            protected set { m_Demotion = Math.Clamp(value, 0, MaxLevel); }
        }




        /// <summary>
        /// Buff的当前等级
        /// </summary>
        public int CurrentLevel
        {
            get { return m_CurrentLevel; }
            set
            {
                //计算出改变值
                int change = Math.Clamp(value, 0, MaxLevel) - m_CurrentLevel;
                OnLevelChange(change);
                m_CurrentLevel += change;
            }
        }
        /// <summary>
        /// Buff的当前剩余时间
        /// </summary>
        public float ResidualDuration
        {
            get { return m_ResidualDuration; }
            set { m_ResidualDuration = Math.Clamp(value, 0, float.MaxValue); }
        }

        /// <summary>
        /// 当Owner获得此buff时触发
        /// 由BuffManager在合适的时候调用
        /// </summary>
        public virtual void OnGet() { }
        /// <summary>
        /// 当Owner失去此buff时触发
        /// 由BuffManager在合适的时候调用
        /// </summary>
        public virtual void OnLost() { }
        /// <summary>
        /// Update,由BuffManager每物理帧调用
        /// </summary>
        public virtual void FixedUpdate() { }
        /// <summary>
        /// 当等级改变时调用
        /// </summary>
        /// <param name="change">改变了多少级</param>
        protected virtual void OnLevelChange(int change) { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="provider"></param>
        /// <exception cref="Exception"></exception>
        public virtual void Initialize(GameObject owner, string provider)
        {
            if (m_Initialized)
            {
                throw new Exception("不能对已经初始化的buff再次初始化");
            }
            if (owner == null || provider == null)
            {
                throw new Exception("初始化值不能为空");
            }
            Owner = owner;
            Provider = provider;
            m_Initialized = true;
        }
    }
}
