using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using FX;

namespace Mechanics
{
    /// <summary>
    /// 子弹会向自身前行方法运动，所有使用时需要旋转
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Bullet : MonoBehaviour
    {
        [Header("发射速度")]
        public float launchSpeed = 1;
        [Header("击中特效预制名")]
        public string hitPrefName = "pref_Hit";

        private Rigidbody2D m_Rigidbody2D;

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            m_Rigidbody2D.velocity = transform.right * launchSpeed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool isHitFXShowed= false;

            foreach (ContactPoint2D point2D in collision.contacts)
            {
                if (isHitFXShowed)
                {
                    break;
                }

                FXDisplay hitFXDisplay = ObjPoolMgr.Instance.SpawnObj<FXDisplay>("pref_Hit");
                hitFXDisplay.transform.position = new Vector3(point2D.point.x, point2D.point.y, 0);
                hitFXDisplay.ShowFX();
                isHitFXShowed = true;
            }
            ObjPoolMgr.Instance.RecycleObj(gameObject);
        }
    }
}
