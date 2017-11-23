using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;  

[TaskCategory("Custom/Distance")]  
[TaskDescription("距离判断-范围内")]  
public class InRange : Conditional {
	//[RequiredField]  
	public SharedTransform target;  

	//[RequiredField]
	public SharedFloat distance;

	public override TaskStatus OnUpdate()  
	{  
		if (Vector3.Distance(transform.position, target.Value.position) < distance.Value)
			return TaskStatus.Success;
		else
			return TaskStatus.Failure;
	}  
}  

[TaskCategory("Custom/Distance")]  
[TaskDescription("距离判断-范围外")]  
public class OutRange : Conditional {
	//[RequiredField]  
	public SharedTransform target;  

	//[RequiredField]
	public SharedFloat distance;

	public override TaskStatus OnUpdate()  
	{  
		if (Vector3.Distance(transform.position, target.Value.position) < distance.Value)
			return TaskStatus.Failure;
		else
			return TaskStatus.Success;
	}  
}  
