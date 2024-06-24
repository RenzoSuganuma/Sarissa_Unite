using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sarissa
{
    public static class GameObjectExtentions
    {
        /// <summary>
        /// 子オブジェクト親オブジェクトかそのオブジェクトに望むコンポーネントがあればそれを返す
        /// </summary>
        public static T GetComponentFromSibiling<T>(this GameObject target) where T : Component
        {
            if (target.GetComponent<T>() != null)
            {
                return target.GetComponent<T>();
            }
            else if (target.GetComponentInChildren<T>() != null)
            {
                return target.GetComponentInChildren<T>();
            }
            else if (target.GetComponentInParent<T>() != null)
            {
                return target.GetComponentInParent<T>();
            }
            else
            {
                return null;
            }
        }
    }
}