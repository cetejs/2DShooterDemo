using UnityEngine;

namespace Extend
{
    /// <summary>
    /// 游戏物体的扩展类
    /// </summary>
    public static class GameObjectEx
    {
        /// <summary>
        /// 安全的激活一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetSafeActive(this GameObject obj, bool value)
        {
            if (obj.activeInHierarchy != value)
            {
                obj.SetActive(value);
            }
        }
    }
}