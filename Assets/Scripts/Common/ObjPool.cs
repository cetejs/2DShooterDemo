using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 对象池，只能生产/回收单个对象，应该未存储抛出去的对象
    /// </summary>
    public class ObjPool
    {
        private readonly GameObject m_Prefab;
        private readonly Queue<GameObject> m_ObjQueue = new Queue<GameObject>();
        private readonly string m_PrefName;
        private int m_Count;

        public ObjPool(GameObject prefab)
        {
            m_Prefab = prefab;
            m_PrefName = prefab.name;
        }

        /// <summary>
        /// 生产一个对象
        /// </summary>
        /// <returns></returns>
        public GameObject SpawnObj()
        {
            GameObject obj;

            if (m_ObjQueue.Count <= 0)
            {
                obj = Object.Instantiate(m_Prefab, ObjPoolMgr.Instance.transform);
                obj.name = string.Format("{0}-{1}", m_PrefName, ++m_Count);
            }
            else
            {
                obj = m_ObjQueue.Dequeue();
                obj.SetActive(true);
            }

            return obj;
        }

        /// <summary>
        /// 回收一个对象
        /// </summary>
        /// <returns></returns>
        public void RecycleObj(GameObject obj)
        {
            m_ObjQueue.Enqueue(obj);
            obj.SetActive(false);
        }
    }
}