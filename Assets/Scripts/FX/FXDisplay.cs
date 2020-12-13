using UnityEngine;
using Common;

namespace FX
{
    /// <summary>
    /// 特效显示器，可回收
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class FXDisplay : ObjRecycler
    {
        [Header("设置决定特效触发的参数")]
        public string fxTriggerParame;

        private Animator m_Animator;

        /// <summary>
        /// 展示特效
        /// </summary>
        public void ShowFX()
        {
            m_Animator.SetTrigger(fxTriggerParame);
            OnShowFx();
        }

        protected virtual void OnShowFx() { }

        protected override void Awake()
        {
            m_Animator = GetComponent<Animator>();

            base.Awake();
        }
    }
}