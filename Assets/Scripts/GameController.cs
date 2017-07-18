using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    // Spawn hazard waves
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount; // Number of hazards that will be spawned during each waves
    public float startWait; // Player preparation time after starting the game
    public float spawnWait; // Wait time between each hazards
    public float waveWait;  // Wait time between each waves
    // Calculate the score, waveCount and lifeCount then display them on the GUI
    public GUIText scoreText;
    public GUIText waveCountText;
    public GUIText lifeCountText;
    public float respawnWait;
    public int lifeCount;
    private int score;
    private int waveCount;
    // Hold reference to player prefab for respawning purpose
    public GameObject player;
    // After the game is over display gameOverText and restartText to notify player
    public GUIText restartText;
    public GUIText gameOverText;
    // Blink text wait time
    public float blinkWait;
    public float blinkRate;

    private bool restart;   // Restart flag
    private bool gameOver;  // Game over flag
    
    void Start() {
        // Set starting score, waveCount to zero and update tham
        UpdateScore(score);
        waveCount = 0;
        UpdateWaveCount(waveCount);
        UpdateLifeCount();
        // Spawning hazard waves
        StartCoroutine(SpawnWaves());
        // Set restartText and gameOverText to empty string so that they're invisible or 'turned off'
        restartText.text = "";
        gameOverText.text = "";
        // Set restart and gameOver flags to false
        restart = false;
        gameOver = false;
    }

    void Update() {
        // Restart the game
        if (gameOver && Input.GetKeyDown(KeyCode.R)) {
            /* This code（below） is obsolete
             * Application.LoadLevel(Application.loadedLevel);
             */
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator SpawnWaves() {
        // Give player some time to prepare
        yield return new WaitForSeconds(startWait);
        while (true) {
            // Update wave count
            UpdateWaveCount(1);
            for (int i = 0; i < hazardCount; i++) {
                // Randomly select the hazrd from hazards array
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
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
            // If game over than break out the loop
            if (gameOver) {
                break;
            }
        }
    }

    IEnumerator BlinkText() {
        yield return new WaitForSeconds(blinkWait);
        while (restart) {
            restartText.text = "Press 'R' to restart.";
            yield return new WaitForSeconds(1 - blinkRate);
            restartText.text = "";
            yield return new WaitForSeconds(blinkRate);
        }
    }

    public void UpdateScore(int scoreAdded) {
        // Add score and update scoreText
        score += scoreAdded;
        scoreText.text = "Score: " + score;
    }

    public void UpdateWaveCount(int countAdded) {
        // Add waveCount and update waveCount
        waveCount += countAdded;
        waveCountText.text = "Wave Count: " + waveCount;
    }

    public void UpdateLifeCount() {
        // Update lifeCountText
        lifeCountText.text = "Life Remain: " + lifeCount;
    }

    public void PlayerRespawn() {
        // Some Respawn Code (instantiate player and set it to isProtected)
        Instantiate(player, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerController>().isProtected = true;
    }

    public void GameOver() {
        lifeCount -= 1;
        UpdateLifeCount();
        // Check if player has life remains
        if (lifeCount > 0) {
            Invoke("PlayerRespawn", respawnWait);
            return;
        }
        // Display gameOverText and set gameOver flag to true
        gameOverText.text = "Game Over!";
        gameOver = true;
        restart = true;
        StartCoroutine(BlinkText());
    }
}
