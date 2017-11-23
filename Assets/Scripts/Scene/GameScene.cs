using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Const;
using Game.Manager;
using BehaviorDesigner.Runtime;

public class GameScene : MonoBehaviour {

    public Player player;
    protected Transform monsTrans; 
    protected List<Enemy> monsList = new List<Enemy>();
    protected List<StageData> stageDatas;
    protected int score;
    protected int curLv = 0; // 当前第几关
    protected int stageNum = 0; // 轮数 
    protected int curStage = 0; // 当前第几波
    protected int curMonsIndex = 0; // 当前创建的怪物索引
    protected int sceneId; // 场景ID
    protected float curStageTime = 0; // 当前波时间
    protected float waitTime; // 等待时间
    protected int createTimeIndex;
    protected float createMonsTime1;
    protected float createMonsTime2;
    protected float createMonsTime3;
    protected float createMonsTime4;
    protected bool isWaitEnterNextStage; // 是否在等待进入下一波
    protected bool isBossLv = false; // 是否BOSS关卡
    protected bool isPlaying;

    protected const int MAX_LV_NUM = 1;
    protected const int MAX_STAGE_NUM = 1; 

    public CountDownView countDownView;
    public ScoreView scoreView;

    protected virtual void BasicInit() { 
		Time.timeScale = 1f;
        if (monsTrans == null) {
            monsTrans = new GameObject("Monsters").transform;
        }

        player = GameObject.Find("MainRole").GetComponent<Player>();
        // player.GetComponent<GyroController>().DetachGyro();
        // player.GetComponent<GyroController>().AttachGyro();

        sceneId = GameSceneManager.Instance.GetSceneId();
        Cursor.lockState = CursorLockMode.Locked;
        GamePoolManager.Instance.Init();
        AudioManager.Instance.Init();
    }

    protected virtual void DynamicCreateMons(int type, Vector3 pos = default(Vector3)) {
		CreateMons(GameObject.Instantiate(GamePoolManager.Instance.GetPrefabFromPool("A").gameObject), pos, true);
    }

     protected virtual void CreateMons(GameObject mons, Vector3 pos, bool isBoss = false) {
        mons.transform.SetParent(monsTrans);
        if (pos != Vector3.zero)
            mons.transform.position = pos;
        else
        {
            mons.transform.position = player.transform.TransformPoint(Random.Range(-10, 10), Random.Range(0, 10), Random.Range(-10, -20));
            mons.transform.LookAt(player.transform.position);
        }

        
        mons.SetActive(true);

        Enemy monsScript = mons.GetComponent<Enemy>();
        if (monsScript != null) { 
            DestroyImmediate(monsScript);
        }
			
		if (mons.name.StartsWith("A"))
		{
			// mons.transform.position = player.transform.FindChild("MonRandomPoint").position;
			mons.transform.LookAt(player.transform.position);
			monsScript = mons.AddComponent<EnemyA>();
			monsList.Add(monsScript);
			BehaviorTree bt = mons.GetComponent<BehaviorTree>();
			bt.EnableBehavior();
		}

        if (isBoss) { // TODO 
            GameObject targetEff = GamePoolManager.Instance.Spawn("TelePort").gameObject;
            targetEff.SetActive(true);
			targetEff.transform.position = mons.transform.position + new Vector3(0, (mons.transform.FindChild("HpBar").position.y - mons.transform.position.y) / 2f);
            GamePoolManager.Instance.Despawn(targetEff.transform, 3);
            AudioManager.Instance.PlaySound("sound_teleport");

            monsScript.data.type = ObjectType.BOSS;
        } else { 
            monsScript.data.type = ObjectType.MONS;
        }

        HPBar hpbar = mons.GetComponent<HPBar>();
        if (hpbar != null) { 
            DestroyImmediate(hpbar);
        }
        hpbar = mons.AddComponent<HPBar>();
        
		mons.GetComponent<Enemy> ().state = EnemyStatus.Stand;
        int stage;
        if (sceneId == 1) {
            stage = MAX_LV_NUM * stageNum + curStage + 1;  
        } else { 
            stage = stageDatas.Count * stageNum + curStage + 1;    
        }
        monsScript.data.maxHP = 2000;
		monsScript.data.curHP = monsScript.data.maxHP;
		foreach(BulletData bdata in monsScript.data.bulletDatas){
            if (mons.name.StartsWith("C") && bdata.modelName == "BulletC")
                bdata.atk = 5;
            else 
    			bdata.atk = stage * 2 + 8;
		}
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

    protected void OnGameStart(BaseEvent evt) { 
        player.canShoot = true;
        isPlaying = true;
		player.IsBeHit (true);
    }

    protected void ResetCreateTime() { 
        createTimeIndex = 0;
        createMonsTime1 = 15;
        createMonsTime2 = 5;
        createMonsTime3 = 10;
        createMonsTime4 = 5;
    }

    protected void CreateCurLevelMons(StageData stage) { 
        int maxIndex = curMonsIndex + stage.monsNum[curLv];
        for (int i = curMonsIndex; i < maxIndex; ++i) {
            if (stage.monsPos != null && stage.monsPos.Length > i)
                DynamicCreateMons(stage.monsId[i], stage.monsPos[i]);
            else
                DynamicCreateMons(stage.monsId[i]);
            curMonsIndex++;
        }
    }

    protected virtual void GoToNextLv() { 
        ++curLv;
        StageData stageData;
        if (sceneId == 1) {
            stageData = stageDatas[curStage + stageNum % MAX_STAGE_NUM * MAX_LV_NUM];
        } else { 
            stageData = stageDatas[curStage];
        }
        if (curLv >= stageData.cond.Length) { 
            isWaitEnterNextStage = true;
        }
    }

    protected virtual void GoToNextStage() { 
        ++curStage;
        curMonsIndex = 0;
        if (sceneId != 1) { 
            if (curStage >= stageDatas.Count) {
                curStage = 0;
                ++stageNum;
            }
        } else {
            if (curStage >= MAX_LV_NUM) {
                curStage = 0;
                ++stageNum;
            }
        }
        curLv = 0;
        KillAllMonster();
        isBossLv = true;
    }

    protected void KillAllMonster() {
        Enemy[] allMons = monsList.ToArray();
        for (int i = 0; i < allMons.Length; ++i) { 
            allMons[i].Death(false);
        }
        monsList.Clear();
    }

    protected void OnBtnADown(BaseEvent evt) {
        if (player != null) { 
            player.isShoot = true;
        }
    }

    protected void OnBtnAUp(BaseEvent evt) { 
         if (player != null) { 
            player.isShoot = false;
        }
    }

    protected void OnChangeGun(BaseEvent evt) {
        if (player != null) { 
            player.ChangeGun((int)evt.Parameters);
        }
    }

    protected void OnDestroy() { 
        GlobalEventSystem.Clear();
    }
}
