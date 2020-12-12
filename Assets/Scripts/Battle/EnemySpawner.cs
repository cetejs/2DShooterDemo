using UnityEngine;
using Common;

namespace Battle
{
    /// <summary>
    /// 敌人孵化器
    /// 没有对敌人人数做限制，会一直孵化敌人
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Header("敌人的预制名称")]
        public string enemyPrefName = "pref_Enemy";
        [Header("孵化频率")]
        public float spawnRate = 1;
        [Header("敌人Transform的模版")]
        public Transform temp;

        private bool m_IsStoped = true;

        public void StartSpawn()
        {
            if (m_IsStoped)
            {
                m_IsStoped = false;
                InvokeRepeating("SpawnEnemy", 0, 1 / spawnRate);
            }
        }

        public void StopSpawn()
        {
            m_IsStoped = true;
            CancelInvoke("SpawnEnemy");
        }

        private void Start()
        {
            StartSpawn();
        }

        private void SpawnEnemy()
        {
            GameObject enemy = ObjPoolMgr.Instance.SpawnObj(enemyPrefName);
            enemy.transform.position = temp.position;
            enemy.transform.rotation = temp.rotation;
            enemy.transform.localScale = temp.localScale;
        }
    }
}