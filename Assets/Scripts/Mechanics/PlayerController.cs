using System.Collections;
using System.Collections.Generic;
using Battle;
using Common;
using UnityEngine;
using Weapon;

namespace Mechanics
{
    /// <summary>
    /// 玩家控制器
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(Damageable))]
    public class PlayerController : KinematicObject, ICharacterController
    {
        [Header("最大移动速度")]
        public float maxSpeed = 4;
        [Header("持续按下跳跃键向上的速度")]
        public float jumpTakeOffSpeed = 7;
        [Header("停止按下跳跃键后的减速度")]
        public float jumpDeceleration = 0.5f;
        [Header("玩家控制器开关")]
        public bool controlEnabled = true;
        [Header("玩家踩踏伤害")]
        public int trampleDamage = 1;
        [Header("玩家踩踏后向上的速度")]
        public int trampleSpeed = 5;
      
        private bool m_IsJumped;
        private bool m_IsStopJump;
        private Vector2 m_Move;
        private JumpState m_JumpState = JumpState.Grounded;

        private Animator m_Animator;
        private Damageable m_Damageable;
        private IGun m_Gun;

        private readonly int DeathSpeedX = 1;
        private readonly int DeathSpeedY = 5;

        /// <summary>
        /// 控制器是否是激活状态
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return controlEnabled;
        }

        /// <summary>
        /// 玩家受伤后可行动效果更好
        /// </summary>
        public void Hit()
        {
            m_Animator.SetTrigger("Hit");
            //controlEnabled = false;
        }

        /// <summary>
        /// 玩家受击后恢复行动，事件已经移除
        /// </summary>
        public void OnHitted()
        {
            Debug.LogError("你不应该在玩家受伤动画下加入事件");
            //controlEnabled = true;
        }

        /// <summary>
        /// 玩家死亡
        /// </summary>
        public void Die(int deathDir)
        {
            m_Move.x = deathDir * DeathSpeedX;
            Bounce(DeathSpeedY);
            m_Animator.SetBool("IsDeath", true);
            controlEnabled = false;
            Time.timeScale = DataMgr.Instance.GameOverTimeScale;
            //m_Collider2d.enabled = false;
        }

        /// <summary>
        /// 玩家复活
        /// </summary>
        /// <param name="point"></param>
        public void Revive(Transform point)
        {
            if (!controlEnabled && IsGrounded)
            {
                Teleport(point.position);
                transform.rotation = point.rotation;
                transform.localScale = point.localScale;

                m_Damageable.Revive();
                m_Animator.SetBool("IsDeath", false);
                controlEnabled = true;
            }
        }

        protected override void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Gun = GetComponentInChildren<IGun>();
            m_Damageable = GetComponent<Damageable>();

            base.Awake();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                m_Move.x = Input.GetAxis("Horizontal");

                if (m_JumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                {
                    m_JumpState = JumpState.PrepareToJump;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    m_IsStopJump = true;
                }
                else if (Input.GetButton("Fire1"))
                {
                    if (m_Gun != null)
                    {
                        m_Gun.Fire();
                    }
                    else
                    {
                        Debug.LogError("检查是否添加了枪械");
                    }
                }
            }
            else if (m_Move.x != 0)
            {
                m_Move.x = Mathf.Lerp(m_Move.x, 0, Time.deltaTime);
            }

            UpdateJumpState();
            base.Update();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (controlEnabled && collision.transform.CompareTag(DataMgr.Instance.EnemyTag))
            {
                KinematicObject enemy = collision.transform.GetComponent<KinematicObject>();
                Damageable enemyDamageable = enemy.GetComponent<Damageable>();

                if (Bounds.center.y >= enemy.Bounds.max.y)
                {
                    //踩踏伤害
                    enemyDamageable.GetDamage(m_Damageable);
                    Bounce(trampleSpeed);
                }
                else
                {
                    //玩家受伤
                    m_Damageable.GetDamage(enemyDamageable);
                }
            }
            else if (!controlEnabled && IsGrounded)
            {
                m_Move *= 0;
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// 处理实体的速度以及动画
        /// </summary>
        protected override void ComputeVelocity()
        {
            if (m_IsJumped && IsGrounded)
            {
                m_Velocity.y = jumpTakeOffSpeed;
                m_IsJumped = false;
            }
            else if (m_IsStopJump)
            {
                m_IsStopJump = false;

                if (m_Velocity.y > 0)
                {
                    m_Velocity.y *= jumpDeceleration;
                }
            }

            //死亡后会修改m_Move并处理了方向，这里不需要处理了
            if (controlEnabled)
            {
                //很多地方依赖于Scale控制玩家的翻转，不可随意修改
                Vector3 scale = transform.localScale;

                if (m_Move.x > 0.01f)
                {
                    scale.x = 1;
                }
                else if (m_Move.x < -0.01f)
                {
                    scale.x = -1;
                }

                transform.localScale = scale;
            }

            m_Animator.SetBool("IsGrounded", IsGrounded);
            m_Animator.SetFloat("VelocityX", Mathf.Abs(m_Velocity.x) / maxSpeed);
            m_Animator.SetFloat("VelocityY", m_Velocity.y);
            m_TargetVelocity = m_Move * maxSpeed;
        }

        private void UpdateJumpState()
        {
            m_IsJumped = false;

            switch (m_JumpState)
            {
                case JumpState.PrepareToJump:
                    {
                        m_IsJumped = true;
                        m_IsStopJump = false;
                        m_JumpState = JumpState.Jumping;
                    }
                    break;
                case JumpState.Jumping:
                    {
                        m_JumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    {
                        if (IsGrounded)
                        {
                            m_JumpState = JumpState.Landed;
                        }
                    }
                    break;
                case JumpState.Landed:
                    {
                        m_JumpState = JumpState.Grounded;
                    }
                    break;
            }
        }

        enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}