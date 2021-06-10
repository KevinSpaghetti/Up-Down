using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

public class ObstacleSpawner : MonoBehaviour
{

    public bool frozen = false;
    
    //Difficulty management
    public int nOfObjectsToSpawnNextWave = 1;
    public float deltaSecondsBetweenWaves = 5;
    public float spawnedObjectsSpeed = 1;

    
    //Behaviour and obstacle settings
    public bool startSpawningOnStart = false;

    public List<GameObject> possibleObstacles;
    public GameObject spawnPointsParent; //children of this gameobject will be used as spawn points

    public Vector3 spawnedObjectDirection;
    public float timeIntervalBetweenTooFarObjectsGarbageCollection = 0.5f; //distance after which we delete the objects created
    public float killDistanceForSpawnedObjects = 50.0f;

    private List<Transform> spawnPoints;
    
    private int maxNOfObjectsForAllPools = 200;
    private GameObjectPool[] possibleObstaclePools; //every possible obstacle has its pool
    private List<KeyValuePair<GameObject, int>> spawnedObstacles; //store the index of the pool from which we borrowed the object
    
    void Start()
    {
        int nOfObjectsPerPool = maxNOfObjectsForAllPools / possibleObstacles.Count;
        possibleObstaclePools = new GameObjectPool[possibleObstacles.Count];
        for (int i = 0; i < possibleObstacles.Count; i++)
        {
            var pool = new GameObjectPool(nOfObjectsPerPool, possibleObstacles[0]);
            possibleObstaclePools[i] = pool;
        }

        spawnedObstacles = new List<KeyValuePair<GameObject, int>>();
        spawnPoints = new List<Transform>();
        foreach (var obj in spawnPointsParent.transform.GetComponentsInChildren<Transform>())
        {
            if (obj.GetInstanceID() == spawnPointsParent.transform.GetInstanceID()) continue; //Skip parent
            spawnPoints.Add(obj);
        }
        //Start a coroutine to spawn objects every t time
        if (startSpawningOnStart)
        {
            StartSpawning();
        }
        StartCoroutine(nameof(DestroyTooFarObstacles));
    }

    void Update()
    {
        if (frozen) return;
        
        //Update all spawned objects positions
        foreach (var obj in spawnedObstacles)
        {
            obj.Key.transform.position += spawnedObjectDirection * (spawnedObjectsSpeed * Time.deltaTime);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(nameof(SpawnObstacles));
    }

    public void EndSpawning()
    {
        StopCoroutine(nameof(SpawnObstacles));
    }

    IEnumerator DestroyTooFarObstacles()
    {
        for (;;)
        {
            yield return new WaitForSeconds(timeIntervalBetweenTooFarObjectsGarbageCollection);
            yield return new WaitForEndOfFrame();

            //Collect too far objects
            var objectsToReturnToPool = spawnedObstacles.FindAll((KeyValuePair<GameObject, int> pair) =>
            {
                float distanceFromSpawnPoint = Vector3.Distance(pair.Key.transform.position, pair.Key.transform.parent.position);
                return  distanceFromSpawnPoint > killDistanceForSpawnedObjects; 
            });

            foreach (var pair in objectsToReturnToPool)
            {
                possibleObstaclePools[pair.Value].ReturnToPool(pair.Key);
            }

            spawnedObstacles.RemoveAll((KeyValuePair<GameObject, int> pair) =>
            {
                return objectsToReturnToPool.Contains(pair);
            });
        }
    }
    
    IEnumerator SpawnObstacles()
    {
        for (;;)
        {
            SpawnObstaclesInTracks();
            
            float timer = 0f;
            while(timer < deltaSecondsBetweenWaves) {
                while(frozen) {
                    yield return null;
                }
 
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void SpawnObstaclesInTracks()
    {

        Assert.IsTrue(nOfObjectsToSpawnNextWave < 6, "Requested more than 5 objects to spawn"); // 6 objects would render the game impossible
        Assert.IsTrue(nOfObjectsToSpawnNextWave >= 0, "Requested less than 0 objects to spawn");

        int nOfObjectsToSpawn = nOfObjectsToSpawnNextWave;
        if (nOfObjectsToSpawn == 0) return;
        foreach (var possibleSpawnPoint in spawnPoints)
        {
            var rand = new Random();
            bool spawnHere = (rand.NextDouble() > 0.3f);
            if (spawnHere)
            {
                var obstacle = possibleObstaclePools[0].GetFromPool();
                if(obstacle == null) Debug.Log("Pool empty when new object requested");
                if (obstacle != null)
                {
                    obstacle.transform.position = possibleSpawnPoint.transform.position;
                    obstacle.transform.rotation = possibleSpawnPoint.transform.rotation;
                    obstacle.transform.parent = possibleSpawnPoint;
                    spawnedObstacles.Add(new KeyValuePair<GameObject, int>(obstacle, 0));
                    nOfObjectsToSpawn--;    
                }
                else
                {
                    break;
                }
            }

            if (nOfObjectsToSpawn == 0) break;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(this.spawnPointsParent.transform.position, 
            this.spawnPointsParent.transform.position + spawnedObjectDirection * killDistanceForSpawnedObjects);
    }
}
