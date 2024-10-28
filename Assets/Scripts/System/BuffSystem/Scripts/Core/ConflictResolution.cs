namespace BuffSystem
{
    /// <summary>
    /// 当两个不同单位向同一个单位施加同一个buff时的冲突处理
    /// </summary>
  public enum ConflictResolution
    {
        /// <summary>
        /// 合并为一个buff，叠层（提高等级）
        /// </summary>
        combine,
        /// <summary>
        /// 独立存在
        /// </summary>
        separate,
        /// <summary>
        /// 覆盖，后者覆盖前者
        /// </summary>
        cover,
    }
}
