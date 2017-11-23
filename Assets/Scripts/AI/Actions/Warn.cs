using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

[TaskCategory("Custom")]
[TaskDescription("预警")]
public class Warn : Action
{

    //[RequiredField]
    public SharedTransform target;

    public SharedEnemy obj;

	public string animName = "Warn";

    public override void OnStart()
    {
		obj.Value.PlayAnim(animName);
        transform.DOLookAt(target.Value.position, 2);
    }

	public override TaskStatus OnUpdate()
    {
		if (obj.Value.IsPlayAnim(animName))
            return TaskStatus.Running;
        else
            return TaskStatus.Success;
    }

}
