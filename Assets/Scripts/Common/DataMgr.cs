using UnityEngine;

namespace Common
{
    /// <summary>
    /// 数据管理者
    /// </summary>
    public class DataMgr : MonoSingleton<DataMgr>
    {
        #region 属性字段

        #region Layers

        public int GroundLayer { get; private set; }
        public int PlayerLayer { get; private set; }
        public int EnemyLayer { get; private set; }
        public int BulletLayer { get; private set; }

        #endregion

        #region Tags

        public readonly string PlayerTag = "Player";
        public readonly string EnemyTag = "Enemy";

        #endregion


        #region SortingOrder

        public readonly int DisableEnemySortingOrder = -2;
        public readonly int DisableShellSortingOrder = -1;

        #endregion


        public readonly float GameOverTimeScale = 0.2f;

        public readonly string ClickSound = "OnHighlightSound";

        #endregion

        #region 内部实现

        protected override void Awake()
        {
            GroundLayer = LayerMask.NameToLayer("Ground");
            PlayerLayer = LayerMask.NameToLayer("Player");
            EnemyLayer = LayerMask.NameToLayer("Enemy");
            BulletLayer = LayerMask.NameToLayer("Bullet");

            base.Awake();
        }

        #endregion
    }
}