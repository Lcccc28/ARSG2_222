using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Game.Util
{
    class TransformUtil
    {

        // 获取绝对transform路径
        public static string GetTransformPath(Transform transform)
        {
            if (transform == null)
                return "";
            return (GetTransformPath(transform.parent) + "/" + transform.name).TrimStart('/');
        }

        // 获取相对transform路径
        public static string GetTransformPath(Transform root, Transform transform)
        {
            if (transform == null || transform.GetInstanceID() == root.GetInstanceID())
                return "";
            return (GetTransformPath(root, transform.parent) + "/" + transform.name).TrimStart('/');
        }
    }
}
