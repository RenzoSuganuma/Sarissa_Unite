using UnityEngine;
using Sarissa;
using System;

namespace Sarissa.CodingFramework
{
    public abstract class Character : CodingFramework
    {
        protected Single _healthPoint;

        /// <summary>
        /// ダメージを与える
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(Single damage)
        {
            _healthPoint -= damage;

            if (_healthPoint <= 0)
            {
                BehaviourWhenDeath();
            }
        }

        /// <summary>
        /// 体力を０にする
        /// </summary>
        public void Kill()
        {
            _healthPoint = 0;
            BehaviourWhenDeath();
        }

        /// <summary>
        /// 体力を返す
        /// </summary>
        /// <returns></returns>
        public virtual Single GetHealthPoint()
        {
            return _healthPoint;
        }

        /// <summary>
        /// 渡された値を体力の値として体力値を初期化する
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetHealthPoint(Single value)
        {
            _healthPoint = value;
        }

        /// <summary>
        /// 死亡時の挙動を実装してください
        /// </summary>
        protected abstract void BehaviourWhenDeath();
    }
}