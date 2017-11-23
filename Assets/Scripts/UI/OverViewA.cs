using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Game.Manager;

public class OverViewA : OverView {

    public override void OnBtnRestartClick() {
        GameSceneManager.Instance.LoadSceneAsync("ModelA");
    }
}
