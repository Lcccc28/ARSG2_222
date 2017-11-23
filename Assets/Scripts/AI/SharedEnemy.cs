using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

public class SharedEnemy : SharedVariable<Enemy>
{
    public static implicit operator SharedEnemy(Enemy value) { return new SharedEnemy { Value = value }; }
}