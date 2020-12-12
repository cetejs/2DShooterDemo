using UnityEngine;
using Mechanics;
using Common;
using FX;

namespace Battle
{
    /// <summary>
    /// 可被损坏的类
    /// </summary>
    [RequireComponent(typeof(ICharacterController))]
    public class Damageable : MonoBehaviour, IDamageable
    {
        [Header("最大生命值")]
        public int maxHealth = 1;
        [Header("当前生命值")]       
        public int health = 1;
        [Header("爆炸特效预制名")]
        public string explosionPrefName = "pref_Explosion";

        private ICharacterController m_CharacterController;

        /// <summary>
        /// 获得损害
        /// </summary>
        /// <param name="damage"></param>
        public void GetDamage(int damage)
        {
            if (!m_CharacterController.IsControlEnabled())
            {
                return;
            }
            m_CharacterController.Hit();
            SubtractHealth(damage);

            if (health <= 0)
            {
                m_CharacterController.Die();

                //一半的几率会爆炸，只是简单的特效展示
                if (Random.Range(0, 100) > 50)
                {
                    FXDisplay explosionFXDisplay = ObjPoolMgr.Instance.SpawnObj<FXDisplay>(explosionPrefName);
                    explosionFXDisplay.transform.position = transform.position;
                    explosionFXDisplay.ShowFX();
                    GameMgr.Instance.shakeCamera.ExplosionShake();
                }
            }
        }

        private void Awake()
        {
            m_CharacterController = GetComponent<ICharacterController>();

            health = maxHealth;
        }

        private void AddHealth(int value)
        {
            health = Mathf.Min(health + value, maxHealth);
        }

        private void SubtractHealth(int value)
        {
            health = Mathf.Max(health - value, 0);
        }

        /// <summary>
        /// 受伤结束后的动画事件
        /// </summary>
        private void OnHittedEvent()
        {
            m_CharacterController.OnHitted();
        }
    }
}