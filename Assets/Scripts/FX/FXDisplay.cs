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
        #region 属性字段

        [Header("设置决定特效触发的参数")]
        public string fxTriggerParame;

        private Animator m_Animator;

        #endregion

        #region 外部接口

        /// <summary>
        /// 展示特效
        /// </summary>
        public void ShowFX()
        {
            m_Animator.SetTrigger(fxTriggerParame);
            OnShowFx();
        }

        #endregion

        #region 内部实现

        protected virtual void OnShowFx() { }

        protected override void Awake()
        {
            m_Animator = GetComponent<Animator>();

            base.Awake();
        }

        #endregion
    }
}