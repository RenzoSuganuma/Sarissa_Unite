using System;
using Sarissa.Singleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

// 作成 すがぬま
namespace Sarissa
{
    namespace Systems
    {
        public class ApplicationQuitter : SingletonBaseClass<ApplicationQuitter>
        {
            [SerializeField, Header("Player Tag")] string _playerTag;

            event Action<GameInfo.SceneTransitStatus> _OnTransit;

            public event Action<GameInfo.SceneTransitStatus>
                EventOnQuitApp
                {
                    add { _OnTransit += value; }
                    remove { _OnTransit -= value; }
                }

            Transform _player;
            GameInfo _gameInfo;

            protected override void ToDoAtAwakeSingleton()
            {
                _gameInfo = GameObject.FindFirstObjectByType<GameInfo>();
            }

            /// <summary> アプリケーションを閉じる </summary>
            public void QuitApplication()
            {
                #region TaskOnEditor

#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif

                #endregion

                _OnTransit(_gameInfo.GetTransitStatus);

                Application.Quit();
            }
        }
    }
}