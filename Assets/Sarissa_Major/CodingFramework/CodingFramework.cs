using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Sarissa.CodingFramework
{
    public class CodingFramework : MonoBehaviour
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

        /// <summary>
        /// アクティブでないコンポーネントも検索対象になる。
        /// 指定のコンポーネントを返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindComponent<T>() where T : Component
        {
            return GameObject.FindAnyObjectByType<T>(FindObjectsInactive.Include);
        }

        /// <summary>
        /// アクティブでないコンポーネントも検索対象になる。
        /// 指定のコンポーネントを配列にして返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] FindComponents<T>() where T : Component
        {
            return GameObject.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }
    }
}
