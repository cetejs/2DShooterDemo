using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

namespace Mechanics
{
    /// <summary>
    /// 主角控制器
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(Collider2D))]
    public class PlayerController : KinematicObject
    {
        [Header("最大移动速度")]
        public float maxSpeed = 4;
        [Header("持续按下跳跃键向上的速度")]
        public float jumpTakeOffSpeed = 7;
        [Header("停止按下跳跃键后的减速度")]
        public float jumpDeceleration = 0.5f;
        [Header("角色控制器开关")]
        public bool controlEnabled = true;

        /// <summary>
        /// 碰撞器的边界
        /// </summary>
        public Bounds Bounds
        {
            get { return m_Collider2d.bounds; }
        }

        private Animator m_Animator;
        private Collider2D m_Collider2d;
        private IGun m_Gun;

        private bool m_IsJumped;
        private bool m_IsStopJump;
        private Vector2 m_Move;

        private JumpState m_JumpState = JumpState.Grounded;


        protected override void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Collider2d = GetComponent<Collider2D>();
            m_Gun = GetComponentInChildren<IGun>();

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
                else if (Input.GetButtonDown("Fire1"))
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

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}