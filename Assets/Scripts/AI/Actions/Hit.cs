using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;  
using BehaviorDesigner.Runtime.Tasks;  

[TaskCategory("Custom")]  
[TaskDescription("受击")]  
public class Hit : Action {

    public SharedEnemy obj;

    public override void OnStart()
    {
		if (obj.Value.state == EnemyStatus.Stand) {
			obj.Value.state = EnemyStatus.Hit;
			obj.Value.PlayAnim ("Hit", true);
		}
    }

    public override TaskStatus OnUpdate()  
	{
        if (obj.Value.IsPlayAnim("Hit"))
            return TaskStatus.Running;
        else
            return TaskStatus.Success;
	}  
}  

