using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionCircleCreator : MonoBehaviour
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

    [SerializeField]
    private float drawFromXPos = 0f;

    [SerializeField]
    private float drawUntilXPos = 0f;

    private float totalDrawAreaXLength = 0f;
    private float increaseValueOnX = 0f;

    private LineRenderer functionView;
    private MeshCollider functionViewMesh;

    private void OnValidate()
    {
        ValidateInput();

        SetupFieldValues();

        CreateFunctionRepresentation();
    }

    private void SetupFieldValues()
    {
        totalDrawAreaXLength = Mathf.Abs(drawFromXPos) + Mathf.Abs(drawUntilXPos);
        increaseValueOnX = totalDrawAreaXLength / ((lineRendererDivisionNum / 2) - 1);

        drawFromXPos = -circleRadius;
        drawUntilXPos = circleRadius;

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
            functionView.SetPosition(i, BaseFunctionValueGenerator.GetBasePosition(xPos, circleRadius));
            xPos += increaseValueOnX;
        }

        for (; i < lineRendererDivisionNum; i++)
        {
            functionView.SetPosition(i, BaseFunctionValueGenerator.GetNegativeBasePosition(xPos, circleRadius));
            xPos -= increaseValueOnX;
        }

        Mesh mesh = new Mesh();
        functionView.BakeMesh(mesh, localCamera, true);
        functionViewMesh.sharedMesh = mesh;
    }

    private void CreateFunctionLineRenderer()
    {
        functionView.material = new Material(Shader.Find("Sprites/Default"));
        functionView.widthMultiplier = 0.02f * circleRadius;
        functionView.positionCount = lineRendererDivisionNum;
        functionView.startColor = Color.cyan;
        functionView.endColor = Color.cyan;
    }

    private void ValidateInput()
    {
        if (lineRendererDivisionNum <= 0) lineRendererDivisionNum = 0;
        if (lineRendererDivisionNum % 2 == 0) lineRendererDivisionNum++;
    }
}