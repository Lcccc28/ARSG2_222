using UnityEngine;
using System.Collections;
using Game.Core;
using Game.Const;
using Game.Manager;
using BehaviorDesigner.Runtime;

public class EnemyA : Enemy {

    void Awake() {
        Init();
		BulletData bdata = new BulletData ();
		bdata.modelName = "BulletD";
		bdata.atk = 15;
		bdata.num = 1;
		bdata.sound = "sound_pulsegun";
		bdata.interval = 0.1f;
		bdata.maxNum = 30;
		bdata.curNum = 30;
		bdata.cdData = 5;
		bdata.cd = 3;

		BulletData bdata2 = new BulletData ();
		bdata2.modelName = "BulletE";
		bdata.atk = 15;
		bdata2.num = 1;
		bdata2.sound = "sound_rocket_loop2";
		bdata2.interval = 0.1f;
        bdata2.maxNum = 30;
		bdata2.curNum = 30;
		bdata2.cdData = 10;
		bdata2.cd = 10;
		bdata2.script = "BulletParapola";

		data.bulletDatas = new BulletData[2] { bdata, bdata2 };

    }
    
    public override void BeHit(int val) {
        if (IsDeath()) return;
        data.curHP -= val;
        if (data.curHP <= 0) {
            Death(false);
        } else {
            GetComponent<BehaviorTree>().SendEvent("Hit");
        }
    }

    public override void Death(bool isSuicide) {
        GetComponent<BehaviorTree>().SendEvent("Die");
		GameObject hit = GamePoolManager.Instance.Spawn("Explosion").gameObject;
        hit.SetActive(true);
		hit.transform.position = this.transform.position + new Vector3(0, (transform.FindChild("HpBar").position.y - transform.position.y) / 2f);
        GamePoolManager.Instance.Despawn(hit.transform, 3f);
        base.Death(isSuicide);
        
    }
}
