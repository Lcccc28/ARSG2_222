using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;  

[TaskCategory("Custom")]  
[TaskDescription("出生")]  
public class Born : Action {

	public SharedEnemy obj;

	public override void OnStart()
	{
		obj.Value.PlayAnim("Born", true);
	}

	public override TaskStatus OnUpdate()  
	{
		if (obj.Value.IsPlayAnim("Born"))
			return TaskStatus.Running;
		else
			return TaskStatus.Success;
	}  
}  

