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
    // Calculate the score and display it on the GUI
    public GUIText scoreText;
    private int score;
    // After the game is over display gameOverText and restartText to notify player
    public GUIText restartText;
    public GUIText gameOverText;
    private bool restart;   // Restart flag
    private bool gameOver;  // Game over flag
    
    void Start() {
        // Set starting score to zero and update the scoreText
        score = 0;
        UpdateScore();
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
        if (restart && Input.GetKeyDown(KeyCode.R)) {
            /* This code is obsolete
             * Application.LoadLevel(Application.loadedLevel);
             */
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator SpawnWaves() {
        // Give player some time to prepare
        yield return new WaitForSeconds(startWait);
        while (true) {
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
                restartText.text = "Press 'R' to restart.";
                restart = true;
                break;
            }
        }
    }

    public void UpdateScore() {
        // Update scoreText
        scoreText.text = "Score: " + score;
    }

    public void AddScore(int scoreAdded) {
        // Add score and update scoreText
        score += scoreAdded;
        UpdateScore();
    }

    public void GameOver() {
        // Display gameOverText and set gameOver flag to true
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
