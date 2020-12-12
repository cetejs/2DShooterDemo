using System.Collections;
using UnityEngine;
using Common;

namespace FX
{
    /// <summary>
    /// 特效显示器，可回收
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class FXDisplay : MonoBehaviour
    {
        [Header("设置决定特效触发的参数")]
        public string fxTriggerParame;
        [Header("持续多长时间就会被回收，当为零时表示不可回收")]
        public float duration = 1;

        private Animator m_Animator;
        private WaitForSeconds m_WaitForSecondsToRecovery;

        /// <summary>
        /// 展示特效
        /// </summary>
        public void ShowFX()
        {
            m_Animator.SetTrigger(fxTriggerParame);
        }

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_WaitForSecondsToRecovery = new WaitForSeconds(duration);
        }

        private void OnEnable()
        {
            if (duration == 0)
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(Recovery());
        }

        private IEnumerator Recovery()
        {
            yield return m_WaitForSecondsToRecovery;
            ObjPoolMgr.Instance.RecycleObj(gameObject);
        }
    }
}