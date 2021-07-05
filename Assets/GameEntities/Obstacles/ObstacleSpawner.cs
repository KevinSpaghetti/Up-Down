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
    public bool startSpawningOnStart = false;

    //Difficulty management
    public int nOfObjectsToSpawnNextWave = 1;
    //the spawner will spawn a number of objects in the range [nOfObjectsToSpawnNextWave -  nOfObjectsToSpawnRange, nOfObjectsToSpawnNextWave + nOfObjectsToSpawnRange]
    public int nOfObjectsToSpawnRange = 1; 
    public float deltaSecondsBetweenWaves = 5;
    public float spawnedObjectsSpeed = 1;

    //Behaviour and obstacle settings
    public List<ObstacleObject> possibleObstacles;
    public List<Transform> spawnPoints; //children of this gameobject will be used as spawn points
    public Vector3 spawnedObjectsDirection = Vector3.back;
    public Vector3 spawnedObjectsPositionOffset = Vector3.zero;
    
    public float timeIntervalBetweenTooFarObjectsGarbageCollection = 0.5f; //distance after which we delete the objects created
    public float killDistanceForSpawnedObjects = 50.0f;

    private const int maxNOfObjectsForAllPools = 200;
    private GameObjectPool[] possibleObstaclePools; //every possible obstacle has its pool
    private List<KeyValuePair<GameObject, int>> spawnedObstacles; //store the index of the pool from which we borrowed the object

    private Random rng;
    
    void Start()
    {
        int nOfObjectsPerPool = maxNOfObjectsForAllPools / possibleObstacles.Count;
        possibleObstaclePools = new GameObjectPool[possibleObstacles.Count];
        for (int i = 0; i < possibleObstacles.Count; i++)
        {
            var pool = new GameObjectPool(nOfObjectsPerPool, possibleObstacles[i].gameObject);
            possibleObstaclePools[i] = pool;
        }

        spawnedObstacles = new List<KeyValuePair<GameObject, int>>();

        rng = new Random();
        
        //Start a coroutine to spawn objects every t time
        if (startSpawningOnStart)
        {
            StartSpawning();
        }
        StartCoroutine(nameof(DestroyTooFarObstacles));
    }

    void Update()
    {
        if (!frozen) UpdateAllSpawnedObjectsPositions();
    }

    void UpdateAllSpawnedObjectsPositions()
    {
        foreach (var obj in spawnedObstacles)
        {
            obj.Key.transform.position += spawnedObjectsDirection * (spawnedObjectsSpeed * Time.deltaTime);
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

    public void ResetState()
    {
        StopCoroutine(nameof(SpawnObstacles));  
        RemoveAllObstacles();
        frozen = false;
    }
    
    private void RemoveAllObstacles()
    {
        foreach (var pair in spawnedObstacles)
        {
            possibleObstaclePools[pair.Value].ReturnToPool(pair.Key);
        }
        spawnedObstacles.Clear();
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
                float distanceFromSpawnPoint = Vector3.Distance(pair.Key.transform.position, pair.Key.transform.parent.position + spawnedObjectsPositionOffset);
                return  distanceFromSpawnPoint > killDistanceForSpawnedObjects; 
            });

            if (objectsToReturnToPool.Count > 0)
            {

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
    }
    
    private bool[] isIndexOfSpawnPointFree; //To avoid expensive small GC allocations
    IEnumerator SpawnObstacles()
    {
        isIndexOfSpawnPointFree = new bool[spawnPoints.Count];
        for (;;)
        {
            SpawnObstaclesInTracks();
            
            //Save the remaining time before the next spawn wave
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
        Debug.Assert(nOfObjectsToSpawnNextWave < 6, "Requested more than 5 objects to spawn"); // 6 objects would render the game impossible
        Debug.Assert(nOfObjectsToSpawnNextWave >= 0, "Requested less than 0 objects to spawn");

        //Casually spawn more or less objects
        int nOfObjectsToSpawn = nOfObjectsToSpawnNextWave;
        nOfObjectsToSpawn = rng.Next(nOfObjectsToSpawn - nOfObjectsToSpawnRange, nOfObjectsToSpawn + nOfObjectsToSpawnRange);
        nOfObjectsToSpawn = Mathf.Clamp(nOfObjectsToSpawn, 0, 5);

        if (nOfObjectsToSpawn == 0) return;
        
        for (int i = 0; i < spawnPoints.Count; ++i) isIndexOfSpawnPointFree[i] = true;
        do
        {
            //Choose a random spawn point not picked before
            int spawnPointIndex;
            do
            {
                spawnPointIndex = rng.Next(0, spawnPoints.Count);
            } while (!isIndexOfSpawnPointFree[spawnPointIndex]);
            isIndexOfSpawnPointFree[spawnPointIndex] = false;
            Transform spawnPoint = spawnPoints[spawnPointIndex];
            
            //Choose a random obstacle from the pools
            KeyValuePair<GameObject, int> obstacleWithPoolInfo = ChooseRandomObstacleFromPools();

            #if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (obstacleWithPoolInfo.Key == null){ 
                string prefabName = possibleObstacles[obstacleWithPoolInfo.Value].name;
                Debug.Log("Request to instantiate " + prefabName + " failed, pool " + obstacleWithPoolInfo.Value + " empty");
            }    
            #endif
            
            Debug.Assert(obstacleWithPoolInfo.Key != null);
            
            if (obstacleWithPoolInfo.Key != null)
            {
                GameObject obstacle = obstacleWithPoolInfo.Key;
                obstacle.transform.SetPositionAndRotation(spawnPoint.transform.position + spawnedObjectsPositionOffset, spawnPoint.transform.rotation);
                obstacle.transform.parent = spawnPoint;
                spawnedObstacles.Add(new KeyValuePair<GameObject, int>(obstacle, obstacleWithPoolInfo.Value));
            }
            
            nOfObjectsToSpawn--;
        } while (nOfObjectsToSpawn > 0);
        
    }
    
    private KeyValuePair<GameObject, int> ChooseRandomObstacleFromPools()
    {
        int poolIndex = rng.Next(0, possibleObstaclePools.Length);
        var obstacle = possibleObstaclePools[poolIndex].GetFromPool();
        return new KeyValuePair<GameObject, int>(obstacle, poolIndex);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        spawnPoints.ForEach((Transform point) =>
        {
            Vector3 startPoint = point.transform.position + spawnedObjectsPositionOffset;
            Gizmos.DrawLine(startPoint, startPoint + spawnedObjectsDirection * killDistanceForSpawnedObjects);
        });
    }
}
