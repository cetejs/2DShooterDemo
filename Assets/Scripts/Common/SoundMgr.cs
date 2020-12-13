using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 音效管理者
    /// </summary>
    public class SoundMgr : MonoSingleton<SoundMgr>
    {
        //PS：可与对象池配合使用生成3D音效的预制，但是Demo中为使用这样的功能，暂不实现

        #region 属性字段

        [Header("2D音效组")]
        public AudioClip[] audioClip2D;
        [Header("2D音效组件")]
        public AudioSource audioSource2D;

        private readonly Dictionary<string, AudioClip> m_2DAudioClipDict = new Dictionary<string, AudioClip>();

        #endregion

        #region 外部接口

        public void PlayClickSound()
        {
            Play2DSource(DataMgr.Instance.ClickSound);
        }

        public void Play2DSource(string clipName)
        {
            if (m_2DAudioClipDict.TryGetValue(clipName, out AudioClip clip))
            {
                audioSource2D.PlayOneShot(clip);
            }
            else
            {
                Debug.LogErrorFormat("不存在这个音效：{0}", clipName);
            }
        }

        #endregion

        #region 内部实现

        protected override void Awake()
        {
            foreach (AudioClip clip in audioClip2D)
            {
                string clipName = clip.name;

                if (!m_2DAudioClipDict.ContainsKey(clipName))
                {
                    m_2DAudioClipDict.Add(clipName, clip);
                }
                else
                {
                    Debug.LogErrorFormat("请检查2D音效是否重名：{0}", clipName);
                }
            }

            base.Awake();
        }

        #endregion
    }
}