
using UnityEngine;

public static class MLinear
{
    public static float Approach(float start, float target, float step)
    {
        if (start > target)
            return Mathf.Max(start - step, target);
        if (start < target)
            return Mathf.Min(start + step, target);
        return target;
    }
}