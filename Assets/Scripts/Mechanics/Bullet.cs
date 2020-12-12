using UnityEngine;
using Common;
using FX;
using Battle;

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
        [Header("威力伤害值")]
        public int damage = 1;
        [Header("击退威力")]
        public int beatBackPower = 1;

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
            bool isHitFXShowed = false;

            //显示击中特效
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

            //敌人受伤并会被击退
            if (collision.transform.CompareTag(DataMgr.Instance.EnemyTag))
            {
                KinematicObject kObj = collision.transform.GetComponent<KinematicObject>();
                kObj.GetComponent<IDamageable>().GetDamage(damage);
                int sign = transform.right.x > 0 ? 1 : -1;

                if (!kObj.IsForwardWalled && !kObj.IsBackObstacled)
                {
                    kObj.transform.position += Vector3.right * beatBackPower * sign * Time.deltaTime;
                }
            }

            ObjPoolMgr.Instance.RecycleObj(gameObject);
        }
    }
}
