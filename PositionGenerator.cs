using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionGenerator : MonoBehaviour
{
    private BaseFunctionValueGenerator baseValueGenerator;
    private NoiseGenerator noiseGenerator;

    private void OnValidate()
    {
        if (baseValueGenerator == null)
        {
            baseValueGenerator = gameObject.GetComponent<BaseFunctionValueGenerator>();
        }
        if (noiseGenerator == null)
        {
            noiseGenerator = gameObject.GetComponent<NoiseGenerator>();
        }
    }

    public Vector3 GetPositionWithNoise(float xPos)
    {
        return baseValueGenerator.BaseValueWithoutNoise(xPos) * (noiseGenerator.GenerateNoise(xPos));
    }

    public Vector3 GetNegativePositionWithNoise(float xPos)
    {
        return baseValueGenerator.NegativeBaseValueWithoutNoise(xPos) * (noiseGenerator.GenerateNoise(xPos));
    }

    public float GetDrawFromXPos()
    {
        return -baseValueGenerator.CircleRadius;
    }

    public float GetDrawUntilXPos()
    {
        return baseValueGenerator.CircleRadius;
    }
}