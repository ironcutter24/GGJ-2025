using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (!Instance)
            {
                Instance = this as T;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public struct Flag
    {
        public bool State { get; private set; }

        public void Set()
        {
            State = true;
        }

        public bool Pop()
        {
            bool app = State;
            State = false;
            return app;
        }
    }

    [Serializable]
    public struct Range
    {
        public float min, max;

        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetRandom()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    public static class Extensions
    {
        public static Vector3 OverrideX(this Vector3 v, float x)
        {
            v.x = x;
            return v;
        }

        public static Vector3 OverrideY(this Vector3 v, float y)
        {
            v.y = y;
            return v;
        }

        public static Vector3 OverrideZ(this Vector3 v, float z)
        {
            v.z = z;
            return v;
        }

        public static Vector3 SnapPositionToGrid(this Vector3 v, float size)
        {
            v /= size;
            v.x = Mathf.RoundToInt(v.x);
            v.y = Mathf.RoundToInt(v.y);
            v.z = Mathf.RoundToInt(v.z);
            return v * size;
        }

        public static Vector3 SnapPositionToGrid(this Vector3 v, Vector3 size)
        {
            v.x = Mathf.RoundToInt(v.x / size.x) * size.x;
            v.y = Mathf.RoundToInt(v.y / size.y) * size.y;
            v.z = Mathf.RoundToInt(v.z / size.z) * size.z;
            return v;
        }

        public static Quaternion SnapRotationToGrid(this Quaternion rot)
        {
            var r = rot.eulerAngles;
            r.x = GetClosestAngle(r.x);
            r.y = GetClosestAngle(r.y);
            r.z = GetClosestAngle(r.z);
            return Quaternion.Euler(r);


            float GetClosestAngle(float val)
            {
                float[] angles = { 0f, 90f, 180f, 270f };
                val %= 360f;
                return angles.OrderBy(x => Mathf.Abs(x - val)).First();
            }
        }
    }

    public static class Helpers
    {
        public static bool IsInLayerMask(int layer, LayerMask layermask) { return layermask == (layermask | (1 << layer)); }

        public static int CircularClamp(int value, int min, int max)
        {
            int range = Mathf.Abs(max) - Mathf.Abs(min) + 1;
            if (value < min) return max + 1 - (min - value) % range;
            if (value > max) return min - 1 + (value - max) % range;
            return value;
        }

        public static string ConvertSceneNameToInkKnot(string input)
        {
            string[] parts = input.Split('_');
            string lastPart = parts[parts.Length - 1];
            return ConvertCamelToSnake(ConvertCamelToSnake(lastPart));
        }

        public static string ConvertCamelToSnake(string input)
        {
            return Regex.Replace(input, "([a-z])([A-Z])", "$1_$2").ToLower();
        }

        public static float MapRange01(float value, float min, float max)
        {
            return MapRange(value, min, max, 0f, 1f);
        }

        public static float MapRange(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }
    }

}