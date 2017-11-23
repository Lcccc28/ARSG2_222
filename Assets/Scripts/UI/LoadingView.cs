using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Game.Manager;

public class LoadingView : BaseView {

    private Image loadingBar;
    private Text loadingNum;

    void Start() { 
        loadingBar = transform.FindChild("LoadingBar/Bar").GetComponent<Image>();
        loadingNum = transform.FindChild("LoadingBar/Num").GetComponent<Text>();
    }

    public void SetBarFillAmount(float val) { 
        if (loadingBar == null) return;
        loadingBar.fillAmount = val;
        loadingNum.text = ((int)(val * 100)) + "%";
    }
}
