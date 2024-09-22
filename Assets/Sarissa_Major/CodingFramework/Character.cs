using UnityEngine;
using Sarissa;
using System;

namespace Sarissa.CodingFramework
{
    /// <summary>
    /// 体力をもち、殺すことができるキャラクターのクラスの基底クラス
    /// </summary>
    public abstract class Character : EnhancedMonoBehaviour
    {
        protected float _healthPoint = 0;

        /// <summary>
        /// ダメージを与える
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        public void AddDamage(float damage)
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
        /// <returns>体力値</returns>
        public virtual float GetHealthPoint()
        {
            return _healthPoint;
        }

        /// <summary>
        /// 渡された値を体力の値として体力値を初期化する
        /// </summary>
        /// <param name="value">新しく初期化する値</param>
        public virtual void SetHealthPoint(float value)
        {
            _healthPoint = value;
        }

        /// <summary>
        /// 死亡時の挙動を実装してください
        /// </summary>
        protected abstract void BehaviourWhenDeath();
    }
}