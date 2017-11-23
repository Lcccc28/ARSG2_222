using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using Game.Util;
using Game.Manager;

[TaskCategory("Custom/Move")]  
[TaskDescription("追踪")]  
public class MoveTrace : Action {  
	public float speed = 0;  
	//[RequiredField]  
	public SharedTransform target;
    //[RequiredField]  
    public SharedEnemy obj;

    public override void OnStart()
    {
		obj.Value.state = EnemyStatus.Move;
		obj.Value.PlayAnim("Move");
    }

    public override TaskStatus OnUpdate()  
	{
        Vector3 look = new Vector3(target.Value.position.x - transform.position.x, 0.2f, target.Value.position.z - transform.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(look), 5 * Time.deltaTime);
		transform.position += transform.forward * (speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        return TaskStatus.Running;
	}  
}



[TaskCategory("Custom/Move")]
[TaskDescription("移动到镜头内")]
public class MoveToCamera : Action
{
    public float speed = 3f;
    public float range = 5f;
    public Vector3 freezePosition = new Vector3();

    public Vector3 fstOffset = new Vector3();
    public Vector3 sndOffset = new Vector3();
    public Vector3 thiOffset = new Vector3();
    public AxisConstraint lockPos = AxisConstraint.None;
    public AxisConstraint lockRot = AxisConstraint.None;

    //[RequiredField]  
    public SharedTransform target;
    //[RequiredField]  
    public SharedEnemy obj;
    
    private bool complete = false;

    public override void OnStart()
    {
		obj.Value.state = EnemyStatus.Move;
		obj.Value.PlayAnim("Move");
		Vector3 monRandomPoint = target.Value.FindChild("MonRandomPoint").position;
		monRandomPoint = new Vector3(Random.Range(-range, range)+ monRandomPoint.x, Random.Range(-range, range) + monRandomPoint.y, Random.Range(0, range) + monRandomPoint.z);
        
        Vector3 fst = VectorUtil.FreezePosition(target.Value.position + fstOffset, transform.position, freezePosition);
        Vector3 snd = VectorUtil.FreezePosition(-transform.position + monRandomPoint + sndOffset, transform.position, freezePosition);
        Vector3 thi = VectorUtil.FreezePosition(monRandomPoint + thiOffset, transform.position, freezePosition);
        Bezier bezier = new Bezier(100, transform.position, fst, snd, thi);
        
        complete = false;
        float duration = Vector3.Distance(monRandomPoint, transform.position) / speed;
		transform.DOPath(bezier.vertexs.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.red).SetEase(Ease.OutQuad).SetOptions(lockPos, lockRot).SetLookAt(1f).OnComplete(OnComplete);
    }


    void OnComplete()
    {
        complete = true;
        transform.DOLookAt(target.Value.position, 2);
    }

    public override TaskStatus OnUpdate()
    {
		if (complete) {
			return TaskStatus.Success;
		}
		transform.LookAt (transform.TransformPoint (Vector3.forward), Vector3.up);
        return TaskStatus.Running;

    }

}

[TaskCategory("Custom/Move")]
[TaskDescription("从地下移动到镜头内")]
public class MoveDownToCamera : Action
{
    public float speed = 3f;
    public float range = 5f;
    public Vector3 freezePosition = new Vector3();

    public Vector3 fstOffset = new Vector3();
    public Vector3 sndOffset = new Vector3();
    public Vector3 thiOffset = new Vector3();
    public AxisConstraint lockPos = AxisConstraint.None;
    public AxisConstraint lockRot = AxisConstraint.None;

    //[RequiredField]  
    public SharedTransform target;
    //[RequiredField]  
    public SharedEnemy obj;

    private bool complete = false;
    private Vector3 lookAtPos;

    public override void OnStart()
    {
		obj.Value.state = EnemyStatus.Move;
        obj.Value.PlayAnim("Move");
		Vector3 monRandomPoint = target.Value.FindChild("MonRandomPoint").position;
		monRandomPoint = new Vector3(Random.Range(-range, range) + monRandomPoint.x, Random.Range(-range/2f, range/2f) + monRandomPoint.y, Random.Range(0, range) + monRandomPoint.z);

        Vector3 fst = VectorUtil.FreezePosition(target.Value.position + fstOffset, transform.position, freezePosition);
        Vector3 thi = VectorUtil.FreezePosition(monRandomPoint + thiOffset, transform.position, freezePosition);
        Bezier bezier = new Bezier(100, transform.position, fst, thi);

        complete = false;
        float duration = Vector3.Distance(monRandomPoint, transform.position) / speed;
        lookAtPos = new Vector3(thi.x, thi.y, 0);
		transform.DOPath(bezier.vertexs.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.red).SetEase(Ease.OutQuad).SetOptions(lockPos, lockRot).OnComplete(OnComplete);
    }


    void OnComplete()
    {
        transform.DOLookAt(target.Value.position, 2);
        complete = true;
    }

    public override TaskStatus OnUpdate()
    {
        transform.LookAt(new Vector3(lookAtPos.x, transform.position.y, 0));
        if (complete)
            return TaskStatus.Success;
        return TaskStatus.Running;

    }

}



[TaskCategory("Custom/Move")]
[TaskDescription("移动到角色后")]
public class MoveToBack : Action
{
	public float speed = 3f;
	public float range = 5f;
    public Vector3 freezePosition = new Vector3();

    public Vector3 fstOffset = new Vector3();
    public Vector3 sndOffset = new Vector3();
    public Vector3 thiOffset = new Vector3();

    //[RequiredField]  
    public SharedTransform target;
	//[RequiredField]  
	public SharedEnemy obj;

	private bool complete = false;

	public override void OnStart()
	{
		obj.Value.state = EnemyStatus.Move;
		obj.Value.PlayAnim("Move");
		Vector3 rand = new Vector3 (Random.Range (-range, range), Random.Range (-range, range), Random.Range (-range, range));

        Vector3 fst = VectorUtil.FreezePosition(target.Value.position + fstOffset, transform.position, freezePosition);
        Vector3 snd = fst + sndOffset;
        Vector3 thi = VectorUtil.FreezePosition(-target.Value.forward * 20 + thiOffset, transform.position, freezePosition);
        Bezier bezier = new Bezier(100, transform.position, fst, snd, thi);
        
		complete = false;
		float duration = Vector3.Distance(-transform.position, transform.position) / speed;
		transform.DOPath(bezier.vertexs.ToArray(), duration, PathType.Linear, PathMode.Full3D, 10, Color.red).SetEase(Ease.InOutCirc).SetLookAt(0.01f).OnComplete(OnComplete);
	}


	void OnComplete()
	{
		complete = true;
	}

	public override TaskStatus OnUpdate()
	{
		if (complete) {
			obj.Value.Death(true);
			return TaskStatus.Success;
		}
		return TaskStatus.Running;

	}

}

[TaskCategory("Custom/Move")]
[TaskDescription("往上移动")]
public class MoveUp : Action
{
	public float speed = 3f;
	public float distance = 50;

	//[RequiredField]  
	public SharedEnemy obj;

	private bool complete = false;

	public override void OnStart()
	{
		obj.Value.state = EnemyStatus.Move;
		obj.Value.PlayAnim("Move");
		complete = false;
		float duration = distance / speed;
		transform.DOMove (transform.position + new Vector3(0, distance), duration).SetEase(Ease.InCirc).OnComplete(OnComplete);
	}


	void OnComplete()
	{
		complete = true;
	}

	public override TaskStatus OnUpdate()
	{
		if (complete) {
			obj.Value.Death(true);
			return TaskStatus.Success;
		}
		return TaskStatus.Running;

	}

}


[TaskCategory("Custom/Move")]
[TaskDescription("朝向不转，不播动画，随机平移（z轴不变）")]
public class MoveLockRotRand : Action
{
	public float speed = 7f;
	public float range = 10f;

    //[RequiredField]  
    public SharedTransform target;
    //[RequiredField]  
    public SharedEnemy obj;

	private bool complete = false;

	public override void OnStart()
	{
		obj.Value.state = EnemyStatus.Move;
//		obj.Value.PlayAnim("Move");
		Vector3 monRandomPoint = target.Value.FindChild("MonRandomPoint").position;
		monRandomPoint = new Vector3(Random.Range(-range, range)+ monRandomPoint.x, Random.Range(-range, range) + monRandomPoint.y, monRandomPoint.z);


		complete = false;
		float duration = Vector3.Distance(transform.position, monRandomPoint) / speed;
		transform.DOMove (monRandomPoint, duration).SetEase(Ease.OutBack).OnComplete(OnComplete);
	}


	void OnComplete()
	{
        transform.DOLookAt(target.Value.position, 2);
        complete = true;
	}

	public override TaskStatus OnUpdate()
	{
		if (complete) 
			return TaskStatus.Success;
		return TaskStatus.Running;

	}

}



[TaskCategory("Custom/Move")]
[TaskDescription("随机平移(Y轴固定)")]
public class MoveRand : Action
{
	public float speed = 7f;
	public float range = 10f;
	public Vector3 monRandomPoint;
    public string animName = "Move";

	//[RequiredField]  
	public SharedTransform target;
	//[RequiredField]  
	public SharedEnemy obj;

	private bool complete = false;

	public override void OnStart()
	{
		obj.Value.state = EnemyStatus.Move;
		obj.Value.PlayAnim(animName);
		Vector3 point = new Vector3(Random.Range(-range, range) + monRandomPoint.x, monRandomPoint.y, Random.Range(-range, range) + monRandomPoint.z);

		complete = false;
		float duration = Vector3.Distance(transform.position, point) / speed;
		transform.DOLookAt(point, duration);
		transform.DOMove (point, duration).OnComplete(OnComplete);
	}


	void OnComplete()
	{
		transform.DOLookAt(new Vector3(target.Value.position.x, monRandomPoint.y, target.Value.position.z), 0.3f);
		complete = true;
	}

	public override TaskStatus OnUpdate()
	{
		if (complete) 
			return TaskStatus.Success;
		if (!obj.Value.IsPlayAnim (animName)) {
			transform.DOKill (false);
			OnComplete();
		}
		return TaskStatus.Running;

	}

}


[TaskCategory("Custom/Move")]
[TaskDescription("随机瞬移(z轴固定)")]
public class MoveTeleportRand : Action
{
    public float range = 10f;
    public float time = 1f;
    public Vector3 offset;

    //[RequiredField]  
    public SharedTransform target;
    //[RequiredField]  
    public SharedEnemy obj;

    private float _time;
    public override void OnStart()
    {
		obj.Value.state = EnemyStatus.Move;
        _time = time;
        GameObject sourceEff = GamePoolManager.Instance.Spawn("TelePort").gameObject;
        sourceEff.SetActive(true);
        sourceEff.transform.position = this.transform.position + offset;
        GamePoolManager.Instance.Despawn(sourceEff.transform, 3);
        AudioManager.Instance.PlaySound("sound_teleport");
        transform.position += new Vector3(1000,1000,1000);

    }

    public override TaskStatus OnUpdate()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            Vector3 monRandomPoint = target.Value.FindChild("MonRandomPoint").position;
            Vector3 point = new Vector3(Random.Range(-range, range) + monRandomPoint.x, Random.Range(-range / 2f, range / 2f) + monRandomPoint.y, monRandomPoint.z);

            transform.position = point;
            transform.LookAt(target.Value.position);

            GameObject targetEff = GamePoolManager.Instance.Spawn("TelePort").gameObject;
            targetEff.SetActive(true);
            targetEff.transform.position = this.transform.position + offset;
            GamePoolManager.Instance.Despawn(targetEff.transform, 3);
            AudioManager.Instance.PlaySound("sound_teleport");

            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

}


[TaskCategory("Custom/Move")]
[TaskDescription("巡逻(Y轴固定)")]
public class MoveParot : Action
{
    public float speed = 7f;
    public float range = 10f;
    public float waitTime = 3f;
    public Vector3 monRandomPoint;
    public string idleAnimName = "Stand";
    public string moveAnimName = "Move";



    //[RequiredField]  
    public SharedEnemy obj;

    private bool complete = false;
    private float _waitTime;
    private Vector3 point;

    public override void OnStart()
    {
        _waitTime = waitTime;
        complete = true;
    }


    void OnComplete()
    {
        complete = true;
        _waitTime = waitTime;
        obj.Value.PlayAnim(idleAnimName);

    }

    void OnRotComplete()
    {
        float duration = Vector3.Distance(transform.position, point) / speed;
        transform.DOMove(point, duration).OnComplete(OnComplete);
    }

    public override TaskStatus OnUpdate()
    {
        if (complete)
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime <= 0)
            {
				obj.Value.state = EnemyStatus.Move;
                obj.Value.PlayAnim(moveAnimName);
				complete = false;
                point = new Vector3(Random.Range(-range, range) + monRandomPoint.x, monRandomPoint.y, Random.Range(-range, range) + monRandomPoint.z);
                transform.DOLookAt(point, 1f).OnComplete(OnRotComplete);
            }
        }
        return TaskStatus.Running;

    }

}