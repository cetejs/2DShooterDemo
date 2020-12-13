using UnityEngine;

namespace FX
{
    /// <summary>
    /// 附加音效的特效展示器
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioFXDisplay : FXDisplay
    {
        #region 属性字段

        private AudioSource m_AudioSource;

        #endregion

        #region 内部实现

        protected override void OnShowFx()
        {
            if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
            }

            m_AudioSource.Play();
        }

        protected override void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();

            base.Awake();
        }

        #endregion
    }
}