using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Game.Manager;
using Game.Core;
using Game.Const;

public class CountDownView : BaseView {

    public Sprite[] txts;
    private Image txt;

    void Start() { 
        txt = transform.FindChild("Text").GetComponent<Image>();
        CountDown();
    }

    public void CountDown() { 
        StartCoroutine(BeginCountDown());
    }

    private IEnumerator BeginCountDown() {
        for (int i = 0; i < 4; ++i) { 
            txt.sprite = txts[i];
            txt.SetNativeSize();
            yield return new WaitForSeconds(1);
        }
        GlobalEventSystem.Fire(new BaseEvent(EventName.GAME_START));
        gameObject.SetActive(false);
    }
}
