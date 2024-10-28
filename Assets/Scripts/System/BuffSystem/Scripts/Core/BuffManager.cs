using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

namespace BuffSystem
{
    public class BuffManager : MonoBehaviour
    {
        /// <summary>
        /// 固定时间更新的更新频率，此值不宜过高，可以过低（会增加性能消耗）。
        /// </summary>
        public const float FixedDeltaTime = 0.1f;


        #region 单例
        private static BuffManager m_Instance;
        public static BuffManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject l_GameObject = new GameObject("Buff Manager");
                    m_Instance = l_GameObject.AddComponent<BuffManager>();
                    DontDestroyOnLoad(l_GameObject);
                }
                return m_Instance;
            }
        }
        #endregion
        /// <summary>
        /// 存储了所有的buff，key为buff持有者，value为他所持有的所有buff。
        /// </summary>
        private Dictionary<GameObject, List<BuffBase>> m_BuffDictionary = new Dictionary<GameObject, List<BuffBase>>(25);
        private Dictionary<GameObject, Action<BuffBase>> m_ObserverDicitinary = new Dictionary<GameObject, Action<BuffBase>>(25);
        #region Public方法
        /// <summary>
        /// 返回要观察的对象现有的buff，并且在对象被添加新buff时通知你
        /// （如果现在对象身上没有buff会返回空列表，不会返回null）
        /// </summary>
        /// <returns></returns>
        public List<BuffBase> StartObserving(GameObject target, Action<BuffBase> listener)
        {
            List<BuffBase> list;
            //添加监听
            if (!m_ObserverDicitinary.ContainsKey(target))
            {
                m_ObserverDicitinary.Add(target, null);
            }
            m_ObserverDicitinary[target] += listener;
            //查找已有buff
            if (m_BuffDictionary.ContainsKey(target))
            {
                list = m_BuffDictionary[target];
            }
            else
            {
                list = new List<BuffBase>();
            }
            //返回
            return list;
        }
        /// <summary>
        /// 停止观察某一对象，请传入与调用开始观察方法时使用的相同参数。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="listener"></param>
        /// <exception cref="Exception"></exception>
        public void StopObsveving(GameObject target, Action<BuffBase> listener)
        {
            if (!m_ObserverDicitinary.ContainsKey(target))
            {
                throw new Exception("要停止观察的对象不存在");
            }
            m_ObserverDicitinary[target] -= listener;
            if (m_ObserverDicitinary[target] == null)
            {
                m_ObserverDicitinary.Remove(target);
            }
        }
        /// <summary>
        /// 在目标身上挂buff
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="provider"></param>
        /// <param name="level"></param>
        public void AddBuff<T>(GameObject target, string provider, int level = 1) where T : BuffBase, new()
        {
            //如果我们的字典里没有存储这个key，就进行初始化
            if (!m_BuffDictionary.ContainsKey(target))
            {
                m_BuffDictionary.Add(target, new List<BuffBase>(5));
                //目标身上自然没有任何buff，直接挂一个新buff即可
                AddNewBuff<T>(target, provider, level);
                return;
            }

            //如果目标身上没有任何buff，直接挂一个新buff即可
            if (m_BuffDictionary[target].Count == 0)
            {
                AddNewBuff<T>(target, provider, level);
                return;
            }

            //遍历看看目标身上有没有已存在的要挂的buff。
            List<T> temp01 = new List<T>();
            foreach (BuffBase item in m_BuffDictionary[target])
            {
                if (item is T)
                {
                    temp01.Add((T)item);
                    break;
                }
            }
            //如果没有直接挂一个新buff就行了
            //如果有已存在的要挂的buff，就要进行冲突处理了
            if (temp01.Count == 0)
            {
                AddNewBuff<T>(target, provider, level);
            }
            else
            {
                switch (temp01[0].ConflictResolution)
                {
                    //如果是独立存在，那也直接挂buff
                    case ConflictResolution.separate:
                        bool temp = true;
                        foreach (T item in temp01)
                        {
                            if (item.Provider == provider)
                            {
                                item.CurrentLevel += level;
                                temp = false;
                                continue;
                            }
                        }
                        if (temp)
                        {
                            AddNewBuff<T>(target, provider, level);
                        }
                        break;
                    //如果是合并，则跟已有的buff叠层。
                    case ConflictResolution.combine:
                        temp01[0].CurrentLevel += level;
                        break;
                    //如果是覆盖，则移除旧buff，然后添加这个buff。
                    case ConflictResolution.cover:
                        RemoveBuff(target, temp01[0]);
                        AddNewBuff<T>(target, provider, level);
                        break;
                }
            }

        }
        /// <summary>
        /// 获得单位身上指定类型的buff的列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public List<T> FindBuff<T>(GameObject Owner) where T : BuffBase, new()
        {
            List<T> result = new List<T>();
            if (m_BuffDictionary.ContainsKey(Owner))
            {
                List<BuffBase> buff = m_BuffDictionary[Owner];
                foreach (BuffBase item in buff)
                {
                    if (item is T)
                    {
                        result.Add(item as T);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获得单位身上所有的buff
        /// 如果单位身上没有任何buff则返回空列表
        /// </summary>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public List<BuffBase> FindAllBuff(GameObject Owner)
        {
            List<BuffBase> result = new List<BuffBase>();
            if (m_BuffDictionary.ContainsKey(Owner))
            {
                result = m_BuffDictionary[Owner];
            }
            return result;
        }
        /// <summary>
        /// 移除单位身上指定的一个buff
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="buff"></param>
        /// <returns>是否成功，如果失败说明目标不存在</returns>
        public bool RemoveBuff(GameObject owner, BuffBase buff)
        {
            if (!m_BuffDictionary.ContainsKey(owner))
            {
                return false;
            }

            bool haveTarget = false;
            foreach (BuffBase item in m_BuffDictionary[owner])
            {
                if (item == buff)
                {
                    haveTarget = true;
                    item.CurrentLevel -= item.CurrentLevel;
                    item.OnLost();
                    m_BuffDictionary[owner].Remove(item);
                    break;
                }
            }
            if (!haveTarget)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Private方法
        private void AddNewBuff<T>(GameObject target, string provider, int level) where T : BuffBase, new()
        {
            T buff = new T();
            buff.Initialize(target, provider);
            m_BuffDictionary[target].Add(buff);
            buff.ResidualDuration = buff.MaxDuration;
            buff.CurrentLevel = level;
            buff.OnGet();
            if (m_ObserverDicitinary.ContainsKey(target))
            {
                m_ObserverDicitinary[target]?.Invoke(buff);
            }
        }
        #endregion


        private WaitForSeconds m_WaitForFixedDeltaTimeSeconds = new WaitForSeconds(FixedDeltaTime);
        private IEnumerator ExecuteFixedUpdate()
        {
            while (true)
            {
                yield return m_WaitForFixedDeltaTimeSeconds;
                //执行所有buff的update；
                foreach (KeyValuePair<GameObject, List<BuffBase>> item1 in m_BuffDictionary)
                {
                    foreach (BuffBase item2 in item1.Value)
                    {
                        if (item2.CurrentLevel > 0 && item2.Owner != null)
                        {
                            item2.FixedUpdate();
                        }
                    }
                }
            }
        }
        private WaitForSeconds m_WaitFor10Seconds = new WaitForSeconds(10f);
        private Dictionary<GameObject, List<BuffBase>> m_BuffDictionaryCopy = new Dictionary<GameObject, List<BuffBase>>(25);
        private IEnumerator ExecuteGrabageCollection()
        {
            while (true)
            {
                yield return m_WaitFor10Seconds;
                //复制一份
                m_BuffDictionaryCopy.Clear();
                foreach (KeyValuePair<GameObject, List<BuffBase>> item in m_BuffDictionary)
                {
                    m_BuffDictionaryCopy.Add(item.Key, item.Value);
                }
                //清理无用对象
                foreach (KeyValuePair<GameObject, List<BuffBase>> item in m_BuffDictionaryCopy)
                {
                    //如果owner被删除，我们这边也跟着删除
                    if (item.Key == null)
                    {
                        m_BuffDictionary.Remove(item.Key);
                        continue;
                    }
                    //如果一个owner身上没有任何buff，就没必要留着他了
                    if (item.Value.Count == 0)
                    {
                        m_BuffDictionary.Remove(item.Key);
                        continue;
                    }
                }
            }
        }
        #region Unity方法

        private void Awake()
        {
            StartCoroutine(ExecuteFixedUpdate());
            StartCoroutine(ExecuteGrabageCollection());
        }


        private BuffBase m_Transfer_Buff;
        private void FixedUpdate()
        {
            //清理无用对象
            foreach (KeyValuePair<GameObject, List<BuffBase>> item in m_BuffDictionary)
            {
                //清理无用buff
                //降低持续时间
                for (int i = item.Value.Count - 1; i >= 0; i--)
                {
                    m_Transfer_Buff = item.Value[i];
                    //如果等级为0，则移除
                    if (m_Transfer_Buff.CurrentLevel == 0)
                    {
                        RemoveBuff(item.Key, m_Transfer_Buff);
                        continue;
                    }
                    //如果持续时间为0，则降级,
                    //降级后如果等级为0则移除，否则刷新持续时间
                    if (m_Transfer_Buff.ResidualDuration == 0)
                    {
                        m_Transfer_Buff.CurrentLevel -= m_Transfer_Buff.Demotion;
                        if (m_Transfer_Buff.CurrentLevel == 0)
                        {
                            RemoveBuff(item.Key, m_Transfer_Buff);
                            continue;
                        }
                        else
                        {
                            m_Transfer_Buff.ResidualDuration = m_Transfer_Buff.MaxDuration;
                        }
                    }
                    //降低持续时间
                    m_Transfer_Buff.ResidualDuration -= Time.fixedDeltaTime;
                }
            }
        }
        #endregion
    }
}
