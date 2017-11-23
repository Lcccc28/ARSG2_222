using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom")]
[TaskDescription("死亡")]
public class Die : Action
{

    public SharedEnemy obj;

    public override void OnStart()
    {
        obj.Value.PlayAnim("Die");
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}

