using UnityEngine;
using System.Collections.Generic;

namespace Game.Util
{
    public class Bezier
    {

        public List<Vector3> vertexs;

        //vertexCount:值越大则越光滑
        public Bezier(float vertexCount, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            vertexs = new List<Vector3>();

            float interval = 1 / vertexCount;
            for (int i = 0; i < vertexCount; i++)
            {
                vertexs.Add(GetPoint(i * interval, p0, p1, p2));
            }
        }

        //vertexCount:值越大则越光滑
        public Bezier(float vertexCount, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            vertexs = new List<Vector3>();

            float interval = 1 / vertexCount;
            for (int i = 0; i < vertexCount; i++)
            {
                vertexs.Add(GetPoint(i * interval, p0, p1, p2, p3));
            }
        }

        //t在[0,1]范围
        private Vector3 GetPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float a = 1 - t;
            Vector3 target = p0 * Mathf.Pow(a, 2) + 2 * p1 * t * a + p2 * Mathf.Pow(t, 2);
            return target;
        }

        //t在[0,1]范围
        private Vector3 GetPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float a = 1 - t;
            Vector3 target = p0 * Mathf.Pow(a, 3) + 3 * p1 * t * Mathf.Pow(a, 2) + 3 * p2 * Mathf.Pow(t, 2) * a + p3 * Mathf.Pow(t, 3);
            return target;
        }

    }
}