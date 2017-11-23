using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;  

[TaskCategory("Custom/Var")]  
[TaskDescription("设技能cd")]  
public class SetCD : Action {

	public SharedEnemy obj;
	public int index = 0; 

	public override TaskStatus OnUpdate()  
	{    
		obj.Value.data.bulletDatas [index].cd = obj.Value.data.bulletDatas [index].cdData;
		return TaskStatus.Success;
	}  
}  

