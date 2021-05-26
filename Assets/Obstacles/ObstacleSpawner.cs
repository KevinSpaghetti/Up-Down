using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> possibleObstacles;
    //the children of this gameobject will be used as spawn point for the tracks
    public GameObject spawnPointsParent;

    public Vector3 spawnedObjectDirection;
    public float spawnedObjectsSpeed = 1;
    public float deltaSecondsBetweenWaves = 5;

    public float timeIntervalBetweenTooFarObjectsGarbageCollection = 0.5f;
    
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
            int nOfObjectsToSpawn = 3;
            SpawnObstaclesInTracks(nOfObjectsToSpawn);

            yield return new WaitForSeconds(deltaSecondsBetweenWaves);
        }
    }

    public void SpawnObstaclesInTracks(int nOfObjectsToSpawn)
    {
        Assert.IsTrue(nOfObjectsToSpawn < 6); // 6 objects would render the game impossible

        foreach (var possibleSpawnPoint in spawnPoints)
        {
            var rand = new Random();
            bool spawnHere = (rand.NextDouble() > 0.3f);
            if (spawnHere)
            {
                var obstacle = Instantiate(possibleObstacles[0], possibleSpawnPoint) as GameObject;
                spawnedObstacles.Add(obstacle);
                nOfObjectsToSpawn--;
            }

            if (nOfObjectsToSpawn == 0) break;
        }
        
        
        
        
    }
    
    
}
