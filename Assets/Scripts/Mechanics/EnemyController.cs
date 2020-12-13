using Battle;
using Common;
using UnityEngine;

namespace Mechanics
{
    /// <summary>
    /// 敌人控制器
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Damageable))]
    public class EnemyController : KinematicObject, ICharacterController
    {
        [Header("最大移动速度")]
        public float maxSpeed = 4;
        [Header("持续跳跃向上的速度")]
        public float jumpSpeed = 7;
        [Header("转向速度")]
        public float turnSpeed = 10;
        [Header("敌人控制器开关")]
        public bool controlEnabled = true;
        [Header("敌人跳跃开关")]
        public bool jumpEnabled = false;

        private int m_SortingOrder;
        private bool m_IsAlive = true;
        private bool m_IsJumped;
        private Vector2 m_Move;
        private JumpState m_JumpState = JumpState.Grounded;

        private Animator m_Animator;
        private Damageable m_Damageable;
        private SpriteRenderer m_SpriteRenderer;

        private readonly int DeathSpeedX = 1;
        private readonly int DeathSpeedY = 5;
        private readonly int RealDeathSec = 3;

        /// <summary>
        /// 控制器是否活着
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return m_IsAlive;
        }

        /// <summary>
        /// 角色受击无法行动
        /// </summary>
        public void Hit()
        {
            m_Animator.SetTrigger("Hit");
            controlEnabled = false;
        }

        /// <summary>
        /// 角色受击后恢复行动，通过动画事件调用
        /// </summary>
        private void OnHittedEvent()
        {
            if (m_IsAlive && !GameMgr.Instance.IsMenuDisplay)
            {
                controlEnabled = true;
            }
        }

        /// <summary>
        /// 角色死亡
        /// </summary>
        public void Die(int deathDir)
        {
            m_Move.x = deathDir * DeathSpeedX;
            Bounce(DeathSpeedY);
            m_Animator.SetBool("IsDeath", true);
            controlEnabled = m_IsAlive = false;
            m_SpriteRenderer.sortingOrder = DataMgr.Instance.DisableEnemySortingOrder;
            m_Collider2d.isTrigger = true;

            //容错处理卡住往上升的情况
            CancelInvoke();
            Invoke("RealDeath", RealDeathSec);
        }

        /// <summary>
        /// 敌人复活，注意复活后会被直接回收
        /// </summary>
        /// <param name="point">复活点</param>
        public void Revive(Transform point)
        {
            if (!m_IsAlive && IsGrounded)
            {
                Teleport(point.position);
                transform.rotation = point.rotation;
                transform.localScale = point.localScale;

                m_Damageable.Revive();
                m_Rigidbody2D.WakeUp();
                m_SpriteRenderer.sortingOrder = m_SortingOrder;
                m_Animator.SetBool("IsDeath", false);
                m_Collider2d.isTrigger = false;
                m_Collider2d.enabled = true;
                controlEnabled = true;
                m_IsAlive = true;
                enabled = true;

                ObjPoolMgr.Instance.RecycleObj(gameObject);
            }
        }

        protected override void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Damageable = GetComponent<Damageable>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_SortingOrder = m_SpriteRenderer.sortingOrder;

            GameMgr.Instance.OnGamePause += OnGamePause;
            GameMgr.Instance.OnNeedRecycleAllEnemies += Revive;

            base.Awake();
        }

        private void OnDestroy()
        {
            GameMgr.Instance.OnGamePause -= OnGamePause;
            GameMgr.Instance.OnNeedRecycleAllEnemies -= Revive;
        }

        private void OnGamePause(bool paused)
        {
            controlEnabled = !paused;
            m_Move *= 0;
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                SimulateControlMove();

                if (jumpEnabled)
                {
                    SimulateControlJump();
                }
            }
            else if (m_Move.x != 0)
            {
                m_Move.x = Mathf.Lerp(m_Move.x, 0, Time.deltaTime);
            }

            UpdateJumpState();
            base.Update();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //死亡后落地处理真正的死亡
            if (!m_IsAlive && IsGrounded)
            {
                RealDeath();
            }
        }

        private void RealDeath()
        {
            CancelInvoke();
            m_Move *= 0;
            m_Rigidbody2D.Sleep();
            m_Collider2d.enabled = false;
            enabled = false;
        }

        /// <summary>
        /// 处理实体的速度以及动画
        /// </summary>
        protected override void ComputeVelocity()
        {
            if (m_IsJumped && IsGrounded)
            {
                m_Velocity.y = jumpSpeed;
                m_IsJumped = false;
            }

            m_Animator.SetBool("IsGrounded", IsGrounded);
            m_Animator.SetFloat("VelocityX", Mathf.Abs(m_Velocity.x) / maxSpeed);
            m_Animator.SetFloat("VelocityY", m_Velocity.y);
            m_TargetVelocity = m_Move * maxSpeed;
        }

        /// <summary>
        /// 模拟Horizontal控制移动
        /// </summary>
        private void SimulateControlMove()
        {
            if (IsForwardWalled)
            {
                Vector3 scale = transform.localScale;
                scale.x = -scale.x;
                transform.localScale = scale;
            }

            m_Move.x = Mathf.Lerp(m_Move.x, 1 * transform.localScale.x, turnSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 模拟Jump控制跳跃
        /// 当前方没有路时，模拟跳跃
        /// </summary>
        private void SimulateControlJump()
        {
            if (m_JumpState == JumpState.Grounded && !IsForwardGrounded)
            {
                m_JumpState = JumpState.PrepareToJump;
            }
        }

        private void UpdateJumpState()
        {
            m_IsJumped = false;

            switch (m_JumpState)
            {
                case JumpState.PrepareToJump:
                    {
                        m_IsJumped = true;
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