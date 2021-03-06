﻿using UnityEngine;
using Common;
using Extend;

namespace Battle
{
    /// <summary>
    /// 敌人孵化器
    /// 没有对敌人人数做限制，会一直孵化敌人
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        #region 属性字段

        [Header("敌人的预制名称")]
        public string enemyPrefName = "pref_Enemy";
        [Header("孵化频率")]
        public float spawnRate = 1;
        [Header("敌人Transform的模版")]
        public Transform temp;

        private bool m_IsStoped = true;

        #endregion

        #region 外部接口

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

        #endregion

        #region 内部实现

        private void Awake()
        {
            GameMgr.Instance.OnGamePause += OnGamePause;
        }

        private void OnDestroy()
        {
            GameMgr.Instance.OnGamePause -= OnGamePause;
        }

        private void OnGamePause(bool paused)
        {
            if (paused)
            {
                StopSpawn();
            }
            else
            {
                StartSpawn();
            }
        }

        private void SpawnEnemy()
        {
            GameObject enemy = ObjPoolMgr.Instance.SpawnObj(enemyPrefName);
            enemy.transform.SetTransform(temp);
        }

        #endregion
    }
}