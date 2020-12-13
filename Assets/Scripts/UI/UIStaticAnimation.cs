using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class UIStaticAnimation : MonoBehaviour
    {
        [Header("动画类型参数")]
        public string animTriggerParam;
        [Header("动画播放速度")]
        public float animSpeed = 1;

        private Animator m_Animator;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(animTriggerParam))
            {
                m_Animator.SetFloat("SpeedMultiplier", animSpeed);
                m_Animator.SetTrigger(animTriggerParam);
            }
            else
            {
                Debug.LogError("请检查动画参数是否为空");
            }
        }
    }
}