using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sarissa
{
    /// <summary> MonoBehaviour拡張クラス </summary>
    public class MonoBehaviourExtended : MonoBehaviour
    {
        /// <summary>
        /// 型パラメータに対応するクラスを継承するコンポーネントを返す
        /// </summary>
        public List<T> GetDerivedComponents<T>(GameObject obj)
        {
            var list = GameObject.FindObjectsOfType<GameObject>()
                .Where(_ => _.GetComponent<T>() != null)
                .Select(_ => _.GetComponent<T>()).ToList();
            return list;
        }
    }
}