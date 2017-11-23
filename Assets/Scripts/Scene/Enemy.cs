using UnityEngine;
using System.Collections;
using Game.Manager;
using Game.Core;
using Game.Const;
using BehaviorDesigner.Runtime;

public class Enemy : BaseObject  {

    public EnemyData data;

    protected Animation anim;

	public EnemyStatus state;

    public void Init() {
        data = new EnemyData();
        anim = GetComponentInChildren<Animation>();
    }

    public override bool IsDeath() {
        return data.curHP <= 0;
    }

    public void PlayAnim(string name, bool isImmediately = false) {
		if (isImmediately) {
			anim.Stop ();
			anim.Play (name);
		} else {
			if (IsPlayAnim(name) && anim.GetClip(name).wrapMode == WrapMode.Loop)
				return;
			anim.CrossFade (name, 0.5f);
		}

    }

    public bool IsPlayAnim(string name)
    {
        return anim.IsPlaying(name);
    }

    public override void Death(bool isSuicide)
    {
        Death(isSuicide, 0.5f);
    }

    public virtual void Death(bool isSuicide, float destroyTime)
    {
		state = EnemyStatus.Die;
        GameObject.Destroy(gameObject, destroyTime);
        GlobalEventSystem.Fire(new BaseEvent(EventName.MONS_DEATH, new object[]{ this, isSuicide }));
    }


    public override int GetType()
    {
        return ObjectType.MONS;
    }
}
