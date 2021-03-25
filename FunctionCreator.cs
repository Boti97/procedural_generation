﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject LinePrefab;

    [SerializeField]
    private Camera localCamera;

    [SerializeField]
    private int lineRendererDivisionNum = 0;

    [SerializeField]
    private float drawFromXPos = 0f;

    [SerializeField]
    private float drawUntilXPos = 0f;

    [SerializeField]
    private float circleRadius = 0f;

    [SerializeField]
    private float noiseAmplifier = 0f;

    [SerializeField]
    [Range(0, 25)]
    private float noiseSeed = 0f;

    [SerializeField]
    [Range(0, 50)]
    private float noiseOffset = 0f;

    [SerializeField]
    [Range(0, 2)]
    private float noiseAmplitude = 0f;

    [SerializeField]
    [Range(0, 1)]
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

        increaseValueOnX = totalDrawAreaXLength / (lineRendererDivisionNum - 1);
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
        functionView.material = new Material(Shader.Find("Sprites/Default"));
        functionView.widthMultiplier = 0.01f * circleRadius;
        functionView.positionCount = lineRendererDivisionNum;
        functionView.startColor = Color.white;
        functionView.endColor = Color.white;

        float xPos = drawFromXPos;
        for (int i = 0; i < lineRendererDivisionNum; i++)
        {
            if (i + 1 >= lineRendererDivisionNum)
            {
                functionView.SetPosition(i, new Vector3(drawUntilXPos, 0f, 0f));
            }

            functionView.SetPosition(i, GetPointPosWithNoise(xPos));
            if (xPos > drawUntilXPos + 0.5f)
            {
                break;
            }
            xPos = xPos + increaseValueOnX;
        }
    }

    private Vector3 GetPointPosWithNoise(float xPos)
    {
        return new Vector3(xPos, BaseFunction(xPos), 0f) * (noiseOffset + GenerateNoise(xPos));
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
        if (circleRadius <= 0) circleRadius = 0;
        circleRadius = Mathf.Round(circleRadius);
        drawFromXPos = -circleRadius;
        drawUntilXPos = circleRadius;
    }

    private void CreateCoordineSystem()
    {
        xAxis.material = new Material(Shader.Find("Sprites/Default"));
        xAxis.widthMultiplier = 0.1f;
        xAxis.positionCount = 2;
        xAxis.SetPosition(0, new Vector3(-1000, 0, 0));
        xAxis.SetPosition(1, new Vector3(1000, 0, 0));
        xAxis.startColor = Color.red;
        xAxis.endColor = Color.red;

        yAxis.material = new Material(Shader.Find("Sprites/Default"));
        yAxis.widthMultiplier = 0.1f;
        yAxis.positionCount = 2;
        yAxis.SetPosition(0, new Vector3(0, -1000, 0));
        yAxis.SetPosition(1, new Vector3(0, 1000, 0));
        yAxis.startColor = Color.green;
        yAxis.endColor = Color.green;
    }
}