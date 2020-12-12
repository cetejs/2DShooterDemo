using UnityEngine;
using Mechanics;
using Common;
using FX;
using Extend;

namespace Weapon
{
    /// <summary>
    /// 可拥有多把枪械
    /// 由于动画原因暂且使用两把枪交替显示射击动画，闲置动画（被使用者接管）
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(AudioSource))]
    public class GunController : MonoBehaviour, IGun
    {
        [Header("枪口闪光特效")]
        public FXDisplay flashFXDisplay;
        [Header("射击频率")]
        public float shootRate = 1f;
        [Header("后坐力")]
        public float recoilForce = 1f;

        [Header("假枪，用于显示空枪动画")]
        public GameObject fakeGun;

        private float m_Timer;

        private Gun[] m_Guns;
        private KinematicObject m_User;
        private Animator m_Animator;
        private AudioSource m_AudioSource;

        /// <summary>
        /// 开火
        /// </summary>
        public void Fire()
        {
            ShowFireGun(true);

            if (m_Timer > 0)
            {
                return;
            }

            if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
            }

            m_AudioSource.Play();
            m_Animator.SetTrigger(m_User.IsGrounded ? "Fire" : "JumpFire");
            m_Timer = 1 / shootRate;
            flashFXDisplay.ShowFX();
            GameMgr.Instance.shakeCamera.FireShake();

            foreach (Gun gun in m_Guns)
            {
                gun.Fire();
            }

            //后坐力实现
            if (m_User.IsGrounded && !m_User.IsBackObstacled)
            {
                m_User.transform.position += Vector3.left * recoilForce * transform.lossyScale.x * Time.deltaTime;
            }
        }

        private void Awake()
        {
            m_Guns = GetComponentsInChildren<Gun>();
            m_User = GetComponentInParent<KinematicObject>();
            m_Animator = GetComponent<Animator>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 射击动画结束事件
        /// </summary>
        private void OnShootIsOverEvent()
        {
            ShowFireGun(false);
        }

        /// <summary>
        /// 显示火枪，同时隐藏假枪
        /// </summary>
        /// <param name="isShow"></param>
        private void ShowFireGun(bool isShow)
        {
            fakeGun.SetSafeActive(!isShow);
            gameObject.SetSafeActive(isShow);
        }
    }
}