using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;  

[TaskCategory("Custom")]  
[TaskDescription("技能冷却中")]  
public class AttackInterval : Conditional {

	public SharedEnemy obj;
	public int index = 0; 

	public override TaskStatus OnUpdate()  
	{    
		if (obj.Value.data.bulletDatas [index].cd <= 0f){
			return TaskStatus.Success;
		} else {
			obj.Value.data.bulletDatas [index].cd -= Time.deltaTime;
			return TaskStatus.Failure;
		}
	}  
}  

