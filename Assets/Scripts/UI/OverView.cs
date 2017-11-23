using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Game.Manager;

public class OverView : BaseView {

    void Start() { 
        Button btnRestart = transform.FindChild("BtnRestart").GetComponent<Button>();
        btnRestart.onClick.AddListener(OnBtnRestartClick);

        Button btnOver = transform.FindChild("BtnOver").GetComponent<Button>();
        btnOver.onClick.AddListener(OnBtnOverClick);
    }

    public virtual void OnBtnOverClick() {
        GameSceneManager.Instance.LoadSceneAsync("Main");
    }

    public virtual void OnBtnRestartClick() {
        GameSceneManager.Instance.LoadSceneAsync("ModelB");
    }
}
