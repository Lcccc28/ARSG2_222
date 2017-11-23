using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom/Info")]
[TaskDescription("设置变量")]
public class SetInfo : Action
{
    public SharedEnemy obj;
	 
	public SharedTransform target;

    public override void OnStart()
    {
        obj.Value = this.Owner.transform.GetComponent<Enemy>();
		target.Value = GameObject.Find ("MainRole").transform;
    }

}

[TaskCategory("Custom/Info")]
[TaskDescription("设置变量(枪专用)")]
public class SetInfoGun : Action
{
	public SharedEnemy obj;

	public SharedTransform target;

	public SharedTransform YRot;
	public SharedTransform XRot;

	public override void OnStart()
	{
		obj.Value = this.Owner.transform.GetComponent<Enemy>();
		target.Value = GameObject.Find ("MainRole").transform;
		YRot.Value = this.Owner.transform.FindChild("Turret");
		XRot.Value = this.Owner.transform.FindChild("Turret/Gun");
	}

}

[TaskCategory("Custom/Var")]
[TaskDescription("设置状态")]
public class SetStatus : Action
{
	public SharedEnemy obj;

	public EnemyStatus status;

	public override void OnStart()
	{
		obj.Value.state = status;
	}

}