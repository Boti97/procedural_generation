using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionViewCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject LinePrefab;

    [SerializeField]
    private Camera localCamera;

    [SerializeField]
    [Range(7, 699)]
    private int lineRendererDivisionNum = 0;

    [SerializeField]
    private int circleRadius;

    //noise variables
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
    private float drawFromXPos = 0f;
    private float drawUntilXPos = 0f;

    private LineRenderer xAxis;
    private LineRenderer yAxis;
    private LineRenderer functionView;
    private MeshCollider functionViewMesh;

    public void OnValidate()
    {
        SetupFieldValues();

        ValidateInput();

        CreateCoordineSystem();

        CreateFunctionRepresentation();
    }

    private void SetupFieldValues()
    {
        drawFromXPos = -circleRadius;
        drawUntilXPos = circleRadius;

        totalDrawAreaXLength = Mathf.Abs(drawFromXPos) + Mathf.Abs(drawUntilXPos);
        increaseValueOnX = totalDrawAreaXLength / ((lineRendererDivisionNum / 2) - 1);

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
    }

    private void CreateFunctionRepresentation()
    {
        CreateFunctionLineRenderer();

        float xPos = drawFromXPos;
        int i;
        for (i = 0; i < lineRendererDivisionNum / 2; i++)
        {
            functionView.SetPosition(i,
                BaseFunctionValueGenerator.GetBasePosition(
                    xPos,
                    circleRadius
                    ) *
                NoiseGenerator.GenerateNoise(
                    xPos,
                    noiseOffset,
                    noiseAmplitude,
                    noiseRoughness,
                    noiseSeed,
                    circleRadius
                    ));
            xPos = xPos + increaseValueOnX;
        }

        for (; i < lineRendererDivisionNum; i++)
        {
            functionView.SetPosition(i,
                BaseFunctionValueGenerator.GetNegativeBasePosition(
                    xPos,
                    circleRadius
                    ) *
                NoiseGenerator.GenerateNoise(
                    xPos,
                    noiseOffset,
                    noiseAmplitude,
                    noiseRoughness,
                    noiseSeed,
                    circleRadius
                    ));
            xPos = xPos - increaseValueOnX;
        }

        Mesh mesh = new Mesh();
        functionView.BakeMesh(mesh, localCamera, true);
        functionViewMesh.sharedMesh = mesh;
    }

    private void ValidateInput()
    {
        if (lineRendererDivisionNum <= 0) lineRendererDivisionNum = 0;
        if (lineRendererDivisionNum % 2 == 0) lineRendererDivisionNum++;
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
        xAxis.widthMultiplier = 0.02f * circleRadius * noiseOffset;
        xAxis.positionCount = 2;
        xAxis.SetPosition(0, new Vector3(-1000, 0, 0));
        xAxis.SetPosition(1, new Vector3(1000, 0, 0));
        xAxis.startColor = Color.red;
        xAxis.endColor = Color.red;

        yAxis.material = new Material(Shader.Find("Sprites/Default"));
        yAxis.widthMultiplier = 0.02f * circleRadius * noiseOffset;
        yAxis.positionCount = 2;
        yAxis.SetPosition(0, new Vector3(0, -1000, 0));
        yAxis.SetPosition(1, new Vector3(0, 1000, 0));
        yAxis.startColor = Color.green;
        yAxis.endColor = Color.green;
    }
}