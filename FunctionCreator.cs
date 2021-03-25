using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject LinePrefab;

    [SerializeField]
    [Range(7, 699)]
    private int lineRendererDivisionNum = 0;

    [SerializeField]
    private float drawFromXPos = 0f;

    [SerializeField]
    private float drawUntilXPos = 0f;

    [SerializeField]
    private int circleRadius = 0;

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

    private float totalDrawAreaXLength = 0f;
    private float increaseValueOnX = 0f;

    private LineRenderer xAxis;
    private LineRenderer yAxis;
    private LineRenderer functionView;
    private MeshCollider functionViewMesh;

    private void OnValidate()
    {
        totalDrawAreaXLength = Mathf.Abs(drawFromXPos) + Mathf.Abs(drawUntilXPos);
        Debug.LogWarning("The full length of the draw area on the x axis: " + totalDrawAreaXLength);

        increaseValueOnX = totalDrawAreaXLength / ((lineRendererDivisionNum / 2) - 1);
        Debug.LogWarning("The value which the x axis value will be increased with: " + increaseValueOnX);

        if (xAxis == null)
        {
            xAxis = Instantiate(LinePrefab).AddComponent<LineRenderer>();
        }
        if (yAxis == null)
        {
            yAxis = Instantiate(LinePrefab).AddComponent<LineRenderer>();
        }
        if (functionView == null)
        {
            functionView = Instantiate(LinePrefab).AddComponent<LineRenderer>();
            functionViewMesh = functionView.gameObject.AddComponent<MeshCollider>();
        }

        ValidateInput();

        CreateCoordineSystem();

        CreateFunctionRepresentation();
    }

    private void CreateFunctionRepresentation()
    {
        CreateFunctionLineRenderer();

        float xPos = drawFromXPos;
        int i;
        for (i = 0; i < lineRendererDivisionNum / 2; i++)
        {
            functionView.SetPosition(i, GetPointPosWithNoise(xPos));
            xPos = xPos + increaseValueOnX;
        }

        for (; i < lineRendererDivisionNum; i++)
        {
            functionView.SetPosition(i, GetNegativePointPosWithNoise(xPos));
            xPos = xPos - increaseValueOnX;
        }
    }

    private Vector3 GetPointPosWithNoise(float xPos)
    {
        return BasePositionWithoutNoise(xPos) * (noiseOffset + GenerateNoise(xPos));
    }

    private Vector3 GetNegativePointPosWithNoise(float xPos)
    {
        return NegativeBasePositionWithoutNoise(xPos) * (noiseOffset + GenerateNoise(xPos));
    }

    private Vector3 BasePositionWithoutNoise(float xPos)
    {
        return new Vector3(xPos, BaseFunction(xPos), 0f);
    }

    private Vector3 NegativeBasePositionWithoutNoise(float xPos)
    {
        return new Vector3(xPos, -BaseFunction(xPos), 0f);
    }

    private float GenerateNoise(float xPos)
    {
        return noiseAmplitude * GenerateFirstLayerNoise(xPos) * GenerateSecondLayerNoise(xPos);
    }

    private float GenerateFirstLayerNoise(float xPos)
    {
        return Mathf.Sin(xPos * noiseRoughness);
    }

    private float GenerateSecondLayerNoise(float xPos)
    {
        return Mathf.PerlinNoise(noiseSeed, BaseFunction(xPos * noiseRoughness));
    }

    private float BaseFunction(float x)
    {
        return (circleRadius * circleRadius - x * x) < 0 ? 0f : Mathf.Sqrt(circleRadius * circleRadius - x * x);
    }

    private void ValidateInput()
    {
        if (lineRendererDivisionNum <= 0) lineRendererDivisionNum = 0;
        if (lineRendererDivisionNum % 2 == 0) lineRendererDivisionNum++;
        if (circleRadius <= 0) circleRadius = 0;
        drawFromXPos = -circleRadius;
        drawUntilXPos = circleRadius;
    }

    private void CreateFunctionLineRenderer()
    {
        functionView.material = new Material(Shader.Find("Sprites/Default"));
        functionView.widthMultiplier = 0.01f * circleRadius * noiseOffset;
        functionView.positionCount = lineRendererDivisionNum;
        functionView.startColor = Color.cyan;
        functionView.endColor = Color.cyan;
    }

    private void CreateCoordineSystem()
    {
        xAxis.material = new Material(Shader.Find("Sprites/Default"));
        xAxis.widthMultiplier = 0.02f * circleRadius;
        xAxis.positionCount = 2;
        xAxis.SetPosition(0, new Vector3(-1000, 0, 0));
        xAxis.SetPosition(1, new Vector3(1000, 0, 0));
        xAxis.startColor = Color.red;
        xAxis.endColor = Color.red;

        yAxis.material = new Material(Shader.Find("Sprites/Default"));
        yAxis.widthMultiplier = 0.02f * circleRadius;
        yAxis.positionCount = 2;
        yAxis.SetPosition(0, new Vector3(0, -1000, 0));
        yAxis.SetPosition(1, new Vector3(0, 1000, 0));
        yAxis.startColor = Color.green;
        yAxis.endColor = Color.green;
    }
}