using UnityEngine;
using System;

namespace Sarissa.Devicies
{
    [Serializable]
    public class RumbleInfo<T, T1, T2>
    {
        [SerializeField] T _lowFreqL;
        public T LeftStrength { get { return _lowFreqL; } }
        [SerializeField] T1 _highFreqR;
        public T1 RightStrength { get { return _highFreqR; } }
        [SerializeField] T2 _rumblingTime;
        public T2 Time { get { return _rumblingTime; } }

        public RumbleInfo(T leftStrength, T1 rightStrength, T2 rumbleTime)
        {
            _lowFreqL = leftStrength;
            _highFreqR = rightStrength;
            _rumblingTime = rumbleTime;
        }
    }
}