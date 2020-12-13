using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// UI动画，Animation总需要时长1秒
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class UIAnimation : MonoBehaviour
    {
        #region 字段属性

        [Header("动画持续时间")]
        public float duration = 1;

        /// <summary>
        /// 动画结束回调
        /// </summary>
        public Action OnPlayAnimationFinish;

        private Animator m_Animator;

        #endregion

        #region 外部接口

        /// <summary>
        /// 动画结束回调
        /// </summary>
        /// <param name="onFinish">回调</param>
        public void SetPlayAnimationFinish(Action onFinish)
        {
            OnPlayAnimationFinish = onFinish;
        }

        /// <summary>
        /// 向前播放动画
        /// </summary>
        public void PlayForward()
        {
            PlayForward(duration);
        }

        /// <summary>
        /// 向后播放动画
        /// </summary>
        public void PlayBackward()
        {
            PlayBackward(duration);
        }

        /// <summary>
        /// 向前播放动画
        /// </summary>
        /// <param name="duration"持续时间></param>
        public void PlayForward(float duration)
        {
            if (duration == 0)
            {
                Debug.Log("请检查持续时间是否为0");
                return;
            }

            PlayAnimationFinish();
            m_Animator.SetFloat("SpeedMultiplier", 1 / duration);
            m_Animator.SetTrigger("PlayForward");
        }

        /// <summary>
        /// 向后播放动画
        /// </summary>
        /// <param name="duration"持续时间></param>
        public void PlayBackward(float duration)
        {
            if (duration == 0)
            {
                Debug.Log("请检查持续时间是否为0");
                return;
            }

            PlayAnimationFinish();
            m_Animator.SetFloat("SpeedMultiplier", 1 / duration);
            m_Animator.SetTrigger("PlayBackward");
        }

        #endregion

        #region 内部实现

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private void PlayAnimationFinish()
        {
            if (OnPlayAnimationFinish != null)
            {
                StopAllCoroutines();
                StartCoroutine(_PlayAnimationFinish());
            }
        }

        private IEnumerator _PlayAnimationFinish()
        {
            yield return new WaitForSeconds(duration);

            if (OnPlayAnimationFinish != null)
            {
                OnPlayAnimationFinish();
                OnPlayAnimationFinish = null;
            }
        }

        #endregion
    }
}