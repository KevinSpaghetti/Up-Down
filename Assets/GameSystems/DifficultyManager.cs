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

    public float deltaSecondsBetweenWaves = 1;
    
    public AnimationCurve howDifficultyScalesWithTime;
    public AnimationCurve howSpeedScalesWithDifficulty;
    public AnimationCurve howNOfObjectsToSpawnScalesWithDifficulty;

    public float secondsToReachMaxDifficulty;

    public ObstacleSpawner spawner;
    
    [SerializeField]
    private float currentDifficulty = 0.0f;
    [SerializeField]
    private int nOfObjectsToSpawn = 1;
    [SerializeField]
    private float currentSpeed = 0.001f;
    
    private float startGameTime = 0.0f;
    private float currentTime = 0.0f;

    public bool gamePaused = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        currentDifficulty = minDifficulty;
        nOfObjectsToSpawn = minObjectsToSpawn;
        spawner.deltaSecondsBetweenWaves = deltaSecondsBetweenWaves;
        spawner.nOfObjectsToSpawnNextWave = minObjectsToSpawn;
        spawner.spawnedObjectsSpeed = minSpeed;
        spawner.StartSpawning();
        startGameTime = Time.time;
        StartCoroutine(nameof(UpdateDifficulty));
    }

    //Reset the difficulty
    public void Reset()
    {
        currentDifficulty = minDifficulty;
        nOfObjectsToSpawn = minObjectsToSpawn;
        spawner.nOfObjectsToSpawnNextWave = minObjectsToSpawn;
        spawner.spawnedObjectsSpeed = minSpeed;
        
        spawner.ResetState();
    }
    
    public void PauseGame()
    {
        spawner.frozen = true;
        gamePaused = true;
    }

    public void ResumeGame()
    {
        spawner.frozen = false;
        gamePaused = false;
    }

    public void StopGame()
    {
        spawner.EndSpawning();
        StopCoroutine(nameof(UpdateDifficulty));
    }
    
    IEnumerator UpdateDifficulty()
    {

        while (currentDifficulty < maxDifficulty)
        {
            //Update all the params necessary to increase the difficulty   
            currentTime = Time.time;
            float timeElapsedSinceGameStart = currentTime - startGameTime;
            float percentToMaxDifficulty = timeElapsedSinceGameStart / secondsToReachMaxDifficulty;
            currentDifficulty = Mathf.Lerp(minDifficulty, maxDifficulty, howDifficultyScalesWithTime.Evaluate(percentToMaxDifficulty));

            nOfObjectsToSpawn = (int) Mathf.Lerp(minObjectsToSpawn, maxObjectsToSpawn, howNOfObjectsToSpawnScalesWithDifficulty.Evaluate(currentDifficulty));
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, howSpeedScalesWithDifficulty.Evaluate(currentDifficulty));
            
            Debug.Assert(nOfObjectsToSpawn >= 0, "Less than 0 objects to spawn");
            Debug.Assert(nOfObjectsToSpawn <= 5, "More than 5 objects to spawn");

            spawner.nOfObjectsToSpawnNextWave = nOfObjectsToSpawn;
            spawner.spawnedObjectsSpeed = currentSpeed;

            if (gamePaused) yield return new WaitUntil(() => gamePaused == false); //Wait until game resumes
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    
    
}
