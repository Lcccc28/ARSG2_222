using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Game.Core;
using Game.Const;
using Game.Manager;

public class ModelBView  : BaseView {

    public Sprite[] num0Sprites;
    public Sprite[] stageNumSprites;
    public Sprite hpBarRed;

    private Transform score;
    private List<Image> nums;

    private Transform bulletSign;
    private Image[] bulletNums;

    private GameObject btnGunA;
    private GameObject btnGunB;

    private Transform radar;
    private Transform radarPoins;
    private GameObject radarPoint;

    private Image hpBar;
    private Animator hurtEfc;
    private Animator aimEfc;

    void Awake() { 
        score = transform.FindChild("Score");
        nums = new List<Image>();
        nums.Add(score.FindChild("Num0").GetComponent<Image>());

        Button btnOver = transform.FindChild("BtnOver").GetComponent<Button>();
        btnOver.onClick.AddListener(OnBtnOverClick);

        hpBar = transform.FindChild("HPBar/Bar").GetComponent<Image>();

        hurtEfc = transform.FindChild("HurtEffect").GetComponent<Animator>();
        aimEfc = transform.FindChild("Aim").GetComponent<Animator>();

        btnGunA = transform.FindChild("Gun/GunA").gameObject;
        btnGunB = transform.FindChild("Gun/GunB").gameObject;

        radar = transform.FindChild("Radar");
        radarPoint = transform.FindChild("Radar/Point").gameObject;
        radarPoins = transform.FindChild("Radar/Points");

        Button btnBullet = transform.FindChild("Bullet").GetComponent<Button>();
        btnBullet.onClick.AddListener(OnBtnBulletClick);

        bulletSign = transform.FindChild("Bullet/Sign");
        bulletNums = new Image[2];
        bulletNums[0] = transform.FindChild("Bullet/Num/0").GetComponent<Image>();
        bulletNums[1] = transform.FindChild("Bullet/Num/1").GetComponent<Image>();
    }

    public void OnBtnGunClick() {
        if (btnGunA.activeSelf) { 
            btnGunA.SetActive(false);
            btnGunB.SetActive(true);
            GlobalEventSystem.Fire(new BaseEvent(EventName.CHANGE_GUN, 1));
        } else if (btnGunB.activeSelf) { 
            btnGunA.SetActive(true);
            btnGunB.SetActive(false);
            GlobalEventSystem.Fire(new BaseEvent(EventName.CHANGE_GUN, 0));
        }
    }

    public void UpdateHPBar(int curVal, int maxVal) { 
        float precent = (float)curVal / maxVal;
        if (precent < 0.3f) { 
            hpBar.sprite = hpBarRed;
        }
        hpBar.fillAmount = precent;
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

    public void SetBullet(int curNum, int maxNum) { 
        int childCount = bulletSign.childCount;
        int num = (int)(childCount * ((float)curNum / maxNum));
        for (int i = 0; i < childCount; ++i) {
            if (i < num) { 
                bulletSign.GetChild(i).gameObject.SetActive(true);
            } else { 
                bulletSign.GetChild(i).gameObject.SetActive(false);
            }
        }

        char[] arr = curNum.ToString().ToCharArray();
        if (arr.Length > 1) {
            bulletNums[0].gameObject.SetActive(true);
            bulletNums[0].sprite = num0Sprites[int.Parse(arr[0].ToString())];
            bulletNums[1].sprite = num0Sprites[int.Parse(arr[1].ToString())];
        } else { 
            bulletNums[0].gameObject.SetActive(false);
            bulletNums[1].sprite = num0Sprites[int.Parse(arr[0].ToString())];
        }
    }

    public void AddRadarPoint() { 
        GameObject p = GameObject.Instantiate(radarPoint);
        p.transform.SetParent(radar.FindChild("Points"));
        p.transform.localScale = Vector3.one;
        p.transform.GetComponent<RectTransform>().anchoredPosition = Random.insideUnitCircle * 32; 
        p.SetActive(true);
    }

    public void RemoveRadarPoint() {
        if (radarPoins.childCount > 0) { 
            GameObject.Destroy(radarPoins.GetChild(0).gameObject);
        }
    }

    public void SetScore(int val) { 
        char[] scoreArr = val.ToString().ToCharArray();
        if (scoreArr.Length > nums.Count) { 
            int numsNum = nums.Count;
            for (int i = numsNum; i < scoreArr.Length; ++i) { 
                GameObject num = GameObject.Instantiate(nums[0].gameObject);
                num.transform.SetParent(score);
                num.name = i.ToString();
                num.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                num.GetComponent<RectTransform>().anchoredPosition = new Vector2(170 - i * 40, - 41);
                nums.Add(num.GetComponent<Image>());
            }
        }

        
        for (int i = 0; i < scoreArr.Length; ++i) { 
            nums[nums.Count - i - 1].sprite = num0Sprites[int.Parse(scoreArr[i].ToString())];
        }
        
    }


    public void SetStageNum(int val) { 
        if (val == 10) {
            transform.FindChild("Stage/Num0").GetComponent<Image>().sprite = stageNumSprites[9];
            return;
        }
        
        char[] strs = val.ToString().ToCharArray();
        if (strs.Length == 1) {
            transform.FindChild("Stage/Num0").GetComponent<Image>().sprite = stageNumSprites[int.Parse(strs[0].ToString()) - 1];
        } else if (strs.Length == 2) {
            if (val == 10) { 
                transform.FindChild("Stage/D").GetComponent<RectTransform>().anchoredPosition = new Vector2(-20, 0);
                transform.FindChild("Stage/Num1").GetComponent<RectTransform>().anchoredPosition = new Vector2(20, 0);
                transform.FindChild("Stage/Num1").gameObject.SetActive(true);
                transform.FindChild("Stage/Num0").GetComponent<RectTransform>().anchoredPosition = new Vector2(60, 0);
                transform.FindChild("Stage/G").GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 0);
            }

            transform.FindChild("Stage/Num0").GetComponent<Image>().sprite = stageNumSprites[int.Parse(strs[1].ToString()) - 1];
        }
    }

    public void OnBtnRestartClick() {
        GameSceneManager.Instance.LoadSceneAsync("ModelB");
    }
}
