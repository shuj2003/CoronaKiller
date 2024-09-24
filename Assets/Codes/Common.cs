using System;
using System.Collections;
using UnityEngine;

public class Common : MonoBehaviour
{
    protected Vector3 ScaleTransform(Vector3 start, Vector3 end, float time, float maxTime, AnimationCurve curve)
    {
        float t = curve.Evaluate(time / maxTime);
        if (t > 1f) t = 1f;
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

}
