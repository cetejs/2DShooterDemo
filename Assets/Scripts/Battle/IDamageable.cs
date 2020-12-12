namespace Battle
{
    /// <summary>
    /// 可损坏者
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// 获得损害
        /// </summary>
        /// <param name="damage"></param>
        void GetDamage(int damage);
    }
}