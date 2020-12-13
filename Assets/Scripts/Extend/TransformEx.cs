using UnityEngine;

namespace Extend
{
    /// <summary>
    /// 转换组件的扩展类
    /// </summary>
    public static class TransformEx
    {
        /// <summary>
        /// 设置为目标转换的属性
        /// </summary>
        /// <param name="trs"></param>
        /// <param name="target"></param>
        public static void SetTransform(this Transform trs, Transform target)
        {
            trs.position = target.position;
            trs.rotation = target.rotation;
            trs.localScale = target.localScale;
        }

        /// <summary>
        /// 重置转换属性
        /// </summary>
        /// <param name="trs"></param>
        public static void ResetTransform(this Transform trs)
        {
            trs.position = Vector3.zero;
            trs.rotation = Quaternion.identity;
            trs.localScale = Vector3.one;
        }
    }
}