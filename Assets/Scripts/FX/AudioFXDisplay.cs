using UnityEngine;

namespace FX
{
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