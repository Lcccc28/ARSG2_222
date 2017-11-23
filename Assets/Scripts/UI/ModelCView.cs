using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Game.Core;
using Game.Const;
using Game.Manager;

public class ModelCView  : BaseView {

    public Sprite[] num0Sprites;

    private Transform scoreNum;
    private List<Image> nums;

    private Transform hpBar;
    private Animator hurtEfc;
    private Animator aimEfc;
    private Animator missEfc;

    void Awake() { 
        scoreNum = transform.FindChild("Score/Num");
        nums = new List<Image>();
        nums.Add(scoreNum.FindChild("Num").GetComponent<Image>());

        Button btnOver = transform.FindChild("BtnOver").GetComponent<Button>();
        btnOver.onClick.AddListener(OnBtnOverClick);

        hpBar = transform.FindChild("HPBar");

        hurtEfc = transform.FindChild("HurtEffect").GetComponent<Animator>();
        aimEfc = transform.FindChild("Aim").GetComponent<Animator>();
        missEfc = transform.FindChild("MissEffect").GetComponent<Animator>(); 
    }


    public void UpdateHPBar(int curVal, int maxVal) {
        if (curVal < 3) {
            for (int i = 0; i < 3; ++i) {
                hpBar.GetChild(i).gameObject.SetActive(i < curVal);
            }
            missEfc.Play("Anim", -1, 0);
        }
    }

    public void PlayAimEfc() {
        aimEfc.Play("Aim", -1, 0);
    }

    public void PlayHurtEfc() { 
        hurtEfc.Play("Hurt");
    }

    public void OnBtnBulletClick()
    {
        KeyInputManager.Instance.BtnCDown();
    }

    public void OnBtnOverClick() {
        GameSceneManager.Instance.LoadSceneAsync("Main");
    }

    public void SetScore(int val) { 
        char[] scoreArr = val.ToString().ToCharArray();
        if (scoreArr.Length > nums.Count) { 
            int numsNum = nums.Count;
            for (int i = numsNum; i < scoreArr.Length; ++i) { 
                GameObject num = GameObject.Instantiate(nums[0].gameObject);
                num.transform.SetParent(scoreNum);
                num.name = i.ToString();
                num.transform.localScale = new Vector3(1f, 1f, 1);
                nums.Add(num.GetComponent<Image>());
            }
        }

        
        for (int i = 0; i < scoreArr.Length; ++i) { 
            nums[i].sprite = num0Sprites[int.Parse(scoreArr[i].ToString())];
        }
        
    }


    //public void SetStageNum(int val) { 
    //    if (val == 10) {
    //        transform.FindChild("Stage/Num0").GetComponent<Image>().sprite = stageNumSprites[9];
    //        return;
    //    }
        
    //    char[] strs = val.ToString().ToCharArray();
    //    if (strs.Length == 1) {
    //        transform.FindChild("Stage/Num0").GetComponent<Image>().sprite = stageNumSprites[int.Parse(strs[0].ToString()) - 1];
    //    } else if (strs.Length == 2) {
    //        if (val == 10) { 
    //            transform.FindChild("Stage/D").GetComponent<RectTransform>().anchoredPosition = new Vector2(-20, 0);
    //            transform.FindChild("Stage/Num1").GetComponent<RectTransform>().anchoredPosition = new Vector2(20, 0);
    //            transform.FindChild("Stage/Num1").gameObject.SetActive(true);
    //            transform.FindChild("Stage/Num0").GetComponent<RectTransform>().anchoredPosition = new Vector2(60, 0);
    //            transform.FindChild("Stage/G").GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 0);
    //        }

    //        transform.FindChild("Stage/Num0").GetComponent<Image>().sprite = stageNumSprites[int.Parse(strs[1].ToString()) - 1];
    //    }
    //}

    public void OnBtnRestartClick() {
        GameSceneManager.Instance.LoadSceneAsync("ModelB");
    }
}
