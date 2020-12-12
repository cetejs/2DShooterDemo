using UnityEngine;

namespace FX
{
    /// <summary>
    /// 附加音效的特效展示器
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioFXDisplay : FXDisplay
    {
        private AudioSource m_AudioSource;

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
    }
}