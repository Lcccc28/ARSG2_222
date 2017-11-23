using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;  

[TaskCategory("Custom")]  
[TaskDescription("重复累计次数")]  
public class HitCount : Conditional {

	public int count = 1; 
	private int _count = 1; 

	public override TaskStatus OnUpdate()
	{    
		if (_count++ >= count) {
			_count = 0;
			return TaskStatus.Success;
		} else {
			return TaskStatus.Failure;
		}
	}
}  

