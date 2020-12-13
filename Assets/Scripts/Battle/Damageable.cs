using UnityEngine;
using Mechanics;
using Common;
using FX;

namespace Battle
{
    /// <summary>
    /// 可被损坏的类，简单实现，主角和敌人公用一个类
    /// </summary>
    [RequireComponent(typeof(ICharacterController))]
    public class Damageable : MonoBehaviour
    {
        [Header("最大生命值")]
        public int maxHealth = 1;
        [Header("当前生命值")]
        public int health = 1;
        [Header("攻击力")]
        public int attackPower = 1;
        [Header("爆炸特效预制名")]
        public string explosionPrefName = "pref_Explosion";

        private ICharacterController m_CharacterController;

        /// <summary>
        /// 获得损害
        /// </summary>
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public bool GetDamage(int damage, int deathDir)
        {
            if (!m_CharacterController.IsAlive())
            {
                return false;
            }

            m_CharacterController.Hit();
            SubtractHealth(damage);

            if (health <= 0)
            {
                m_CharacterController.Die(deathDir);

                //一半的几率会爆炸，只是简单的特效展示
                if (Random.Range(0, 100) > 50)
                {
                    FXDisplay explosionFXDisplay = ObjPoolMgr.Instance.SpawnObj<FXDisplay>(explosionPrefName);
                    explosionFXDisplay.transform.position = transform.position;
                    explosionFXDisplay.ShowFX();
                    GameMgr.Instance.shakeCamera.ExplosionShake();
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得伤害，传入一个Damageable
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public bool GetDamage(Damageable attacker)
        {
            int deathDir = (int)Mathf.Sign(transform.position.x - attacker.transform.position.x);

            return GetDamage(attacker.attackPower, deathDir);
        }

        /// <summary>
        /// 复活，恢复最大血量
        /// </summary>
        public void Revive()
        {
            health = maxHealth;
        }

        private void Awake()
        {
            m_CharacterController = GetComponent<ICharacterController>();

            Revive();
        }

        private void AddHealth(int value)
        {
            health = Mathf.Min(health + value, maxHealth);
        }

        private void SubtractHealth(int value)
        {
            health = Mathf.Max(health - value, 0);
        }
    }
}