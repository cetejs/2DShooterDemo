using UnityEngine;

namespace Mechanics
{
    /// <summary>
    /// 角色控制器接口定义，设计的可能不太合理但是满足当前需求
    /// </summary>
    public interface ICharacterController 
    {
        /// <summary>
        /// 角色是否活着
        /// </summary>
        /// <returns></returns>
        bool IsAlive();

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
        /// <param name="deathDir">死亡方向</param>
        void Die(int deathDir);

        /// <summary>
        /// 角色复活
        /// </summary>
        /// <param name="point"></param>
        void Revive(Transform point);
    }
}