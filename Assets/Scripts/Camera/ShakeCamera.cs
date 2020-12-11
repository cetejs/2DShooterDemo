using UnityEngine;

namespace Camera
{
    /// <summary>
    /// 相机震动器
    /// </summary>
    public class ShakeCamera : MonoBehaviour
    {
        private float m_ShakeStrength = 1f;
        private float m_ShakeDuration = 1f;
        private bool m_IsShaked = false;
        private Vector3 m_DeltaPositon;

        private const int ShakeAdjustValue = 100;

        /// <summary>
        /// 震动相机
        /// </summary>
        /// <param name="strength">震动强度</param>
        /// <param name="duration">震动持续时间</param>
        public void Shake(float strength, float duration)
        {
            m_ShakeStrength = strength;
            m_ShakeDuration = duration;
            m_IsShaked = true;
        }

        /// <summary>
        /// 枪械开火震动
        /// </summary>
        public void FireShake()
        {
            Shake(2, 0.2f);
        }

        /// <summary>
        /// 爆炸震动
        /// </summary>
        public void ExplosionShake()
        {
            Shake(3, 0.3f);
        }

        private void Update()
        {
            if (m_IsShaked)
            {
                //震动计时
                m_ShakeDuration -= Time.deltaTime;
                if (m_ShakeDuration < 0)
                {
                    m_IsShaked = false;
                }

                transform.localPosition -= m_DeltaPositon;
                m_DeltaPositon = Random.insideUnitSphere / ShakeAdjustValue * m_ShakeStrength;
                transform.localPosition += m_DeltaPositon;
            }
        }
    }
}