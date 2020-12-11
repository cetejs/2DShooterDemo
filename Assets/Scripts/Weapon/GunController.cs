using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class GunController : MonoBehaviour, IGun
    {
        private Gun[] m_Guns;

        public void Fire()
        {
            foreach (Gun gun in m_Guns)
            {
                gun.Fire();
            }
        }

        private void Awake()
        {
            m_Guns = GetComponentsInChildren<Gun>();
        }
    }
}