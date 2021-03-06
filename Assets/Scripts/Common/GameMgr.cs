﻿using System;
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
        #region 属性字段

        [Header("主角控制器")]
        public PlayerController playerController;
        [Header("相机震动器")]
        public ShakeCamera shakeCamera;
        [Header("主角复活点")]
        public Transform playerRevivePoint;
        [Header("敌人孵化器")]
        public EnemySpawner enemySpawner;

        /// <summary>
        /// 是否是菜单界面展示
        /// </summary>
        public bool IsMenuDisplay { get; private set; } = true;

        /// <summary>
        /// 游戏是否暂停，正在意义上当暂停
        /// </summary>
        public bool IsGamePaused { get; private set; }

        /// <summary>
        /// 游戏暂停事件，非正在意义上的暂停
        /// </summary>
        public event Action<bool> OnGamePause;

        /// <summary>
        /// 清理回收所有敌人的事件
        /// </summary>
        public event Action<Transform> OnNeedRecycleAllEnemies;

        /// <summary>
        /// 清理所有公共回收物的事件
        /// 如果传入的字符串为空，将会全部回收
        /// </summary>
        public event Action<string> OnNeedRecycleAllCommonObjs;

        private bool m_IsTransiting = false;

        #endregion

        #region 内部实现

        private void Start()
        {
            playerController.controlEnabled = false;
        }

        private void Update()
        {
            if (!IsMenuDisplay)
            {
                //复活
                if (Input.GetButtonDown("Revive"))
                {
                    playerController.Revive(playerRevivePoint);
                    SoundMgr.Instance.PlayClickSound();
                }

                //清理尸体、弹壳
                if (Input.GetButtonDown("Clean Scene"))
                {
                    if (OnNeedRecycleAllEnemies != null)
                    {
                        OnNeedRecycleAllEnemies(enemySpawner.temp);
                    }
                    if (OnNeedRecycleAllCommonObjs != null)
                    {
                        OnNeedRecycleAllCommonObjs("pref_BulletShell");
                    }

                    SoundMgr.Instance.PlayClickSound();
                }

                //呼出菜单，在死亡动画期间无法操作
                if (Input.GetButtonDown("Switch Menu"))
                {
                    if (playerController.IsFallToGround && !IsGamePaused)
                    {
                        TransitToMenu();
                    }

                    SoundMgr.Instance.PlayClickSound();
                }

                //暂停
                if (Input.GetButtonDown("Pause"))
                {
                    if (Time.timeScale == 0)
                    {
                        Time.timeScale = 1;
                        IsGamePaused = false;
                    }
                    else if (Time.timeScale == 1)
                    {
                        Time.timeScale = 0;
                        IsGamePaused = true;
                    }

                    SoundMgr.Instance.PlayClickSound();
                }
            }
            else
            {
                //切入战斗，在死亡动画期间无法操作
                if (Input.GetButtonDown("Switch Menu"))
                {
                    TransitToBattle();
                    SoundMgr.Instance.PlayClickSound();
                }
            }
        }

        private void TransitToMenu()
        {
            if (!m_IsTransiting)
            {
                UIMgr.Instance.TransitToMenu(() =>
                {
                    m_IsTransiting = false;
                });
                m_IsTransiting = true;
                IsMenuDisplay = true;
                OnGamePause(true);
            }
        }

        private void TransitToBattle()
        {
            if (!m_IsTransiting)
            {
                UIMgr.Instance.TransitToBattle(() =>
                {
                    m_IsTransiting = false;
                    IsMenuDisplay = false;
                    OnGamePause(false);
                });
                m_IsTransiting = true;
            }
        }

        #endregion
    }
}