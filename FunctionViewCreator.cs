using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseFunctionValueGenerator;

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
    [Range(0, 500)]
    private int baseCircleRadius;

    //noise variables
    [SerializeField]
    [Range(0, 5)]
    private float noiseSeed = 0f;

    [SerializeField]
    [Range(0, 2)]
    private float noiseAmplitude = 0f;

    [SerializeField]
    [Range(0, 0.2f)]
    private float noiseRoughness = 0f;

    private LineRenderer xAxis;
    private GameObject xAxisObject;
    private LineRenderer yAxis;
    private GameObject yAxisObject;
    private LineRenderer functionView;
    private GameObject functionViewObject;

    public LineRenderer FunctionView { get => functionView; set => functionView = value; }
    public int LineRendererDivisionNum { get => lineRendererDivisionNum; set => lineRendererDivisionNum = value; }

    public void Update()
    {
        SetupFieldValues();

        ValidateInput();

        CreateCoordineSystem();

        CreateFunctionRepresentation();

        gameObject.GetComponent<PlanetCreator>().CreatePlanets();
    }

    private void SetupFieldValues()
    {
        xAxisObject = GameObject.FindGameObjectWithTag("XAxis");
        if (xAxis == null)
        {
            xAxisObject = Instantiate(LinePrefab);
            xAxisObject.tag = "YAxis";
            xAxis = xAxisObject.AddComponent<LineRenderer>();
        }
        yAxisObject = GameObject.FindGameObjectWithTag("YAxis");
        if (yAxis == null)
        {
            yAxisObject = Instantiate(LinePrefab);
            yAxisObject.tag = "YAxis";
            yAxis = yAxisObject.AddComponent<LineRenderer>();
        }
        functionViewObject = GameObject.FindGameObjectWithTag("FunctionView");
        if (functionViewObject == null)
        {
            functionViewObject = Instantiate(LinePrefab);
            functionViewObject.tag = "FunctionView";
            FunctionView = functionViewObject.AddComponent<LineRenderer>();
        }
    }

    private void CreateFunctionRepresentation()
    {
        CreateFunctionLineRenderer();

        float angle = 360f / (LineRendererDivisionNum - 1);
        for (int i = 0; i < LineRendererDivisionNum; i++)
        {
            FloatPoint point = BaseFunction(baseCircleRadius, angle * i);
            float noise = NoiseGenerator.GenerateNoise(point.X, point.Y, noiseAmplitude, noiseRoughness, noiseSeed);
            FunctionView.SetPosition(i, new Vector3(point.X, point.Y, 0f) * noise);
        }
    }

    private void ValidateInput()
    {
        if (LineRendererDivisionNum <= 0) LineRendererDivisionNum = 0;
        if (LineRendererDivisionNum % 2 == 0) LineRendererDivisionNum++;
    }

    private void CreateFunctionLineRenderer()
    {
        FunctionView.material = new Material(Shader.Find("Sprites/Default"));
        FunctionView.widthMultiplier = 0.01f * baseCircleRadius;
        FunctionView.positionCount = LineRendererDivisionNum;
        FunctionView.startColor = Color.cyan;
        FunctionView.endColor = Color.cyan;
    }

    private void CreateCoordineSystem()
    {
        xAxis.material = new Material(Shader.Find("Sprites/Default"));
        xAxis.widthMultiplier = 0.01f * baseCircleRadius;
        xAxis.positionCount = 2;
        xAxis.SetPosition(0, new Vector3(-1000, 0, 0));
        xAxis.SetPosition(1, new Vector3(1000, 0, 0));
        xAxis.startColor = Color.red;
        xAxis.endColor = Color.red;

        yAxis.material = new Material(Shader.Find("Sprites/Default"));
        yAxis.widthMultiplier = 0.01f * baseCircleRadius;
        yAxis.positionCount = 2;
        yAxis.SetPosition(0, new Vector3(0, -1000, 0));
        yAxis.SetPosition(1, new Vector3(0, 1000, 0));
        yAxis.startColor = Color.green;
        yAxis.endColor = Color.green;
    }
}