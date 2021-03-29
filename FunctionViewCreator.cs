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

    private float totalDrawAreaXLength = 0f;
    private float increaseValueOnXUp = 0f;
    private float increaseValueOnXDown = 0f;
    private float drawFromXPos = 0f;
    private float drawUntilXPos = 0f;
    private int numberOfVerticesUp;
    private int numberOfVerticesDown;

    private LineRenderer xAxis;
    private GameObject xAxisObject;
    private LineRenderer yAxis;
    private GameObject yAxisObject;
    private LineRenderer functionView;
    private GameObject functionViewObject;
    private MeshCollider functionViewMesh;
    private MeshCollider xAxisMesh;
    private MeshCollider yAxisMesh;

    public LineRenderer FunctionView { get => functionView; set => functionView = value; }
    public int LineRendererDivisionNum { get => lineRendererDivisionNum; set => lineRendererDivisionNum = value; }

    public void Update()
    {
        SetupFieldValues();

        ValidateInput();

        CreateCoordineSystem();

        CreateFunctionRepresentation();

        StartCoroutine(gameObject.GetComponent<PlanetCreator>().CreatePlanets());
    }

    private void SetupFieldValues()
    {
        drawFromXPos = -baseCircleRadius;
        drawUntilXPos = baseCircleRadius;

        totalDrawAreaXLength = Mathf.Abs(drawFromXPos) + Mathf.Abs(drawUntilXPos);

        numberOfVerticesUp = Mathf.RoundToInt(LineRendererDivisionNum / 2);
        numberOfVerticesDown = LineRendererDivisionNum - numberOfVerticesUp;

        increaseValueOnXUp = totalDrawAreaXLength / (numberOfVerticesUp);
        increaseValueOnXDown = totalDrawAreaXLength / (numberOfVerticesDown - 1);

        xAxisObject = GameObject.FindGameObjectWithTag("XAxis");
        if (xAxis == null)
        {
            xAxisObject = Instantiate(LinePrefab);
            xAxisObject.tag = "YAxis";
            xAxis = xAxisObject.AddComponent<LineRenderer>();
            xAxisMesh = xAxisObject.AddComponent<MeshCollider>();
        }
        yAxisObject = GameObject.FindGameObjectWithTag("YAxis");
        if (yAxis == null)
        {
            yAxisObject = Instantiate(LinePrefab);
            yAxisObject.tag = "YAxis";
            yAxis = yAxisObject.AddComponent<LineRenderer>();
            yAxisMesh = yAxisObject.AddComponent<MeshCollider>();
        }
        functionViewObject = GameObject.FindGameObjectWithTag("FunctionView");
        if (functionViewObject == null)
        {
            functionViewObject = Instantiate(LinePrefab);
            functionViewObject.tag = "FunctionView";
            FunctionView = functionViewObject.AddComponent<LineRenderer>();
            functionViewMesh = functionViewObject.AddComponent<MeshCollider>();
        }
    }

    private void CreateFunctionRepresentation()
    {
        CreateFunctionLineRenderer();

        float xPos = drawFromXPos;
        int i;
        for (i = 0; i < numberOfVerticesUp; i++)
        {
            FunctionView.SetPosition(i,
                BaseFunctionValueGenerator.GetBasePosition(xPos, baseCircleRadius) *
                NoiseGenerator.GenerateNoise(xPos, noiseAmplitude, noiseRoughness, noiseSeed, baseCircleRadius));
            xPos += increaseValueOnXUp;
        }

        for (; i < LineRendererDivisionNum; i++)
        {
            FunctionView.SetPosition(i,
                BaseFunctionValueGenerator.GetNegativeBasePosition(xPos, baseCircleRadius) *
                NoiseGenerator.GenerateNoise(xPos, noiseAmplitude, noiseRoughness, noiseSeed, baseCircleRadius));
            xPos -= increaseValueOnXDown;
        }

        Mesh mesh = new Mesh();
        FunctionView.BakeMesh(mesh, localCamera, true);
        functionViewMesh.sharedMesh = mesh;
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

        Mesh meshX = new Mesh();
        xAxis.BakeMesh(meshX, localCamera, true);
        xAxisMesh.sharedMesh = meshX;

        yAxis.material = new Material(Shader.Find("Sprites/Default"));
        yAxis.widthMultiplier = 0.01f * baseCircleRadius;
        yAxis.positionCount = 2;
        yAxis.SetPosition(0, new Vector3(0, -1000, 0));
        yAxis.SetPosition(1, new Vector3(0, 1000, 0));
        yAxis.startColor = Color.green;
        yAxis.endColor = Color.green;

        Mesh meshY = new Mesh();
        yAxis.BakeMesh(meshY, localCamera, true);
        yAxisMesh.sharedMesh = meshY;
    }
}