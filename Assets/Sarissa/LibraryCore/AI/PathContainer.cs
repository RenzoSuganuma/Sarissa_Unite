﻿// 管理者 菅沼
using System;
using System.Linq;
using UnityEngine;

namespace Sarissa
{
    /// <summary> 道筋の座標情報を格納している </summary>
    public class PathContainer : MonoBehaviour
    {
        [SerializeField, Header("Path Color")] Color _color = Color.yellow;

        [SerializeField, Header("Marker Color")]
        Color _markerColor = Color.cyan;

        [SerializeField, Header("Point Marker Size")]
        float _markerSize = 1.0f;

        /// <summary> AIのパトロールする道筋の各分岐点のトランスフォームを返す </summary>
        public Vector3[] Path
        {
            get
            {
                var temp = Array.ConvertAll(transform.GetComponentsInChildren<Transform>(), x => x.position)
                    .ToList();
                temp.RemoveAt(0);
                return temp.ToArray();
            }
        }

        private void OnDrawGizmos()
        {
            var points = transform.GetComponentsInChildren<Transform>();
            var work = points.ToList();
            work.RemoveAt(0);
            points = work.ToArray();
            Gizmos.color = _markerColor;
            foreach (var p in points)
            {
                Gizmos.DrawCube(p.position, Vector3.one * _markerSize);
            } // draw sphere to each point's position

            Gizmos.color = _color;
            Gizmos.DrawLineStrip(Array.ConvertAll(points, x => x.position), true);
        }
    }
}
