using System;
using System.Collections;
using UnityEngine;

public class Common : MonoBehaviour
{
    protected Vector3 ScaleTransform(Vector3 start, Vector3 end, float time, float maxTime, AnimationCurve curve)
    {
        float t = curve.Evaluate(time / maxTime);
        if (t > 1f) t = 1f;
        if (t < 0f) t = 0f;
        return (end - start) * t + start;
    }

    protected Vector3 LoopScaleTransform(Vector3 start, Vector3 end, float time, float maxTime)
    {
        if((int)(time / maxTime) % 2 == 0)
        {
            return start;
        }
        return end;
    }

    protected T GetNearest<T>(float diff)
    {
        T result = default(T);
        if(diff < 0f) diff = 0f;

        foreach (T target in GameManager.instance.pool.GetComponentsInChildren<T>())
        {
            MonoBehaviour obj = target as MonoBehaviour;
            float dis = Vector3.Distance(transform.position, obj.transform.position);
            if (dis < diff)
            {
                diff = dis;
                result = target;
            }
        }

        return result;

    }

}
