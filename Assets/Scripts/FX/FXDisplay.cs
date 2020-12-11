using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FX
{
    /// <summary>
    /// 特效显示器
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class FXDisplay : MonoBehaviour
    {
        /// <summary>
        /// 设置决定特效触发的参数
        /// </summary>
        public string fxTriggerParame;

        private Animator m_Animator;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        //private void OnEnable()
        //{
        //    ShowFX();
        //}

        /// <summary>
        /// 展示特效
        /// </summary>
        private void ShowFX()
        {
            m_Animator.SetTrigger(fxTriggerParame);
        }
    }
}