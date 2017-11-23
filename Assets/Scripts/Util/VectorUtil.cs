using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Util
{
    class VectorUtil
    {
        public static Vector3 FreezePosition(Vector3 targetPoint, Vector3 sourcePoint, Vector3 freezePosition)
        {
            Vector3 res = new Vector3();
            res.x = freezePosition.x != 0 ? sourcePoint.x : targetPoint.x;
            res.y = freezePosition.y != 0 ? sourcePoint.y : targetPoint.y;
            res.z = freezePosition.z != 0 ? sourcePoint.z : targetPoint.z;
            return res;
        }

    }
}
