using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlanetPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject planetPrefab;

    [SerializeField]
    private GameObject planetsParent;

    private List<Vector3> planetPositions;
    private List<GameObject> planetPool;
    private List<GameObject> activePlanets;
    private static readonly object padlock = new object();
    private static PlanetPoolManager instance = null;

    public static PlanetPoolManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new PlanetPoolManager();
                }
                return instance;
            }
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        planetPool = new List<GameObject>();
        activePlanets = new List<GameObject>();
        planetPositions = new List<Vector3>();

        InstantiatePlanets();
    }

    public void MovePlanetToPos(Vector3 newPosition)
    {
        GameObject planet = GetPlanetFromPool();
        planet.SetActive(true);
        planet.transform.position = newPosition;

        planetPositions.Add(newPosition);
        planetPool.Remove(planet);
        activePlanets.Add(planet);
    }

    public void ResetPlanets()
    {
        int numberOfActivePlanets = activePlanets.Count;
        List<GameObject> tempActivePlanetList = new List<GameObject>(activePlanets);
        for (int i = 0; i < numberOfActivePlanets; i++)
        {
            GameObject planet = tempActivePlanetList[i];
            planet.SetActive(false);
            planetPool.Add(planet);
            activePlanets.Remove(planet);
        }

        planetPositions.Clear();
    }

    public List<Vector3> GetPlanetPositions()
    {
        return planetPositions;
    }

    private GameObject GetPlanetFromPool()
    {
        for (int i = 0; i < planetPool.Count; i++)
        {
            if (!planetPool[i].activeInHierarchy)
            {
                return planetPool[i];
            }
        }
        return null;
    }

    private void InstantiatePlanets()
    {
        if (planetPool == null || planetPool.Count.Equals(0))
        {
            for (int i = 0; i < 1000; i++)
            {
                InstantiatePlanet();
            }
        }
    }

    private void InstantiatePlanet()
    {
        GameObject planet = Instantiate(planetPrefab);
        planet.transform.parent = planetsParent.transform;
        planet.SetActive(false);
        planetPool.Add(planet);
    }
}