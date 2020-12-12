using System;
using UnityEngine;
using Mechanics;
using Camera;
using Battle;

namespace Common
{
    /// <summary>
    /// 游戏管理者
    /// </summary>
    public class GameMgr : MonoSingleton<GameMgr>
    {
        [Header("主角控制器")]
        public PlayerController playerController;
        [Header("相机震动器")]
        public ShakeCamera shakeCamera;
        [Header("主角复活点")]
        public Transform playerRevivePoint;
        [Header("敌人孵化器")]
        public EnemySpawner enemySpawner;

        /// <summary>
        /// 清理回收所有敌人的事件
        /// </summary>
        public event Action<Transform> OnNeedRecycleAllEnemies;

        private void Update()
        {
            //复活
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerController.Revive(playerRevivePoint);
            }

            //清理尸体
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnNeedRecycleAllEnemies(enemySpawner.temp);
            }

            //暂停
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                else if (Time.timeScale == 1)
                {
                    Time.timeScale = 0;
                }
            }
        }
    }
}