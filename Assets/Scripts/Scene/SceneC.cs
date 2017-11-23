using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Const;
using Game.Manager;
using BehaviorDesigner.Runtime;

public class SceneC : GameScene {

    public static SceneC Instance = null;

    public float timeSum = 2;

    public ModelCView view; // 主界面
    public OverView overView;
    
    private int maxLv = 6;
    private bool isCreating;


    void Start() {
        Instance = this;
        BasicInit();
        AudioManager.Instance.PlayMusic("music_free_style");

        InitData();
        InitEvents();
        InitView();

        countDownView.gameObject.SetActive(true);
    }

    void FixedUpdate() {
        if (isCreating || !isPlaying) return;

        if (isWaitEnterNextStage) {
            if (monsList.Count <= 0)
                GoToNextLv();
            return;
        }

        timeSum -= Time.fixedDeltaTime;
        if (timeSum <= 0) {
            if (curLv <= (maxLv / 3)) {
                // 当前波数<最大关卡波数除以3 ，出一个水果
                StartCoroutine(CreateFruits(1, 1f));
            } else if (curLv <= (maxLv / 3) * 2) { 
                // 当前波数>关卡波数除以3且点前波数小于关卡波数的三分之二 ，出两个个水果，水果出的间隔为0.5秒
                StartCoroutine(CreateFruits(2, 1));
            } else { 
                // 当前波数>关卡波数除以3乘以2 ，出三个水果 ，水果的抛射间隔为0.5秒
                StartCoroutine(CreateFruits(3, 1));
            }
            isWaitEnterNextStage = true;
        }
    }

    private IEnumerator CreateFruits(int num, float delay) {
        isCreating = true;
        int bombIndex = -1;
        if (curLv % 2 == 0) { 
            bombIndex = Random.Range(0, num);
        }
        for (int i = 0; i < num; ++i) {
            if (bombIndex == i) { 
                DynamicCreateMons(10);
            } else { 
                DynamicCreateMons(Random.Range(0, 7));    
            }
            if (num > 1) { 
                yield return new WaitForSeconds(delay);
            }
        }
        isCreating = false;
    }

    private void InitData() {
        curLv = 1;

        player.data.maxHP = 3;
        player.data.curHP = player.data.maxHP;
        player.bullets[0].maxNum = int.MaxValue;
        player.bullets[0].curNum = int.MaxValue;
        player.bullets[0].speed = 90;
        player.bullets[0].lifeTime = 1f;
        player.bullets[0].isRandomRotation = true;
        player.bullets[0].sound = "sound_wem";
    }

    private void InitEvents() {
        GlobalEventSystem.Bind(EventName.MONS_DEATH, OnMonsDeath);
        GlobalEventSystem.Bind(EventName.PLAYER_BE_HIT, OnPlayerBeHit);
        GlobalEventSystem.Bind(EventName.GAME_START, OnGameStart);

        KeyInputManager.Instance.Bind(EventName.BTN_A_DOWN, OnBtnADown);
        KeyInputManager.Instance.Bind(EventName.BTN_A_UP, OnBtnAUp);
    }

    private void InitView() {
        if (view != null) { 
            view.SetScore(0);
        }
    }

    protected void DynamicCreateMons(int type) {
        if (type == 1) { 
            CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("B").gameObject).gameObject);
        } else if (type == 2) { 
            CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("C").gameObject).gameObject);
        } else if (type == 3) { 
            CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("D").gameObject).gameObject);
        } else if (type == 4) { 
            CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("E").gameObject).gameObject);
        } else if (type == 5) { 
            CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("F").gameObject).gameObject);
        } else if (type == 6) { 
            CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("G").gameObject).gameObject);
        } else if (type == 10) {
            CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("Bomb").gameObject).gameObject, true);
        } else { 
			CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("A").gameObject).gameObject);
        }
    }

    protected void CreateMons(GameObject mons, bool isBoss = false) {
        mons.transform.SetParent(monsTrans);
		mons.transform.position = new Vector3(Random.Range(-12f, -10f), 0, Random.Range(12f, 13f));
        mons.SetActive(true);

        Fruit fruitScript = mons.GetComponent<Fruit>();
        if (fruitScript != null) { 
            DestroyImmediate(fruitScript);
        }
        fruitScript = mons.AddComponent<Fruit>();
        if (isBoss) { 
            fruitScript.data.type = ObjectType.BOSS;
        } else { 
            fruitScript.data.type = ObjectType.MONS;
        }
        monsList.Add(fruitScript);
    }

     protected override void GoToNextLv() {
        ++curLv;
        if (curLv > maxLv) { 
            GoToNextStage();
            timeSum = 2;
        } else { 
            timeSum = 2;    
        }
        
        isWaitEnterNextStage = false;
    }

    protected override void GoToNextStage() {
        ++curStage;
        curLv = 1;
        maxLv = 6 + curStage;
        //if (view != null) { 
        //    view.SetStageNum(curStage + 1);
        //}
        StartCoroutine(ShowScoreView());
    }

    protected void RemoveMonster(Enemy enemy, bool isAddScore) { 
        for (int i = 0; i < monsList.Count; ++i) {
            if (monsList[i].transform == enemy.transform) {
                monsList.RemoveAt(i);
                
                if (view != null) {
                    if (isAddScore) { 
                        score += 20;
                        view.SetScore(score);
                    }
                }
                return;
            }
        }
    }

    protected void OnPlayerBeHit(BaseEvent evt) {
        object[] parms = evt.Parameters as object[];
        int curHP = (int)parms[0];
        if (view != null) { 
            view.UpdateHPBar(curHP, (int)parms[1]);
            view.PlayHurtEfc();
        }

        if (curHP <= 0) { 
			Time.timeScale = 0;
            overView.gameObject.SetActive(true);
        }
    }

    protected void OnMonsDeath(BaseEvent evt) {
        object[] parms = evt.Parameters as object[];
        Enemy enemy = parms[0] as Enemy;
        bool isSuicide = (bool)parms[1];
        if (enemy.data.type == ObjectType.MONS) { 
            if (isSuicide) {
                player.BeHit(1);
            }
            RemoveMonster(enemy, !isSuicide);
        } else {
            if (!isSuicide) {
                player.BeHit(3);
            }
            RemoveMonster(enemy, false);
        }
    }
}
