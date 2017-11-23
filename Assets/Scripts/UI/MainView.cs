using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Game.Manager;
using cn.sharesdk.unity3d;

public class MainView : BaseView {

    void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Button btnStart = transform.FindChild("BtnStart").GetComponent<Button>();
        btnStart.onClick.AddListener(OnModelABtnClick);

        Button btnModelA = transform.FindChild("MainPanel/BtnModelA").GetComponent<Button>();
        btnModelA.onClick.AddListener(OnModelABtnClick);

        Button btnModelB = transform.FindChild("MainPanel/BtnModelB").GetComponent<Button>();
        btnModelB.onClick.AddListener(OnModelBBtnClick);

        Button btnModelC = transform.FindChild("MainPanel/BtnModelC").GetComponent<Button>();
        btnModelC.onClick.AddListener(OnModelCBtnClick);

        GameObject blueTooth = GameObject.Find("BlueToothMotor");
        if (blueTooth == null) { 
            blueTooth = new GameObject("BlueToothMotor");
            blueTooth.AddComponent<BlueToothMotor>();
        }

        //GameObject shareSDK = GameObject.Find("ShareSDK");
        //if (shareSDK == null) { 
        //    shareSDK = new GameObject("ShareSDK");
        //    SDKManager.Instance.InitSDK(shareSDK.AddComponent<ShareSDK>());
        //    DontDestroyOnLoad(shareSDK);
        //}
 
        AudioManager.Instance.Init();
        AudioManager.Instance.PlayMusic("music_start");
    }

    public void OnStartBtnClick() {
        transform.FindChild("BtnStart").gameObject.SetActive(false);
        transform.FindChild("MainPanel").gameObject.SetActive(true);
    }

    public void OnModelABtnClick() {
        //SDKManager.Instance.Share();
        GameSceneManager.Instance.LoadSceneAsync("ModelA");
    }

    public void OnModelBBtnClick() {
        GameSceneManager.Instance.LoadSceneAsync("ModelB");
    }

    public void OnModelCBtnClick() {
        GameSceneManager.Instance.LoadSceneAsync("ModelC");
    }
}
