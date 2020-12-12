using UnityEngine;

namespace Mechanics
{
    /// <summary>
    /// 敌人控制器
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
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

        private Animator m_Animator;

        private bool m_IsJumped;
        private Vector2 m_Move;
        private JumpState m_JumpState = JumpState.Grounded;

        private SpriteRenderer m_SpriteRenderer;

        /// <summary>
        /// 控制器是否是激活状态
        /// </summary>
        /// <returns></returns>
        public bool IsControlEnabled()
        {
            return controlEnabled;
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
        /// 角色受击后恢复行动
        /// </summary>
        public void OnHitted()
        {
            controlEnabled = true;
        }

        /// <summary>
        /// 角色死亡
        /// </summary>
        public void Die()
        {
            m_Animator.SetTrigger("Death");
            controlEnabled = false;
            m_Collider2d.enabled = false;
            enabled = false;
            m_SpriteRenderer.sortingOrder = -1;
        }

        protected override void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

            base.Awake();
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
            else if(m_Move.x != 0)
            {
                m_Move.x = 0;
            }

            UpdateJumpState();
            base.Update();
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