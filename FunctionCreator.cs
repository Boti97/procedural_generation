using System;
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
    private float perlonNoiseAmplifier = 0f;

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

    private void ValidateInput()
    {
        if (lineRendererDivisionNum <= 0) lineRendererDivisionNum = 0;
        if (drawFromXPos >= 0) drawFromXPos = 0;
        if (drawUntilXPos <= 0) drawUntilXPos = 0;
        if (circleRadius <= 0) circleRadius = 0;
    }

    private void CreateFunctionRepresentation()
    {
        functionView.material = new Material(Shader.Find("Sprites/Default"));
        functionView.widthMultiplier = 0.2f;
        functionView.positionCount = lineRendererDivisionNum;
        functionView.startColor = Color.white;
        functionView.endColor = Color.white;

        float x = drawFromXPos;
        for (int i = 0; i < lineRendererDivisionNum; i++)
        {
            float perlonNoiseValue = perlonNoiseAmplifier * Mathf.PerlinNoise(0f, -Function(x));

            functionView.SetPosition(i, new Vector3(x + perlonNoiseValue, Function(x) + perlonNoiseValue, 0.0f));
            if (x > drawUntilXPos + 0.5f)
            {
                break;
            }
            x = x + increaseValueOnX;
        }

        //x = drawUntilXPos;
        //for (int i = lineRendererDivisionNum / 2; i < lineRendererDivisionNum; i++)
        //{
        //    functionView.SetPosition(i, new Vector3(x, -Function(x), 0.0f));
        //    if (x < drawFromXPos - 0.5f)
        //    {
        //        break;
        //    }
        //    x = x - increaseValueOnX;
        //}

        //Mesh mesh = new Mesh();
        //functionView.BakeMesh(mesh, localCamera, true);
        //functionViewMesh.sharedMesh = mesh;
    }

    public void OnFunctionViewerSettingsUpdated()
    {
        CreateFunctionRepresentation();
    }

    private float Function(float x)
    {
        return Mathf.Sqrt(Mathf.Abs(circleRadius * circleRadius - x * x));
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