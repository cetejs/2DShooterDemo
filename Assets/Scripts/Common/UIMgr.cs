using System;
using UnityEngine;
using UI;

namespace Common
{
    public class UIMgr : MonoSingleton<UIMgr>
    {
        [Header("转场UI动画")]
        public UIAnimation transitionAnimation;

        /// <summary>
        /// 转换到战斗界面
        /// </summary>
        /// <param name="onFinish"></param>
        public void TransitToBattle(Action onFinish = null)
        {
            transitionAnimation.SetPlayAnimationFinish(onFinish);
            transitionAnimation.PlayForward();
        }

        /// <summary>
        /// 转换到菜单界面
        /// </summary>
        /// <param name="onFinish"></param>
        public void TransitToMenu(Action onFinish = null)
        {
            transitionAnimation.SetPlayAnimationFinish(onFinish);
            transitionAnimation.PlayBackward();
        }
    }
}