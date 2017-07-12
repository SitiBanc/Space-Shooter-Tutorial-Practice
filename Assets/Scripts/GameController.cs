using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount; // Number of hazards that will be spawned during each waves
    public float startWait; // Player preparation time after starting the game
    public float spawnWait; // Wait time between each hazards
    public float waveWait;  // Wait time between each waves

    void Start() {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves() {
        // Give player some time to prepare
        yield return new WaitForSeconds(startWait);
        while (true) {
            for (int i = 0; i < hazardCount; i++) {
                // Randomly select the spawnPosition.x（range from -spawnValues.x to spawnValues.x）
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                // No rotation
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                // Pause before spawning
                yield return new WaitForSeconds(spawnWait);
            }
            // Pause before next wave
            yield return new WaitForSeconds(waveWait);
        }
    }
}
