using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using FX;

namespace Weapon
{
    /// <summary>
    /// 可拥有多把枪械
    /// </summary>
    public class GunController : MonoBehaviour, IGun
    {
        [Header("枪口闪光特效")]
        public FXDisplay flashFXDisplay;
        [Header("射击频率")]
        public float shootRate = 1f;

        private float m_Timer;

        private Gun[] m_Guns;

        /// <summary>
        /// 开火
        /// </summary>
        public void Fire()
        {
            if (m_Timer > 0)
            {
                return;
            }

            m_Timer = 1 / shootRate;
            flashFXDisplay.ShowFX();
            GameMgr.Instance.shakeCamera.FireShake();

            foreach (Gun gun in m_Guns)
            {
                gun.Fire();
            }
        }

        private void Awake()
        {
            m_Guns = GetComponentsInChildren<Gun>();
        }

        private void Update()
        {
            if (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
            }
        }
    }
}