using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Game.Manager;
using Game.Core;
using Game.Const;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Alert : BaseView {

    public UnityAction<object[]> confirmHandler;
    public UnityAction<object[]> completeHandler;

    public static Alert alert;

    public static void Show(string msg, UnityAction<object[]> confirmAction, bool needBtn = true) {
        if (alert == null) { 
            GameObject alertView = GameObject.Instantiate(Resources.Load("View/Alert")) as GameObject;
            alertView.transform.SetParent(GameObject.FindGameObjectWithTag("UICamera").transform.parent);
            alertView.transform.localScale = Vector3.one;
            alert = alertView.AddComponent<Alert>();
        }
        alert.SetData(msg, confirmAction, needBtn);
        alert.gameObject.SetActive(true);
    }

    public static void Close() {
        if (alert != null) {
            alert.Destroy();
        }
    }

    void Awake() { 
        Button btnConfirm = transform.FindChild("BtnConfirm").GetComponent<Button>();
        btnConfirm.onClick.RemoveAllListeners();
        btnConfirm.onClick.AddListener(OnClick);
    }

    private void SetData(string msg, UnityAction<object[]> confirmAction, bool needBtn) { 
        transform.FindChild("Text").GetComponent<Text>().text = msg;
        confirmHandler = confirmAction;
        transform.FindChild("BtnConfirm").gameObject.SetActive(needBtn);
    }

    private void OnClick() {
        if (confirmHandler != null) { 
            confirmHandler(null);
            confirmHandler = null;
        }
        Destroy();
    }

    void OnDestroy() { 
        Destroy();
    }

    private void Destroy() {
        if (Alert.alert == this) { 
            Alert.alert = null;    
        }
        GameObject.Destroy(gameObject);
    }
}
