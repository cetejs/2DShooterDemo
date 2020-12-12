using UnityEngine;
using Mechanics;
using Common;

namespace Weapon
{
    /// <summary>
    /// 一杆枪，一个种类的弹夹
    /// </summary>
    public class Gun : MonoBehaviour, IGun
    {
        [Header("弹夹中子弹的名称")]
        public string bulletPrefName = "pref_Bullet";
        [Header("枪口，发射点")]
        public Transform muzzle;
        [Header("随机角度")]
        public float randomAngleRange = 0.5f;

        /// <summary>
        /// 开火
        /// </summary>
        public void Fire()
        {
            Bullet bullet = ObjPoolMgr.Instance.SpawnObj<Bullet>(bulletPrefName);
            float angle = Random.Range(-randomAngleRange, randomAngleRange);
            Vector3 eulerAngles = new Vector3(0, transform.lossyScale.x > 0 ? 0 : 180, angle) + transform.localEulerAngles;
            bullet.transform.position = muzzle.position;
            bullet.transform.eulerAngles = eulerAngles;
        }
    }
}