using System.Collections;
using System.Collections.Generic;
using Sarissa.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

// auth 菅沼

namespace Sarissa
{
    namespace Systems
    {
        /// <summary> list 登録されたHUDを管理する </summary>
        public class HUDManager : SingletonBaseClass<HUDManager> // list 最後尾が一番後ろ
        {
            [SerializeField, Header("The Object All HUD's Parent")]
            GameObject _parent;

            [SerializeField, Header("The Preset Of HUD")]
            HUDPreset _hudPreset;

            SceneLoader _loader;

            /// <summary> 最前面へ移動 </summary>
            public void ToFront(int index)
            {
                var huds = _parent.GetChildObjects();
                for (int i = 0; i < _parent.transform.childCount; i++)
                {
                    huds[i].gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                }

                _hudPreset.HUDList[index].transform.SetAsLastSibling();
                _hudPreset.HUDList[index].gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            }

            /// <summary> 最前面へ移動 </summary>
            public void ToFront(string hudObjectName)
            {
                var huds = _parent.GetChildObjects();
                for (int i = 0; i < _parent.transform.childCount; i++)
                    huds[i].gameObject.GetComponent<CanvasGroup>().alpha =
                        (huds[i].name == hudObjectName) ? 1.0f : 0.0f;
            }

            /// <summary> すべて非表示にする </summary>
            public void HideAll()
            {
                var huds = _parent.GetChildObjects();
                foreach (var hud in huds)
                {
                    hud.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                }
            }

            protected override void ToDoAtAwakeSingleton()
            {
                GameObject.DontDestroyOnLoad(_parent);
                _loader = GameObject.FindFirstObjectByType<SceneLoader>();
            }
        }
    }
}