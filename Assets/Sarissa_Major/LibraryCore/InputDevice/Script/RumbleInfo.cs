using UnityEngine;
using System;

namespace Sarissa.Devicies
{
    [Serializable]
    public class RumbleInfo<T, T1, T2>
    {
        [SerializeField] public T LeftStrength;
        [SerializeField] public T1 RightStrength;
        [SerializeField] public T2 Time;

        public RumbleInfo(T leftStrength, T1 rightStrength, T2 rumbleTime)
        {
            LeftStrength = leftStrength;
            rightStrength = rightStrength;
            Time = rumbleTime;
        }
    }
}