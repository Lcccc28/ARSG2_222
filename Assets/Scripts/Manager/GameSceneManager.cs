using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Core;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Manager {

    public class GameSceneManager : Singleton<GameSceneManager> {

        public GameSceneManager() {
        }

        public void LoadSceneAsync(string sceneName) { 
            SceneManager.LoadSceneAsync(sceneName);
            Resources.UnloadUnusedAssets();
            Time.timeScale = 1;
        }

        public int GetSceneId() { 
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name;
            if (sceneName.Equals("ModelA")) { 
                return 1;
            } else if (sceneName.Equals("ModelB")) { 
                return 2;
            } else { 
                return 3;
            }
        }
    }
}