using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Game.Manager;
using Game.Core;
using Game.Const;

public class ScoreView : BaseView {

    public Sprite[] nums;
    public Image[] numImgs;
    private Image txt;

    void Awake() { 
        Transform score = transform.FindChild("Score");
        numImgs = new Image[score.childCount];
        for (int i = 0; i < score.childCount; ++i) {
            numImgs[i] = score.GetChild(i).GetComponent<Image>();
        }
    }

    public void SetScore(int val) { 
        char[] numArr = val.ToString().ToCharArray();
        if (numArr.Length > 4) return;
        for (int i = 0; i < numImgs.Length; ++i) {
            if (i < numArr.Length) {
                numImgs[i].sprite = nums[int.Parse(numArr[i].ToString())];
                numImgs[i].gameObject.SetActive(true);
            } else { 
                numImgs[i].gameObject.SetActive(false);
            }
        }
    }
}
