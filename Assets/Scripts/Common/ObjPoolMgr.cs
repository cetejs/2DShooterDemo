using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 对象池管理者
    /// </summary>
    public class ObjPoolMgr : MonoSingleton<ObjPoolMgr>
    {
        [Header("需要池子管理的实体")]
        public GameObject[] prefabs;

        private readonly Dictionary<string, ObjPool> m_ObjPoolDict = new Dictionary<string, ObjPool>();

        private void Start()
        {
            foreach (GameObject pref in prefabs)
            {
                string key = pref.name;

                if (m_ObjPoolDict.ContainsKey(key))
                {
                    Debug.LogError("ObjPoolMgr.Start：已经存在同名的对象池");
                }

                ObjPool objPool = new ObjPool(pref);
                m_ObjPoolDict.Add(pref.name, objPool);
            }
        }

        /// <summary>
        /// 生产一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefName"></param>
        /// <returns></returns>
        public T SpawnObj<T>(string prefName) where T : MonoBehaviour
        {
            return SpawnObj(prefName).GetComponent<T>();
        }

        /// <summary>
        /// 生产一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefName"></param>
        /// <returns></returns>
        public GameObject SpawnObj(string prefName)
        {
            if (m_ObjPoolDict.TryGetValue(prefName, out ObjPool objPool))
            {
                return objPool.SpawnObj();
            }
            else
            {
                throw new System.Exception("ObjPoolMgr.RecycleObj：不存在这个对象池，无法生产，请检查是否添加到了ObjPoolMgr中");
            }
        }

        /// <summary>
        /// 回收一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefName"></param>
        /// <returns></returns>
        public void RecycleObj(GameObject obj)
        {
            string objName = obj.name;
            string prefName = objName.Substring(0, objName.IndexOf('-'));

            if (m_ObjPoolDict.TryGetValue(prefName, out ObjPool objPool))
            {
                objPool.RecycleObj(obj);
            }
            else
            {
                Debug.LogError("ObjPoolMgr.RecycleObj：不存在这个对象池，无法回收");
            }
        }
    }
}