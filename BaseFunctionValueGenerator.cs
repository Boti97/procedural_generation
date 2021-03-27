using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BaseFunctionValueGenerator
{
    public static Vector3 GetBasePosition(float xPos, float circleRadius)
    {
        return new Vector3(xPos, BaseFunction(xPos, circleRadius), 0f);
    }

    public static Vector3 GetNegativeBasePosition(float xPos, float circleRadius)
    {
        return new Vector3(xPos, -BaseFunction(xPos, circleRadius), 0f);
    }

    public static float BaseFunction(float x, float circleRadius)
    {
        return (circleRadius * circleRadius - x * x) < 0 ? 0f : Mathf.Sqrt(circleRadius * circleRadius - x * x);
    }
}