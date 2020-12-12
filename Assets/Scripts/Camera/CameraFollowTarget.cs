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

        private Vector3 m_RealOffset;

        private void Start()
        {
            offest.z = 0;
        }

        private void LateUpdate()
        {
            m_RealOffset = offest;
            m_RealOffset.x *= target.localScale.x;
            Vector3 targetPosition = target.position + m_RealOffset;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followModifier * Time.deltaTime);
        }
    }
}