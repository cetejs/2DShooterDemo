using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camera
{
    /// <summary>
    /// 镜头跟随器
    /// </summary>
    public class CameraFollowTarget : MonoBehaviour
    {
        [Header("跟随的目标")]
        public Transform target;

        [Header("与跟随目标的偏移向量")]
        public Vector3 offest;

        [Header("跟随速度系数")]
        public float followModifier = 5f;

        /// <summary>
        /// 跟随目标是否面向右边
        /// </summary>
        public bool IsTargetFaceToRight
        {
            get 
            {
                return target.localScale.x > 0;
            }
        }

        private Vector3 m_RealOffset;

        private void Start()
        {
            offest.z = 0;
        }

        private void LateUpdate()
        {
            int sign = IsTargetFaceToRight ? 1 : -1;
            m_RealOffset = offest;
            m_RealOffset.x *= sign;
            Vector3 targetPosition = target.position + m_RealOffset;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followModifier * Time.deltaTime);
        }

    }
}