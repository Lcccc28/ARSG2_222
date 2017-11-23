using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

[TaskCategory("Custom/Gun/Move")]  
[TaskDescription("枪瞄准")]  
public class GunLock : Action {  

	//[RequiredField]
	public SharedTransform target;

	public SharedEnemy obj;

	public SharedTransform YRot;
	public SharedTransform XRot;


    private Vector3 XPos;

	private bool complete;

	public override void OnStart()
	{
		complete = false;
		Vector3 YPos = new Vector3 (target.Value.position.x, YRot.Value.position.y, target.Value.position.z);
        XPos = new Vector3(target.Value.position.x, target.Value.position.y, target.Value.position.z); // 这里先存着，人可以跑动，枪管不会歪
        YRot.Value.DOLookAt(YPos, 1.5f).OnComplete(OnComplete);
		
	}

	void OnComplete()
	{
        XRot.Value.DOLookAt(XPos, 1f).OnComplete(OnComplete2);
	}

    void OnComplete2()
    {
        complete = true;
    }

    public override TaskStatus OnUpdate()
	{
		if (complete)
			return TaskStatus.Success;
		else
			return TaskStatus.Running;
	}
			  
}

[TaskCategory("Custom/Gun/Move")]  
[TaskDescription("枪摆动")]  
public class GunShakeX : Action {  

	//[RequiredField]
	public SharedTransform target;

	public SharedEnemy obj;

	public SharedTransform YRot;
	public SharedTransform XRot;

	private bool complete;

	public override void OnStart()
	{
		Vector3 XPos = XRot.Value.TransformPoint (Vector3.forward);
		XPos = new Vector3(XPos.x, XRot.Value.position.y, XPos.z);
		XRot.Value.DOLookAt(XPos, 2).SetEase(Ease.OutBounce).OnComplete(OnComplete);
	}

	void OnComplete()
	{
		complete = true;
	}

	public override TaskStatus OnUpdate()
	{
		if (complete)
			return TaskStatus.Success;
		else
			return TaskStatus.Running;
	}

}

[TaskCategory("Custom/Gun/Attack")]
[TaskDescription("枪攻击")]
public class GunAttack : Attack
{

    public override void OnStart()
    {
        obj.Value.state = EnemyStatus.Attack;
        _atkTime = atkTime;
        fire = false;
        obj.Value.PlayAnim(animName);
        //transform.LookAt(target.Value.position);
    }


}