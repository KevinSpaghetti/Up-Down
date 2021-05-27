using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Range(0, 1)]
    public float minDifficulty = 0.0f;
    [Range(0, 1)]
    public float maxDifficulty = 1.0f;
    
    public float minSpeed = 0.0f;
    public float maxSpeed = 1.0f;
    
    [Range(0, 5)]
    public int minObjectsToSpawn = 0;
    [Range(0, 5)]
    public int maxObjectsToSpawn = 5;

    public AnimationCurve howDifficultyScalesWithTime;
    
    public AnimationCurve howSpeedScalesWithDifficulty;
    public AnimationCurve howNOfObjectsToSpawnScalesWithDifficulty;

    public float secondsToReachMaxDifficulty;

    public ObstacleSpawner spawner;
    
    [SerializeField]
    private float currentDifficulty = 0.0f;

    private float startGameTime = 0.0f;
    private float currentTime = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        currentDifficulty = minDifficulty;
    }

    public void StartGame()
    {
        startGameTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDifficulty >= maxDifficulty) return;
     
        //Update all the params necessary to increase the difficulty   
        currentTime = Time.time;
        float timeElapsedSinceGameStart = currentTime - startGameTime;
        float percentToMaxDifficulty = timeElapsedSinceGameStart / secondsToReachMaxDifficulty;
        currentDifficulty = Mathf.Lerp(minDifficulty, maxDifficulty, howDifficultyScalesWithTime.Evaluate(percentToMaxDifficulty));
        
        int currentObjectSpawnCount = (int) howNOfObjectsToSpawnScalesWithDifficulty.Evaluate(currentDifficulty);
        float currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, howSpeedScalesWithDifficulty.Evaluate(currentDifficulty));

        spawner.nOfObjectsToSpawnNextWave = currentObjectSpawnCount;
        spawner.spawnedObjectsSpeed = currentSpeed;
        
        
    }
}
