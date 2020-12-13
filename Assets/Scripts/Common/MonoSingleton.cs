using UnityEngine;

namespace Common
{
    /// <summary>
    /// Mono通用单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        #region 属性字段

        [Header("加载新场景时是否保留")]
        public bool dontDestroyOnLoad;

        public static T Instance { get; private set; }

        #endregion

        #region 内部实现

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;

                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}