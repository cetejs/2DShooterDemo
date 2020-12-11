using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics;

namespace Common
{
    /// <summary>
    /// 游戏管理者
    /// </summary>
    public class GameMgr : MonoSingleton<GameMgr>
    {
        public PlayerController playerController;
    }
}