using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// 枪的接口定义
    /// </summary>
    public interface IGun
    {
        /// <summary>
        /// 开火
        /// </summary>
        void Fire();
    }
}