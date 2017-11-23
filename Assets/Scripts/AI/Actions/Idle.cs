using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;  

[TaskCategory("Custom")]  
[TaskDescription("待机")]  
public class Idle : Action {

    public SharedEnemy obj;

    public override void OnStart()
    {
		obj.Value.state = EnemyStatus.Stand;
        obj.Value.PlayAnim("Stand");
    }

    public override TaskStatus OnUpdate()  
	{    
		return TaskStatus.Success;
	}  
}  

