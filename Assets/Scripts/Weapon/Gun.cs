using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics;
using Common;

namespace Weapon
{
    public class Gun : MonoBehaviour, IGun
    {
        public string bulletPrefName;
        public Transform muzzle;

        [Header("随机角度")]
        public float randomAngleRange = 0.5f;
        private Vector3 m_RangeVec3;

        public void Fire()
        {
            Bullet bullet = ObjPoolMgr.Instance.SpawnObj<Bullet>(bulletPrefName);
            float angle = Random.Range(-randomAngleRange, randomAngleRange);
            Vector3 eulerAngles = new Vector3(0, transform.lossyScale.x > 0 ? 0 : 180, angle);
            eulerAngles.z += angle;
            bullet.transform.position = muzzle.position;
            bullet.transform.eulerAngles = eulerAngles;
        }
        private void Start()
        {
            m_RangeVec3 = new Vector3(0, randomAngleRange);
        }
    }
}