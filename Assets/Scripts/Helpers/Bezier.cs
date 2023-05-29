using UnityEngine;

namespace Game.Helpers
{
    public static class Bezier
    {
        public static Vector3 GetPoint(BezierSegmentSettings path, float t)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 point = uuu * path.StartPoint;
            point += 3f * uu * t * path.StartTangent;
            point += 3f * u * tt * path.EndTangent;
            point += ttt * path.EndPoint;

            return point;
        }

        public static float GetLength(BezierSegmentSettings path, int numSegments = 100)
        {
            float length = 0f;
            Vector3 previousPoint = path.StartPoint;

            for (int i = 1; i <= numSegments; i++)
            {
                float t = i / (float)numSegments;
                Vector3 point = GetPoint(path, t);
                length += Vector3.Distance(previousPoint, point);
                previousPoint = point;
            }

            return length;
        }
    }

    public struct BezierSegmentSettings
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
        public Vector3 StartTangent;
        public Vector3 EndTangent;

        public BezierSegmentSettings(Vector3 startPoint, Vector3 endPoint, Vector3 startTangent, Vector3 endTangent)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            StartTangent = startTangent;
            EndTangent = endTangent;
        }
    }
}
