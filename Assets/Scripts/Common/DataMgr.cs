using UnityEngine;

namespace Common
{
    /// <summary>
    /// 数据管理者
    /// </summary>
    public class DataMgr : MonoSingleton<DataMgr>
    {
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

        protected override void Awake()
        {
            GroundLayer = LayerMask.NameToLayer("Ground");
            PlayerLayer = LayerMask.NameToLayer("Player");
            EnemyLayer = LayerMask.NameToLayer("Enemy");
            BulletLayer = LayerMask.NameToLayer("Bullet");

            base.Awake();
        }
    }
}