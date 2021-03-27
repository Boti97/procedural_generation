using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    public static float GenerateNoise(float xPos, float noiseOffset, float noiseAmplitude, float noiseRoughness, float noiseSeed, float circleRadius)
    {
        return noiseOffset + (noiseAmplitude * GenerateFirstLayerNoise(xPos, noiseRoughness) * GenerateSecondLayerNoise(xPos, noiseSeed, noiseRoughness, circleRadius));
    }

    private static float GenerateFirstLayerNoise(float xPos, float noiseRoughness)
    {
        return Mathf.Sin(xPos * noiseRoughness);
    }

    private static float GenerateSecondLayerNoise(float xPos, float noiseSeed, float noiseRoughness, float circleRadius)
    {
        return Mathf.PerlinNoise(noiseSeed, BaseFunctionValueGenerator.BaseFunction(xPos * noiseRoughness, circleRadius));
    }
}