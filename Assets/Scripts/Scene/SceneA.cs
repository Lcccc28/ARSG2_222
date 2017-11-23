using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Const;
using Game.Manager;
using BehaviorDesigner.Runtime;

public class SceneA : GameScene {

    public static SceneA Instance = null;

    private Transform scene;
    private float timeSum = 0;
    private int curPoint = 0;
    private Transform[] fightPoints;
    private bool isMoveNextPoint;
    public ModelBView view; // 主界面
    public OverView overView;
    public LoadingView loadingView;
    private bool isLoading = false;

    void Start() {
        Instance = this;

        base.BasicInit();
        AudioManager.Instance.PlayMusic("music_game_bg");

        InitScene(true);
        InitData();
        InitEvents();

        //countDownView.gameObject.SetActive(true);
    }

    void FixedUpdate() {
        if (isMoveNextPoint || !isPlaying) return;

        if (isWaitEnterNextStage) {
            if (monsList.Count <= 0) {
                GoToNextStage();
            }
            return;
        }
        if (curStage < stageDatas.Count)
        {
            StageData stage = stageDatas[curStage + stageNum % MAX_STAGE_NUM * MAX_LV_NUM];

            if (stage.cond[curLv] == 2 && monsList.Count <= 0)
            {
                // 立马创建BOSS
                CreateCurLevelMons(stage);
                //monsList[i].transform.position = scene.FindChild("BossPos").position;
                isBossLv = true;
                ResetCreateTime();
                isWaitEnterNextStage = true;
            }
        }
        
    }

    private void InitScene(bool needLoadingView = false) {
        if (scene != null) { 
            GameObject.Destroy(scene.gameObject);
            Resources.UnloadUnusedAssets();
        }
        if (needLoadingView) { 
            StartCoroutine(ShowLoadingAnim());    
        }

        StartCoroutine(LoadScene(!needLoadingView));
        //scene = GameObject.Instantiate(Resources.LoadAsync("Scene/" + ((stageNum % 2) + 1)) as GameObject).transform;
        //scene.name = "Scene";
    }

    protected IEnumerator ShowScoreView() {
        if (scoreView != null) { 
            yield return new WaitForSeconds(1);
            scoreView.gameObject.SetActive(true);
            scoreView.SetScore(score);
            player.canShoot = false;
            isPlaying = false;
            yield return new WaitForSeconds(2);
            scoreView.gameObject.SetActive(false);
            countDownView.gameObject.SetActive(true);
            countDownView.CountDown();
        }
    }

    private IEnumerator ShowLoadingAnim() { 
        loadingView.gameObject.SetActive(true);
        float time = 0;
        while (time < 2) {
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
            loadingView.SetBarFillAmount(time / 2);
        }

        while (isLoading) { 
            yield return new WaitForFixedUpdate();
        }
        ContinueGame();
        loadingView.gameObject.SetActive(false);
        countDownView.gameObject.SetActive(true);
    }

    private IEnumerator LoadScene(bool isStartGame) { 
        ResourceRequest req = Resources.LoadAsync("Scene/" + ((stageNum % MAX_STAGE_NUM) + 1));
        isLoading = true;
        yield return req;
        isLoading = false;
        scene = GameObject.Instantiate(req.asset as GameObject).transform;
        scene.name = "Scene";
        StaticBatchingUtility.Combine(scene.gameObject);
        if (isStartGame) { 
            ContinueGame();
        }
    }

    private void ContinueGame() { 
        InitFightPoints();
        ResetPlayerPos();
        if (player != null) { 
            player.IsBeHit(true);
        }
        isMoveNextPoint = false;
    }

    private void InitFightPoints() { 
        Transform points = scene.FindChild("Point");
        fightPoints = new Transform[points.childCount];
        for (int i = 0; i < fightPoints.Length; ++i) { 
            fightPoints[i] = points.GetChild(i);
        }
    }

    private void InitEvents() {
        GlobalEventSystem.Bind(EventName.MONS_DEATH, OnMonsDeath);
        GlobalEventSystem.Bind(EventName.BULLET_NUM_CHANGE, OnBulletNumChange);
        GlobalEventSystem.Bind(EventName.PLAYER_BE_HIT, OnPlayerBeHit);
        GlobalEventSystem.Bind(EventName.MOVE_COMPLETE, OnMoveComplete);
        GlobalEventSystem.Bind(EventName.GAME_START, OnGameStart);

        KeyInputManager.Instance.Bind(EventName.BTN_A_DOWN, OnBtnADown);
        KeyInputManager.Instance.Bind(EventName.BTN_A_UP, OnBtnAUp);
        KeyInputManager.Instance.Bind(EventName.BTN_B_DOWN, OnBtnBDown);
        KeyInputManager.Instance.Bind(EventName.BTN_C_DOWN, OnBtnCDown);
    }


    private void InitData() {
        stageDatas = new List<StageData>();

        StageData stage6 = new StageData();
        stage6.monsId = new int[]{ 0};
        stage6.monsPos = new Vector3[] { new Vector3(-12f, 0.2f, 20f)};
        stage6.monsNum = new int[] { 1 };
        stage6.cond = new int[] { 2};
        stage6.condParm = new int[]{ 0 };
        stage6.condState = new bool[stage6.cond.Length];
        stageDatas.Add(stage6);
    }

    private void OnBtnBDown(BaseEvent evt) {
        if (view != null) {
            view.OnBtnGunClick();
        }
    }

    protected override void GoToNextStage() {
        base.GoToNextStage();
        StartCoroutine(StartGoToNextStage());
    }

    private IEnumerator StartGoToNextStage() {
        if (player != null)
        {
            player.IsBeHit(false);
        }

        ////isMoveNextPoint = true;
        ////if (stageNum > 0 && curStage == 0) {
        ////    loadingView.SetBarFillAmount(0);
        yield return new WaitForSeconds(3);
        isWaitEnterNextStage = false;
        ////    InitScene(true);
        ////} else { 
        ////    player.MoveToPoint(fightPoints[curStage % 3].position);    
        ////}

        if (view != null)
        {
            view.SetStageNum(curStage + stageNum * MAX_LV_NUM + 1);
        }
    }

    protected void OnBtnCDown(BaseEvent evt) {
        player.ChangeBullet();
    }

    private void ResetPlayerPos() { 
        player.transform.position = fightPoints[0].position;
    }

    protected void OnBulletNumChange(BaseEvent evt) {
        if (view != null) { 
            object[] parms = evt.Parameters as object[];
            view.SetBullet((int)parms[0], (int)parms[1]);
            view.PlayAimEfc();
        }
    }

    protected void OnMoveComplete(BaseEvent evt) { 
        isMoveNextPoint = false;
		player.IsBeHit (true);
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

    protected override void CreateMons(GameObject mons, Vector3 pos, bool isBoss = false) {
        base.CreateMons(mons, pos, isBoss);
        if (view != null) { 
            view.AddRadarPoint();
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
}
