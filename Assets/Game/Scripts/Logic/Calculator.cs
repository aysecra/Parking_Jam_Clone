using System;
using ParkingJamClone.Structs;
using UnityEngine;

namespace ParkingJamClone.Logic
{
    public static class Calculator
    {
        // Möller-Trumbore algorithm
        public static bool TriangleIntersect(Vector3 p1, Vector3 p2, Vector3 p3,
            ScreenToWorldPointData screenToWorldPoint)
        {
            Vector3 e1, e2;

            Vector3 p, q, t;
            float det, invDet, u, v;

            e1 = p2 - p1;
            e2 = p3 - p1;

            p = Vector3.Cross(screenToWorldPoint.Direction, e2);

            det = Vector3.Dot(e1, p);

            if (det > -Double.Epsilon && det < Double.Epsilon)
            {
                return false;
            }

            invDet = 1.0f / det;

            t = screenToWorldPoint.Origin - p1;

            u = Vector3.Dot(t, p) * invDet;

            if (u < 0 || u > 1)
            {
                return false;
            }

            q = Vector3.Cross(t, e1);

            v = Vector3.Dot(screenToWorldPoint.Direction, q) * invDet;

            if (v < 0 || u + v > 1)
            {
                return false;
            }

            if ((Vector3.Dot(e2, q) * invDet) > Double.Epsilon)
            {
                return true;
            }

            return false;
        }

        public static Vector3 LocalPointToWorld(Vector3 p, Transform objectTransform) => objectTransform.localToWorldMatrix.MultiplyPoint3x4(p);
        
        private static Vector3 SolveEulerAngle(Transform objectTransform)
        {
            Vector3 result = objectTransform.eulerAngles;
            Vector3 prevAngle = result;


            if (Vector3.Dot(objectTransform.up, Vector3.up) >= 0f)
            {
                if (prevAngle.x >= 0f && prevAngle.x <= 90f)
                {
                    result.x = prevAngle.x;
                }

                if (prevAngle.x >= 270f && prevAngle.x <= 360f)
                {
                    result.x = prevAngle.x - 360f;
                }
            }

            if (Vector3.Dot(objectTransform.up, Vector3.up) < 0f)
            {
                if (prevAngle.x >= 0f && prevAngle.x <= 90f)
                {
                    result.x = 180 - prevAngle.x;
                }

                if (prevAngle.x >= 270f && prevAngle.x <= 360f)
                {
                    result.x = 180 - prevAngle.x;
                }
            }
            return result;
        }
        
        public static Vector3 EulerAngleToSinVector(Transform objectTransform)
        {
            var angle = SolveEulerAngle(objectTransform) * Mathf.Deg2Rad;
            Vector3 sinRad = new Vector3(Mathf.Sin(angle.x), Mathf.Sin(angle.y), Mathf.Sin(angle.z));
            return sinRad;
        }

        public static Vector3 EulerAngleToCosVector(Transform objectTransform)
        {
            var angle = SolveEulerAngle(objectTransform) * Mathf.Deg2Rad;
            Vector3 cosRad = new Vector3(Mathf.Cos(angle.x), Mathf.Cos(angle.y), Mathf.Cos(angle.z));
            return cosRad;
        }
    }
}