using UnityEngine;
using Mechanics;
using Camera;

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
    }
}