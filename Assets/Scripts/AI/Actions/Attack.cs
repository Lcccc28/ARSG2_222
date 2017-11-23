using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;  
using Game.Manager;
using Game.Util;


[TaskCategory("Custom/Attack")]  
[TaskDescription("攻击")]  
public class Attack : Action {

	//[RequiredField]
	public SharedTransform target;
	//[RequiredField]
    public SharedEnemy obj;

	public string animName = "Attack";
	public int bulletIndex = 0;
    public float randomRange = 1f;
	public float atkTime = 0.01f;

    public List<string> bornTransforms = new List<string>();
	public List<Vector3> bornOffests = new List<Vector3>();

	protected float _atkTime;
	protected bool fire;

    public override void OnStart()
    {
		obj.Value.state = EnemyStatus.Attack;
		_atkTime = atkTime;
		fire = false;
		obj.Value.PlayAnim(animName);
		transform.LookAt (target.Value.position);
    }

    public override TaskStatus OnUpdate()
    {
		_atkTime -= Time.deltaTime;
		if (!fire && _atkTime <= 0) {
			Fire ();
		}
		if (obj.Value.IsPlayAnim(animName))
            return TaskStatus.Running;
        else
            return TaskStatus.Success;
    }

	public virtual void Fire(){
		fire = true;
		if (bornTransforms.Count == 0) {
			Vector3 bornPos = obj.Value.transform.position;
			Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
			SkillManager.Instance.Fire (obj.Value, target.Value.GetComponent<BaseObject>(), bornPos, (target.Value.position - bornPos + random).normalized, obj.Value.data.bulletDatas [bulletIndex]);
		} else {
			int i = 0;
			foreach (string path in bornTransforms) {
				// string path = TransformUtil.GetTransformPath(transform, bornTransform);
				Vector3 bornPos = transform.Find(path).TransformPoint (bornOffests[i++]);
				Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
				SkillManager.Instance.Fire (obj.Value, target.Value.GetComponent<BaseObject>(), bornPos, (target.Value.position - bornPos + random).normalized, obj.Value.data.bulletDatas [bulletIndex]);
			}
		}
	}
    
}

[TaskCategory("Custom/Attack")]  
[TaskDescription("自爆攻击")]  
public class SkillBomb : Attack {

    public override void OnStart()
    {
        obj.Value.state = EnemyStatus.Attack;
        _atkTime = atkTime;
        fire = false;
        obj.Value.PlayAnim(animName);
        //transform.LookAt(target.Value.position);
    }

    public override void Fire()
	{
		fire = true;
		target.Value.GetComponent<Player> ().BeHit (obj.Value.data.bulletDatas [bulletIndex].atk);
		obj.Value.Death(true);
	}

} 


[TaskCategory("Custom/Attack")]  
[TaskDescription("多重技能")]  
public class SkillLoop: Action {

		//[RequiredField]
		public SharedTransform target;
		//[RequiredField]
		public SharedEnemy obj;

		public string animName = "Attack";
		public int bulletIndex = 0;
		public float interval = 1f;
        public float randomRange = 1f;

        public List<string> bornTransforms = new List<string>();
        public List<Vector3> bornOffests = new List<Vector3>();

		private float _interval = 0f;

		public override void OnStart()
		{
			obj.Value.state = EnemyStatus.Attack;
			obj.Value.PlayAnim(animName);
			transform.LookAt (target.Value.position);
		}

		public override TaskStatus OnUpdate()
		{
			if (obj.Value.IsPlayAnim (animName)) {
				_interval -= Time.deltaTime;
				if (_interval < 0) {
					_interval = interval;
					if (bornTransforms.Count == 0) {
						Vector3 bornPos = obj.Value.transform.position;
                        Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
					SkillManager.Instance.Fire (obj.Value, target.Value.GetComponent<BaseObject>(), bornPos, (target.Value.position - bornPos + random).normalized, obj.Value.data.bulletDatas [bulletIndex]);
					} else {
						int i = 0;
						foreach (string path in bornTransforms) {
                            Vector3 bornPos = transform.Find(path).TransformPoint (bornOffests[i++]);
                            Vector3 random = new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
						SkillManager.Instance.Fire (obj.Value, target.Value.GetComponent<BaseObject>(), bornPos, (target.Value.position - bornPos + random).normalized, obj.Value.data.bulletDatas [bulletIndex]);
						}
					}
				} 
			return TaskStatus.Running;
			} else {
				return TaskStatus.Success;
			}
		}

	}  
