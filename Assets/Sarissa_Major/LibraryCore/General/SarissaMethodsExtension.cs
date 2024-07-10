// 管理者 菅沼

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Sarissa.CodingFramework
{
    /// <summary> 拡張メソッドを提供するクラス </summary>
    public static class SarissaMethodsExtension
    {
        /* GameObjects */
        /// <summary>指定されたトランスフォームの子オブジェクトにする</summary>
        public static void ToChildObject(this GameObject obj, Transform parent)
        {
            obj.transform.parent = parent;
        }

        /// <summary>オブジェクトの親子関係を切る</summary>
        public static void ToParenObject(this GameObject obj)
        {
            obj.transform.parent = null;
        }

        /// <summary>子オブジェクトのみ取得する</summary>
        public static List<Transform> GetChildObjects(this GameObject parent)
        {
            List<Transform> list = new();
            var cnt = parent.transform.childCount;
            for (int i = 0; i < cnt; i++)
            {
                var child = parent.transform.GetChild(i);
                list.Add(child);
            }

            return list;
        }
    }
}
