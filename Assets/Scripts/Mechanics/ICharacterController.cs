namespace Mechanics
{
    /// <summary>
    /// 角色控制器接口定义，设计的可能不太合理但是满足当前需求
    /// </summary>
    public interface ICharacterController 
    {
        /// <summary>
        /// 控制器是否是激活状态
        /// </summary>
        /// <returns></returns>
        bool IsControlEnabled();

        /// <summary>
        /// 角色受击无法行动
        /// </summary>
        void Hit();

        /// <summary>
        /// 角色受击后恢复行动
        /// </summary>
        void OnHitted();

        /// <summary>
        /// 角色死亡
        /// </summary>
        void Die();
    }
}