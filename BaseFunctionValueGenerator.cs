using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFunctionValueGenerator : MonoBehaviour
{
    [SerializeField]
    [Range(10, 500)]
    private int circleRadius = 0;

    private FunctionViewCreator functionViewCreator;

    public int CircleRadius { get => circleRadius; set => circleRadius = value; }

    public Vector3 BaseValueWithoutNoise(float xPos)
    {
        return new Vector3(xPos, BaseFunction(xPos), 0f);
    }

    public Vector3 NegativeBaseValueWithoutNoise(float xPos)
    {
        return new Vector3(xPos, -BaseFunction(xPos), 0f);
    }

    private void OnValidate()
    {
        if (CircleRadius <= 0) CircleRadius = 0;
        if (functionViewCreator == null)
        {
            functionViewCreator = GetComponent<FunctionViewCreator>();
        }
        functionViewCreator.OnValidate();
    }

    public float BaseFunction(float x)
    {
        return (CircleRadius * CircleRadius - x * x) < 0 ? 0f : Mathf.Sqrt(CircleRadius * CircleRadius - x * x);
    }
}