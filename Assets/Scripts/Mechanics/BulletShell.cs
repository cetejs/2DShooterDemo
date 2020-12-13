using UnityEngine;
using Common;

namespace Mechanics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletShell : MonoBehaviour
    {
        #region 属性字段

        private Rigidbody2D m_Rigidbody2D;
        private SpriteRenderer m_SpriteRenderer;

        private int m_SortingOrder;

        #endregion

        #region 外部接口

        /// <summary>
        /// 弹壳弹出
        /// </summary>
        public void Eject(Vector2 power)
        {
            m_Rigidbody2D.AddForce(power);
        }

        #endregion

        #region 内部实现

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

            m_SortingOrder = m_SpriteRenderer.sortingOrder;
        }

        private void OnEnable()
        {
            m_SpriteRenderer.sortingOrder = m_SortingOrder;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            m_SpriteRenderer.sortingOrder = DataMgr.Instance.DisableShellSortingOrder;
        }

        #endregion
    }
}