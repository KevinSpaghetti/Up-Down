using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

public class ObstacleSpawner : MonoBehaviour
{

    //Difficulty management
    public int nOfObjectsToSpawnNextWave = 0;
    public float deltaSecondsBetweenWaves = 5;
    public float spawnedObjectsSpeed = 1;

    
    //Behaviour and obstacle settings
    public List<GameObject> possibleObstacles;
    public GameObject spawnPointsParent; //children of this gameobject will be used as spawn points

    public Vector3 spawnedObjectDirection;
    public float timeIntervalBetweenTooFarObjectsGarbageCollection = 0.5f; //distance after which we delete the objects created
    public float killDistanceForSpawnedObjects = 50.0f;

    private List<Transform> spawnPoints;
    private List<GameObject> spawnedObstacles;
    void Start()
    {
        spawnedObstacles = new List<GameObject>();
        spawnPoints = new List<Transform>();
        foreach (var obj in spawnPointsParent.transform.GetComponentsInChildren<Transform>())
        {
            if (obj.GetInstanceID() == spawnPointsParent.transform.GetInstanceID()) continue; //Skip parent
            spawnPoints.Add(obj);
        }
        //Start a coroutine to spawn objects every t time
        StartCoroutine(nameof(SpawnObstacles));
        StartCoroutine(nameof(DestroyTooFarObstacles));

    }

    void Update()
    {
        //Update all spawned objects positions
        foreach (var obj in spawnedObstacles)
        {
            obj.transform.position += spawnedObjectDirection * (spawnedObjectsSpeed * Time.deltaTime);
        }
    }

    public void StartSpawning(){}
    public void EndSpawning(){}

    IEnumerator DestroyTooFarObstacles()
    {
        for (;;)
        {
            yield return new WaitForSeconds(timeIntervalBetweenTooFarObjectsGarbageCollection);
            yield return new WaitForEndOfFrame();

            //Collect too far objects
            var objectToRemoveAndDestroy = spawnedObstacles.FindAll((GameObject gameObject) =>
            {
                float distanceFromSpawnPoint = Vector3.Distance(gameObject.transform.position, gameObject.transform.parent.position);
                return  distanceFromSpawnPoint > killDistanceForSpawnedObjects; 
            });

            foreach (var obj in objectToRemoveAndDestroy)
            {
                spawnedObstacles.Remove(obj);
                Destroy(obj);
            }
        }
    }
    
    IEnumerator SpawnObstacles()
    {
        for (;;)
        {
            SpawnObstaclesInTracks();
            yield return new WaitForSeconds(deltaSecondsBetweenWaves);
        }
    }

    public void SpawnObstaclesInTracks()
    {

        Assert.IsTrue(nOfObjectsToSpawnNextWave < 6); // 6 objects would render the game impossible
        Assert.IsTrue(nOfObjectsToSpawnNextWave >= 0); // 6 objects would render the game impossible

        
        foreach (var possibleSpawnPoint in spawnPoints)
        {
            var rand = new Random();
            bool spawnHere = (rand.NextDouble() > 0.3f);
            if (spawnHere)
            {
                var obstacleToSpawn = pickObstacleToSpawn();
                
                
                
                var obstacle = Instantiate(obstacleToSpawn, possibleSpawnPoint) as GameObject;
                spawnedObstacles.Add(obstacle);
                nOfObjectsToSpawnNextWave--;
            }

            if (nOfObjectsToSpawnNextWave == 0) break;
        }
    }

    public GameObject pickObstacleToSpawn()
    {
        Assert.IsTrue(possibleObstacles.Count > 0);
        return possibleObstacles[0];
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(this.spawnPointsParent.transform.position, 
            this.spawnPointsParent.transform.position + spawnedObjectDirection * killDistanceForSpawnedObjects);
    }
}
