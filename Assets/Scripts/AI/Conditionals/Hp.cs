using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom/Hp")]
[TaskDescription("是否存活")]
public class IsAlive : Conditional
{
	//[RequiredField]  
	public SharedEnemy obj;

	public override TaskStatus OnUpdate()
	{
		if (obj.Value.data.curHP > 0)
			return TaskStatus.Success;
		else
			return TaskStatus.Failure;
	}
}

[TaskCategory("Custom/Hp")]
[TaskDescription("自身血量小于")]
public class SelfHpLess : Conditional
{
    //[RequiredField]  
    public SharedEnemy obj;
    public float hp;

    public override TaskStatus OnUpdate()
    {
        if (obj.Value.data.curHP < hp)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}

[TaskCategory("Custom/Hp")]
[TaskDescription("自身血量小于触发")]
public class SelfHpLessTri : Conditional
{
	//[RequiredField]  
	public SharedEnemy obj;
	public float hp;

	private bool boolean = true;

	public override TaskStatus OnUpdate()
	{
		if (obj.Value.data.curHP <= hp && boolean) {
			boolean = false;
			return TaskStatus.Success;
		} else {
			return TaskStatus.Failure;
		}
	}
}

[TaskCategory("Custom/Hp")]
[TaskDescription("自身血量小于百分比触发")]
public class SelfHpPerLessTri : Conditional
{
    //[RequiredField]  
    public SharedEnemy obj;
    public float hpPer;


    public bool boolean = true;

    public override TaskStatus OnUpdate()
    {
		if ((float)obj.Value.data.curHP / (float)obj.Value.data.maxHP <= hpPer && boolean)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}

[TaskCategory("Custom/Con")]
[TaskDescription("自身血量小于百分比标记")]
public class SelfHpPerLessTriDone : Action
{
	//[RequiredField]  
	public SelfHpPerLessTri task;

	public override TaskStatus OnUpdate()
	{
		task.boolean = false;
		return TaskStatus.Success;
	}
}

[TaskCategory("Custom/Hp")]
[TaskDescription("目标血量小于")]
public class TargetHpLess : Conditional
{
    //[RequiredField]  
    public SharedTransform target;

    //[RequiredField]
    public SharedFloat hp;

    public override TaskStatus OnUpdate()
    {
        //if (target.Value.data.curHP < hp.Value)
        //    return TaskStatus.Success;
        //else
            return TaskStatus.Failure;
    }
}