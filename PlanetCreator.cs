using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreator : MonoBehaviour
{
    [SerializeField]
    [Range(0, 200)]
    private float objectDensity = 0f;

    private int numberOfPointsInFunction = 0;
    private FunctionViewCreator functionViewCreator;
    private LineRenderer functionView;
    private Vector3 currentPos;

    public IEnumerator CreatePlanets()
    {
        PlanetPoolManager.Instance.ResetPlanets();

        functionViewCreator = GetComponent<FunctionViewCreator>();
        numberOfPointsInFunction = functionViewCreator.LineRendererDivisionNum;
        functionView = functionViewCreator.FunctionView;
        currentPos = functionView.GetPosition(0);

        Vector3 lastPosition = Vector3.positiveInfinity;
        int indexOfClosestPoint = 0;

        yield return StartCoroutine(PlanetPoolManager.Instance.MovePlanetToPos(currentPos));
        for (int l = 0; l < numberOfPointsInFunction; l++)
        {
            Vector3 closestPoint = Vector3.positiveInfinity;
            float closestDistance = float.PositiveInfinity;

            for (int i = indexOfClosestPoint; i < numberOfPointsInFunction; i++)
            {
                Vector3 nextPoint = functionView.GetPosition(i);
                float distance = GetDistance(objectDensity, currentPos, nextPoint);
                if (distance < closestDistance)
                {
                    if (IsNewPoint(nextPoint) && indexOfClosestPoint < i)
                    {
                        closestDistance = distance;
                        closestPoint = nextPoint;
                        indexOfClosestPoint = i;
                    }
                }
            }

            if (!closestPoint.Equals(Vector3.positiveInfinity) && !closestPoint.Equals(lastPosition) && !currentPos.Equals(lastPosition))
            {
                yield return StartCoroutine(PlanetPoolManager.Instance.MovePlanetToPos(closestPoint));
                lastPosition = currentPos;
                currentPos = closestPoint;
            }
            else break;
        }
    }

    private bool IsNewPoint(Vector3 nextPoint)
    {
        foreach (Vector3 vector in PlanetPoolManager.Instance.GetPlanetPositions())
        {
            //(circleRadius / (numberOfPointsInFunction / 4)) is about one point to point length in the line renderer
            //four time this length should be enought
            if (Vector3.Distance(nextPoint, vector) < (float)(objectDensity - 4 * (objectDensity / (numberOfPointsInFunction / 4))))
            {
                return false;
            }
        }
        return true;
    }

    private static float GetDistance(float radius, Vector3 circleCenter, Vector3 point)
    {
        return Mathf.Abs(Mathf.Sqrt(((point.x - circleCenter.x) * (point.x - circleCenter.x)) + ((point.y - circleCenter.y) * (point.y - circleCenter.y))) - radius);
    }
}