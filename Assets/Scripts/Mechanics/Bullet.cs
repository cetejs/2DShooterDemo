using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Bullet : MonoBehaviour
    {
        public float launchSpeed = 1;

        private Rigidbody2D m_Rigidbody2D;

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            m_Rigidbody2D.velocity = transform.right * launchSpeed;
        }
    }
}
