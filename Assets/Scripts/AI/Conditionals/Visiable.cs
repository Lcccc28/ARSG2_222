using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom/Camera")]
[TaskDescription("是否在屏幕内")]
public class IsCameraVisiable : Conditional
{
    private Camera camera;

    public override void OnStart()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public override TaskStatus OnUpdate()
    {
        Vector3 pos = camera.WorldToViewportPoint(transform.position);
        bool isVisible = (camera.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
        if (isVisible)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}


