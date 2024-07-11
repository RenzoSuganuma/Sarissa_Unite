using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

// 作成 ： 菅沼
namespace Sarissa.Devicies
{
    /// <summary> ゲームパッドの振動をさせる機能を提供する </summary>
    public class GamepadRumbler : MonoBehaviour
    {
        Gamepad _gamepad;

        /// <summary> 振動プリセット 左から 左の振動 右の振動 振動の時間 </summary>
        [SerializeField] GamepadRamblePreset _rumblePreset;

        private void Start()
        {
            _gamepad = Gamepad.current;
        }

        public void Rumble(float leftSpeed, float rightSpeed)
        {
            _gamepad.SetMotorSpeeds(leftSpeed, rightSpeed);
        }

        public IEnumerator RumbleRoutine(float leftSpeed, float rightSpeed, float rumbleTime)
        {
            _gamepad.SetMotorSpeeds(leftSpeed, rightSpeed);

            yield return new WaitForSeconds(rumbleTime);

            _gamepad.SetMotorSpeeds(0f, 0f);
        }

        public IEnumerator RumbleRoutine(float leftSpeed, float rightSpeed, float rumbleTime, int repeatTimes)
        {
            for (int i = 0; i < repeatTimes; i++)
            {
                _gamepad.SetMotorSpeeds(leftSpeed, rightSpeed);

                yield return new WaitForSeconds(rumbleTime);

                _gamepad.SetMotorSpeeds(0f, 0f);
            }
        }

        public IEnumerator RumbleByTable()
        {
            foreach (var table in _rumblePreset.Rumbles)
            {
                _gamepad.SetMotorSpeeds(table.LeftStrength, table.RightStrength);

                yield return new WaitForSeconds(table.Time);

                _gamepad.SetMotorSpeeds(0f, 0f);
            }
        }
    }
}