using UnityEngine;
using System.Collections;
using Game.Manager;
using Game.Const;
using Game.Core;
using DG.Tweening;

public class Player : BaseObject {
	
    public Camera camera;
    public Transform gun;
    public PlayerData data;
    public BulletData[] bullets;
    public bool isShoot = false;
    public bool canShoot = false;
    public int curGun = 0; //当前的枪序号    
    private float shootTimer = 0;
    private float changeBulletTimer = 0;
    private bool isChangeBullet = false;

	public ReloadView reloadView;

    void Awake() { 
        data = new PlayerData();
        data.maxHP = 200;
        data.curHP = data.maxHP;

        bullets = new BulletData[2];
        bullets[0] = new BulletData();
		bullets[0].atk = 10;
        bullets[0].modelName = "BulletA";
        bullets[0].hitModelName = "Hit";
		bullets[0].hitDestroyTime = 3;
        bullets[0].sound = "sound_pulsegun";
        bullets[0].num = 1;
        bullets[0].maxNum = 30;
        bullets[0].curNum = 30;
		bullets[0].speed = 20;
        

        bullets[1] = new BulletData();
        bullets[1].modelName = "BulletB";
        bullets[1].hitModelName = "Hit";
		bullets[1].hitDestroyTime = 3;
        bullets[1].sound = "sound_laser";
        bullets[1].num = 3;
        bullets[1].interval = 0.1f;
        bullets[1].maxNum = 30;
        bullets[1].curNum = 30;
		bullets[1].speed = 50;

        gun = transform.FindChild("Camera/GunA");

#if UNITY_EDITOR
        GetComponent<MouseLook>().enabled = true;
        GetComponent<GyroController>().enabled = false;
#else
        GetComponent<MouseLook>().enabled = false;
        GetComponent<GyroController>().enabled = true;
#endif
    }

    void Update () {
        if (!canShoot) return;
        shootTimer -= Time.deltaTime;

        if (isChangeBullet) { 
            if (changeBulletTimer > 0) { 
                changeBulletTimer -= Time.deltaTime;
                return;
            } else { 
				reloadView.gameObject.SetActive(false);
                BulletData data = bullets[curGun];
                data.curNum = data.maxNum;
                GlobalEventSystem.Fire(new BaseEvent(EventName.BULLET_NUM_CHANGE, new object[]{ data.curNum, data.maxNum }));
                isChangeBullet = false;
                return;
            }
        }

        if ((Input.GetMouseButton(0) || isShoot) && shootTimer <= 0) {
            shootTimer = 0.2f;
            BulletData data = bullets[curGun];

            if (data.curNum >= 1)
            {
                data.curNum -= data.num;

                // 生成子弹
                //SkillManager.Instance.Fire(this, gun.position + gun.up, camera.transform.forward, bullets[curGun]);
                SkillManager.Instance.Fire(this, null, gun.position, camera.transform.forward, bullets[curGun]);
                GlobalEventSystem.Fire(new BaseEvent(EventName.BULLET_NUM_CHANGE, new object[] { data.curNum, data.maxNum }));
            }
			else
			{
				reloadView.gameObject.SetActive(!reloadView.gameObject.activeSelf);
			}
            // 射线检测法
            //RaycastHit rayHit;

            //bool hit = Physics.Raycast(gun.position, camera.transform.TransformDirection(Vector3.forward), out rayHit, 100, 1 << 8);
            //if (hit) {
            //    Enemy enemy = rayHit.transform.GetComponent<Enemy>();
            //    if (enemy != null) {
            //        enemy.BeHit(10);
            //    }
            //}
        }
	}

    public override void BeHit(int val) {
        if (!data.isBeHit) return;
        data.curHP -= val;
        if (data.curHP < 0) data.curHP = 0;
        GlobalEventSystem.Fire(new BaseEvent(EventName.PLAYER_BE_HIT, new object[]{ data.curHP, data.maxHP}));
    }

    public void IsBeHit(bool isBeHit) { 
        data.isBeHit = isBeHit;
    }

    public void MoveToPoint(Vector3 pos) {
        // transform.GetComponent<GyroController>().enabled = false;
        transform.DOMove(pos, 3).OnComplete(delegate() {
            // transform.GetComponent<GyroController>().enabled = true;
            GlobalEventSystem.Fire(new BaseEvent(EventName.MOVE_COMPLETE));
        });
    }

    public void ChangeBullet() {
        changeBulletTimer = 0.1f;
        isChangeBullet = true;
		AudioManager.Instance.PlaySound("sound_chg_bullet");
    }

    public void ChangeGun(int index) {
        curGun = index;
        if (index == 0) {
            transform.FindChild("Camera/GunA").gameObject.SetActive(true);
            transform.FindChild("Camera/GunB").gameObject.SetActive(false);
        } else { 
            transform.FindChild("Camera/GunA").gameObject.SetActive(false);
            transform.FindChild("Camera/GunB").gameObject.SetActive(true);
        }
        BulletData data = bullets[curGun];
        GlobalEventSystem.Fire(new BaseEvent(EventName.BULLET_NUM_CHANGE, new object[]{ data.curNum, data.maxNum }));
    }

    public override int GetType()
    {
        return ObjectType.PLAYER;
    }
}
