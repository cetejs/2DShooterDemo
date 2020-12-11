﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    /// <summary>
    /// 模拟实体的运动学，可物理运动对象的基类
    /// </summary>
    public class KinematicObject : MonoBehaviour
    {
        [Header("地面法线最小对比值，决定了是否位于地表的判断")]
        public float minGroundNormalY = 0.65f;

        [Header("重力系数")]
        public float gravityModifier = 1f;

        /// <summary>
        /// 是否位于地面
        /// </summary>
        public bool IsGrounded { get; private set; }

        /// <summary>
        /// 只有水平速度有意义
        /// </summary>
        protected Vector2 m_TargetVelocity;

        /// <summary>
        /// 只能修改垂直速度
        /// </summary>
        [Header("当前的速度")]
        [SerializeField]
        protected Vector2 m_Velocity;

        private float m_ShellRadius = 0.01f;
        private float m_MinMoveDistance = 0.001f;
        private Rigidbody2D m_Rigidbody2D;
        private ContactFilter2D m_ContactFilter2D;
        private RaycastHit2D[] m_HitBuffer = new RaycastHit2D[16];

        /// <summary>
        /// 传送到指定地点
        /// </summary>
        /// <param name="position"></param>
        public void Teleport(Vector3 position)
        {
            m_Rigidbody2D.position = position;
            m_Rigidbody2D.velocity *= 0;
            m_Velocity *= 0;
        }

        protected virtual void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            //初始化运动筛选器
            m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            m_ContactFilter2D.useTriggers = false;
            m_ContactFilter2D.useLayerMask = true;
            m_ContactFilter2D.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        }

        protected virtual void Update()
        {
            m_TargetVelocity *= 0;
            ComputeVelocity();
        }

        protected virtual void ComputeVelocity() { }

        protected virtual void FixedUpdate()
        {
            IsGrounded = false;

            m_Velocity.x = m_TargetVelocity.x;

            //每次叠加重力
            if (m_Velocity.y < 0)
            {
                m_Velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            }
            else
            {
                m_Velocity += Physics2D.gravity * Time.deltaTime;
            }

            //获得每帧需要移动的增量
            Vector2 deltaPosition = m_Velocity * Time.deltaTime;

            //先水平移动
            Vector2 move = Vector2.right * deltaPosition.x;
            PerformMovement(move, false);

            //再垂直移动
            move = Vector2.up * deltaPosition.y;
            PerformMovement(move, true);
        }

        private void PerformMovement(Vector2 move, bool isYMovement)
        {
            float distance = move.magnitude;

            if (distance > m_MinMoveDistance)
            {
                int count = m_Rigidbody2D.Cast(move, m_ContactFilter2D, m_HitBuffer, distance + m_ShellRadius);

                for (int i = 0; i < count; i++)
                {
                    Vector2 curNormal = m_HitBuffer[i].normal;

                    if (curNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;

                        //当垂直移动时，矫正地面法线向量
                        if (isYMovement)
                        {
                            curNormal.x = 0;
                        }
                    }

                    if (IsGrounded)
                    {
                        //如果相对于地面法线移动，速度会降低。
                        float projection = Vector2.Dot(m_Velocity, curNormal);
                        if (projection < 0)
                        {
                            m_Velocity -= projection * curNormal;
                        }
                    }
                    else
                    {
                        //空中碰到物体，取消垂直和水平向上的速度
                        m_Velocity.x *= 0;
                        m_Velocity.y = Mathf.Min(m_Velocity.y, 0);
                    }

                    //上面进行投射时加上了ShellRadius，需要减去获得实际距离
                    //如果碰撞的距离过小时，会获得负值，如果移动方向是向下的，会向上移动
                    float modifiedDistance = m_HitBuffer[i].distance - m_ShellRadius;
                    distance = Mathf.Min(modifiedDistance, distance);
                }
            }

            m_Rigidbody2D.position += move.normalized * distance;
        }
    }
}