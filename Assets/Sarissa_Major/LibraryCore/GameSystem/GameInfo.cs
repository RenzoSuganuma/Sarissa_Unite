using Sarissa.Singleton;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// auth 菅沼
public class GameInfo : SingletonBaseClass<GameInfo>
{
    /// <summary> 遷移先シーンがどのようなものかを選択、保持する </summary>
    public enum SceneTransitStatus
    {
        WentToTitleScene, // タイトルシーン
        WentToUniqueScene, // ムービーシーンなどの特殊シーン
        WentToInGameScene, // インゲームシーン
    }

    [SerializeField] SceneTransitStatus _transitStatus;

    [SerializeField] SceneInfo _info;

    public SceneTransitStatus GetTransitStatus
    {
        get { return _transitStatus; }
    }

    public SceneInfo GetSceneInfo
    {
        get { return _info; }
    }

    protected override void ToDoAtAwakeSingleton()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            #region find matching scene type

            var titles = _info.TitleScenesName.Select(_ => _).ToList();
            var ingames = _info.IngameScenesName.Select(_ => _).ToList();
            var uniques = _info.UniqueScenesName.Select(_ => _).ToList();

            foreach (var title in titles)
            {
                if (title == arg1.name)
                {
                    _transitStatus = SceneTransitStatus.WentToTitleScene;
                    break;
                }
            }


            foreach (var ingame in ingames)
            {
                if (ingame == arg1.name)
                {
                    _transitStatus = SceneTransitStatus.WentToInGameScene;
                    break;
                }
            }

            foreach (var unique in uniques)
            {
                if (unique == arg1.name)
                {
                    _transitStatus = SceneTransitStatus.WentToUniqueScene;
                    break;
                }
            }
        }

        #endregion
    }
}