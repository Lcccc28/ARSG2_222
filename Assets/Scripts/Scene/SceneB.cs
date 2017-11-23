using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Const;
using Game.Manager;
using BehaviorDesigner.Runtime;

public class SceneB : GameScene {

    public static SceneB Instance = null;

    public ModelBView view; // 主界面
    public OverView overView;

    void Start() {
        Instance = this;
        BasicInit();
        AudioManager.Instance.PlayMusic("music_game_bg");

        InitData();
        InitEvents();
        InitView();

        countDownView.gameObject.SetActive(true);
    }

    void FixedUpdate() {
        if (!isPlaying) return;

        if (isBossLv) {
            for (int i = 0; i < monsList.Count; ++i) {
                if (monsList[i].data.type == ObjectType.BOSS) { 
                    if (createTimeIndex == 0) {
                        createMonsTime1 -= Time.fixedDeltaTime;
                    } else if (createTimeIndex == 1) {
                        createMonsTime2 -= Time.fixedDeltaTime;
                    } else {
                        createMonsTime3 -= Time.fixedDeltaTime;
                    }

                    if (monsList.Count < 4) {
                        if (createTimeIndex == 0 && createMonsTime1 <= 0) { 
                            DynamicCreateMons(0);
                            DynamicCreateMons(0);
                            createTimeIndex = 1;
                        } else if (createTimeIndex == 1 && createMonsTime2 <= 0) { 
                            DynamicCreateMons(0);
                            DynamicCreateMons(0);
                            createTimeIndex = 2;
                        } else if (createMonsTime3 <= 0) { 
                            DynamicCreateMons(0);
                            DynamicCreateMons(0);
                            ResetCreateTime();
                        }
                    }
                    return;
                }
            }
            GoToNextStage();
            return;
        }

        if (isWaitEnterNextStage) {
            if (monsList.Count <= 0) { 
                isWaitEnterNextStage = false;
                GoToNextStage();
            }
            return;
        }
        
        if (curStage < stageDatas.Count) { 
            StageData stage = stageDatas[curStage];

            if (stage.cond[curLv] == 0) { 
                // 隔多少秒创建怪物
                if (stage.condState[curLv]) {
                    // 已经触发
                    waitTime -= Time.fixedDeltaTime;
                    if (waitTime <= 0) { 
                        stage.condState[curLv] = false;
                        CreateCurLevelMons(stage);
                        GoToNextLv();
                    }
                } else { 
                    stage.condState[curLv] = true;
                    waitTime = stage.condParm[curLv];
                }
            } else if (stage.cond[curLv] == 1) {
                // 清除怪物以后
                if (monsList.Count <= 0) { 
                    CreateCurLevelMons(stage);
                    GoToNextLv();
                }
            } else if (stage.cond[curLv] == 2) { 
                // 立马创建BOSS
				if (stageNum % 3 == 0) { 
					DynamicCreateMons (2);
				} else if (stageNum % 3 == 1) { 
					DynamicCreateMons (3);
				} else {
					DynamicCreateMons (7);
				}
                isBossLv = true;
                ResetCreateTime();
            }
        } 
    }

    private void InitData() {
        stageDatas = new List<StageData>();

        // 规则3配置
        StageData stage1 = new StageData();
        stage1.monsId = new int[]{ 0, 0, 0, 0, 0, 0 };
        stage1.monsNum = new int[]{ 1, 2, 3 };
        stage1.cond = new int[]{ 0, 1, 1 };
        stage1.condParm = new int[]{ 2, 0, 0 };
        stage1.condState = new bool[stage1.cond.Length];
        stageDatas.Add(stage1);

        StageData stage2 = new StageData();
        stage2.monsId = new int[]{ 1, 1, 1, 1, 1, 1, 1 };
        stage2.monsNum = new int[]{ 2, 2, 3 };
        stage2.cond = new int[]{ 0, 1, 1 };
        stage2.condParm = new int[]{ 2, 0, 0 };
        stage2.condState = new bool[stage2.cond.Length];
        stageDatas.Add(stage2);

        StageData stage3 = new StageData();
        stage3.monsId = new int[]{ 0, 2, 0 };
        stage3.monsNum = new int[] { 1, 1, 1 };
        stage3.cond = new int[] { 0, 2, 0 };
        stage3.condParm = new int[]{ 2, 2, 0 };
        stage3.condState = new bool[stage3.cond.Length];
        stageDatas.Add(stage3);
    }

    private void InitEvents() {
        GlobalEventSystem.Bind(EventName.MONS_DEATH, OnMonsDeath);
        GlobalEventSystem.Bind(EventName.BULLET_NUM_CHANGE, OnBulletNumChange);
        GlobalEventSystem.Bind(EventName.PLAYER_BE_HIT, OnPlayerBeHit);
        GlobalEventSystem.Bind(EventName.GAME_START, OnGameStart);

        KeyInputManager.Instance.Bind(EventName.BTN_A_DOWN, OnBtnADown);
        KeyInputManager.Instance.Bind(EventName.BTN_A_UP, OnBtnAUp);
        KeyInputManager.Instance.Bind(EventName.BTN_B_DOWN, OnBtnBDown);
        KeyInputManager.Instance.Bind(EventName.BTN_C_DOWN, OnBtnCDown);
    }

    private void InitView() {
        if (view != null) { 
            view.SetScore(0);
            BulletData data = player.bullets[player.curGun];
            view.SetBullet(data.curNum, data.maxNum);
        }
    }

    private void OnBtnBDown(BaseEvent evt) {
        if (view != null) {
            view.OnBtnGunClick();
        }
    }

    protected override void CreateMons(GameObject mons, Vector3 pos, bool isBoss = false) {
        base.CreateMons(mons, pos, isBoss);
        if (view != null) { 
            view.AddRadarPoint();
        }
    }

    protected override void GoToNextStage() {
        base.GoToNextStage();
        StartCoroutine(ShowScoreView());
        if (view != null) { 
            view.SetStageNum(curStage + stageNum * stageDatas.Count + 1);
        }
    }

    protected void RemoveMonster(Enemy enemy, bool isAddScore) { 
        for (int i = 0; i < monsList.Count; ++i) {
            if (monsList[i].transform == enemy.transform) {
                monsList.RemoveAt(i);
                
                if (view != null) {
                    if (isAddScore) { 
                        score += 100;
                        view.SetScore(score);
                    }
                    view.RemoveRadarPoint();
                }
                return;
            }
        }
    }

    protected void OnBtnCDown(BaseEvent evt) {
        player.ChangeBullet();
    }

    protected void OnBulletNumChange(BaseEvent evt) {
        if (view != null) { 
            object[] parms = evt.Parameters as object[];
            view.SetBullet((int)parms[0], (int)parms[1]);
            view.PlayAimEfc();
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
        RemoveMonster(parms[0] as Enemy, !(bool)parms[1]);
    }
}
