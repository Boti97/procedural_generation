using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    [SerializeField]
    [Range(0, 5)]
    private float noiseSeed = 0f;

    [SerializeField]
    [Range(1, 50)]
    private float noiseOffset = 0f;

    [SerializeField]
    [Range(0, 2)]
    private float noiseAmplitude = 0f;

    [SerializeField]
    [Range(0, 0.2f)]
    private float noiseRoughness = 0f;

    private BaseFunctionValueGenerator baseValueGenerator;
    private FunctionViewCreator functionViewCreator;

    private void OnValidate()
    {
        if (baseValueGenerator == null)
        {
            baseValueGenerator = gameObject.GetComponent<BaseFunctionValueGenerator>();
        }
        if (functionViewCreator == null)
        {
            functionViewCreator = GetComponent<FunctionViewCreator>();
        }
        functionViewCreator.OnValidate();
    }

    public float GenerateNoise(float xPos)
    {
        return noiseOffset + (noiseAmplitude * GenerateFirstLayerNoise(xPos) * GenerateSecondLayerNoise(xPos));
    }

    private float GenerateFirstLayerNoise(float xPos)
    {
        return Mathf.Sin(xPos * noiseRoughness);
    }

    private float GenerateSecondLayerNoise(float xPos)
    {
        return Mathf.PerlinNoise(noiseSeed, baseValueGenerator.BaseFunction(xPos * noiseRoughness));
    }
}